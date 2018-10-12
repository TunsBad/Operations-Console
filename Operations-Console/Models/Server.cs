using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Operations_Console.Models
{
    public class Server
    {
        public string IpAddress { get; set; }
        public string ServerName { get; set; }
        public int Id { get; set; } 
        public string ServerType { get; set; }
        public List<string> Apps { get; set; }
    }

    public class ServerStat
    {
        public DateTime Time { get; set; }
        public int ServerId { get; set; }
        public string IpAddress { get; set; }
        public double DiskUsage { get; set; }
        public double DiskUsagePercent { get; set; }
        public double CpuUsage { get; set; }
        public double CpuUsagePercent { get; set; }
        public double NetworkUsage { get; set; }
        public double NetworkUsagePercent { get; set; }
        public double MemoryUsage { get; set; }
        public double MemoryUsagePercent { get; set; }
        public List<string> DiskData { get; set; }
        public string TimeDisplayClass { get; set; }
    }

    public class ServerDetails
    {
        public Server ServerInfor { get; set; }
        public ServerStat ServerStatistic { get; set; }
        public Status Status { get; set; }
    }

    public class AjaxResponse
    {
        public List<ServerDetails> Serverdetails { get; set; }
        public int CriticalErrorCount { get; set; }
        public int DiskErrorCount { get; set; }
        public int WarningErrorCount { get; set; }
    }

    public class ServerReport
    {
        public DateTime Time { get; set; }
        public string ServerName { get; set; }
        public string IpAddress { get; set; }
        public double DiskUsage { get; set; }
        public double CpuUsage { get; set; }
        public double NetworkUsage { get; set; }
        public double MemoryUsage { get; set; }
        public List<string> DiskData { get; set; }
       
    }

    public class Status
    {
        public string Color { get; set; }
        public string TextColor { get; set; }
        public string DbType { get; set; }
        public string Errors { get; set; }
       
    }

}