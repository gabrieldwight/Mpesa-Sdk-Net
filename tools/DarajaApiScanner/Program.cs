using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

internal static class Program
{
    private const string DefaultPortalUrl = "https://developer.safaricom.co.ke/apis";
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    private static async Task<int> Main(string[] args)
    {
        var options = ScanOptions.Parse(args);
        if (options.ShowHelp)
        {
            Console.WriteLine(ScanOptions.Usage);
            return 0;
        }

        var manifest = JsonSerializer.Deserialize<Inventory>(await File.ReadAllTextAsync(options.ManifestPath), JsonOptions)
            ?? throw new InvalidDataException($"The inventory at '{options.ManifestPath}' is empty.");
        var scanDate = DateTimeOffset.UtcNow;

        ScanSource source;
        if (options.SpecFile is not null)
        {
            var specJson = await File.ReadAllTextAsync(options.SpecFile);
            source = new ScanSource(specJson, Path.GetFullPath(options.SpecFile), false, null);
        }
        else
        {
            source = await FetchPortalSchemaAsync(options.PortalUrl);
        }

        if (source.Json is null && (source.Documents is null || source.Documents.Count == 0))
        {
            var failureStatus = source.FailureStatus ?? "PORTAL_SCHEMA_UNAVAILABLE";
            var failureReport = BuildFailureReport(scanDate, source, failureStatus, manifest.Endpoints.Count);
            await AppendReportAsync(options.ReportPath, failureReport);
            Console.WriteLine($"Scan recorded as {failureStatus}: {source.FailureReason}");
            return 2;
        }

        var portalEndpoints = source.Json is not null
            ? ParseOpenApi(source.Json)
            : source.Documents!
                .SelectMany(ParseOpenApi)
                .GroupBy(endpoint => $"{endpoint.Method} {NormalizePath(endpoint.Path)}", StringComparer.OrdinalIgnoreCase)
                .Select(group => group.First())
                .ToList();
        var changes = Compare(manifest.Endpoints, portalEndpoints);
        var unresolved = FindUnresolved(manifest.Endpoints, portalEndpoints);
        var portalVersions = GetOpenApiVersions(source);
        var versionText = portalVersions.Count == 0 ? "Not published" : string.Join("; ", portalVersions);
        var previousVersion = await ReadLatestPortalVersionAsync(options.ReportPath);
        var versionChanged = source.Verified
            && previousVersion is not null
            && !previousVersion.Equals(versionText, StringComparison.Ordinal);
        var status = !source.Verified
            ? "UNVERIFIED_INPUT"
            : changes.Any(change => change.Status == "NEW_ENDPOINT") ? "MISSING_APIS_FOUND"
            : changes.Count > 0 ? "UPDATES_DETECTED"
            : versionChanged ? "CHANGELOG_UPDATES"
            : unresolved.Count > 0 ? "SCHEMA_INCOMPLETE"
            : "NO_CHANGES";

        var previews = source.Verified
            ? await GeneratePreviewsAsync(changes, options.PreviewDirectory, scanDate)
            : [];
        var report = BuildDiffReport(
            scanDate,
            source,
            status,
            manifest.Endpoints.Count,
            portalEndpoints.Count,
            versionText,
            versionChanged ? previousVersion : null,
            changes,
            unresolved,
            previews);
        await AppendReportAsync(options.ReportPath, report);

        Console.WriteLine($"Scan recorded as {status}: {changes.Count} difference(s), {previews.Count} preview(s).");
        return source.Verified ? 0 : 3;
    }

