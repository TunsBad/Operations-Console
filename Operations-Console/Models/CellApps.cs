using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Operations_Console.Models
{
    public class CellApps
    {
        public string ShortCode { get; set; }
        public string ApplicationType { get; set; }
        public double ExecutionTime { get; set; }
        public bool IsSuccessful { get; set; }
        public string ServiceName { get; set; }
        public string ColourCode { get; set; }
        public string TextColor { get; set; }
        public string FailureReason { get; set; }
        public string IpAddress { get; set; }
        public string CellAppsName { get; set; }
    }
}