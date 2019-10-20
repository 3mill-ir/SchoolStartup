using SchoolService.Areas.Admin3mill.Models;
using SchoolService.CustomFilters;
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
    public class BarnameEmtehaniController : Controller
    {
        public BarnameEmtehaniController()
        {
            ViewBag.MenuAccessNemyandegiAlias = "Admin";
            ViewBag.MenuAccessmadreseAlias = "Admin";
        }

        private SCEntities db = new SCEntities();

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "BarnameEmtehani_Create")]
        public ActionResult Create(int? NemayandegiId, int? ModirId, int kelasId)
        {
            int? MadreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            var kelas = db.Kelas.Where(u => u.isDeleted == false && u.F_MadaresID == MadreseId && u.ID == kelasId).FirstOrDefault();
            if (kelas == null)
            {
                return View("NotFound");
            }

            BarnameEmtehani_DAL BD = new BarnameEmtehani_DAL(db);

            return View(BD.ListDoroosEmtehani(kelasId));
        }


        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "BarnameEmtehani_Create")]
        public ActionResult Create(BarnameEmtehani_ModelList model, int? NemayandegiId, int? ModirId, int kelasId)
        {
            int? MadreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            var kelas = db.Kelas.Where(u => u.isDeleted == false && u.F_MadaresID == MadreseId && u.ID == kelasId).FirstOrDefault();
            if (kelas == null)
            {
                return View("NotFound");
            }
            foreach (var item in model.BarnameEMtehaniList)
            {
                PersianCalendar p = new PersianCalendar(); DateTime date;
                Tools.GetJalaliDateReturnDateTime(item.Tarikh.ToString(), out date);
                item.Tarikh = date;
            }
            BarnameEmtehani_DAL BD = new BarnameEmtehani_DAL(db);
            string result = BD.CreateListBarnameEmtehani(model, kelasId, HttpContext.Items["ParrentId"] as string);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListKelas", "Kelas", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            else
            {

                return View(model);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}