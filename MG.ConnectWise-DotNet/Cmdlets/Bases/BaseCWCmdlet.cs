using MG.Api.PowerShell;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace MG.ConnectWise.Cmdlets
{
    public abstract class BaseCWCmdlet : BaseApiCmdlet
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

        #endregion

        #region BACKEND METHODS
        protected string GetEndpointWithId(int id) => string.Format(CWContext.ZERO_SLASH_ONE, this.Endpoint, id);

        protected override bool IsNotReady() =>
            this.HttpClient == null || (this.HttpClient != null && !this.HttpClient.DefaultRequestHeaders.Contains("Authorization"));

        #endregion
    }
}