    private static async Task<ScanSource> FetchPortalSchemaAsync(string portalUrl)
    {
        if (!Uri.TryCreate(portalUrl, UriKind.Absolute, out var portalUri) || portalUri.Scheme != Uri.UriSchemeHttps)
        {
            throw new ArgumentException("The portal URL must be an absolute HTTPS URL.", nameof(portalUrl));
        }
        if (!IsSafaricomHost(portalUri.Host))
        {
            throw new ArgumentException("The portal URL must use a Safaricom-owned host.", nameof(portalUrl));
        }

        using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(30) };
        client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("DarajaApiScanner", "1.0"));

        try
        {
            var pages = new Queue<Uri>();
            var visitedPages = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var visitedSchemas = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var linkFailures = new List<string>();
            var documents = new List<string>();
            var documentUrls = new List<string>();
            pages.Enqueue(portalUri);

            while (pages.Count > 0 && visitedPages.Count < 30)
            {
                var pageUri = pages.Dequeue();
                if (!visitedPages.Add(pageUri.GetLeftPart(UriPartial.Path) + pageUri.Query))
                {
                    continue;
                }

                HttpResponseMessage response;
                try
                {
                    response = await client.GetAsync(pageUri);
                }
                catch (HttpRequestException ex) when (visitedPages.Count > 1)
                {
                    linkFailures.Add($"Could not fetch portal page {pageUri}: {ex.Message}");
                    continue;
                }
                catch (TaskCanceledException ex) when (visitedPages.Count > 1)
                {
                    linkFailures.Add($"Timed out fetching portal page {pageUri}: {ex.Message}");
                    continue;
                }

                using (response)
                {
                    var finalUri = response.RequestMessage?.RequestUri ?? pageUri;
                    if (!IsSafaricomHost(finalUri.Host))
                    {
                        if (pageUri == portalUri)
                        {
                            return ScanSource.Failed(finalUri.ToString(), "PORTAL_SOURCE_REJECTED", "The portal redirected outside the Safaricom domain.");
                        }
                        linkFailures.Add($"Skipped portal page redirected outside Safaricom: {finalUri}");
                        continue;
                    }

                    if (!response.IsSuccessStatusCode)
                    {
                        if (pageUri == portalUri)
                        {
                            return ScanSource.Failed(finalUri.ToString(), "PORTAL_UNAVAILABLE", $"The portal returned HTTP {(int)response.StatusCode}.");
                        }
                        linkFailures.Add($"Portal page {finalUri} returned HTTP {(int)response.StatusCode}.");
                        continue;
                    }

                    var body = await response.Content.ReadAsStringAsync();
                    if (IsOpenApiJson(body))
                    {
                        if (visitedSchemas.Add(finalUri.AbsoluteUri))
                        {
                            documents.Add(body);
                            documentUrls.Add(finalUri.ToString());
                        }
                        continue;
                    }

                    var links = ExtractLinks(body, finalUri).ToList();
                    foreach (var schemaUri in links.Where(IsSchemaUri))
                    {
                        if (!visitedSchemas.Add(schemaUri.AbsoluteUri))
                        {
                            continue;
                        }

                        try
                        {
                            using var schemaResponse = await client.GetAsync(schemaUri);
                            if (!schemaResponse.IsSuccessStatusCode)
                            {
                                continue;
                            }

                            var resolvedUri = schemaResponse.RequestMessage?.RequestUri ?? schemaUri;
                            if (!IsSafaricomHost(resolvedUri.Host))
                            {
                                linkFailures.Add($"Skipped schema link redirected outside Safaricom: {resolvedUri}");
                                continue;
                            }

                            if (!schemaResponse.IsSuccessStatusCode)
                            {
                                linkFailures.Add($"Schema link {resolvedUri} returned HTTP {(int)schemaResponse.StatusCode}.");
                                continue;
                            }

                            var schemaJson = await schemaResponse.Content.ReadAsStringAsync();
                            if (IsOpenApiJson(schemaJson))
                            {
                                documents.Add(schemaJson);
                                documentUrls.Add(resolvedUri.ToString());
                            }
                        }
                        catch (HttpRequestException ex)
                        {
                            linkFailures.Add($"Could not fetch schema link {schemaUri}: {ex.Message}");
                        }
                        catch (TaskCanceledException ex)
                        {
                            linkFailures.Add($"Timed out fetching schema link {schemaUri}: {ex.Message}");
                        }
                    }

                    foreach (var linkedPage in links.Where(IsPortalApiPage))
                    {
                        if (!visitedPages.Contains(linkedPage.GetLeftPart(UriPartial.Path) + linkedPage.Query))
                        {
                            pages.Enqueue(linkedPage);
                        }
                    }
                }
            }

            if (documents.Count > 0)
            {
                return new ScanSource(null, portalUri.ToString(), true, null, null, documents, documentUrls, linkFailures);
            }

            var failureDetails = linkFailures.Count == 0
                ? string.Empty
                : $" Link errors: {string.Join(" | ", linkFailures.Take(5))}";
            return ScanSource.Failed(portalUri.ToString(), "PORTAL_SCHEMA_UNAVAILABLE",
                $"No supported machine-readable OpenAPI or Swagger document was found in {visitedPages.Count} portal page(s).{failureDetails}");
        }
        catch (HttpRequestException ex)
        {
            return ScanSource.Failed(portalUri.ToString(), "PORTAL_UNAVAILABLE", ex.Message);
        }
        catch (TaskCanceledException ex)
        {
            return ScanSource.Failed(portalUri.ToString(), "PORTAL_UNAVAILABLE", ex.Message);
        }
    }

    private static IEnumerable<Uri> ExtractLinks(string html, Uri pageUri)
    {
        var hrefPattern = new Regex("(?:href|src)\\s*=\\s*['\\\"]([^'\\\"]+)['\\\"]", RegexOptions.IgnoreCase);
        var links = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (Match match in hrefPattern.Matches(html))
        {
            if (!Uri.TryCreate(pageUri, WebUtility.HtmlDecode(match.Groups[1].Value), out var candidate)
                || !IsSafaricomHost(candidate.Host))
            {
                continue;
            }

            var normalized = new UriBuilder(candidate) { Fragment = string.Empty }.Uri;
            if (links.Add(normalized.AbsoluteUri))
            {
                yield return normalized;
            }
        }
    }

    private static bool IsSchemaUri(Uri uri)
    {
        var path = uri.AbsolutePath;
        return path.EndsWith(".json", StringComparison.OrdinalIgnoreCase)
            || path.EndsWith(".yaml", StringComparison.OrdinalIgnoreCase)
            || path.EndsWith(".yml", StringComparison.OrdinalIgnoreCase)
            || uri.AbsoluteUri.Contains("openapi", StringComparison.OrdinalIgnoreCase)
            || uri.AbsoluteUri.Contains("swagger", StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsPortalApiPage(Uri uri) =>
        uri.AbsolutePath.StartsWith("/apis", StringComparison.OrdinalIgnoreCase)
        && !IsSchemaUri(uri);

    private static bool IsSafaricomHost(string host) =>
        host.Equals("safaricom.co.ke", StringComparison.OrdinalIgnoreCase)
        || host.EndsWith(".safaricom.co.ke", StringComparison.OrdinalIgnoreCase);

    private static bool IsOpenApiJson(string content)
    {
        try
        {
            using var document = JsonDocument.Parse(content);
            var root = document.RootElement;
            return root.ValueKind == JsonValueKind.Object
                && (root.TryGetProperty("openapi", out _) || root.TryGetProperty("swagger", out _))
                && root.TryGetProperty("paths", out _);
        }
        catch (JsonException)
        {
            return false;
        }
    }

    private static List<PortalEndpoint> ParseOpenApi(string json)
    {
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        if (!root.TryGetProperty("paths", out var paths) || paths.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidDataException("The OpenAPI document has no paths object.");
        }

        var endpoints = new List<PortalEndpoint>();
        foreach (var path in paths.EnumerateObject())
        {
            if (path.Value.ValueKind != JsonValueKind.Object)
            {
                continue;
            }

            foreach (var operation in path.Value.EnumerateObject())
            {
                var method = operation.Name.ToUpperInvariant();
                if (method is not ("GET" or "POST" or "PUT" or "PATCH" or "DELETE" or "OPTIONS" or "HEAD"))
                {
                    continue;
                }

                endpoints.Add(ParseOperation(root, path.Name, method, operation.Value));
            }
        }

        return endpoints;
    }

    private static List<string> GetOpenApiVersions(ScanSource source)
    {
        var jsonDocuments = source.Json is not null ? [source.Json] : source.Documents ?? [];
        return jsonDocuments
            .Select(json =>
            {
                using var document = JsonDocument.Parse(json);
                return document.RootElement.TryGetProperty("info", out var info)
                    ? ReadString(info, "version")
                    : null;
            })
            .Where(version => !string.IsNullOrWhiteSpace(version))
            .Select(version => version!)
            .Distinct(StringComparer.Ordinal)
            .OrderBy(version => version, StringComparer.Ordinal)
            .ToList();
    }

    private static async Task<string?> ReadLatestPortalVersionAsync(string reportPath)
    {
        if (!File.Exists(reportPath))
        {
            return null;
        }

        var report = await File.ReadAllTextAsync(reportPath);
        var matches = Regex.Matches(report, @"^- \*\*OpenAPI versions:\*\* (.+)$", RegexOptions.Multiline);
        if (matches.Count == 0)
        {
            return null;
        }

        var latestVersion = matches[^1].Groups[1].Value.Trim();
        return latestVersion.Equals("Not published", StringComparison.OrdinalIgnoreCase) ? null : latestVersion;
    }

    private static PortalEndpoint ParseOperation(JsonElement root, string path, string method, JsonElement operation)
    {
        var requestSchema = TryGetRequestSchema(root, operation);
        var responseSchema = TryGetResponseSchema(root, operation);
        var requestFields = requestSchema is null ? null : GetSchemaFields(root, requestSchema.Value);
        var responseFields = responseSchema is null ? null : GetSchemaFields(root, responseSchema.Value);
        var callbackFields = GetCallbackFields(root, operation);
        var auth = GetAuth(root, operation);
        var headers = GetParameterNames(root, operation, "header");
        var queryFields = GetParameterNames(root, operation, "query");
        var summary = operation.TryGetProperty("summary", out var summaryValue) && summaryValue.ValueKind == JsonValueKind.String
            ? summaryValue.GetString()
            : null;
        var operationId = operation.TryGetProperty("operationId", out var operationIdValue) && operationIdValue.ValueKind == JsonValueKind.String
            ? operationIdValue.GetString()
            : null;

        return new PortalEndpoint(
            summary ?? operationId ?? $"{method} {path}",
            method,
            path,
            auth,
            headers,
            queryFields,
            requestFields,
            responseFields,
            callbackFields);
    }

    private static List<string>? GetCallbackFields(JsonElement root, JsonElement operation)
    {
        if (!operation.TryGetProperty("callbacks", out var callbacks) || callbacks.ValueKind != JsonValueKind.Object)
        {
            return null;
        }

        var fields = new SortedSet<string>(StringComparer.Ordinal);
        foreach (var callback in callbacks.EnumerateObject())
        {
            var callbackValue = ResolveReference(root, callback.Value);
            foreach (var expression in callbackValue.EnumerateObject())
            {
                var callbackPath = ResolveReference(root, expression.Value);
                foreach (var callbackOperation in callbackPath.EnumerateObject())
                {
                    if (callbackOperation.Name is not ("get" or "post" or "put" or "patch" or "delete"))
                    {
                        continue;
                    }

                    var schema = TryGetRequestSchema(root, callbackOperation.Value);
                    if (schema is not null)
                    {
                        CollectAllSchemaFields(root, schema.Value, fields, new HashSet<string>(StringComparer.Ordinal));
                    }
                }
            }
        }

        return fields.Count == 0 ? null : fields.ToList();
    }

    private static void CollectAllSchemaFields(JsonElement root, JsonElement schema, ISet<string> fields, ISet<string> visited)
    {
        if (schema.ValueKind != JsonValueKind.Object)
        {
            return;
        }

        if (schema.TryGetProperty("$ref", out var reference) && reference.ValueKind == JsonValueKind.String)
        {
            var referenceValue = reference.GetString()!;
            if (visited.Add(referenceValue))
            {
                CollectAllSchemaFields(root, ResolveReference(root, schema), fields, visited);
            }
            return;
        }

        if (schema.TryGetProperty("properties", out var properties) && properties.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in properties.EnumerateObject())
            {
                fields.Add(property.Name);
                CollectAllSchemaFields(root, property.Value, fields, visited);
            }
        }

        foreach (var composite in new[] { "allOf", "oneOf", "anyOf" })
        {
            if (!schema.TryGetProperty(composite, out var options) || options.ValueKind != JsonValueKind.Array)
            {
                continue;
            }

            foreach (var option in options.EnumerateArray())
            {
                CollectAllSchemaFields(root, option, fields, visited);
            }
        }
    }

    private static JsonElement? TryGetRequestSchema(JsonElement root, JsonElement operation)
    {
        if (operation.TryGetProperty("requestBody", out var requestBody))
        {
            requestBody = ResolveReference(root, requestBody);
            if (!requestBody.TryGetProperty("content", out var content))
            {
                return null;
            }

            foreach (var mediaType in content.EnumerateObject())
            {
                if (mediaType.Name.Contains("json", StringComparison.OrdinalIgnoreCase)
                    && mediaType.Value.TryGetProperty("schema", out var schema))
                {
                    return schema;
                }
            }

            return null;
        }

        if (!operation.TryGetProperty("parameters", out var parameters) || parameters.ValueKind != JsonValueKind.Array)
        {
            return null;
        }

        foreach (var parameter in parameters.EnumerateArray())
        {
            var resolvedParameter = ResolveReference(root, parameter);
            if (string.Equals(ReadString(resolvedParameter, "in"), "body", StringComparison.OrdinalIgnoreCase)
                && resolvedParameter.TryGetProperty("schema", out var schema))
            {
                return schema;
            }
        }

        return null;
    }

    private static JsonElement? TryGetResponseSchema(JsonElement root, JsonElement operation)
    {
        if (!operation.TryGetProperty("responses", out var responses))
        {
            return null;
        }

        foreach (var response in responses.EnumerateObject()
                     .Where(item => item.Name.StartsWith('2'))
                     .OrderBy(item => item.Name, StringComparer.Ordinal))
        {
            var responseValue = ResolveReference(root, response.Value);
            if (!responseValue.TryGetProperty("content", out var content))
            {
                if (responseValue.TryGetProperty("schema", out var swaggerSchema))
                {
                    return swaggerSchema;
                }
                continue;
            }

            foreach (var mediaType in content.EnumerateObject())
            {
                if (mediaType.Name.Contains("json", StringComparison.OrdinalIgnoreCase)
                    && mediaType.Value.TryGetProperty("schema", out var schema))
                {
                    return schema;
                }
            }
        }

        return null;
    }

    private static string? GetAuth(JsonElement root, JsonElement operation)
    {
        JsonElement security;
        if (!operation.TryGetProperty("security", out security)
            && !root.TryGetProperty("security", out security))
        {
            return null;
        }

        if (security.ValueKind != JsonValueKind.Array || security.GetArrayLength() == 0)
        {
            return "None";
        }

        JsonElement schemes = default;
        var hasSchemes = root.TryGetProperty("components", out var components)
            && components.TryGetProperty("securitySchemes", out schemes);
        if (!hasSchemes)
        {
            hasSchemes = root.TryGetProperty("securityDefinitions", out schemes);
        }
        if (!hasSchemes)
        {
            return "Unknown";
        }

        var types = new SortedSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var requirement in security.EnumerateArray())
        {
            foreach (var schemeName in requirement.EnumerateObject().Select(item => item.Name))
            {
                if (!schemes.TryGetProperty(schemeName, out var scheme))
                {
                    types.Add("Unknown");
                    continue;
                }

                var type = ReadString(scheme, "type");
                var schemeType = ReadString(scheme, "scheme");
                types.Add(type switch
                {
                    "basic" => "Basic",
                    "http" when string.Equals(schemeType, "bearer", StringComparison.OrdinalIgnoreCase) => "Bearer",
                    "http" when string.Equals(schemeType, "basic", StringComparison.OrdinalIgnoreCase) => "Basic",
                    "apiKey" => "ApiKey",
                    "oauth2" or "openIdConnect" => "OAuth",
                    _ => "Unknown"
                });
            }
        }

        return types.Count == 0 ? "Unknown" : string.Join(" or ", types);
    }

    private static List<string>? GetSchemaFields(JsonElement root, JsonElement schema)
    {
        var fields = new SortedSet<string>(StringComparer.Ordinal);
        var visited = new HashSet<string>(StringComparer.Ordinal);
        CollectSchemaFields(root, schema, string.Empty, fields, visited);
        return fields.Count == 0 ? null : fields.ToList();
    }

    private static void CollectSchemaFields(
        JsonElement root,
        JsonElement schema,
        string prefix,
        ISet<string> fields,
        ISet<string> visited)
    {
        if (schema.ValueKind != JsonValueKind.Object)
        {
            return;
        }

        if (schema.TryGetProperty("$ref", out var reference) && reference.ValueKind == JsonValueKind.String)
        {
            var referenceValue = reference.GetString()!;
            if (visited.Add($"{referenceValue}|{prefix}"))
            {
                var resolved = ResolveReference(root, schema);
                CollectSchemaFields(root, resolved, prefix, fields, visited);
            }
            return;
        }

        if (schema.TryGetProperty("properties", out var properties) && properties.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in properties.EnumerateObject())
            {
                var fieldPath = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix}.{property.Name}";
                fields.Add(fieldPath);
                CollectSchemaFields(root, property.Value, fieldPath, fields, visited);
            }
        }

        if (schema.TryGetProperty("items", out var items))
        {
            CollectSchemaFields(root, items, prefix, fields, visited);
        }

        foreach (var composite in new[] { "allOf", "oneOf", "anyOf" })
        {
            if (!schema.TryGetProperty(composite, out var options) || options.ValueKind != JsonValueKind.Array)
            {
                continue;
            }

            foreach (var option in options.EnumerateArray())
            {
                CollectSchemaFields(root, option, prefix, fields, visited);
            }
        }
    }

    private static List<string> GetParameterNames(JsonElement root, JsonElement operation, string location)
    {
        var names = new SortedSet<string>(StringComparer.Ordinal);
        if (!operation.TryGetProperty("parameters", out var parameters) || parameters.ValueKind != JsonValueKind.Array)
        {
            return names.ToList();
        }

        foreach (var parameter in parameters.EnumerateArray())
        {
            var resolved = ResolveReference(root, parameter);
            if (string.Equals(ReadString(resolved, "in"), location, StringComparison.OrdinalIgnoreCase))
            {
                var name = ReadString(resolved, "name");
                if (name is not null)
                {
                    names.Add(name);
                }
            }
        }

        return names.ToList();
    }

    private static JsonElement ResolveReference(JsonElement root, JsonElement element)
    {
        var current = element;
        var resolved = new HashSet<string>(StringComparer.Ordinal);
        while (current.ValueKind == JsonValueKind.Object
               && current.TryGetProperty("$ref", out var reference)
               && reference.ValueKind == JsonValueKind.String)
        {
            var referencePath = reference.GetString()!;
            if (!referencePath.StartsWith("#/", StringComparison.Ordinal) || !resolved.Add(referencePath))
            {
                return current;
            }

            var target = root;
            foreach (var segment in referencePath[2..].Split('/'))
            {
                var propertyName = segment.Replace("~1", "/", StringComparison.Ordinal)
                    .Replace("~0", "~", StringComparison.Ordinal);
                if (!target.TryGetProperty(propertyName, out target))
                {
                    return current;
                }
            }

            current = target;
        }

        return current;
    }

    private static string? ReadString(JsonElement element, string propertyName) =>
        element.ValueKind == JsonValueKind.Object
        && element.TryGetProperty(propertyName, out var value)
        && value.ValueKind == JsonValueKind.String
            ? value.GetString()
            : null;

    private static List<ApiChange> Compare(IReadOnlyCollection<Endpoint> inventory, IReadOnlyCollection<PortalEndpoint> portal)
    {
        var changes = new List<ApiChange>();
        foreach (var remote in portal)
        {
            var local = inventory.FirstOrDefault(endpoint =>
                endpoint.Method.Equals(remote.Method, StringComparison.OrdinalIgnoreCase)
                && NormalizePath(endpoint.Path).Equals(NormalizePath(remote.Path), StringComparison.OrdinalIgnoreCase));

            if (local is null)
            {
                changes.Add(new ApiChange(remote, "NEW_ENDPOINT", null, DescribeNewEndpoint(remote)));
                continue;
            }

            var differences = new List<string>();
            AddFieldDifference(differences, "request", local.RequestFields, remote.RequestFields);
            AddFieldDifference(differences, "response", local.ResponseFields, remote.ResponseFields);
            AddFieldDifference(differences, "callback", local.CallbackFields, remote.CallbackFields);
            AddFieldDifference(differences, "header", local.Headers, remote.Headers);
            AddFieldDifference(differences, "query parameter", local.QueryFields, remote.QueryFields);

            if (local.Auth is not null && remote.Auth is not null
                && !local.Auth.Equals(remote.Auth, StringComparison.OrdinalIgnoreCase))
            {
                differences.Add($"authorization: {local.Auth} -> {remote.Auth}");
            }

            if (differences.Count > 0)
            {
                changes.Add(new ApiChange(remote, "MODIFIED_SCHEMA", local, string.Join("; ", differences)));
            }
        }

        return changes;
    }

    private static List<string> FindUnresolved(IReadOnlyCollection<Endpoint> inventory, IReadOnlyCollection<PortalEndpoint> portal)
    {
        var unresolved = new List<string>();
        foreach (var local in inventory)
        {
            var remote = portal.FirstOrDefault(endpoint =>
                endpoint.Method.Equals(local.Method, StringComparison.OrdinalIgnoreCase)
                && NormalizePath(endpoint.Path).Equals(NormalizePath(local.Path), StringComparison.OrdinalIgnoreCase));

            if (remote is null)
            {
                unresolved.Add($"{local.Method} {local.Path} is absent from the downloaded portal schema; omission alone does not confirm deprecation.");
                continue;
            }

            if (local.RequestFields is { Count: > 0 } && remote.RequestFields is null)
            {
                unresolved.Add($"{local.Method} {local.Path} has no comparable request schema.");
            }
            if (local.ResponseFields is { Count: > 0 } && remote.ResponseFields is null)
            {
                unresolved.Add($"{local.Method} {local.Path} has no comparable response schema.");
            }
            if (local.CallbackFields is { Count: > 0 } && remote.CallbackFields is null)
            {
                unresolved.Add($"{local.Method} {local.Path} has no comparable callback schema.");
            }
            if (local.Auth is not null && remote.Auth is null)
            {
                unresolved.Add($"{local.Method} {local.Path} does not declare an authorization scheme in the downloaded schema.");
            }
            if (remote.RequestFields?.Any(IsCallbackField) == true && remote.CallbackFields is null)
            {
                unresolved.Add($"{local.Method} {local.Path} contains callback URL fields but no callback schema.");
            }
        }

        foreach (var remote in portal)
        {
            var local = inventory.FirstOrDefault(endpoint =>
                endpoint.Method.Equals(remote.Method, StringComparison.OrdinalIgnoreCase)
                && NormalizePath(endpoint.Path).Equals(NormalizePath(remote.Path), StringComparison.OrdinalIgnoreCase));
            if (local is null && (remote.RequestFields is null || remote.ResponseFields is null))
            {
                unresolved.Add($"New operation {remote.Method} {remote.Path} lacks a request or response schema; code preview suppressed.");
            }
            if (local is null && remote.RequestFields?.Any(IsCallbackField) == true && remote.CallbackFields is null)
            {
                unresolved.Add($"New operation {remote.Method} {remote.Path} contains callback URL fields but no callback schema.");
            }
        }

        return unresolved;
    }

    private static bool IsCallbackField(string field) =>
        field.Contains("callback", StringComparison.OrdinalIgnoreCase)
        || field.Contains("resulturl", StringComparison.OrdinalIgnoreCase)
        || field.Contains("queuetimeouturl", StringComparison.OrdinalIgnoreCase);

    private static string NormalizePath(string path)
    {
        var pathWithoutQuery = path.Split('?', 2)[0].Trim('/');
        return Regex.Replace(pathWithoutQuery, "\\{[^}]+\\}", "{}");
    }

    private static void AddFieldDifference(List<string> differences, string label, IReadOnlyCollection<string>? local, IReadOnlyCollection<string>? remote)
    {
        if (local is null || remote is null)
        {
            return;
        }

        var localSet = local.ToHashSet(StringComparer.Ordinal);
        var remoteSet = remote.ToHashSet(StringComparer.Ordinal);
        var added = remoteSet.Except(localSet).OrderBy(value => value, StringComparer.Ordinal).ToArray();
        var removed = localSet.Except(remoteSet).OrderBy(value => value, StringComparer.Ordinal).ToArray();
        if (added.Length > 0 || removed.Length > 0)
        {
            var parts = new List<string>();
            if (added.Length > 0)
            {
                parts.Add($"added {string.Join(", ", added)}");
            }
            if (removed.Length > 0)
            {
                parts.Add($"removed {string.Join(", ", removed)}");
            }
            differences.Add($"{label} fields: {string.Join(" and ", parts)}");
        }
    }

    private static string DescribeNewEndpoint(PortalEndpoint endpoint)
    {
        var request = endpoint.RequestFields is null ? "request schema unavailable" : $"request: {string.Join(", ", endpoint.RequestFields)}";
        var response = endpoint.ResponseFields is null ? "response schema unavailable" : $"response: {string.Join(", ", endpoint.ResponseFields)}";
        return $"{request}; {response}; auth: {endpoint.Auth ?? "unspecified"}";
    }

    private static async Task<List<string>> GeneratePreviewsAsync(
        IReadOnlyCollection<ApiChange> changes,
        string previewDirectory,
        DateTimeOffset scanDate)
    {
        var previewable = changes.Where(change =>
            change.Endpoint.RequestFields is not null && change.Endpoint.ResponseFields is not null).ToList();
        if (previewable.Count == 0)
        {
            return [];
        }

        var runDirectory = Path.Combine(previewDirectory, scanDate.ToString("yyyyMMdd-HHmmss"));
        Directory.CreateDirectory(runDirectory);
        var files = new List<string>();

        foreach (var change in previewable)
        {
            var typeName = ToIdentifier(change.Endpoint.OperationName, change.Endpoint.Path);
            var fileName = $"{typeName}.preview.md";
            var body = BuildPreview(change, typeName);
            var filePath = Path.Combine(runDirectory, fileName);
            await File.WriteAllTextAsync(filePath, body);
            files.Add(Path.GetRelativePath(Environment.CurrentDirectory, filePath));
        }

        return files;
    }

    private static string BuildPreview(ApiChange change, string typeName)
    {
        var builder = new StringBuilder();
        builder.AppendLine("# Non-applied Daraja code preview");
        builder.AppendLine();
        builder.AppendLine($"> Source: `{EscapeMarkdown(change.Endpoint.Path)}`. This file is generated for review and is not included in the SDK.");
        builder.AppendLine();
        builder.AppendLine("```csharp");
        builder.AppendLine("using System.Text.Json.Serialization;");
        builder.AppendLine();
        builder.AppendLine($"public sealed class {typeName}Request");
        builder.AppendLine("{");
        AppendPreviewProperties(builder, change.Endpoint.RequestFields!);
        builder.AppendLine("}");
        builder.AppendLine();
        builder.AppendLine($"public sealed class {typeName}Response");
        builder.AppendLine("{");
        AppendPreviewProperties(builder, change.Endpoint.ResponseFields!);
        builder.AppendLine("}");
        builder.AppendLine("```");
        builder.AppendLine();
        builder.AppendLine("Review the field types, requiredness, callback contracts, validation rules, and endpoint mapping before implementation.");
        return builder.ToString();
    }

    private static void AppendPreviewProperties(StringBuilder builder, IEnumerable<string> fields)
    {
        foreach (var field in fields)
        {
            if (field.Contains('.', StringComparison.Ordinal))
            {
                builder.AppendLine($"    // Nested JSON field path requiring review: {JsonSerializer.Serialize(field)}");
                continue;
            }

            builder.AppendLine($"    [JsonPropertyName({JsonSerializer.Serialize(field)})]");
            builder.AppendLine($"    public string? {ToPropertyIdentifier(field)} {{ get; init; }}");
        }
    }

    private static string ToIdentifier(string? operationName, string path)
    {
        var source = string.IsNullOrWhiteSpace(operationName) ? path : operationName;
        var words = Regex.Matches(source!, "[A-Za-z0-9]+").Select(match => match.Value).ToArray();
        var result = string.Concat(words.Select(word => char.ToUpperInvariant(word[0]) + word[1..]));
        if (result.Length == 0 || char.IsDigit(result[0]))
        {
            result = "Daraja" + result;
        }
        return result;
    }

    private static string ToPropertyIdentifier(string source)
    {
        var words = Regex.Matches(source, "[A-Za-z0-9]+").Select(match => match.Value).ToArray();
        var result = string.Concat(words.Select(word => char.ToUpperInvariant(word[0]) + word[1..]));
        if (result.Length == 0 || char.IsDigit(result[0]))
        {
            result = "Field" + result;
        }
        return result;
    }

    private static async Task AppendReportAsync(string reportPath, string section)
    {
        var directory = Path.GetDirectoryName(Path.GetFullPath(reportPath));
        if (directory is not null)
        {
            Directory.CreateDirectory(directory);
        }

        var existing = File.Exists(reportPath) ? await File.ReadAllTextAsync(reportPath) : "# Daraja API audit log\n";
        var separator = existing.EndsWith(Environment.NewLine, StringComparison.Ordinal) ? "" : Environment.NewLine;
        await File.WriteAllTextAsync(reportPath, $"{existing}{separator}{Environment.NewLine}{section}{Environment.NewLine}");
    }

    private static string BuildFailureReport(DateTimeOffset scanDate, ScanSource source, string status, int inventoryCount)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"## {scanDate:yyyy-MM-dd} — `{status}`");
        builder.AppendLine();
        builder.AppendLine($"- **Date of scan:** {scanDate:yyyy-MM-dd}");
        builder.AppendLine($"- **Source:** <{EscapeMarkdown(source.SourceUrl)}>");
        if (source.DocumentUrls is { Count: > 0 })
        {
            builder.AppendLine($"- **OpenAPI documents found:** {source.DocumentUrls.Count}");
            foreach (var documentUrl in source.DocumentUrls)
            {
                builder.AppendLine($"  - <{EscapeMarkdown(documentUrl)}>");
            }
        }
        if (source.Warnings is { Count: > 0 })
        {
            builder.AppendLine("- **Crawl warnings:**");
            foreach (var warning in source.Warnings.Take(5))
            {
                builder.AppendLine($"  - {EscapeMarkdown(warning)}");
            }
        }
        builder.AppendLine($"- **Detected status:** `{status}`");
        builder.AppendLine($"- **Result:** {EscapeMarkdown(source.FailureReason ?? "No machine-readable portal schema was available.")}");
        builder.AppendLine("- **Interpretation:** This is not a `NO_CHANGES` result. No API changes are marked verified.");
        builder.AppendLine($"- **Local inventory:** {inventoryCount} operations.");
        builder.AppendLine();
        builder.AppendLine("| Endpoint | Status | Field changes |");
        builder.AppendLine("|---|---|---|");
        builder.AppendLine("| Safaricom API catalog | UNVERIFIED | Portal schema unavailable; no field comparison was possible |");
        return builder.ToString();
    }

    private static string BuildDiffReport(
        DateTimeOffset scanDate,
        ScanSource source,
        string status,
        int inventoryCount,
        int portalCount,
        string versionText,
        string? previousVersion,
        IReadOnlyCollection<ApiChange> changes,
        IReadOnlyCollection<string> unresolved,
        IReadOnlyCollection<string> previews)
    {
        var builder = new StringBuilder();
        builder.AppendLine($"## {scanDate:yyyy-MM-dd} — `{status}`");
        builder.AppendLine();
        builder.AppendLine($"- **Date of scan:** {scanDate:yyyy-MM-dd}");
        builder.AppendLine($"- **Source:** <{EscapeMarkdown(source.SourceUrl)}>");
        builder.AppendLine(source.Verified
            ? $"- **OpenAPI versions:** {EscapeMarkdown(versionText)}"
            : $"- **Unverified input OpenAPI versions:** {EscapeMarkdown(versionText)}");
        if (previousVersion is not null)
        {
            builder.AppendLine($"- **Previous recorded OpenAPI version:** {EscapeMarkdown(previousVersion)}");
        }
        builder.AppendLine($"- **Detected status:** `{status}`");
        builder.AppendLine($"- **Operations compared:** {portalCount} portal operations against {inventoryCount} local mappings.");
        builder.AppendLine(source.Verified
            ? "- **Interpretation:** Schema differences are compared with the repository-derived inventory; no SDK implementation changes are applied."
            : "- **Interpretation:** This local input is unverified and is not evidence of Safaricom changes; no SDK implementation changes are applied.");
        builder.AppendLine();
        builder.AppendLine("| Endpoint | Status | Field changes |");
        builder.AppendLine("|---|---|---|");

        if (changes.Count == 0 && unresolved.Count == 0)
        {
            builder.AppendLine("| All parsed operations | NO_DIFFERENCES | No differences found in the fields available for comparison |");
        }
        foreach (var change in changes)
        {
            builder.AppendLine($"| `{EscapeMarkdown(change.Endpoint.Method)} {EscapeMarkdown(change.Endpoint.Path)}` | {change.Status} | {EscapeMarkdown(change.Details)} |");
        }
        foreach (var item in unresolved)
        {
            builder.AppendLine($"| Portal coverage | UNRESOLVED | {EscapeMarkdown(item)} |");
        }
        if (previousVersion is not null)
        {
            builder.AppendLine($"| OpenAPI version | {(versionText.Equals(previousVersion, StringComparison.Ordinal) ? "UNCHANGED" : "CHANGELOG_UPDATES")} | {EscapeMarkdown(previousVersion)} -> {EscapeMarkdown(versionText)} |");
        }

        if (previews.Count > 0)
        {
            builder.AppendLine();
            builder.AppendLine("**Non-applied code previews:**");
            foreach (var preview in previews)
            {
                builder.AppendLine($"- `{EscapeMarkdown(preview)}`");
            }
        }

        return builder.ToString();
    }

    private static string EscapeMarkdown(string value) =>
        value.Replace("|", "\\|", StringComparison.Ordinal)
            .Replace("\r", " ", StringComparison.Ordinal)
            .Replace("\n", " ", StringComparison.Ordinal);

    private sealed record Inventory(int SchemaVersion, string Description, string PortalSource, List<Endpoint> Endpoints);

    private sealed record Endpoint(
        string Name,
        string Method,
        string Path,
        string? Auth,
        List<string>? Headers,
        List<string>? QueryFields,
        List<string>? RequestFields,
        List<string>? ResponseFields,
        List<string>? CallbackFields,
        Mapping Mapping);

    private sealed record Mapping(string? RequestType, string? ResponseType, List<string> CallbackDtos);

    private sealed record PortalEndpoint(
        string OperationName,
        string Method,
        string Path,
        string? Auth,
        List<string> Headers,
        List<string> QueryFields,
        List<string>? RequestFields,
        List<string>? ResponseFields,
        List<string>? CallbackFields);

    private sealed record ApiChange(PortalEndpoint Endpoint, string Status, Endpoint? Local, string Details);

    private sealed record ScanSource(
        string? Json,
        string SourceUrl,
        bool Verified,
        string? FailureReason,
        string? FailureStatus = null,
        List<string>? Documents = null,
        List<string>? DocumentUrls = null,
        List<string>? Warnings = null)
    {
        public static ScanSource Failed(string sourceUrl, string status, string reason) =>
            new(null, sourceUrl, false, reason, status);
    }

    private sealed class ScanOptions
    {
        public string ManifestPath { get; private set; } = Path.Combine("src", "daraja", "schema.json");
        public string ReportPath { get; private set; } = Path.Combine("docs", "CHANGELOG_DARAJA.md");
        public string PreviewDirectory { get; private set; } = Path.Combine("artifacts", "daraja-code-preview");
        public string PortalUrl { get; private set; } = DefaultPortalUrl;
        public string? SpecFile { get; private set; }
        public bool ShowHelp { get; private set; }

        public const string Usage = """
            Daraja API scanner
              dotnet run --project tools\DarajaApiScanner\DarajaApiScanner.csproj
              dotnet run --project tools\DarajaApiScanner\DarajaApiScanner.csproj -- --spec-file path\to\openapi.json

            Options:
              --manifest <path>     Repository-derived endpoint inventory
              --report <path>       Append-only markdown audit report
              --preview-dir <path>  Output directory for verified, non-applied code previews
              --portal-url <url>    Safaricom portal page to inspect
              --spec-file <path>    Local OpenAPI JSON input (always unverified; no previews)
              --help                Show this help
            """;

        public static ScanOptions Parse(string[] args)
        {
            var options = new ScanOptions();
            for (var index = 0; index < args.Length; index++)
            {
                var argument = args[index];
                if (argument is "--help" or "-h")
                {
                    options.ShowHelp = true;
                    continue;
                }

                if (index + 1 >= args.Length)
                {
                    throw new ArgumentException($"Missing value for '{argument}'.");
                }

                var value = args[++index];
                switch (argument)
                {
                    case "--manifest":
                        options.ManifestPath = value;
                        break;
                    case "--report":
                        options.ReportPath = value;
                        break;
                    case "--preview-dir":
                        options.PreviewDirectory = value;
                        break;
                    case "--portal-url":
                        options.PortalUrl = value;
                        break;
                    case "--spec-file":
                        options.SpecFile = value;
                        break;
                    default:
                        throw new ArgumentException($"Unknown argument '{argument}'.");
                }
            }

            return options;
        }
    }
}
