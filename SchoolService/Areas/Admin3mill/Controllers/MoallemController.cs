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
    [AuthLog(Roles = "Admin,Nemayandegi,Modir")]
    public class MoallemController : Controller
    {
        public MoallemController()
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
        [PageTittleAttributeActionFilter(Function = "Moallem_ListMoallem")]
        public ActionResult ListMoallem(int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            ViewBag.jsNotifyMessage = TempData["Notification"];
            MoallemManagement sm = new MoallemManagement();
            return View(sm.ListMoallem(HttpContext.Items["ParrentId"] as string));
        }
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Moallem_AddMoallem")]
        public ActionResult AddMoallem(int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            int? madreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            DoroosManagement dm = new DoroosManagement();
            Moallem_Dars md = new Moallem_Dars();
            md.Darsha = dm.DoroosComboAll(madreseId ?? default(int));
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "مرد", Value = "true" });
            listItems.Add(new SelectListItem { Text = "زن", Value = "false" });
            ViewBag.Jensiat = listItems;
            return View(md);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Moallem_AddMoallem")]
        public ActionResult AddMoallem(Moallem_Dars model, int? NemayandegiId, int? ModirId, string Username, string Password, string ConfirmPassword)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            MoallemManagement mm = new MoallemManagement();
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "مرد", Value = "true" });
            listItems.Add(new SelectListItem { Text = "زن", Value = "false" });
            ViewBag.Jensiat = listItems;
            if (!string.IsNullOrEmpty(model.moallem.Mobile))
                model.moallem.Mobile = '0' + model.moallem.Mobile.Replace(")", "").Replace("(", "").Replace("-", "").Replace(" ", "");
            int? madreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            model.moallem.F_ParrentID = HttpContext.Items["ParrentId"] as string;
            model.moallem.F_MadaaresID = madreseId;
            model.moallem.UserInformation.F_MadaaresID = madreseId;
            DoroosManagement dm = new DoroosManagement();
            model.Darsha = dm.DoroosComboAll(madreseId ?? default(int));
            int? Id;
            string result = mm.AddMoallem(model, Username, Password, ConfirmPassword, ModelState, out Id);
            if (result == "error")
            {
                ViewBag.jsNotifyMessage = result;
                return View(model);
            }
            result = mm.AssignMoallemToDars(Id ?? default(int), model.DarsId, HttpContext.Items["ParrentId"] as string);
            if (result == "error")
            {
                ViewBag.jsNotifyMessage = result;
                return View(model);
            }
            TempData["Notification"] = result;
            return RedirectToAction("ListMoallem", "Moallem", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
        }


        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Moallem_EditMoallem")]
        public ActionResult EditMoallem(int? NemayandegiId, int? ModirId, int MoallemId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            Moallem_Dars md = new Moallem_Dars();
            MoallemManagement mm = new MoallemManagement();
            var model = mm.DetailMoallem(MoallemId, HttpContext.Items["ParrentId"] as string);
            if (model != null)
            {
                if (!string.IsNullOrEmpty(model.Mobile))
                {
                    model.Mobile = model.Mobile.Substring(1);
                }
                var listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem { Text = "مرد", Value = "true" });
                listItems.Add(new SelectListItem { Text = "زن", Value = "false" });
                ViewBag.Jensiat = listItems;
                ViewBag.MoallemUsername = Tools.F_UserName(model.F_UserInformation);
                md.moallem = model;
                md.DarsId = md.moallem.Mapping_Moallem_Doroos.Select(u => u.F_DoroosID).ToArray();
                DoroosManagement dm = new DoroosManagement();
                md.Darsha = dm.DoroosComboAll(Tools.MadreseId(HttpContext.Items["ParrentId"] as string) ?? default(int), md.DarsId);

                return View(md);
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
        [PageTittleAttributeActionFilter(Function = "Moallem_EditMoallem")]
        public ActionResult EditMoallem(Moallem_Dars model, int? NemayandegiId, int? ModirId, int MoallemId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            MoallemManagement sm = new MoallemManagement();
            var temp = sm.DetailMoallem(model.moallem.ID, HttpContext.Items["ParrentId"] as string);
            if (temp == null)
            {
                return View("NotFound");
            }
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "مرد", Value = "true" });
            listItems.Add(new SelectListItem { Text = "زن", Value = "false" });
            ViewBag.Jensiat = listItems;
            model.moallem.Mobile = "0" + model.moallem.Mobile.Replace(")", "").Replace("(", "").Replace("-", "").Replace(" ", "");

            string result = sm.EditMoallem(model.moallem);
            MoallemManagement mm = new MoallemManagement();
            mm.ReAssignMoallemToDars(model.moallem.ID, model.DarsId, HttpContext.Items["ParrentId"] as string);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListMoallem", "Moallem", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            else
            {
                ViewBag.MoallemUsername = Tools.F_UserName(model.moallem.F_UserInformation);
                ViewBag.jsNotifyMessage = result;
                DoroosManagement dm = new DoroosManagement();
                model.Darsha = dm.DoroosComboAll(Tools.MadreseId(HttpContext.Items["ParrentId"] as string) ?? default(int), model.DarsId);
                return View(model);
            }
        }

        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult ChangeDisplayMoallem(int? NemayandegiId, int? ModirId, int MoallemId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            MoallemManagement sm = new MoallemManagement();
            string result = sm.ChangeStatusMoallem(MoallemId, HttpContext.Items["ParrentId"] as string);
            TempData["Notification"] = result;
            return RedirectToAction("ListMoallem", "Moallem", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
        }
        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult DeleteMoallem(int? NemayandegiId, int? ModirId, int MoallemId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            MoallemManagement sm = new MoallemManagement();
            string result = sm.DeleteMoallem(MoallemId, HttpContext.Items["ParrentId"] as string);
            TempData["Notification"] = result;
            if (result == "success")
            {
                return RedirectToAction("ListMoallem", "Moallem", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            else
            {
                return RedirectToAction("DetailMoallem", "Moallem", new { NemayandegiId = NemayandegiId, ModirId = ModirId, MoallemId = MoallemId });
            }
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Moallem_DetailMoallem")]
        public ActionResult DetailMoallem(int? NemayandegiId, int? ModirId, int MoallemId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            Moallem_Dars md = new Moallem_Dars();
            MoallemManagement sm = new MoallemManagement();
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var model = sm.DetailMoallem(MoallemId, HttpContext.Items["ParrentId"] as string);
            if (model == null)
            {
                return View("NotFound");
            }
            else
            {
                md.moallem = model;
                md.DarsId = md.moallem.Mapping_Moallem_Doroos.Select(u => u.F_DoroosID).ToArray();
                DoroosManagement dm = new DoroosManagement();
                //md.Darsha = dm.DoroosComboAll(Tools.MadreseId(HttpContext.Items["ParrentId"] as string) ?? default(int), md.DarsId);
                md.Darsha = new SelectList(md.moallem.Mapping_Moallem_Doroos.Select(u => new { Value = u.Doroos.ID, Text = u.Doroos.NaameDars + "( پایه " + u.Doroos.F_PayeID + " )" }), "Value", "Text", md.DarsId);
                ViewBag.Username = UserManager.FindById(model.F_UserInformation).UserName;
                return View(md);
            }
        }
    }
}