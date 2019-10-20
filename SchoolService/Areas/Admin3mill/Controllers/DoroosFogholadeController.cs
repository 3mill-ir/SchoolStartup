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
    [AuthLog(Roles = "Admin,Nemayandegi,Modir")]
    public class DoroosFogholadeController : Controller
    {

        public DoroosFogholadeController()
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
                [PageTittleAttributeActionFilter(Function = "DoroosFogholade_ListDoroos")]
        public ActionResult ListDoroos(int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            DoroosManagement sm = new DoroosManagement(); 
                    ViewBag.jsNotifyMessage = TempData["Notification"];
                    return View(sm.ListFovgholade(Tools.MadreseId(HttpContext.Items["ParrentId"] as string) ?? default(int)));
        }


        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "DoroosFogholade_AddDoroos")]
        public ActionResult AddDoroos(int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            MadaresManagement sm = new MadaresManagement();
            var sss = HttpContext.Items["ParrentId"] as string;
            int? MadaresId = Tools.MadreseId(sss);
            var madrese = sm.DetailMadares(MadaresId ?? default(int), Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int)));
            PaayeManagement pm = new PaayeManagement();
            ViewBag.Paaye = pm.PaayeCombo(madrese.F_MaghaateID ?? default(int));
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "1", Value = "1" });
            listItems.Add(new SelectListItem { Text = "2", Value = "2" });
            listItems.Add(new SelectListItem { Text = "3", Value = "3" });
            ViewBag.Zarib = listItems;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "DoroosFogholade_AddDoroos")]
        public ActionResult AddDoroos(Doroos model,int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            int? MadaresId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            model.F_MadaaresID = MadaresId;
            model.F_ParrentID = HttpContext.Items["ParrentId"] as string;
            DoroosManagement mm = new DoroosManagement();
            string result = mm.AddDoroosFovgholade(model,MadaresId ?? default(int),ModelState);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListDoroos", "DoroosFogholade", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            else
            {
                ViewBag.jsNotifyMessage = result;
                PaayeManagement pm = new PaayeManagement();
                MadaresManagement sm = new MadaresManagement();
                var madrese = sm.DetailMadares(MadaresId ?? default(int), Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int)));
                ViewBag.Paaye = pm.PaayeCombo(madrese.F_MaghaateID ?? default(int));
                var listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem { Text = "1", Value = "1" });
                listItems.Add(new SelectListItem { Text = "2", Value = "2" });
                listItems.Add(new SelectListItem { Text = "3", Value = "3" });
                ViewBag.Zarib = listItems;
                return View(model);
            }
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "DoroosFogholade_EditDoroos")]
        public ActionResult EditDoroos(int DoroosId,int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            int? MadaresId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            DoroosManagement mm = new DoroosManagement();
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "1", Value = "1" });
            listItems.Add(new SelectListItem { Text = "2", Value = "2" });
            listItems.Add(new SelectListItem { Text = "3", Value = "3" });
            ViewBag.Zarib = listItems;
            var model = mm.DetailDoroosFovgholade(DoroosId, MadaresId ?? default(int));
            if (model != null)
            {
                return View(model);
            }
            else return View("NotFound");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "DoroosFogholade_EditDoroos")]
        public ActionResult EditDoroos(Doroos model, int? NemayandegiId, int? ModirId, int? DoroosId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            int? MadaresId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            DoroosManagement sm = new DoroosManagement();
            var temp = sm.DetailDoroosFovgholade(model.ID, MadaresId ?? default(int));
            if (temp == null)
            {
                return View("NotFound");
            }
            string result = sm.EditDoroosFovgholade(model, MadaresId ?? default(int), ModelState);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListDoroos", "DoroosFogholade", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
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
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult DeleteDoroos(int DoroosId,int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            int? MadaresId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            DoroosManagement sm = new DoroosManagement();
            string result = sm.DeleteDoroosFovgholade(DoroosId,MadaresId ?? default(int));
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListDoroos", "DoroosFogholade", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            else
            {
                ViewBag.jsNotifyMessage = result; 
                return RedirectToAction("DetailDoroos", "DoroosFogholade", new { NemayandegiId = NemayandegiId, ModirId = ModirId, DoroosId = DoroosId });
            }
        }
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "DoroosFogholade_DetailDoroos")]
        public ActionResult DetailDoroos(int? NemayandegiId, int? ModirId, int DoroosId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            int? MadaresId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            DoroosManagement sm = new DoroosManagement();
            var model = sm.DetailDoroosFovgholade(DoroosId,MadaresId ?? default(int));
            if (model != null)
            {
                return View(model);
            }
            else return View("NotFound");
        }


    }
}