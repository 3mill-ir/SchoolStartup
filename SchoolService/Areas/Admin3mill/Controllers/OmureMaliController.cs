using SchoolService.Areas.Admin3mill.Models;
using SchoolService.CustomFilters;
using SchoolService.Models.BLL;
using SchoolService.Models.DAL;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolService.Areas.Admin3mill.Controllers
{
    [AuthLog(Roles = "Admin,Nemayandegi,Modir")]
    public class OmureMaliController : Controller
    {
        public OmureMaliController()
        {
            ViewBag.MenuAccessNemyandegiAlias = "Admin";
            ViewBag.MenuAccessmadreseAlias = "Admin";
        }
        #region Services
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_ListServices")]
        public ActionResult ListServices(int? NemayandegiId, int? ModirId)
        {
            int? MadreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            ViewBag.jsNotifyMessage = TempData["Notification"];
            OmureMaliManagement omm = new OmureMaliManagement();
            return View(omm.ListService(MadreseId ?? default(int), HttpContext.Items["ParrentId"] as string));
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_AddServices")]
        public ActionResult AddServices(int? NemayandegiId, int? ModirId)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_AddServices")]
        public ActionResult AddServices(Service model, int? NemayandegiId, int? ModirId)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            if (ModelState.IsValid)
            {
                model.F_ParrentID = HttpContext.Items["ParrentId"] as string;
                model.F_MadraseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
                string scale = omm.AddService(model);
                if (scale == "OK")
                    TempData["Notification"] = "success";
                else
                {
                    ViewBag.jsNotifyMessage = "سرویس مورد نظر تکراری می باشد !";
                    return View(model);
                }
                return RedirectToAction("ListServices", "OmureMali", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            return View(model);
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_EditServices")]
        public ActionResult EditServices(int? NemayandegiId, int? ModirId, int ServiceId)
        {
            int? madreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            OmureMaliManagement omm = new OmureMaliManagement();
            var model = omm.DetailService(madreseId ?? default(int), ServiceId, HttpContext.Items["ParrentId"] as string);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_EditServices")]
        public ActionResult EditServices(Service model, int? NemayandegiId, int? ModirId)
        {
            int? madreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            OmureMaliManagement omm = new OmureMaliManagement();
            var temp = omm.DetailService(madreseId ?? default(int), model.ID, HttpContext.Items["ParrentId"] as string);
            if (temp == null)
            {
                return View("NotFound");
            }


            if (ModelState.IsValid)
            {
                string scale = omm.EditService(model);
                if (scale == "OK")
                    TempData["Notification"] = "success";
                else
                    TempData["Notification"] = "error";
                return RedirectToAction("ListServices", "OmureMali", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            return View(model);
        }
        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult DeleteServices(int? NemayandegiId, int? ModirId, int ServiceId)
        {
            int? madreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);

            OmureMaliManagement omm = new OmureMaliManagement();
            if (omm.DeleteService(madreseId ?? default(int), ServiceId, HttpContext.Items["ParrentId"] as string) == "NotFound")
                TempData["Notification"] = "error";
            else
                TempData["Notification"] = "success";
            return RedirectToAction("ListServices", "OmureMali", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_DetailServices")]
        public ActionResult DetailServices(int? NemayandegiId, int? ModirId, int ServiceId)
        {
            int? madreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            OmureMaliManagement omm = new OmureMaliManagement();
            var model = omm.DetailService(madreseId ?? default(int), ServiceId, HttpContext.Items["ParrentId"] as string);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }

        #endregion

        #region Hazine
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_ListHazine")]
        public ActionResult ListHazine(int? NemayandegiId, int? ModirId, int OvliaId)
        {
            ViewBag.jsNotifyMessage = TempData["Notification"];
            ViewBag.F_OvliaId = OvliaId;
            OmureMaliManagement omm = new OmureMaliManagement();
            return View(omm.ListHazine(OvliaId, HttpContext.Items["ParrentId"] as string));
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_AddHazine")]
        public ActionResult AddHazine(int? NemayandegiId, int? ModirId, int OvliaId)
        {
            int? madreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);

            OmureMaliManagement omm = new OmureMaliManagement();
            ViewBag.Services = omm.ServiceCombo(madreseId ?? default(int));
            ViewBag.F_OvliaId = OvliaId;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_AddHazine")]
        public ActionResult AddHazine(Hazine model, int? NemayandegiId, int? ModirId, int OvliaId)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            if (ModelState.IsValid)
            {
                model.F_ParrentID = HttpContext.Items["ParrentId"] as string;
                model.F_OvliaId = OvliaId;
                string scale = omm.AddHazine(model);
                if (scale == "OK")
                    TempData["Notification"] = "success";
                else
                {
                    ViewBag.jsNotifyMessage = "سرویس مورد نظر تکراری می باشد !";
                    return View(model);
                }
                return RedirectToAction("ListHazine", "OmureMali", new { NemayandegiId = NemayandegiId, ModirId = ModirId, OvliaId = model.F_OvliaId });
            }
            else
            {
                int? madreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
                ViewBag.Services = omm.ServiceCombo(madreseId ?? default(int));
            }
            return View(model);
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_EditHazine")]
        public ActionResult EditHazine(int? NemayandegiId, int? ModirId, int HazineId, int OvliaId)
        {
            int? madreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            OmureMaliManagement omm = new OmureMaliManagement();
            ViewBag.Services = omm.ServiceCombo(madreseId ?? default(int));
            var model = omm.DetailHazine(HazineId, OvliaId, HttpContext.Items["ParrentId"] as string);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_EditHazine")]
        public ActionResult EditHazine(Hazine model, int? NemayandegiId, int? ModirId)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            if (ModelState.IsValid)
            {
                string scale = omm.EditHazine(model);
                if (scale == "OK")
                    TempData["Notification"] = "success";
                else
                    TempData["Notification"] = "error";
                return RedirectToAction("ListHazine", "OmureMali", new { NemayandegiId = NemayandegiId, ModirId = ModirId, OvliaId = model.F_OvliaId });
            }
            else
            {
                int? madreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);

                ViewBag.Services = omm.ServiceCombo(madreseId ?? default(int));
            }
            return View(model);
        }
        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult DeleteHazine(int? NemayandegiId, int? ModirId, int HazineId, int OvliaId)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            string result = omm.DeleteHazine(HazineId, OvliaId, HttpContext.Items["ParrentId"] as string);
            TempData["Notification"] = result;
            return RedirectToAction("ListHazine", "OmureMali", new { NemayandegiId = NemayandegiId, ModirId = ModirId, OvliaId = OvliaId });
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_DetailHazine")]
        public ActionResult DetailHazine(int? NemayandegiId, int? ModirId, int HazineId, int OvliaId)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            var model = omm.DetailHazine(HazineId, OvliaId, HttpContext.Items["ParrentId"] as string);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }

        #endregion

        #region Pardakht
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_ListPardakht")]
        public ActionResult ListPardakht(int? NemayandegiId, int? ModirId, int HazineId, int OvliaId)
        {
            ViewBag.jsNotifyMessage = TempData["Notification"];
            OmureMaliManagement omm = new OmureMaliManagement();
            return View(omm.ListPardakht(HazineId, HttpContext.Items["ParrentId"] as string));
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_AddPardakht")]
        public ActionResult AddPardakht(int? NemayandegiId, int? ModirId, int HazineId, int OvliaId)
        {
            Hazine_DAL dal = new Hazine_DAL(new SCEntities());
            var temp = dal.Details(HazineId, OvliaId, HttpContext.Items["ParrentId"] as string);
            ViewBag.AzBabate = "هزینه  ( " + temp.Service.ServiceName + " )  ثبت شده در تاریخ : " + Tools.JalaliDateWithoutHour(temp.Tarikh ?? default(DateTime));
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_AddPardakht")]
        public ActionResult AddPardakht(Pardakht model, int? NemayandegiId, int? ModirId, int HazineId, int OvliaId)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            Hazine_DAL dal = new Hazine_DAL(new SCEntities());
            var temp = dal.Details(HazineId, OvliaId, HttpContext.Items["ParrentId"] as string);
            if (temp == null)
            {
                return View("NotFound");
            }
            if (ModelState.IsValid)
            {

                model.F_ParrentID = HttpContext.Items["ParrentId"] as string;
                model.F_HazineId = temp.ID ;
                string scale = omm.AddPardakht(model);
                if (scale == "OK")
                    TempData["Notification"] = "success";
                else
                {
                    ViewBag.AzBabate = "هزینه  ( " + temp.Service.ServiceName + " )  ثبت شده در تاریخ : " + Tools.JalaliDateWithoutHour(temp.Tarikh ?? default(DateTime));
                    ViewBag.jsNotifyMessage = "مبلغ مورد نظر بیش از بدهی می باشد !";
                    return View(model);
                }
                return RedirectToAction("ListPardakht", "OmureMali", new { NemayandegiId = NemayandegiId, ModirId = ModirId, HazineId = model.F_HazineId, OvliaId = OvliaId });
            }
            ViewBag.AzBabate = "هزینه  ( " + temp.Service.ServiceName + " )  ثبت شده در تاریخ : " + Tools.JalaliDateWithoutHour(temp.Tarikh ?? default(DateTime));
            return View(model);
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_EditPardakht")]
        public ActionResult EditPardakht(int? NemayandegiId, int? ModirId, int PardakhtId, int HazineId, int OvliaId)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            var model = omm.DetailPardakht(PardakhtId, HttpContext.Items["ParrentId"] as string,HazineId);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_EditPardakht")]
        public ActionResult EditPardakht(Pardakht model, int? NemayandegiId, int? ModirId, int PardakhtId, int HazineId, int OvliaId)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            var tem = omm.DetailPardakht(PardakhtId, HttpContext.Items["ParrentId"] as string, HazineId);
            if (tem == null)
            {
                return View("NotFound");
            }
            if (ModelState.IsValid)
            {
                string scale = omm.EditPardakht(model);
                if (scale == "OK")
                    TempData["Notification"] = "success";
                else
                    TempData["Notification"] = "error";
                return RedirectToAction("ListPardakht", "OmureMali", new { NemayandegiId = NemayandegiId, ModirId = ModirId, HazineId = model.F_HazineId, OvliaId = OvliaId });
            }
            return View(model);
        }
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult DeletePardakht(int? NemayandegiId, int? ModirId, int PardakhtId, int HazineId, int OvliaId)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            if (omm.DeletePardakht(PardakhtId, HttpContext.Items["ParrentId"] as string, HazineId) == "NotFound")
                TempData["Notification"] = "error";
            else
                TempData["Notification"] = "success";
            return RedirectToAction("ListPardakht", "OmureMali", new { NemayandegiId = NemayandegiId, ModirId = ModirId, HazineId = HazineId });
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_DetailPardakht")]
        public ActionResult DetailPardakht(int? NemayandegiId, int? ModirId, int PardakhtId, int HazineId, int OvliaId)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            var model = omm.DetailPardakht(PardakhtId, HttpContext.Items["ParrentId"] as string, HazineId);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }

        #endregion

        #region Checks
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_ListCheck")]
        public ActionResult ListCheck(int? NemayandegiId, int? ModirId, int HazineId, int OvliaId)
        {
            ViewBag.jsNotifyMessage = TempData["Notification"];
            OmureMaliManagement omm = new OmureMaliManagement();
            return View(omm.ListCheck(HazineId, HttpContext.Items["ParrentId"] as string));
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_AddCheck")]
        public ActionResult AddCheck(int? NemayandegiId, int? ModirId, int HazineId, int OvliaId)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            Hazine_DAL dal = new Hazine_DAL(new SCEntities());
            var temp = dal.Details(HazineId, OvliaId, HttpContext.Items["ParrentId"] as string);
            ViewBag.AzBabate = "هزینه  ( " + temp.Service.ServiceName + " )  ثبت شده در تاریخ : " + Tools.JalaliDateWithoutHour(temp.Tarikh ?? default(DateTime));
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_AddCheck")]
        public ActionResult AddCheck(Check model, int? NemayandegiId, int? ModirId,int HazineId, int OvliaId, string PipoDate)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            if (ModelState.IsValid)
            {
                PersianCalendar p = new PersianCalendar(); DateTime date;
                Tools.GetJalaliDateReturnDateTime(PipoDate, out date);
                model.TarikheCheck = date;
                model.F_HazineId = HazineId;
                model.F_ParrentID = HttpContext.Items["ParrentId"] as string;
                string scale = omm.AddCheck(model);
                if (scale == "OK")
                    TempData["Notification"] = "success";
                else
                {
                    ViewBag.jsNotifyMessage = "مبلغ مورد نظر بیش از بدهی می باشد !";
                    return View(model);
                }
                return RedirectToAction("ListCheck", "OmureMali", new { NemayandegiId = NemayandegiId, ModirId = ModirId, HazineId = model.F_HazineId, OvliaId = OvliaId });
            }
            return View(model);
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_EditCheck")]
        public ActionResult EditCheck(int? NemayandegiId, int? ModirId, int CheckId, int HazineId, int OvliaId)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            var model = omm.DetailCheck(CheckId, HazineId, HttpContext.Items["ParrentId"] as string);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_EditCheck")]
        public ActionResult EditCheck(Check model, int? NemayandegiId, int? ModirId, int CheckId, int HazineId, int OvliaId, string PipoDate)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            var temp = omm.DetailCheck(CheckId, HazineId, HttpContext.Items["ParrentId"] as string);
            if (temp == null)
            {
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                PersianCalendar p = new PersianCalendar(); DateTime date;
                Tools.GetJalaliDateReturnDateTime(PipoDate, out date);
                model.TarikheCheck = date;
                string scale = omm.EditCheck(model);
                if (scale == "OK")
                    TempData["Notification"] = "success";
                else
                    TempData["Notification"] = "error";
                return RedirectToAction("ListCheck", "OmureMali", new { NemayandegiId = NemayandegiId, ModirId = ModirId, HazineId = model.F_HazineId, OvliaId = OvliaId });
            }
            return View(model);
        }
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult DeleteCheck(int? NemayandegiId, int? ModirId, int CheckId, int HazineId, int OvliaId)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            if (omm.DeleteCheck(CheckId, HttpContext.Items["ParrentId"] as string, HazineId) == "NotFound")
                TempData["Notification"] = "error";
            else
                TempData["Notification"] = "success";
            return RedirectToAction("ListCheck", "OmureMali", new { NemayandegiId = NemayandegiId, ModirId = ModirId, HazineId = HazineId, OvliaId = OvliaId });
        }
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult VosuleCheck(int? NemayandegiId, int? ModirId, int CheckId, int HazineId, int OvliaId)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            if (omm.VosuleCheck(CheckId, HttpContext.Items["ParrentId"] as string, HazineId) == "NotFound")
                TempData["Notification"] = "error";
            else
                TempData["Notification"] = "success";
            return RedirectToAction("ListCheck", "OmureMali", new { NemayandegiId = NemayandegiId, ModirId = ModirId, HazineId = HazineId, OvliaId = OvliaId });
        }
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult BargashteCheck(int? NemayandegiId, int? ModirId, int CheckId, int HazineId, int OvliaId)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            if (omm.BargashteCheck(CheckId, HttpContext.Items["ParrentId"] as string, HazineId) == "NotFound")
                TempData["Notification"] = "error";
            else
                TempData["Notification"] = "success";
            return RedirectToAction("ListCheck", "OmureMali", new { NemayandegiId = NemayandegiId, ModirId = ModirId, HazineId = HazineId, OvliaId = OvliaId });
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "OmureMali_DetailCheck")]
        public ActionResult DetailCheck(int? NemayandegiId, int? ModirId, int CheckId, int HazineId, int OvliaId)
        {
            OmureMaliManagement omm = new OmureMaliManagement();
            var model = omm.DetailCheck(CheckId, HazineId, HttpContext.Items["ParrentId"] as string);
            if (model == null)
            {
                return View("NotFound");
            }
            return View(model);
        }

        #endregion
    }
}