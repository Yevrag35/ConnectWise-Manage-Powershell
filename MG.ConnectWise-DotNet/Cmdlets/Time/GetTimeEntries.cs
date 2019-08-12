using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Reflection;

namespace MG.ConnectWise.Cmdlets.Time
{
    [Cmdlet(VerbsCommon.Get, "TimeEntries", ConfirmImpact = ConfirmImpact.None, DefaultParameterSetName = "None")]
    public class GetTimeEntries : BaseCWCmdlet
    {
        #region FIELDS/CONSTANTS
        private const string EP = "/time/entries";
        private string _ep = EP;
        protected override string Endpoint => _ep;
        private bool _nonDefaultEnd = false;
        private string _sd;
        private string _ed = DateTime.Now.ToString(CWContext.JSON_DT_FORMAT);

        #endregion

        #region PARAMETERS
        [Parameter(Mandatory = true, ParameterSetName = "ByMemberIdWithoutDateRange")]
        [Parameter(Mandatory = true, ParameterSetName = "ByMemberIdWithDateRange")]
        public int MemberId { get; set; }

        [Parameter(Mandatory = true, ParameterSetName = "ByMemberNameWithoutDateRange")]
        [Parameter(Mandatory = true, ParameterSetName = "ByMemberNameWithDateRange")]
        [SupportsWildcards]
        public string MemberName { get; set; }

        //[Parameter(Mandatory = false)]
        //public 

        [Parameter(Mandatory = true, ParameterSetName = "ByMemberNameWithDateRange")]
        [Parameter(Mandatory = true, ParameterSetName = "ByMemberIdWithDateRange")]
        public DateTime StartDate
        {
            get => DateTime.Parse(_sd);
            set => _sd = value.ToString(CWContext.JSON_DT_FORMAT);
        }

        [Parameter(Mandatory = false, ParameterSetName = "ByMemberNameWithDateRange")]
        [Parameter(Mandatory = false, ParameterSetName = "ByMemberIdWithDateRange")]
        public DateTime EndDate
        {
            get => DateTime.Parse(_ed);
            set
            {
                _ed = value.ToString(CWContext.JSON_DT_FORMAT);
                _nonDefaultEnd = true;
            }
        }

        #endregion

        #region CMDLET PROCESSING
        protected override void BeginProcessing() => base.BeginProcessing();

        protected override void ProcessRecord()
        {
            List<string> conditions = new List<string>(5);
            if (this.ParameterSetName != "None")
            {
                if (this.MyInvocation.BoundParameters.ContainsKey("MemberName"))
                {
                    string memIden = "member/identifier{0}\"{1}\"";
                    string op = "=";
                    if (WildcardPattern.ContainsWildcardCharacters(this.MemberName))
                    {
                        op = " like ";
                    }

                    conditions.Add(string.Format(memIden, op, this.MemberName));
                }
                else if (this.MyInvocation.BoundParameters.ContainsKey("MemberId"))
                { 
                    conditions.Add(string.Format("member/id={0}", this.MemberId));
                }

                if (this.MyInvocation.BoundParameters.ContainsKey("StartDate"))
                {
                    conditions.Add(string.Format("timeStart>={0}", _sd));
                }

                if (_nonDefaultEnd)
                {
                    conditions.Add(string.Format("timeEnd<={0}", _ed));
                }   
            }

            if (conditions.Count > 0)
            {
                _ep = EP + "?conditions=" + string.Join("%20and%20", conditions);
            }

            string jsonRes = base.TryGet(_ep);
            if (!string.IsNullOrEmpty(jsonRes))
            {
                List<TimeEntry> timeEntries = Conversion.ToCwResults<TimeEntry>(jsonRes);
                base.WriteObject(timeEntries, true);
            }
        }

        #endregion

        #region BACKEND METHODS


        #endregion
    }
}