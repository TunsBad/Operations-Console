using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Operations_Console.Models
{
    public class Apps
    {
        public int AppId { get; set; }
        public string Name { get; set; }
        public int CriticalErrorCount { get; set; }
        public int DiskErrorCount { get; set; }
        public int WarningErrorCount { get; set; }
    }

    public class AppNodes
    {
        public int AppNodeId { get; set; }
        public int AppId { get; set; }
        public int ServerId { get; set; }
        public string ServerType { get; set; }
    }
}