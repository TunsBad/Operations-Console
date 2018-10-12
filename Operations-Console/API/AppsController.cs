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
    public class AppsController : ApiController
    {

        [HttpGet]
        public List<Apps> GetApps()
        {

            List<Apps> apps = new List<Apps>();
            AjaxResponse serverdetails = new AjaxResponse();
            ServerController statisticgatherer = new ServerController();

            try
            {
                apps = DbHelper.Instance.GetServerApps();
                apps.OrderByDescending(x => x.AppId);

                foreach (var app in apps)
                {
                    serverdetails = GetAppServerStatistic(app.AppId);

                    app.CriticalErrorCount = serverdetails.CriticalErrorCount;
                    app.DiskErrorCount = serverdetails.DiskErrorCount;
                    app.WarningErrorCount = serverdetails.WarningErrorCount;
                }

                apps.Add(new Apps {
                    CriticalErrorCount = statisticgatherer.GetServerStatistic(null).CriticalErrorCount,
                    DiskErrorCount = statisticgatherer.GetServerStatistic(null).DiskErrorCount,
                    WarningErrorCount = statisticgatherer.GetServerStatistic(null).WarningErrorCount
                });
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return new List<Apps>()
                {

                };
            }

            return apps;
        }

        public AjaxResponse GetAppServerStatistic(int appid)
        {
            AjaxResponse serverdetails = new AjaxResponse();
            List<Server> servers = new List<Server>();
            ServerController statisticgatherer = new ServerController();

            servers = GetAppServers(appid);
            serverdetails = statisticgatherer.GetServerStatistic(servers);

            return serverdetails;

        }

        private List<Server> GetAppServers(int appid)
        {
            List<Server> servers = new List<Server>();
            Server serverinfo = new Server ();
            List<AppNodes> appnodes = new List<AppNodes>();
            try
            {

                appnodes = DbHelper.Instance.GetAppServers(appid);
                foreach( var node in appnodes)
                {
                    serverinfo = DbHelper.Instance.GetServersInfo(node.ServerId);
                    servers.Add(serverinfo);
                };
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return new List<Server>()
                {

                };
            }

            return servers;
        }

    }
}
