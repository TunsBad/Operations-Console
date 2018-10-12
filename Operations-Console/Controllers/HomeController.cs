using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using Operations_Console.Models;
using Operations_Console.DBHelper;

namespace Operations_Console.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "ACS - OPERATIONS  CONSOLE";

            return View();

        }

        public ActionResult AppStatus()
        {
            ViewBag.Message = "ACS - OPERATIONS  CONSOLE";

            return View();
        }

        public ActionResult Viewmore(int serverid, string servername)
        {
            ViewBag.Message = servername;
            ViewBag.Id = serverid;

            return View();
        }


    }
}