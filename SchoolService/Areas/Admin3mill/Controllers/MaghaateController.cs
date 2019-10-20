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
    public class MaghaateController : Controller
    {
        [PageTittleAttributeActionFilter(Function = "Maghaate_ListMaghaate")]
        public ActionResult ListMaghaate()
        {
            ViewBag.jsNotifyMessage = TempData["Notification"];
            MaghaateManagement sm = new MaghaateManagement();
            return View(sm.ListMaghaate());
        }

        [PageTittleAttributeActionFilter(Function = "Maghaate_AddMaghaate")]
        public ActionResult AddMaghaate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PageTittleAttributeActionFilter(Function = "Maghaate_AddMaghaate")]
        public ActionResult AddMaghaate(Maghaate model)
        {
     
            MaghaateManagement mm = new MaghaateManagement();
            string result = mm.AddMaghaate(model, ModelState);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListMaghaate", "Maghaate");
            }
            else
            {
                ViewBag.jsNotifyMessage = result;
                return View(model);
            }
        }

        [PageTittleAttributeActionFilter(Function = "Maghaate_EditMaghaate")]
        public ActionResult EditMaghaate(int MaghaateId)
        {
            MaghaateManagement mm = new MaghaateManagement();
            var model = mm.DetailMaghaate(MaghaateId);
            if (model != null)
            {
                return View(model);
            }
            else return View("NotFound");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PageTittleAttributeActionFilter(Function = "Maghaate_EditMaghaate")]
        public ActionResult EditMaghaate(Maghaate model)
        {
            MaghaateManagement mm = new MaghaateManagement();
            string result = mm.EditMaghaate(model, ModelState);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListMaghaate", "Maghaate");
            }
            else
            {
                ViewBag.jsNotifyMessage = result;
                return View(model);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMaghaate(int MaghaateId)
        {
            MaghaateManagement sm = new MaghaateManagement();
            string result=sm.DeleteMaghaate(MaghaateId);

                TempData["Notification"] = result;

            return RedirectToAction("ListMaghaate", "Maghaate");
        }

        [PageTittleAttributeActionFilter(Function = "Maghaate_DetailMaghaate")]
        public ActionResult DetailMaghaate(int MaghaateId)
        {
            MaghaateManagement mm = new MaghaateManagement();
            var model = mm.DetailMaghaate(MaghaateId);
            if (model != null)
            {
                return View(model);
            }
            else return View("NotFound");
        }
    }
}