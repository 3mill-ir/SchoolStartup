using SchoolService.Areas.Admin3mill.Models;
using SchoolService.CustomFilters;
using SchoolService.Models.BLL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { area = "admin3mill" });

            }
            else
            {
                return RedirectToAction("login", "account", new { area = "admin3mill" });

            }
        }

        //[PageTittleAttributeActionFilter(Function="dd")]
        //public ActionResult Index()
        //{
        //    System.Resources.ResourceManager rm = Resource.Resource.ResourceManager;
        //    string someString = rm.GetString("ApplicationHelp");
        //    NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("PagetittleSection");
        //    var p = section["TaklifeKelassid"];


        //    var z = p;
        //    return View();
        //}

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}

        [Authorize]
        [HttpPost]
        public string TaklifeKelasi(int DaneshAmoozId, int BarnameHaftegiId, string Date, int Hafte, string OnvaneFile, string TozihateFile, HttpPostedFileBase File,string Type)
        {
            var asm = new AndroidServicesManagement();
            return asm.SetTaklifeKelasi(DaneshAmoozId, BarnameHaftegiId, Date, Hafte, OnvaneFile, TozihateFile, File,Type);
        }
    }
}