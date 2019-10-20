using PagedList;
using SchoolService.Areas.Admin3mill.Models;
using SchoolService.CustomFilters;
using SchoolService.Models.BLL;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolService.Areas.Admin3mill.Controllers
{
         [AuthLog(Roles = "Admin")]
    public class DoroosController : Controller
    {
        [PageTittleAttributeActionFilter(Function = "Doroos_ListDoroos")]
        public ActionResult ListDoroos(int? PaayeId)
        {
            ViewBag.PaayeId = PaayeId;
            ViewBag.jsNotifyMessage = TempData["Notification"];
            DoroosManagement sm = new DoroosManagement();
            return View(sm.ListDoroos(PaayeId));
        }


        [PageTittleAttributeActionFilter(Function = "Doroos_AddDoroos")]
        public ActionResult AddDoroos(int? PaayeId)
        {
            int? maghtaId = null;
            PaayeManagement pm = new PaayeManagement();
            if (PaayeId != null)
            {
                var Paaye = pm.DetailPaaye(PaayeId ?? default(int));
                if (Paaye != null)
                {
                    maghtaId = Paaye.F_MaghaateID;
                }
            }
            ViewBag.Paaye = pm.PaayeCombo(maghtaId ?? default(int),PaayeId);
            MaghaateManagement mg = new MaghaateManagement();
            ViewBag.Maghaate = mg.MaghaateCombo(maghtaId);     
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "1", Value = "1" });
            listItems.Add(new SelectListItem { Text = "2", Value = "2" });
            listItems.Add(new SelectListItem { Text = "3", Value = "3" });
            ViewBag.Zarib = listItems;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PageTittleAttributeActionFilter(Function = "Doroos_AddDoroos")]
        public ActionResult AddDoroos(Doroos model, int? PaayeId)
        {
            DoroosManagement mm = new DoroosManagement();
            string result = mm.AddDoroos(model, ModelState);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListDoroos", "Doroos", new { PaayeId = PaayeId });
            }
            else
            {
                ViewBag.jsNotifyMessage = result;
                int? maghtaId = null;
                PaayeManagement pm = new PaayeManagement();
                if (PaayeId != null)
                {
                    var Paaye = pm.DetailPaaye(PaayeId ?? default(int));
                    if (Paaye != null)
                    {
                        maghtaId = Paaye.F_MaghaateID;
                    }
                }
                ViewBag.Paaye = pm.PaayeCombo(maghtaId ?? default(int), PaayeId);
                MaghaateManagement mg = new MaghaateManagement();
                ViewBag.Maghaate = mg.MaghaateCombo(maghtaId);
                var listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem { Text = "1", Value = "1" });
                listItems.Add(new SelectListItem { Text = "2", Value = "2" });
                listItems.Add(new SelectListItem { Text = "3", Value = "3" });
                ViewBag.Zarib = listItems;
                return View(model);
            }
        }


        [PageTittleAttributeActionFilter(Function = "Doroos_EditDoroos")]
        public ActionResult EditDoroos(int DoroosId, int? PaayeId)
        {
            DoroosManagement mm = new DoroosManagement();
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "1", Value = "1" });
            listItems.Add(new SelectListItem { Text = "2", Value = "2" });
            listItems.Add(new SelectListItem { Text = "3", Value = "3" });
            ViewBag.Zarib = listItems;
            var model = mm.DetailDoroos(DoroosId);
            if (model != null)
            {
                return View(model);
            }
            else return View("NotFound");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PageTittleAttributeActionFilter(Function = "Doroos_EditDoroos")]
        public ActionResult EditDoroos(Doroos model, int? PaayeId)
        {
            DoroosManagement sm = new DoroosManagement();
            string result = sm.EditDoroos(model, ModelState);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListDoroos", "Doroos", new { PaayeId = PaayeId });
            }
            else
            {
                ViewBag.jsNotifyMessage = result;
                var listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem { Text = "1", Value = "1" });
                listItems.Add(new SelectListItem { Text = "2", Value = "2" });
                listItems.Add(new SelectListItem { Text = "3", Value = "3" });
                ViewBag.Zarib = listItems;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult DeleteDoroos(int DoroosId, int? PaayeId)
        {
            DoroosManagement sm = new DoroosManagement();
            string result = sm.DeleteDoroos(DoroosId);
                TempData["Notification"] = result;

            return RedirectToAction("ListDoroos", "Doroos", new { PaayeId = PaayeId });
        }
     
             
             
             [PageTittleAttributeActionFilter(Function = "Doroos_EditDoroos")]
        public ActionResult DetailDoroos(int DoroosId, int? PaayeId)
        {
            ViewBag.PaayeId = PaayeId;
            DoroosManagement sm = new DoroosManagement();
            var model=sm.DetailDoroos(DoroosId);
            if (model != null)
            {
                return View(model);
            }
            else return View("NotFound");
        }
    }
}