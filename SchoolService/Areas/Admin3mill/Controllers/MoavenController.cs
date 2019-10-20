using SchoolService.Areas.Admin3mill.Models;
using SchoolService.Models.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolService.Models.DataModel;
using SchoolService.Models.DAL;
using SchoolService.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SchoolService.CustomFilters;

namespace SchoolService.Areas.Admin3mill.Controllers
{
    [AuthLog(Roles = "Admin,Nemayandegi,Modir")]
    public class MoavenController : Controller
    {
        public MoavenController()
        {
            ViewBag.MenuAccessNemyandegiAlias = "Admin";
            ViewBag.MenuAccessmadreseAlias = "Admin";

        }

        //private int? NemayandegiIdPrepare(int? NemayandegiId)
        //{
        //    int? Id;
        //    if (User.IsInRole("Nemayandegi"))
        //    {
        //        Id = SchoolService.Areas.Admin3mill.Models.Tools.NemayandegiId_Current();

        //    }
        //    else if (User.IsInRole("Modir"))
        //    {
        //        Id = SchoolService.Areas.Admin3mill.Models.Tools.NemayandegiId_CurrentModir(SchoolService.Areas.Admin3mill.Models.Tools.ModirParrent_Current());
        //    }
        //    else { Id = NemayandegiId; }
        //    ViewBag.NemayandegiId = Id;
        //    return Id;
        //}
        //private int? ModirIdPrepare(int? ModirId,out string ParrentId)
        //{
        //    int? Id;
        //    if (User.IsInRole("Modir"))
        //    {
        //        Id = SchoolService.Areas.Admin3mill.Models.Tools.ModirId_Current();
        //        ParrentId = Tools.F_UserID();
        //    }
        //    else if (User.IsInRole("Nemayandegi"))
        //    {
        //        if (SchoolService.Areas.Admin3mill.Models.Tools.ModirParrent(ModirId ?? default(int)) == SchoolService.Areas.Admin3mill.Models.Tools.F_UserID())
        //        {
        //            Id = ModirId;
        //            ParrentId = SchoolService.Areas.Admin3mill.Models.Tools.ModirParrent(ModirId ?? default(int));
        //        }
        //        else
        //        {
        //            ParrentId = null;
        //            Id = null;
        //        }

        //    }
        //    else {
        //        Id = ModirId;
        //        ParrentId = SchoolService.Areas.Admin3mill.Models.Tools.F_UserID_Modir(ModirId ?? default(int));
        //    }
        //    ViewBag.ModirId = Id;
        //    return Id;
        //}

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Moaven_ListMoaven")]
        public ActionResult ListMoaven(int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            ViewBag.jsNotifyMessage = TempData["Notification"];
            MoavenManagement sm = new MoavenManagement();
            return View(sm.ListMoaven(HttpContext.Items["ParrentId"] as string));
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Moaven_AddMoaven")]
        public ActionResult AddMoaven(int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Moaven_AddMoaven")]
        public ActionResult AddMoaven(Karmandaan model, int? NemayandegiId, int? ModirId, string Username, string Password, string ConfirmPassword)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            if (!String.IsNullOrEmpty(model.Mobile))
                model.Mobile = '0' + model.Mobile.Replace(")", "").Replace("(", "").Replace("-", "").Replace(" ", "");
            MoavenManagement mm = new MoavenManagement();
            model.F_ParrentID = HttpContext.Items["ParrentId"] as string;
            model.UserInformation.F_MadaaresID = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            int? Id;
            string result = mm.AddMoaven(model, Username, Password, ConfirmPassword, ModelState, out Id);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListMoaven", "Moaven", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            ViewBag.jsNotifyMessage = result;
            return View(model);
        }
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Moaven_EditMoaven")]
        public ActionResult EditMoaven(int? NemayandegiId, int? ModirId, int MoavenId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            MoavenManagement mm = new MoavenManagement();
            var model = mm.DetailMoaven(MoavenId, HttpContext.Items["ParrentId"] as string);
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.Mobile))
                    model.Mobile = model.Mobile.Substring(1);
                ViewBag.MoavenUsername = Tools.F_UserName(model.F_UserInfromation);
                return View(model);
            }
            else
            {
                return View("NotFound");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Moaven_EditMoaven")]
        public ActionResult EditMoaven(Karmandaan model, int? NemayandegiId, int? ModirId, int MoavenId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            MoavenManagement sm = new MoavenManagement();
            var temp = sm.DetailMoaven(model.ID, HttpContext.Items["ParrentId"] as string);
            if (temp == null)
            {
                return View("NotFound");
            }
            model.Mobile = "0" + model.Mobile.Replace(")", "").Replace("(", "").Replace("-", "").Replace(" ", "");

            string result = sm.EditMoaven(model);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListMoaven", "Moaven", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            else
            {
                ViewBag.jsNotifyMessage = result;
                ViewBag.MoavenUsername = Tools.F_UserName(model.F_UserInfromation);
                return View(model);
            }
        }
        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult ChangeDisplayMoaven(int? NemayandegiId, int? ModirId, int MoavenId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            MoavenManagement sm = new MoavenManagement();
            string result = sm.ChangeStatusMoaven(MoavenId, HttpContext.Items["ParrentId"] as string);
            TempData["Notification"] = result;
            return RedirectToAction("ListMoaven", "Moaven", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
        }

        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult DeleteMoaven(int? NemayandegiId, int? ModirId, int MoavenId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            MoavenManagement sm = new MoavenManagement();
            string result = sm.DeleteMoaven(MoavenId, HttpContext.Items["ParrentId"] as string);
            TempData["Notification"] = result;
            if (result == "success")
            {
                return RedirectToAction("ListMoaven", "Moaven", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            else
            {
                return RedirectToAction("DetailMoaven", "Moaven", new { NemayandegiId = NemayandegiId, ModirId = ModirId, MoavenId = MoavenId });
            }

        }


        [PageTittleAttributeActionFilter(Function = "Moaven_DetailMoaven")]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult DetailMoaven(int? NemayandegiId, int? ModirId, int MoavenId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            MoavenManagement sm = new MoavenManagement();
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var model = sm.DetailMoaven(MoavenId, HttpContext.Items["ParrentId"] as string);
            if (model == null)
            {
                return View("NotFound");
            }
            ViewBag.Username = UserManager.FindById(model.F_UserInfromation).UserName;
            return View(model);
        }
    }
}