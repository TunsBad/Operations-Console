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
    public class AppStatusController : ApiController
    {
        [HttpGet]
        public List<CellApps> GetAllAppsStatus()
        {
            CellApps appstatus = new CellApps { };
            List<CellApps> allAppsStatus = new List<CellApps>() { };

            var apps = DbHelper.Instance.GetCellApps();

            foreach(var app in apps)
            {
                appstatus = DbHelper.Instance.GetCellAppStatus(app.ShortCode);
                if (appstatus == null) continue;

                if (appstatus.IsSuccessful && appstatus.FailureReason == "None")
                {
                    appstatus.ColourCode = "green";
                    appstatus.TextColor = "white";
                }
                if (appstatus.IsSuccessful && appstatus.FailureReason == "Time Out")
                {
                    appstatus.ColourCode = "yellow";
                    appstatus.TextColor = "black";
                }
                if (appstatus.IsSuccessful && appstatus.FailureReason == "Bad Response")
                {
                    appstatus.ColourCode = "red";
                    appstatus.TextColor = "white";
                }
                if (appstatus. IsSuccessful == false)
                {
                    appstatus.ColourCode = "red";
                    appstatus.TextColor = "white";
                }

                allAppsStatus.Add(appstatus);
            }

            return allAppsStatus;
        }
    }
}
