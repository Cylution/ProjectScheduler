﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectScheduler.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Project Scheduler version 0.0.1";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact the Systems Team";

            return View();
        }
    }
}