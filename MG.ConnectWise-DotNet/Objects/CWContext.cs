using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace MG.ConnectWise
{
    public static class CWContext
    {
        internal const string BASE_URI = "/v4_6_release/apis/3.0";
        internal static readonly Uri BASE_URL = new Uri("https://api-na.myconnectwise.net", UriKind.Absolute);
        public static HttpClient HttpClient { get; set; }
    }
}
