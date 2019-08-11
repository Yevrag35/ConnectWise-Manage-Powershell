using MG.Api.PowerShell;
using Microsoft.PowerShell.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;

namespace MG.ConnectWise.Cmdlets
{
    [Cmdlet(VerbsCommon.Get, "Project", ConfirmImpact = ConfirmImpact.None)]
    public class GetProject : JsonPsObjectCmdlet
    {
        internal const string EP = "/project/projects";

        protected override string BaseUri => CWContext.BASE_URI;
        protected override Uri BaseUrl => CWContext.BASE_URL;
        protected override HttpClient HttpClient => CWContext.HttpClient;

        [Parameter(Mandatory = false, Position = 0)]
        public int[] Id { get; set; }

        protected override void BeginProcessing() => base.BeginProcessing();

        protected override void ProcessRecord()
        {
            if (!this.MyInvocation.BoundParameters.ContainsKey("Id"))
            {
                string jsonResult = base.TryGet(EP);
                if (!string.IsNullOrEmpty(jsonResult))
                {
                    base.StringResults.Add(jsonResult);
                }
            }
            else
            {
                for (int i = 0; i < this.Id.Length; i++)
                {
                    int id = this.Id[i];
                    string ep = string.Format(EP + "/{0}", id);
                    string jsonResult = base.TryGet(ep);
                    base.StringResults.Add(jsonResult);
                }
            }
            base.ProcessRecord();
        }
    }
}
