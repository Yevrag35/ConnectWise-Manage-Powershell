using MG.Api.PowerShell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;
using System.Reflection;

namespace MG.ConnectWise.Cmdlets
{
    public abstract class BaseLazyCWCmdlet : JsonPsObjectCmdlet
    {
        #region FIELDS/CONSTANTS
        protected abstract string Endpoint { get; }

        protected override string BaseUri => CWContext.BASE_URI;
        protected override Uri BaseUrl => CWContext.BASE_URL;
        protected override HttpClient HttpClient => CWContext.HttpClient;

        #endregion

        #region PARAMETERS


        #endregion

        #region CMDLET PROCESSING
        protected override void BeginProcessing() => base.BeginProcessing();

        protected override void ProcessRecord() => base.ProcessRecord();

        #endregion

        #region BACKEND METHODS

        protected string GetEndpointWithId(int id) => string.Format(CWContext.ZERO_SLASH_ONE, this.Endpoint, id);

        protected override bool IsNotReady() => 
            this.HttpClient == null || (this.HttpClient != null && !this.HttpClient.DefaultRequestHeaders.Contains("Authorization"));

        #endregion
    }
}