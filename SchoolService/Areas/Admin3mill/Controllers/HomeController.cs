using SchoolService.CustomFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolService.Areas.Admin3mill.Controllers
{
    [AuthLog(Roles = "Admin,Nemayandegi,Modir")]

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}