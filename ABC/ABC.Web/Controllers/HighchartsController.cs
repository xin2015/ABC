using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ABC.Web.Controllers
{
    public class HighchartsController : Controller
    {
        // GET: Highcharts
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pie()
        {
            return View();
        }

        public ActionResult Spline()
        {
            return View();
        }
    }
}