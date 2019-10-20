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
    public class ModirController : Controller
    {
        public ModirController()
        {
            ViewBag.MenuAccessNemyandegiAlias = "Admin";
        }
        private int? NemayandegiIdPrepare(int? NemayandegiId)
        {
            int? Id;
            if (User.IsInRole("Nemayandegi"))
            {
                Id = Tools.NemayandegiId_Current();

            }
            else
            {
                Id = NemayandegiId;
            }
            ViewBag.NemayandegiId = Id;
            return Id;
        }
        [PageTittleAttributeActionFilter(Function = "Modir_ListModir")]
        [NemayandegiIdPrepareAttributeActionFilter]
        public ActionResult ListModir(int? NemayandegiId)
        {
            string ParrentID;
            if (User.IsInRole("Admin"))
            {
                ParrentID = Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int));
            }
            else
            {
                ParrentID = Tools.F_UserID();
            }

            ViewBag.jsNotifyMessage = TempData["Notification"];
            ModirManagement sm = new ModirManagement();
            return View(sm.ListModir(ParrentID));
        }
        //[ChildActionOnly]
        //public ActionResult preAddModir(int MadreseId)
        //{
        //    @ViewBag.MadreseId = MadreseId;
        //    return PartialView();
        //}
        [PageTittleAttributeActionFilter(Function = "Modir_AddModir")]
        [NemayandegiIdPrepareAttributeActionFilter]
        public ActionResult AddModir(int MadreseId, int? NemayandegiId)
        {
            string ParrentID;
            if (User.IsInRole("Admin"))
            {
                ParrentID = Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int));
            }
            else
            {
                ParrentID = Tools.F_UserID();
            }
            MadaresManagement mg = new MadaresManagement();
            if (mg.hasModir(MadreseId, ParrentID) != null)
            {
                return View("NotFound");
            }
            ViewBag.jsNotifyMessage = TempData["Notification"];
            @ViewBag.MadreseId = MadreseId;
            MadaresManagement mm = new MadaresManagement();
            if (mm.DetailMadares(MadreseId, Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int))) != null)
            {
                return View();
            }
            else
            {
                return View("NotFound");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PageTittleAttributeActionFilter(Function = "Modir_AddModir")]
        [NemayandegiIdPrepareAttributeActionFilter]
        public ActionResult AddModir(Karmandaan model, string Username, string Password, string ConfirmPassword, int MadreseId, int? NemayandegiId)
        {
            string ParrentID;
            if (User.IsInRole("Admin"))
            {
                ParrentID = Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int));
            }
            else
            {
                ParrentID = Tools.F_UserID();
            }
            MadaresManagement mg = new MadaresManagement();
            if (mg.hasModir(MadreseId, ParrentID) != null)
            {
                return View("NotFound");
            }
            model.F_ParrentID = ParrentID;
            ModirManagement mm = new ModirManagement();
            model.Mobile = '0' + model.Mobile.Replace(")", "").Replace("(", "").Replace("-", "").Replace(" ", "");
            int? modirId;
            string result = mm.AddModir(model, Username, Password, ConfirmPassword, ModelState, out modirId);
            if (result == "error")
            {
                ViewBag.jsNotifyMessage = result;
                return View(model);
            }
            result = mm.AssignModirToMadrese(modirId ?? default(int), MadreseId);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("DetailMadares", "Madares", new { MadaresId = MadreseId, NemayandegiId = NemayandegiId });
            }
            else
            {
                ViewBag.jsNotifyMessage = result;
                return View(model);
            }
        }

        //[PageTittleAttributeActionFilter(Function = "Modir_EditModir")]
        //public ActionResult EditModir(int ModirId, int? NemayandegiId)
        //{
        //    NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
        //    string ParrentID;
        //    if (User.IsInRole("Admin"))
        //    {
        //        ParrentID = Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int));
        //    }
        //    else
        //    {
        //        ParrentID = Tools.F_UserID();
        //    }
        //    ModirManagement mm = new ModirManagement();
        //    var model = mm.DetailModir(ModirId, ParrentID);
        //    if (model != null)
        //    {
        //        model.Mobile = model.Mobile.Substring(1);
        //        ViewBag.ModirUsername = Tools.F_UserName(model.F_UserInfromation);
        //        return View(model);
        //    }
        //    else
        //    {
        //        return View("NotFound");
        //    }
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[PageTittleAttributeActionFilter(Function = "Modir_EditModir")]
        //public ActionResult EditModir(Karmandaan model, int? NemayandegiId)
        //{
        //    NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
        //    string ParrentID;
        //    if (User.IsInRole("Admin"))
        //    {
        //        ParrentID = Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int));
        //    }
        //    else
        //    {
        //        ParrentID = Tools.F_UserID();
        //    }
        //    ModirManagement mm = new ModirManagement();
        //    var found = mm.DetailModir(model.ID, ParrentID);
        //    if (found == null)
        //    {
        //        return View("NotFound");
        //    }
        //    model.Mobile = "0" + model.Mobile.Replace(")", "").Replace("(", "").Replace("-", "").Replace(" ", "");
        //    string result = mm.EditModir(model);
        //    if (result == "success")
        //    {
        //        TempData["Notification"] = "success";
        //        return RedirectToAction("ListModir", "Modir", new { NemayandegiId = NemayandegiId });
        //    }
        //    else
        //    {
        //        ViewBag.jsNotifyMessage = result;
        //        return View(model);
        //    }
        //}
        //[HttpPost]
        //public ActionResult ChangeDisplayModir(int ModirId, int? NemayandegiId)
        //{
        //    ModirManagement sm = new ModirManagement();
        //    if (sm.ChangeStatusModir(ModirId) == "NotFound")
        //        TempData["Notification"] = "error";
        //    else
        //        TempData["Notification"] = "success";
        //    return RedirectToAction("ListModir", "Modir");
        //}
        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        public ActionResult DeleteModir(int ModirId, int? NemayandegiId)
        {
            return View("NotFound");
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //string ParrentID;
            //if (User.IsInRole("Admin"))
            //{
            //    ParrentID = Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int));
            //}
            //else
            //{
            //    ParrentID = Tools.F_UserID();
            //}
            //int? madreseId;
            //ModirManagement sm = new ModirManagement();
            //string result = sm.DeleteModir(ModirId, ParrentID, out madreseId);
            //TempData["Notification"] = result;
            //return RedirectToAction("DetailMadares", "Madares", new { MadaresId = madreseId, NemayandegiId = NemayandegiId });

        }


        [ChildActionOnly]
        //[PageTittleAttributeActionFilter(Function = "Modir_DetailModir")]
        public ActionResult DetailModir(int ModirId, string ParrentId)
        {
            ModirManagement sm = new ModirManagement();
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var model = sm.DetailModir(ModirId, ParrentId);
            ViewBag.Username = UserManager.FindById(model.F_UserInfromation).UserName;
            return PartialView(model);
        }
    }
}