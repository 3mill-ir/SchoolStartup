using PagedList;
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
    [AuthLog(Roles = "Admin,Nemayandegi")]
    public class MadaresController : Controller
    {
        private SCEntities db = new SCEntities();

        public MadaresController()
        {
            ViewBag.MenuAccessNemyandegiAlias = "Admin";

        }
        [NemayandegiIdPrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Madares_ListMadares")]
        public ActionResult ListMadares(int? NemayandegiId)
        {
            ViewBag.jsNotifyMessage = TempData["Notification"];
            MadaresManagement sm = new MadaresManagement();
            string ParrentID;
            if (User.IsInRole("Admin"))
            {
                ParrentID = Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int));
            }
            else
            {
                ParrentID = Tools.F_UserID();
            }
            return View(sm.ListMadares(ParrentID));
        }
        private void ViewBagPrepare()
        {
            ViewBag.saletahsili = new SelectList(db.SaleTahsili.Select(u => new { Value = u.ID, Text = u.Sal }), "Value", "Text");
            ViewBag.StateList = new SelectList(db.AddressState.Select(u => new { Value = u.Id, Text = u.Name }), "Value", "Text");
            MaghaateManagement mm = new MaghaateManagement();
            ViewBag.Maghaate = mm.MaghaateCombo();
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
        [NemayandegiIdPrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Madares_AddMadares")]
        public ActionResult AddMadares(int? NemayandegiId)
        {
            if (User.IsInRole("Admin"))
            {
                if (NemayandegiId == null)
                {
                    return View("NotFound");
                }
            }
            ViewBagPrepare();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Madares_AddMadares")]
        public ActionResult AddMadares(ModirMadrese_Model model, string Username, string Password, string ConfirmPassword, int? NemayandegiId)
        {
            if (User.IsInRole("Admin"))
            {
                if (NemayandegiId == null)
                {
                    return View("NotFound");
                }
            }
            ViewBagPrepare();
            MadaresManagement mm = new MadaresManagement();
            ModirManagement modir = new ModirManagement();
            string ParrentID;
            if (User.IsInRole("Admin"))
            {
                ParrentID = Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int));
            }
            else
            {
                ParrentID = Tools.F_UserID();
            }
            model.madrese.F_NemayandegiID = NemayandegiId;
            model.madrese.F_ParrentID = ParrentID;
            model.karmand.F_ParrentID = ParrentID;
            int? modirId;
            string result = modir.AddModir(model.karmand, Username, Password, ConfirmPassword, ModelState, out modirId);
            if (result == "error")
            {
                ViewBag.jsNotifyMessage = result;
                return View(model);
            }

            model.madrese.ModirID = modirId;
            int? madreseId;
            result = mm.AddMadares(model.madrese, ModelState, ParrentID, out madreseId);
            if (result == "error")
            {
                ViewBag.jsNotifyMessage = result;
                return View(model);
            }
            result = modir.AssignModirToMadrese(modirId ?? default(int), madreseId ?? default(int));
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListMadares", "Madares", new { NemayandegiId = NemayandegiId });
            }
            else
            {
                ViewBag.jsNotifyMessage = result;
                return View(model);
            }
        }


        [NemayandegiIdPrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Madares_EditMadares")]
        public ActionResult EditMadares(int MadaresId, int? NemayandegiId)
        {
            ViewBagPrepare();
            string ParrentID;
            if (User.IsInRole("Admin"))
            {
                ParrentID = Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int));
            }
            else
            {
                ParrentID = Tools.F_UserID();
            }

            MadaresManagement mm = new MadaresManagement();
            //ModirMadrese_Model madrese_moallem = new ModirMadrese_Model();
            var madrese = mm.DetailMadares(MadaresId, Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int)));
            ModirManagement modir = new ModirManagement();

            if (madrese != null)
            {
                var karmand = modir.DetailModir(madrese.ModirID ?? default(int), ParrentID);
                if (karmand != null)
                {
                    ViewBag.ModirUsername = Tools.F_UserName(karmand.F_UserInfromation);
                }
                return View(karmand);
            }
            else
            {
                return View("NotFound");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Madares_EditMadares")]
        public ActionResult EditMadares(Karmandaan model, int? NemayandegiId)
        {
            ViewBagPrepare();
            string ParrentID;
            if (User.IsInRole("Admin"))
            {
                ParrentID = Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int));
            }
            else
            {
                ParrentID = Tools.F_UserID();
            }
            if (model.F_ParrentID != ParrentID || model.UserInformation.Madaares.F_ParrentID != ParrentID)
            {
                return View("NotFound");
            }
            MadaresManagement mm = new MadaresManagement();
            ModirManagement modir = new ModirManagement();
            var temp = modir.DetailModir(model.ID, ParrentID);
            ViewBag.ModirUsername = Tools.F_UserName(model.F_UserInfromation);
            if (temp == null)
            {
                return View("NotFound");
            }
            string result = modir.EditModir(model, ModelState, ParrentID);
            if (result == "error")
            {
                ViewBag.jsNotifyMessage = result;
                return View(model);
            }
            //    result = mm.EditMadares(model.madrese, ModelState, NemayandegiId ?? default(int));
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListMadares", "Madares", new { NemayandegiId });
            }
            else
            {
                ViewBag.jsNotifyMessage = result;
                return View(model);
            }
        }
        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        public ActionResult ChangeDisplayMadares(int MadaresId, int? NemayandegiId)
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
            MadaresManagement sm = new MadaresManagement();
            string result = sm.ChangeStatusMadares(MadaresId, ParrentID);
            TempData["Notification"] = result;
            return RedirectToAction("ListMadares", "Madares", new { NemayandegiId });
        }

        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        public ActionResult DeleteMadares(int MadaresId, int? NemayandegiId)
        {
            MadaresManagement sm = new MadaresManagement();
            string ParrentID;
            if (User.IsInRole("Admin"))
            {
                ParrentID = Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int));
            }
            else
            {
                ParrentID = Tools.F_UserID();
            }
            string result = sm.DeleteMadares(MadaresId, ParrentID);
            TempData["Notification"] = result;
            return RedirectToAction("ListMadares", "Madares", new { NemayandegiId });
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Madares_DetailMadares")]
        public ActionResult DetailMadares(int MadaresId, int? NemayandegiId)
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
            MadaresManagement sm = new MadaresManagement();
            var model = sm.DetailMadares(MadaresId, Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int)));
            if (model != null)
            {
                ViewBag.MenuAccessmadreseAlias = "Admin";
                ViewBag.ModirId = model.ModirID;
                return View(model);
            }
            else
            {
                return View("NotFound");
            }

        }
    }
}