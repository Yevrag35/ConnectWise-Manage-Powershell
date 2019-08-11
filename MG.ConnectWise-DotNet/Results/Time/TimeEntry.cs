using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MG.ConnectWise
{
    public class TimeEntry : BaseResult
    {
        public decimal ActualHours { get; set; }
        public bool AddToDetailDescriptionFlag { get; set; }
        public bool AddToInternalAnalysisFlag { get; set; }
        public bool AddToResolutionFlag { get; set; }
        public string BillableOption { get; set; }
        public int BusinessUnitId { get; set; }
        public int ChargeToId { get; set; }
        public string ChargeToType { get; set; }
        public Dictionary<string, object> Company { get; set; }
        public DateTime DateEntered { get; set; }
        public bool EmailCcFlag { get; set; }
        public bool EmailContactFlag { get; set; }
        public bool EmailResourceFlag { get; set; }
        public string EnteredBy { get; set; }
        public decimal HoursBilled { get; set; }
        public decimal HoursDeduct { get; set; }
        public decimal HourlyRate { get; set; }
        public int LocationId { get; set; }
        public Dictionary<string, object> Member { get; set; }
        public Guid MobileGuid { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        [JsonProperty("timeEnd")]
        public DateTime TimeEntryEnd { get; set; }
        [JsonProperty("id")]
        public int TimeEntryId { get; set; }
        [JsonProperty("timeStart")]
        public DateTime TimeEntryStart { get; set; }
        public Dictionary<string, object> TimeSheet { get; set; }
        public Dictionary<string, object> WorkRole { get; set; }
        public Dictionary<string, object> WorkType { get; set; }
    }
}
