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
    public class GetProject : BaseLazyCWCmdlet
    {
        protected const string EP = "/project/projects";
        protected override string Endpoint => EP;


        [Parameter(Mandatory = false, Position = 0, ValueFromPipelineByPropertyName = true)]
        public int[] ProjectId { get; set; }

        protected override void BeginProcessing() => base.BeginProcessing();

        protected override void ProcessRecord()
        {
            if (!this.MyInvocation.BoundParameters.ContainsKey("ProjectId"))
            {
                string jsonResult = base.TryGet(EP + "?pageSize=1000");
                if (!string.IsNullOrEmpty(jsonResult))
                {
                    base.StringResults.Add(jsonResult);
                }
            }
            else
            {
                for (int i = 0; i < this.ProjectId.Length; i++)
                {
                    int id = this.ProjectId[i];
                    string ep = base.GetEndpointWithId(id);
                    string jsonResult = base.TryGet(ep + "?pageSize=1000");
                    if (!string.IsNullOrEmpty(jsonResult))
                    {
                        base.StringResults.Add(jsonResult);
                    }
                }
            }
        }

        protected override void EndProcessing() => base.EndProcessing();
    }
}
