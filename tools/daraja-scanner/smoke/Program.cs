using System.Net;
using MpesaSdk;
using MpesaSdk.Extensions;
using MpesaSdk.Interfaces;

var handler = new DarajaHandler();
using var httpClient = new HttpClient(handler)
{
    BaseAddress = new Uri("https://daraja.test/")
};
IMpesaClient client = new MpesaClient(httpClient);

await client.MakeB2BPaymentAsync(null!, "consumer-key", "consumer-secret");

if (handler.Requests.Count != 2)
{
    throw new InvalidOperationException($"Expected two requests, got {handler.Requests.Count}.");
}

if (handler.Requests[0].Authorization != "Basic Y29uc3VtZXIta2V5OmNvbnN1bWVyLXNlY3JldA==")
{
    throw new InvalidOperationException("OAuth request did not use Basic authentication.");
}

if (handler.Requests[1].Authorization != "Bearer test-token")
{
    throw new InvalidOperationException("API request did not use the acquired Bearer token.");
}

if (httpClient.DefaultRequestHeaders.Authorization is not null)
{
    throw new InvalidOperationException("Authorization leaked into shared HttpClient defaults.");
}

Console.WriteLine("Auto-token overload and per-request authorization passed.");

sealed class DarajaHandler : HttpMessageHandler
{
    public List<(HttpMethod Method, string Authorization)> Requests { get; } = new();

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        Requests.Add((
            request.Method,
            request.Headers.Authorization?.ToString() ?? string.Empty));

        var body = request.Method == HttpMethod.Get
            ? "{\"access_token\":\"test-token\"}"
            : "{}";

        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(body)
        });
    }
}
