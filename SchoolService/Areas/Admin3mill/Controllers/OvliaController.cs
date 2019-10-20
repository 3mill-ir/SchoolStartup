using SchoolService.Areas.Admin3mill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolService.Models.BLL;
using SchoolService.Models.DataModel;
using PagedList;
using SchoolService.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SchoolService.CustomFilters;

namespace SchoolService.Areas.Admin3mill.Controllers
{
    [AuthLog(Roles = "Admin,Nemayandegi,Modir")]
    public class OvliaController : Controller
    {

        public OvliaController()
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
            //        Id = SchoolService.Areas.Admin3mill.Models.Tools.NemayandegiId_CurrentModir(SchoolService.Areas.Admin3mill.Models.Tools.F_UserID());
            //    }
            //    else { Id = NemayandegiId; }
            //    ViewBag.NemayandegiId = Id;
            //    return Id;
            //}
            //private int? ModirIdPrepare(int? ModirId, out string ParrentId)
            //{
            //    int? Id;
            //    if (User.IsInRole("Modir"))
            //    {
            //        Id = SchoolService.Areas.Admin3mill.Models.Tools.ModirId_Current();
            //        ParrentId = Tools.F_UserID();
            //    }
            //    else if (User.IsInRole("Nemayandegi"))
            //    {
            //        if (SchoolService.Areas.Admin3mill.Models.Tools.F_UserID_Modir(ModirId ?? default(int)) == SchoolService.Areas.Admin3mill.Models.Tools.F_UserID())
            //        {
            //            Id = ModirId;
            //            ParrentId = SchoolService.Areas.Admin3mill.Models.Tools.F_UserID_Modir(ModirId ?? default(int));
            //        }
            //        else
            //        {
            //            ParrentId = null;
            //            Id = null;
            //        }

            //    }
            //    else
            //    {
            //        Id = ModirId;
            //        ParrentId = SchoolService.Areas.Admin3mill.Models.Tools.F_UserID_Modir(ModirId ?? default(int));
            //    }
            //    ViewBag.ModirId = Id;
            //    return Id;
            //}
            [NemayandegiIdPrepareAttributeActionFilter]
            [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Ovlia_ListOvlia")]
            public ActionResult ListOvlia(int? NemayandegiId, int? ModirId)
            {
                //string ParrentId;
                //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
                //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            ViewBag.jsNotifyMessage = TempData["Notification"];
            OvliaManagement sm = new OvliaManagement();
            return View(sm.ListOvlia(HttpContext.Items["ParrentId"] as string));
        }
            [NemayandegiIdPrepareAttributeActionFilter]
            [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Ovlia_AddOvlia")]
        public ActionResult AddOvlia(int? NemayandegiId, int? ModirId)
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
        [PageTittleAttributeActionFilter(Function = "Ovlia_AddOvlia")]
        public ActionResult AddOvlia(Ovlia model,int? NemayandegiId, int? ModirId, string Username, string Password, string ConfirmPassword)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            OvliaManagement mm = new OvliaManagement();
            int? Id;
            model.F_ParrentID = HttpContext.Items["ParrentId"] as string;
            model.UserInformation.F_MadaaresID = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            string result = mm.AddOvlia(model, Username, Password, ConfirmPassword, ModelState, out Id);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListOvlia", "Ovlia", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            ViewBag.jsNotifyMessage = result;
            return View(model);   
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult AddOvliaFromDaneshAmuz(Ovlia model,int? NemayandegiId, int? ModirId, string Username, string Password, string ConfirmPassword)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            OvliaManagement mm = new OvliaManagement();
            int? Id;
            model.F_ParrentID = HttpContext.Items["ParrentId"] as string;
            model.UserInformation.F_MadaaresID = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            string result = mm.AddOvlia(model, Username, Password, ConfirmPassword, ModelState, out Id);
            ViewBag.jsNotifyMessage = result;
            OvliaManagement OM = new OvliaManagement();
            ViewBag.Ovlia = OM.ComboBoxeOvlia(Tools.MadreseId(HttpContext.Items["ParrentId"] as string) ?? default(int));
            return PartialView();
        }
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Ovlia_EditOvlia")]
        public ActionResult EditOvlia(int? NemayandegiId, int? ModirId,int OvliaId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            OvliaManagement mm = new OvliaManagement();
            var model = mm.DetailOvlia(OvliaId, HttpContext.Items["ParrentId"] as string);
            if (model != null)
            {
                ViewBag.OvliaUsername = Tools.F_UserName(model.F_UserInformationID);
                if (!string.IsNullOrEmpty(model.Mobile))
                {
                    model.Mobile = model.Mobile.Substring(1);
                }
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
        [PageTittleAttributeActionFilter(Function = "Ovlia_EditOvlia")]
        public ActionResult EditOvlia(Ovlia model, int? NemayandegiId, int? ModirId,int OvliaId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            OvliaManagement sm = new OvliaManagement();
            var temp = sm.DetailOvlia(model.ID, HttpContext.Items["ParrentId"] as string);
            if (temp == null)
            {
                return View("NotFound");
            }
            model.Mobile = "0" + model.Mobile.Replace(")", "").Replace("(", "").Replace("-", "").Replace(" ", "");

            string result = sm.EditOvlia(model);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListOvlia", "Ovlia", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            else
            {
                ViewBag.OvliaUsername = Tools.F_UserName(model.F_UserInformationID);
                ViewBag.jsNotifyMessage = result;
                ViewBag.MoavenUsername = Tools.F_UserName(model.F_UserInformationID);
                return View(model);
            }
        }


        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult ChangeDisplayOvlia(int? NemayandegiId, int? ModirId,int OvliaId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            OvliaManagement sm = new OvliaManagement();
            string result = sm.ChangeStatusOvlia(OvliaId, HttpContext.Items["ParrentId"] as string);
            TempData["Notification"] = result;
            return RedirectToAction("ListOvlia", "Ovlia", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
        }

        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult DeleteOvlia(int? NemayandegiId, int? ModirId,int OvliaId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            OvliaManagement sm = new OvliaManagement();
            string result = sm.DeleteOvlia(OvliaId, HttpContext.Items["ParrentId"] as string);
            TempData["Notification"] = result;
            if (result == "success")
            {
                return RedirectToAction("ListOvlia", "Ovlia", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            else
            {
                return RedirectToAction("DetailOvlia", "Ovlia", new { NemayandegiId = NemayandegiId, ModirId = ModirId, OvliaId = OvliaId });
            }
        }
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Ovlia_DetailOvlia")]
        public ActionResult DetailOvlia(int? NemayandegiId, int? ModirId,int OvliaId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            OvliaManagement sm = new OvliaManagement();
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var model = sm.DetailOvlia(OvliaId, HttpContext.Items["ParrentId"] as string);
            ViewBag.Username = UserManager.FindById(model.F_UserInformationID).UserName;
            return View(model);
        }
    }
}