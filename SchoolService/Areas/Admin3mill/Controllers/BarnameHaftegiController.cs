using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchoolService.Models.DataModel;
using SchoolService.Models.DAL;
using System.Collections;
using SchoolService.Models.BLL;
using SchoolService.Areas.Admin3mill.Models;
using SchoolService.CustomFilters;

namespace SchoolService.Areas.Admin3mill.Controllers
{
    [AuthLog(Roles = "Admin,Nemayandegi,Modir")]
    public class BarnameHaftegiController : Controller
    {
        private SCEntities db = new SCEntities();


        public BarnameHaftegiController()
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



        //public ActionResult Index()
        //{
        //    var barnamehaftegi = db.BarnameHaftegi.Include(b => b.Kelas).Include(b => b.Mapping_Moallem_Doroos);
        //    return View(barnamehaftegi.ToList());
        //}


        //public ActionResult Details(int? NemayandegiId, int? ModirId, int kelasId, int payeId)
        //{


        //    BarnameHaftegi_DAL BD = new BarnameHaftegi_DAL(db);

        //    return View(BD.Details(kelasId));
        //}

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "BarnameHaftegi_Create")]
        public ActionResult Create(int? NemayandegiId, int? ModirId, int kelasId)
        {

            int? MadreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            var kelas = db.Kelas.Where(u => u.isDeleted == false && u.F_MadaresID == MadreseId && u.ID == kelasId).FirstOrDefault();
            if (kelas == null)
            {
                return View("NotFound");
            }
            else
            {
                if (kelas.BarnameHaftegi.Where(u => u.isDeleted == false).Count() != 0)
                {
                    @ViewBag.isKlashasBarname = true;
                    return View();
                }
            }
            @ViewBag.isKlashasBarname = false;
            @ViewBag.isKlasNull = false;
            @ViewBag.MaxZang = kelas.MaxZang;
            BarnameHaftegi_DAL BD = new BarnameHaftegi_DAL(db);
            var pipo = BD.List_Moallem_Doroos(MadreseId ?? default(int), kelas.F_PayeID ?? default(int));
            var pipoMoallemList = from a in pipo
                                  group a by a.Barnamehaftegi_MoallemID into g
                                  select new
                                  {
                                      Moallem_ID = g.FirstOrDefault().Barnamehaftegi_MoallemID,
                                      Moallem_FullName = g.FirstOrDefault().Barnamehaftegi_MoallemFullName
                                  };
            var pipoDoroosList = from a in pipo
                                 group a by a.Barnamehaftegi_DoroosName into g
                                 select new
                                 {
                                     Doroos_ID = g.FirstOrDefault().Barnamehaftegi_DoroosID,
                                     Doroos_Name = g.FirstOrDefault().Barnamehaftegi_DoroosName
                                 };
            ArrayList arraypipoMoallemList = new ArrayList(pipoMoallemList.ToList());
            arraypipoMoallemList.Insert(0, new { Moallem_ID = 0, Moallem_FullName = "انتخاب معلم" });
            ArrayList arraypipoDoroosList = new ArrayList(pipoDoroosList.ToList());
            arraypipoDoroosList.Insert(0, new { Doroos_ID = 0, Doroos_Name = "انتخاب درس" });
            ViewBag.MoallemList = new SelectList(arraypipoMoallemList, "Moallem_ID", "Moallem_FullName");
            ViewBag.DoroosList = new SelectList(arraypipoDoroosList, "Doroos_ID", "Doroos_Name");
            return View();
        }


        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "BarnameHaftegi_Create")]
        public ActionResult Create(BarnameHaftegi_ModelList barnamehaftegi, int? NemayandegiId, int? ModirId, int kelasId)
        {
            int? MadreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            var kelas = db.Kelas.Where(u => u.isDeleted == false && u.F_MadaresID == MadreseId && u.ID == kelasId).FirstOrDefault();
            if (kelas == null)
            {
                return View("NotFound");
            }
            else
            {
                if (kelas.BarnameHaftegi.Where(u => u.isDeleted == false).Count() != 0)
                {
                    @ViewBag.isKlashasBarname = true;
                    return View();
                }
            }
            BarnameHaftegi_BLL BB = new BarnameHaftegi_BLL();
            string result = BB.BarnameHaftegi_Update(barnamehaftegi, kelasId, kelas.F_PayeID ?? default(int), HttpContext.Items["ParrentId"] as string);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListKelas", "Kelas", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            else
            {
                @ViewBag.isKlashasBarname = false;
                @ViewBag.isKlasNull = false;
                @ViewBag.MaxZang = kelas.MaxZang;
                ViewBag.jsNotifyMessage = result;
                return View(barnamehaftegi);
            }
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "BarnameHaftegi_Edit")]
        public ActionResult Edit(int? NemayandegiId, int? ModirId, int kelasId)
        {
            int? MadreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            var kelas = db.Kelas.Where(u => u.isDeleted == false && u.F_MadaresID == MadreseId && u.ID == kelasId).FirstOrDefault();
            if (kelas == null)
            {
                return View("NotFound");
            }
            else
            {
                if (kelas.BarnameHaftegi.Where(u => u.isDeleted == false).Count() == 0)
                {
                    @ViewBag.isKlashasBarname = false;
                    return View();
                }
            }
            @ViewBag.isKlashasBarname = true;
            @ViewBag.isKlasNull = false;
            @ViewBag.MaxZang = kelas.MaxZang;
            BarnameHaftegi_DAL BD = new BarnameHaftegi_DAL(db);
            var pipo = BD.List_Moallem_Doroos(MadreseId ?? default(int), kelas.F_PayeID ?? default(int));
            var pipoMoallemList = from a in pipo
                                  group a by a.Barnamehaftegi_MoallemID into g
                                  select new
                                  {
                                      Moallem_ID = g.FirstOrDefault().Barnamehaftegi_MoallemID,
                                      Moallem_FullName = g.FirstOrDefault().Barnamehaftegi_MoallemFullName
                                  };
            var pipoDoroosList = from a in pipo
                                 group a by a.Barnamehaftegi_DoroosName into g
                                 select new
                                 {
                                     Doroos_ID = g.FirstOrDefault().Barnamehaftegi_DoroosID,
                                     Doroos_Name = g.FirstOrDefault().Barnamehaftegi_DoroosName
                                 };

            ArrayList arraypipoMoallemList = new ArrayList(pipoMoallemList.ToList());
            arraypipoMoallemList.Insert(0, new { Moallem_ID = 0, Moallem_FullName = "انتخاب معلم" });
            ArrayList arraypipoDoroosList = new ArrayList(pipoDoroosList.ToList());
            arraypipoDoroosList.Insert(0, new { Doroos_ID = 0, Doroos_Name = "انتخاب درس" });
            ViewBag.MoallemList = arraypipoMoallemList;// new SelectList(arraypipoMoallemList, "Moallem_ID", "Moallem_FullName");
            ViewBag.DoroosList = arraypipoDoroosList;// new SelectList(arraypipoDoroosList, "Doroos_ID", "Doroos_Name");

            var temp = BD.Details(kelasId);
            if (temp == null)
            {
               return RedirectToAction("create", new { NemayandegiId = NemayandegiId, ModirId = ModirId, kelasId = kelasId });
            }
            return View(temp);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "BarnameHaftegi_Edit")]
        public ActionResult Edit(BarnameHaftegi_ModelList barnamehaftegi, int? NemayandegiId, int? ModirId, int kelasId)
        {
            int? MadreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            var kelas = db.Kelas.Where(u => u.isDeleted == false && u.F_MadaresID == MadreseId && u.ID == kelasId).FirstOrDefault();
            if (kelas == null)
            {
                return View("NotFound");
            }
            else
            {
                if (kelas.BarnameHaftegi.Where(u=>u.isDeleted==false).Count() == 0)
                {
                    @ViewBag.isKlashasBarname = false;
                    return View();
                }
            }
            @ViewBag.isKlashasBarname = true;
            @ViewBag.isKlasNull = false;
            @ViewBag.MaxZang = kelas.MaxZang;
            BarnameHaftegi_BLL BB = new BarnameHaftegi_BLL();
            BB.FreeBarnameHaftegi(kelas.ID);
            string result = BB.BarnameHaftegi_Update(barnamehaftegi, kelasId, kelas.F_PayeID ?? default(int), HttpContext.Items["ParrentId"] as string);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListKelas", "Kelas", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            else
            {
                @ViewBag.isKlashasBarname = false;
                @ViewBag.isKlasNull = false;
                @ViewBag.MaxZang = kelas.MaxZang;
                ViewBag.jsNotifyMessage = result;
                return View(barnamehaftegi);
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
