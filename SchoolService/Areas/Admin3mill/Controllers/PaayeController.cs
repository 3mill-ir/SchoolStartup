using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SchoolService.Areas.Admin3mill.Models;
using SchoolService.CustomFilters;
using SchoolService.Models;
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
    public class PaayeController : Controller
    {
        [PageTittleAttributeActionFilter(Function = "Paaye_ListPaaye")]
        public ActionResult ListPaaye(int? MaghaateId)
        {
            ViewBag.MaghaateId = MaghaateId;
            ViewBag.jsNotifyMessage = TempData["Notification"];
            PaayeManagement sm = new PaayeManagement();
            return View(sm.ListPaaye(MaghaateId));
        }
        [PageTittleAttributeActionFilter(Function = "Paaye_AddPaaye")]
        public ActionResult AddPaaye(int? MaghaateId)
        {
            MaghaateManagement mg = new MaghaateManagement();
            ViewBag.Maghaate = mg.MaghaateCombo(MaghaateId);

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PageTittleAttributeActionFilter(Function = "Paaye_AddPaaye")]
        public ActionResult AddPaaye(Paaye model, int? MaghaateId)
        {
            PaayeManagement mm = new PaayeManagement();
            string result = mm.AddPaaye(model, ModelState);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListPaaye", "Paaye", new { MaghaateId = MaghaateId });
            }
            else
            {
                ViewBag.jsNotifyMessage = result;
                MaghaateManagement mg = new MaghaateManagement();
                ViewBag.Maghaate = mg.MaghaateCombo(MaghaateId);
                return View(model);
            }
        }
        [PageTittleAttributeActionFilter(Function = "Paaye_EditPaaye")]
        public ActionResult EditPaaye(int PaayeId, int? MaghaateId)
        {

            PaayeManagement mm = new PaayeManagement();
            MaghaateManagement mg = new MaghaateManagement();
            var model = mm.DetailPaaye(PaayeId);
            if (model != null)
            {
                ViewBag.Maghaate = mg.MaghaateCombo(model.F_MaghaateID);
                return View(model);
            }
            else return View("NotFound");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PageTittleAttributeActionFilter(Function = "Paaye_EditPaaye")]
        public ActionResult EditPaaye(Paaye model, int? MaghaateId)
        {

            PaayeManagement sm = new PaayeManagement();
            string result = sm.EditPaaye(model, ModelState);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListPaaye", "Paaye", new { MaghaateId = MaghaateId });
            }
            else
            {
                ViewBag.jsNotifyMessage = result;
                MaghaateManagement mm = new MaghaateManagement();
                ViewBag.Maghaate = mm.MaghaateCombo(model.F_MaghaateID);
                return View(model);
            }
        }


        [HttpPost]
        public ActionResult DeletePaaye(int PaayeId, int? MaghaateId)
        {
            PaayeManagement sm = new PaayeManagement();
            string result = sm.DeletePaaye(PaayeId);

                TempData["Notification"] = result;
  
            return RedirectToAction("ListPaaye", "Paaye", new { MaghaateId = MaghaateId });
        }

        [PageTittleAttributeActionFilter(Function = "Paaye_DetailPaaye")]
        public ActionResult DetailPaaye(int PaayeId, int? MaghaateId)
        {
            @ViewBag.MaghaateId = MaghaateId;
            PaayeManagement sm = new PaayeManagement();
            var model = sm.DetailPaaye(PaayeId);
            if (model != null)
            {
                return View(model);
            }
            else return View("NotFound");
        }
    }
}