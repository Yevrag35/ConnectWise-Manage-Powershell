using MG.Api.PowerShell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;
using System.Net.Http.Headers;

namespace MG.ConnectWise.Cmdlets
{
    [Cmdlet(VerbsCommunications.Connect, "Online", ConfirmImpact = ConfirmImpact.None)]
    [CmdletBinding(PositionalBinding = false)]
    public class ConnectOnline : PSCmdlet
    {
        [Parameter(Mandatory = true, Position = 0)]
        public string Company { get; set; }

        [Parameter(Mandatory = true, Position = 1)]
        public string PublicKey { get; set; }

        [Parameter(Mandatory = true, Position = 2)]
        public Password PrivateKey { get; set; }

        protected override void ProcessRecord()
        {
            string authStr = IntegratorAuth.ConvertToAuth(this.Company, this.PublicKey, this.PrivateKey);
            var handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                UseDefaultCredentials = true
            };

            CWContext.HttpClient = new HttpClient(handler)
            {
                BaseAddress = CWContext.BASE_URL
            };

            CWContext.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(CWContext.BASIC_AUTH, authStr);
        }
    }
}
