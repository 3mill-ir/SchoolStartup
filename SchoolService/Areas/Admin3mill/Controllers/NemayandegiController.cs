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
    public class NemayandegiController : Controller
    {
        [PageTittleAttributeActionFilter(Function = "Nemayandegi_ListNemayandegi")]
        public ActionResult ListNemayandegi()
        {
            ViewBag.jsNotifyMessage = TempData["Notification"];
            NemayandegiManagement sm = new NemayandegiManagement();
            return View(sm.ListNemayandegi());
        }

        [PageTittleAttributeActionFilter(Function = "Nemayandegi_AddNemayandegi")]
        public ActionResult AddNemayandegi()
        {
            SCEntities db = new SCEntities();
            ViewBag.StateList = new SelectList(db.AddressState.Where(u => u.isDelete == false).Select(u => new { Value = u.Id, Text = u.Name }), "Value", "Text");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PageTittleAttributeActionFilter(Function = "Nemayandegi_AddNemayandegi")]
        public ActionResult AddNemayandegi(Nemayandegi model, string Username, string Password, string ConfirmPassword)
        {
            SCEntities db = new SCEntities();
            ViewBag.StateList = new SelectList(db.AddressState.Where(u => u.isDelete == false).Select(u => new { Value = u.Id, Text = u.Name }), "Value", "Text");
            NemayandegiManagement mm = new NemayandegiManagement();

            string result = mm.AddNemayandegi(model, Username, Password, ConfirmPassword, ModelState);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListNemayandegi", "Nemayandegi");
            }
            else
            {
                ViewBag.jsNotifyMessage = result;
                return View(model);
            }
        }

        [PageTittleAttributeActionFilter(Function = "Nemayandegi_EditNemayandegi")]
        public ActionResult EditNemayandegi(int NemayandegiId)
        {
            SCEntities db = new SCEntities();


            NemayandegiManagement mm = new NemayandegiManagement();
            var model = mm.DetailNemayandegi(NemayandegiId);
            if (model != null)
            {
                ViewBag.NemayandegiUsername = Tools.F_UserName(model.F_UserID);
                ViewBag.StateList = new SelectList(db.AddressState.Select(u => new { Value = u.Id, Text = u.Name }), "Value", "Text", model.AddressCity.F_StateId);
                return View(model);
            }
            else
                return View("NotFound");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PageTittleAttributeActionFilter(Function = "Nemayandegi_EditNemayandegi")]
        public ActionResult EditNemayandegi(Nemayandegi model)
        {
            SCEntities db = new SCEntities();

            NemayandegiManagement sm = new NemayandegiManagement();
            model.AddressCity.Id = model.F_CityId ?? default(int);
            string result = sm.EditNemayandegi(model, ModelState);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListNemayandegi", "Nemayandegi");
            }
            else
            {
                ViewBag.StateList = new SelectList(db.AddressState.Select(u => new { Value = u.Id, Text = u.Name }), "Value", "Text", model.AddressCity.F_StateId);
                ViewBag.NemayandegiUsername = Tools.F_UserName(model.F_UserID);
                ViewBag.jsNotifyMessage = result;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult ChangeDisplayNemayandegi(int NemayandegiId)
        {
            NemayandegiManagement sm = new NemayandegiManagement();
            string result = sm.ChangeStatusNemayandegi(NemayandegiId);

            TempData["Notification"] = result;
            return RedirectToAction("ListNemayandegi", "Nemayandegi");
        }

        [HttpPost]
        public ActionResult DeleteNemayandegi(int NemayandegiId)
        {
            NemayandegiManagement sm = new NemayandegiManagement();
            string result = sm.DeleteNemayandegi(NemayandegiId);
            TempData["Notification"] = result;
            return RedirectToAction("ListNemayandegi", "Nemayandegi");
        }


        [PageTittleAttributeActionFilter(Function = "Nemayandegi_DetailNemayandegi")]
        public ActionResult DetailNemayandegi(int NemayandegiId)
        {
            NemayandegiManagement sm = new NemayandegiManagement();
            var model = sm.DetailNemayandegi(NemayandegiId);
            if (model != null)
            {
                @ViewBag.MenuAccessNemyandegiAlias = "Admin";
                ViewBag.NemayandegiId = NemayandegiId;
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                ViewBag.Username = Tools.F_UserName(model.F_UserID);
                return View(model);
            }
            else return View("NotFound");


        }
    }
}