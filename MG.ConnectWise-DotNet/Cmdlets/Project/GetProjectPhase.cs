using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Reflection;

namespace MG.ConnectWise.Cmdlets.Project
{
    [Cmdlet(VerbsCommon.Get, "ProjectPhase", ConfirmImpact = ConfirmImpact.None, DefaultParameterSetName = "None")]
    public class GetProjectPhase : BaseLazyCWCmdlet
    {
        #region FIELDS/CONSTANTS
        private const string EP = "/project/projects/{0}/phases";
        private string _ep = EP;
        protected override string Endpoint => _ep;

        #endregion

        #region PARAMETERS
        [Parameter(Mandatory = true, Position = 0, ValueFromPipelineByPropertyName = true)]
        public int ProjectId { get; set; }

        [Parameter(Mandatory = true, Position = 1, ParameterSetName = "ByPhaseId")]
        public int[] PhaseId { get; set; }

        #endregion

        #region CMDLET PROCESSING
        protected override void BeginProcessing() => base.BeginProcessing();

        protected override void ProcessRecord()
        {
            switch (this.ParameterSetName)
            {
                case "ByPhaseId":
                {
                    _ep = string.Format(EP, this.ProjectId) + "/{0}";
                    this.GetMultiPhases(this.PhaseId);
                    break;
                }
                default:
                {
                    _ep = string.Format(EP, this.ProjectId);
                    this.GetAllPhases();
                    break;
                }
            }
        }

        protected override void EndProcessing() => base.EndProcessing();

        #endregion

        #region BACKEND METHODS
        private void GetAllPhases()
        {
            string allRes = base.TryGet(_ep);
            if (!string.IsNullOrEmpty(allRes))
                base.StringResults.Add(allRes);
        }

        private void GetMultiPhases(params int[] phaseIds)
        {
            if (phaseIds != null)
            {
                for (int i = 0; i < phaseIds.Length; i++)
                {
                    int pid = phaseIds[i];
                    string pEp = string.Format(_ep, pid);
                    string result = base.TryGet(pEp);
                    if (!string.IsNullOrEmpty(result))
                        base.StringResults.Add(result);
                }
            }
        }

        #endregion
    }
}