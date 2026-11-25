# Daraja API scanner

The scanner compares a Safaricom OpenAPI document with the repository-derived inventory in `src/daraja/schema.json`. It does not update SDK source files. For verified differences, it writes review-only C# snippets under `artifacts/daraja-code-preview/`.

Run from the repository root:

```powershell
dotnet run --project tools\DarajaApiScanner\DarajaApiScanner.csproj
```

By default, the scanner fetches `https://developer.safaricom.co.ke/apis`, crawls up to 30 same-domain `/apis` pages, and follows links to JSON OpenAPI documents. If the portal does not publish a machine-readable schema, the scan is recorded as unresolved instead of treating absent metadata as no changes.

An OpenAPI JSON file can also be supplied for local testing:

```powershell
dotnet run --project tools\DarajaApiScanner\DarajaApiScanner.csproj -- --spec-file path\to\daraja-openapi.json
```

Local input is always marked unverified and cannot produce code previews. Options:

- `--manifest <path>` changes the local inventory path.
- `--report <path>` changes the append-only audit report path.
- `--preview-dir <path>` changes the generated preview directory.
- `--portal-url <url>` changes the portal landing page.
- `--help` prints usage.

The scanner supports JSON OpenAPI 3.x and Swagger 2.0. It compares endpoint methods, authorization schemes, header/query parameter names, and request/response/callback property paths. Field presence is checked; value types, requiredness, constraints, and semantic deprecation annotations are not yet compared. Verified document versions are recorded and compared with the most recent verified changelog entry. Generated snippets use placeholder string types and are review-only. Non-machine-readable pages and unsupported formats are reported as unresolved.
