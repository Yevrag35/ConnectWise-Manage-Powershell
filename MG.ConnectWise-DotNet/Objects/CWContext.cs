using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MG.ConnectWise
{
    public static class CWContext
    {
        internal const string BASE_URI = "/v4_6_release/apis/3.0";
        internal const string BASIC_AUTH = "Basic";
        internal static readonly Uri BASE_URL = new Uri(CW_ONLINE, UriKind.Absolute);
        private const string CW_ONLINE = "https://api-na.myconnectwise.net";
        internal const string JSON_DT_FORMAT = "[yyyy-MM-ddTHH:mm:ssZ]";
        internal const string ZERO_SLASH_ONE = "{0}/{1}";

        public static HttpClient HttpClient { get; set; }

#if DEBUG

        [Obsolete]
        public async static Task<string> TryGet(string endpoint)
        {
            endpoint = BASE_URI + endpoint;
            using (HttpResponseMessage msg = await HttpClient.GetAsync(endpoint, HttpCompletionOption.ResponseContentRead))
            {
                using (HttpResponseMessage resp = msg.EnsureSuccessStatusCode())
                {
                    using (var content = resp.Content)
                    {
                        string strRes = await content.ReadAsStringAsync();
                        object idontknow = JsonConvert.DeserializeObject(strRes);
                        return JsonConvert.SerializeObject(idontknow, Formatting.Indented);
                    }
                }
            }
        }

#endif
    }
}
