using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TwitchClient.Github
{
    class GithubApi
    {
        private static readonly Uri LatestReleaseUri = new Uri("https://api.github.com/repos/PureCS/twitch-client/releases/latest");

        public static async Task<LatestReleaseJsonObject.RootObject> GetLatestReleaseJsonObject()
        {
            var client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            string json = await client.DownloadStringTaskAsync(LatestReleaseUri);
            return json.Length < 0 ? null : JsonConvert.DeserializeObject<LatestReleaseJsonObject.RootObject>(json);
        }
    }
}
