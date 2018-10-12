using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Operations_Console.Models;
using Operations_Console.DBHelper;

namespace Operations_Console.API
{
    public class ServerController : ApiController
    {
        private double memoryWarningMinimum, memoryWarningMaximum, cpuWarningMinimum, cpuWarningMaximum,
            memoryCriticalMinimum, memoryCriticalMaximum, cpuCriticalMinimum, cpuCriticalMaximum, diskSpaceMinimum;

        public ServerController()
        {
            memoryWarningMinimum = double.Parse(System.Configuration.ConfigurationManager.AppSettings["MEMORY_WARNING_MINIMUM"]);
            memoryWarningMaximum = double.Parse(System.Configuration.ConfigurationManager.AppSettings["MEMORY_WARNING_MAXIMUM"]);
            cpuWarningMinimum = double.Parse(System.Configuration.ConfigurationManager.AppSettings["CPU_WARNING_MINIMUM"]);
            cpuWarningMaximum = double.Parse(System.Configuration.ConfigurationManager.AppSettings["CPU_WARNING_MAXIMUM"]);
            memoryCriticalMinimum = double.Parse(System.Configuration.ConfigurationManager.AppSettings["MEMORY_CRITICAL_MINIMUM"]);
            memoryCriticalMaximum = double.Parse(System.Configuration.ConfigurationManager.AppSettings["MEMORY_CRITICAL_MAXIMUM"]);
            cpuCriticalMinimum = double.Parse(System.Configuration.ConfigurationManager.AppSettings["CPU_CRITICAL_MINIMUM"]);
            cpuCriticalMaximum = double.Parse(System.Configuration.ConfigurationManager.AppSettings["CPU_CRITICAL_MAXIMUM"]);
            diskSpaceMinimum = double.Parse(System.Configuration.ConfigurationManager.AppSettings["DISK_SPACE_MINIMUM"]);
        }
         
        [HttpPost]
        public KendoResponse GetServerReport([FromBody] KendoRequest req, int serverid)
        {
            List<ServerReport> report = new List<ServerReport>() { };
            report = DbHelper.Instance.GetServerReport(serverid);

            var query = report.AsQueryable();
            var data = query.ToArray();

            return new KendoResponse(data, query.Count());
        }

        public AjaxResponse GetServerStatistic(List<Server> servers)
        {

            List<ServerDetails> serverdetails = new List<ServerDetails>();
            ServerStat serverstatistics = new ServerStat();
            AjaxResponse serverdata = new AjaxResponse();

            int criticalErrorCount = 0;
            int diskErrorCount = 0;
            int warningErrorCount = 0;

            try
            {
                if (servers == null)
                {
                    servers = DbHelper.Instance.GetServers();
                }
                
                foreach (var serverinfo in servers)
                {
                    Status status = new Status { };
                    switch(serverinfo.ServerType)
                    {
                        case "api":
                            status.DbType = "/Content/api.png";
                            break;
                        case "app":
                            status.DbType = "/Content/tower.png";
                            break;
                        case "web":
                            status.DbType = "/Content/web.png";
                            break;
                        case "db":
                            status.DbType = "/Content/database.png";
                            break;
                        case "pulse":
                            status.DbType = "/Content/pulse.jpg";
                            break;
                        case "beng":
                            status.DbType = "/Content/processing.jpg";
                            break;
                        case "bkp":
                            status.DbType = "/Content/backup.jpg";
                            break;
                        default:
                            status.DbType = "/Content/database.png";
                            break;
                    };

                    serverstatistics = GetServerStats(serverinfo.Id);
                    status.Errors = "";

                    if ((serverstatistics.MemoryUsagePercent < memoryWarningMinimum) && (serverstatistics.CpuUsagePercent < cpuWarningMinimum) == true)
                    {
                        status.Color = "green";
                        status.TextColor = "white";
                        status.Errors ="";
                    } 
                    if ((serverstatistics.MemoryUsagePercent > memoryWarningMinimum && serverstatistics.MemoryUsagePercent < memoryWarningMaximum) || (serverstatistics.CpuUsagePercent > cpuWarningMinimum && serverstatistics.CpuUsagePercent < cpuWarningMaximum) == true)
                    {
                        status.Color = "yellow";
                        status.TextColor = "black";
                        if (serverstatistics.MemoryUsagePercent > memoryWarningMinimum)
                        {
                            status.Errors += ( string.IsNullOrEmpty(status.Errors) ? "" : ", " ) + "Memory Warning";
                        }
                        if (serverstatistics.CpuUsagePercent > cpuWarningMinimum)
                        {
                            status.Errors += ( string.IsNullOrEmpty(status.Errors) ? "" : ", " ) + "CPU Warning";
                        }

                        warningErrorCount += 1;

                    }
                    if (DoesStatHaveDiskIssue(serverstatistics.DiskData))
                    {
                        status.Color = "Orange";
                        status.TextColor = "black";
                        status.Errors += (string.IsNullOrEmpty(status.Errors) ? "" : ", ") + "Disk Space Issue";

                        diskErrorCount += 1;

                    }
                    if (serverstatistics.CpuUsagePercent > cpuCriticalMinimum || serverstatistics.MemoryUsagePercent > memoryCriticalMinimum)
                    {
                        status.Color = "red";
                        status.TextColor = "white";
                        if (serverstatistics.MemoryUsagePercent > memoryCriticalMinimum)
                        {
                            status.Errors += (string.IsNullOrEmpty(status.Errors) ? "" : ", ") + "Memory Critical";
                        }
                        if (serverstatistics.CpuUsagePercent > cpuCriticalMinimum)
                        {
                            status.Errors += (string.IsNullOrEmpty(status.Errors) ? "" : ", ") + "CPU Critical";
                        }

                        criticalErrorCount += 1;
                    }
                    if ((DateTime.Now - serverstatistics.Time).TotalMinutes > 10) {
                        serverstatistics.TimeDisplayClass = "statsoldtime";
                    } else
                    {
                        serverstatistics.TimeDisplayClass = "";
                    }

                    serverdetails.Add(new ServerDetails
                    {
                        ServerInfor = serverinfo,
                        ServerStatistic = serverstatistics,
                        Status = status
                    });

                }

                serverdata.Serverdetails = serverdetails;
                serverdata.CriticalErrorCount = criticalErrorCount;
                serverdata.DiskErrorCount = diskErrorCount;
                serverdata.WarningErrorCount = warningErrorCount;

                return serverdata;

            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return new AjaxResponse()
                {

                };
            }
        }

        private ServerStat GetServerStats(int serverid)
        {
            ServerStat ServerStatistics = new ServerStat { };
            try
            {
                ServerStatistics = DbHelper.Instance.GetServerStats(serverid);

                return ServerStatistics;

            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return new ServerStat { };
            }

        }

        private double GetPercentageFreeForDiskData(string diskdata)
        {
            var indexOfOpeningBracket = diskdata.IndexOf("(");
            var indexOfClosingBracket = diskdata.IndexOf(")", indexOfOpeningBracket + 1);
            if(indexOfOpeningBracket < 0 || indexOfClosingBracket < 0)
            {
                return 100;
            }
            var percentFreeString = diskdata.Substring(indexOfOpeningBracket + 1, indexOfClosingBracket - indexOfOpeningBracket - 2);
            return double.Parse(percentFreeString);
        }

        private bool DoesStatHaveDiskIssue(List<string> diskdata)
        {
            if (diskdata == null) return false;
            foreach (var diskitem in diskdata)
            {
                if (GetPercentageFreeForDiskData(diskitem) < diskSpaceMinimum)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
