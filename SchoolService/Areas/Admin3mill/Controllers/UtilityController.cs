using SchoolService.Areas.Admin3mill.Models;
using SchoolService.Models.AndroidJsonModel;
using SchoolService.Models.DAL;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace SchoolService.Areas.Admin3mill.Controllers
{
    public class UtilityController : Controller
    {
        private SCEntities db = new SCEntities();

        public JsonResult GetCity(string Id)
        {

            int StateId = -1;
            int.TryParse(Id, out StateId);

            var _City = db.AddressCity.Where(u => u.F_StateId == StateId && u.isDelete==false);
            if (_City != null)
            {
                return Json(new SelectList(_City.Select(u => new { Value = u.Id, Text = u.Name }), "Value", "Text"));
            }
            else
            {
                return null;
            }
        }
        public JsonResult GetPaaye(string Id)
        {

            int maghtaId = -1;
            int.TryParse(Id, out maghtaId);

            var _paye = db.Paaye.Where(u => u.F_MaghaateID == maghtaId && u.isDeleted==false);
            if (_paye != null)
            {
                return Json(new SelectList(_paye.Select(u => new { Value = u.ID, Text = u.NaamePaye }), "Value", "Text"));
            }
            else
            {
                return null;
            }
        }
        private int? NemayandegiIdPrepare(int? NemayandegiId)
        {
            int? Id;
            if (User.IsInRole("Nemayandegi"))
            {
                Id = SchoolService.Areas.Admin3mill.Models.Tools.NemayandegiId_Current();

            }
            else if (User.IsInRole("Modir"))
            {
                //Id = SchoolService.Areas.Admin3mill.Models.Tools.NemayandegiId_CurrentModir(SchoolService.Areas.Admin3mill.Models.Tools.F_UserID());
                Id = Tools.NemayandegiId_CurrentModir(Tools.ModirParrent(SchoolService.Areas.Admin3mill.Models.Tools.ModirId()));
            }
            else { Id = NemayandegiId; }
            ViewBag.NemayandegiId = Id;
            return Id;
        }
        private int? ModirIdPrepare(int? ModirId, out string ParrentId)
        {
            int? Id;
            if (User.IsInRole("Modir"))
            {
                Id = SchoolService.Areas.Admin3mill.Models.Tools.ModirId_Current();
                ParrentId = Tools.F_UserID();
            }
            else if (User.IsInRole("Nemayandegi"))
            {
                if (SchoolService.Areas.Admin3mill.Models.Tools.F_UserID_Modir(ModirId ?? default(int)) == SchoolService.Areas.Admin3mill.Models.Tools.F_UserID())
                {
                    Id = ModirId;
                    ParrentId = SchoolService.Areas.Admin3mill.Models.Tools.F_UserID_Modir(ModirId ?? default(int));
                }
                else
                {
                    ParrentId = null;
                    Id = null;
                }

            }
            else
            {
                Id = ModirId;
                ParrentId = SchoolService.Areas.Admin3mill.Models.Tools.F_UserID_Modir(ModirId ?? default(int));
            }
            ViewBag.ModirId = Id;
            return Id;
        }


        public ActionResult NemudareRoshdeTahsili(int DaneshAmoozId)
        {
            Doroos_DAL doroos = new Doroos_DAL(new SCEntities());
            return PartialView(doroos.NemudarePishrafteTahsili(DaneshAmoozId));
        }
        public ActionResult ListeDoroos(int? NemayandegiId, int? ModirId, int KelasId, int DaneshAmoozId)
        {
            string ParrentId;
            NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            ModirId = ModirIdPrepare(ModirId, out ParrentId);
            int? MadreseId = Tools.MadreseId(ParrentId);
            Doroos_DAL dal = new Doroos_DAL(new SCEntities());
            ViewBag.DaneshAmoozId = DaneshAmoozId;
            return PartialView(dal.GetListDoroosForAdmin(KelasId, MadreseId ?? default(int)));
        }
     
        public ActionResult NomarateKelasiVaEmtehani(int? NemayandegiId, int? ModirId,int DarsId, int DaneshAmoozId)
        {
            string ParrentId;
            NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            ModirId = ModirIdPrepare(ModirId, out ParrentId);
            int? MadreseId = Tools.MadreseId(ParrentId);
            Doroos_DAL dal = new Doroos_DAL(new SCEntities());
            return PartialView(dal.ListNomarateDarsForAdmin(MadreseId ?? default(int), DaneshAmoozId, DarsId));
        }

        [HttpPost]
        public ActionResult GozaresheFilterShode(int? NemayandegiId, int? ModirId, int DarsId, string Mah, int DaneshAmoozId)
        {
            string ParrentId;
            NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            ModirId = ModirIdPrepare(ModirId, out ParrentId);
            int? MadreseId = Tools.MadreseId(ParrentId);
            Doroos_DAL dal = new Doroos_DAL(new SCEntities());
            return PartialView(dal.GozaresheFilterShode(DarsId, Mah, DaneshAmoozId));
        }


        public ActionResult GozaresheHuzurGhiab(int? NemayandegiId, int? ModirId,int DaneshAmoozId, string Mah = "07")
        {
            var db = new SCEntities(); PersianCalendar p = new PersianCalendar(); var now = DateTime.Now; DateTime date;
            Tools.GetJalaliDateReturnDateTime(p.GetYear(now) + "/" + Mah + "/01 00:00:00", out date);
            DateTime date2 = date.AddMonths(1);
            var temp = db.HozoorGhiabKelasi.Include(t => t.HozoorGhiab).Where(u => (u.HozoorGhiab.Tarikh > date || u.HozoorGhiab.Tarikh == date) && (u.HozoorGhiab.Tarikh < date2) && u.isDeleted == false && u.F_DaneshAmuzID == DaneshAmoozId).Select(x => new { x.HozoorGhiab.Tarikh, x.Takhir, x.Tovzihat });
            var Result = new List<Huzurghiab_Model>();
            foreach (var item in temp)
                Result.Add(new Huzurghiab_Model(Tools.SpecialJalaliFormat(item.Tarikh ?? default(DateTime)), item.Takhir.ToString(), item.Tovzihat));
            ViewBag.ListTarikh = Tools.Months();
            return PartialView(Result);
        }

        public ActionResult GozaresheTashvighTanbih(int DaneshAmoozId, string Mah = "07")
        {
            var db = new SCEntities(); PersianCalendar p = new PersianCalendar(); var now = DateTime.Now; DateTime date;
            Tools.GetJalaliDateReturnDateTime(p.GetYear(now) + "/" + Mah + "/01 00:00:00", out date);
            DateTime date2 = date.AddMonths(1);
            var temp = db.TashvighTanbihKelasi.Where(u => (u.Tarikh > date || u.Tarikh == date) && (u.Tarikh < date2) && u.isDeleted == false && u.F_DaneshAmuzID == DaneshAmoozId).Select(x => new { x.Tarikh, x.Emtiaz, x.Tovzihat });
            var Result = new List<TashvighTanbih_Model>();
            foreach (var item in temp)
                Result.Add(new TashvighTanbih_Model(Tools.SpecialJalaliFormat(item.Tarikh ?? default(DateTime)), item.Tovzihat, item.Emtiaz.ToString()));
            ViewBag.ListTarikh = Tools.Months();
            return PartialView(Result);
        }

        [HttpPost]
        public ActionResult GozaresheHuzurGhiab(int DaneshAmoozId, string Mah = "07",string Nothing="")
        {
            var db = new SCEntities(); PersianCalendar p = new PersianCalendar(); var now = DateTime.Now; DateTime date;
            Tools.GetJalaliDateReturnDateTime(p.GetYear(now) + "/" + Mah + "/01 00:00:00", out date);
            DateTime date2 = date.AddMonths(1);
            var temp = db.HozoorGhiabKelasi.Include(t => t.HozoorGhiab).Where(u => (u.HozoorGhiab.Tarikh > date || u.HozoorGhiab.Tarikh == date) && (u.HozoorGhiab.Tarikh < date2) && u.isDeleted == false && u.F_DaneshAmuzID == DaneshAmoozId).Select(x => new { x.HozoorGhiab.Tarikh, x.Takhir, x.Tovzihat });
            var Result = new List<Huzurghiab_Model>();
            foreach (var item in temp)
                Result.Add(new Huzurghiab_Model(Tools.SpecialJalaliFormat(item.Tarikh ?? default(DateTime)), item.Takhir.ToString(), item.Tovzihat));
            return PartialView(Result);
        }

        [HttpPost]
        public ActionResult GozaresheTashvighTanbih(int DaneshAmoozId, string Mah = "07",string Nothing="")
        {
            var db = new SCEntities(); PersianCalendar p = new PersianCalendar(); var now = DateTime.Now; DateTime date;
            Tools.GetJalaliDateReturnDateTime(p.GetYear(now) + "/" + Mah + "/01 00:00:00", out date);
            DateTime date2 = date.AddMonths(1);
            var temp = db.TashvighTanbihKelasi.Where(u => (u.Tarikh > date || u.Tarikh == date) && (u.Tarikh < date2) && u.isDeleted == false && u.F_DaneshAmuzID == DaneshAmoozId).Select(x => new { x.Tarikh, x.Emtiaz, x.Tovzihat });
            var Result = new List<TashvighTanbih_Model>();
            foreach (var item in temp)
                Result.Add(new TashvighTanbih_Model(Tools.SpecialJalaliFormat(item.Tarikh ?? default(DateTime)), item.Tovzihat, item.Emtiaz.ToString()));
            return PartialView(Result);
        }
    }
}