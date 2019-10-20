using SchoolService.Areas.Admin3mill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolService.Models.BLL;
using SchoolService.Models.DataModel;
using PagedList;
using SchoolService.CustomFilters;
using System.Globalization;
using SchoolService.Models.DAL;
using SchoolService.Models.AndroidJsonModel;
using System.Data.Entity;

namespace SchoolService.Areas.Admin3mill.Controllers
{
    [AuthLog(Roles = "Admin,Nemayandegi,Modir")]
    public class DaneshAmoozController : Controller
    {
        public DaneshAmoozController()
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
        [PageTittleAttributeActionFilter(Function = "DaneshAmooz_ListDaneshAmooz")]
        public ActionResult ListDaneshAmooz(int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            ViewBag.jsNotifyMessage = TempData["Notification"];
            DaneshAmoozManagement sm = new DaneshAmoozManagement();
            return View(sm.ListDaneshAmuz(HttpContext.Items["ParrentId"] as string));
        }
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "DaneshAmooz_AddDaneshAmooz")]
        public ActionResult AddDaneshAmooz(int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "مرد", Value = "true" });
            listItems.Add(new SelectListItem { Text = "زن", Value = "false" });
            ViewBag.Jensiat = listItems;
            var listItems2 = new List<SelectListItem>();
            listItems2.Add(new SelectListItem { Text = "پدر", Value = "پدر" });
            listItems2.Add(new SelectListItem { Text = "مادر", Value = "مادر" });
            listItems2.Add(new SelectListItem { Text = "برادر", Value = "برادر" });
            listItems2.Add(new SelectListItem { Text = "خواهر", Value = "خواهر" });
            listItems2.Add(new SelectListItem { Text = "سایر", Value = "سایر" });
            ViewBag.Nesbat = listItems2;
            KelasManagement KM = new KelasManagement();
            ViewBag.Class = KM.ComboBoxeKelasha(Tools.MadreseId(HttpContext.Items["ParrentId"] as string) ?? default(int));
            OvliaManagement OM = new OvliaManagement();
            ViewBag.Ovlia = OM.ComboBoxeOvlia(Tools.MadreseId(HttpContext.Items["ParrentId"] as string) ?? default(int));
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "DaneshAmooz_AddDaneshAmooz")]
        public ActionResult AddDaneshAmooz(DaneshAmuz model, int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            DaneshAmoozManagement mm = new DaneshAmoozManagement();
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "مرد", Value = "true" });
            listItems.Add(new SelectListItem { Text = "زن", Value = "false" });
            ViewBag.Jensiat = listItems;
            var listItems2 = new List<SelectListItem>();
            listItems2.Add(new SelectListItem { Text = "پدر", Value = "پدر" });
            listItems2.Add(new SelectListItem { Text = "مادر", Value = "مادر" });
            listItems2.Add(new SelectListItem { Text = "برادر", Value = "برادر" });
            listItems2.Add(new SelectListItem { Text = "خواهر", Value = "خواهر" });
            listItems2.Add(new SelectListItem { Text = "سایر", Value = "سایر" });
            ViewBag.Nesbat = listItems2;
            KelasManagement KM = new KelasManagement();
            ViewBag.Class = KM.ComboBoxeKelasha(Tools.MadreseId(HttpContext.Items["ParrentId"] as string) ?? default(int));
            OvliaManagement OM = new OvliaManagement();
            ViewBag.Ovlia = OM.ComboBoxeOvlia(Tools.MadreseId(HttpContext.Items["ParrentId"] as string) ?? default(int));
            int? Id;
            model.F_ParrentID = HttpContext.Items["ParrentId"] as string;
            string result = mm.AddDaneshAmuz(model, ModelState, out Id);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListDaneshAmooz", "DaneshAmooz", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            ViewBag.jsNotifyMessage = result;
            return View(model);
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "DaneshAmooz_AddDaneshAmooz")]
        public ActionResult AddDaneshAmoozWithExcel(int? NemayandegiId, int? ModirId)
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "DaneshAmooz_AddDaneshAmooz")]
        public ActionResult AddDaneshAmoozWithExcel(HttpPostedFileBase ExcelFile, int? NemayandegiId, int? ModirId)
        {
            DaneshAmoozManagement mm = new DaneshAmoozManagement();
            int? Id;
            string F_ParrentID = HttpContext.Items["ParrentId"] as string;
            string result = mm.AddDaneshAmuzWithExcel(ExcelFile, F_ParrentID, ModelState, out Id);
            if (result == "OK")
            {
                TempData["Notification"] = "success";
                return RedirectToAction("ListDaneshAmooz", "DaneshAmooz", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            ViewBag.jsNotifyMessage = result;
            return View();
        }
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "DaneshAmooz_EditDaneshAmooz")]
        public ActionResult EditDaneshAmooz(int? NemayandegiId, int? ModirId, int DaneshAmoozId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            DaneshAmoozManagement mm = new DaneshAmoozManagement();
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "مرد", Value = "true" });
            listItems.Add(new SelectListItem { Text = "زن", Value = "false" });
            ViewBag.Jensiat = listItems;
            var listItems2 = new List<SelectListItem>();
            listItems2.Add(new SelectListItem { Text = "پدر", Value = "پدر" });
            listItems2.Add(new SelectListItem { Text = "مادر", Value = "مادر" });
            listItems2.Add(new SelectListItem { Text = "برادر", Value = "برادر" });
            listItems2.Add(new SelectListItem { Text = "خواهر", Value = "خواهر" });
            listItems2.Add(new SelectListItem { Text = "سایر", Value = "سایر" });
            ViewBag.Nesbat = listItems2;

            var model = mm.DetailDaneshAmuz(DaneshAmoozId, HttpContext.Items["ParrentId"] as string);
            if (model != null)
            {
                KelasManagement KM = new KelasManagement();
                ViewBag.Class = KM.ComboBoxeKelasha(Tools.MadreseId(HttpContext.Items["ParrentId"] as string) ?? default(int));
                OvliaManagement OM = new OvliaManagement();
                ViewBag.Ovlia = OM.ComboBoxeOvlia(Tools.MadreseId(HttpContext.Items["ParrentId"] as string) ?? default(int), model.F_OvliaID);
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
        [PageTittleAttributeActionFilter(Function = "DaneshAmooz_EditDaneshAmooz")]
        public ActionResult EditDaneshAmooz(DaneshAmuz model, int? NemayandegiId, int? ModirId, int DaneshAmoozId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            DaneshAmoozManagement sm = new DaneshAmoozManagement();
            var temp = sm.DetailDaneshAmuz(model.ID, HttpContext.Items["ParrentId"] as string);
            if (temp == null)
            {
                return View("NotFound");
            }
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "مرد", Value = "true" });
            listItems.Add(new SelectListItem { Text = "زن", Value = "false" });
            ViewBag.Jensiat = listItems;
            var listItems2 = new List<SelectListItem>();
            listItems2.Add(new SelectListItem { Text = "پدر", Value = "پدر" });
            listItems2.Add(new SelectListItem { Text = "مادر", Value = "مادر" });
            listItems2.Add(new SelectListItem { Text = "برادر", Value = "برادر" });
            listItems2.Add(new SelectListItem { Text = "خواهر", Value = "خواهر" });
            listItems2.Add(new SelectListItem { Text = "سایر", Value = "سایر" });
            ViewBag.Nesbat = listItems2;
            KelasManagement KM = new KelasManagement();
            ViewBag.Class = KM.ComboBoxeKelasha(Tools.F_MadraseId());
            OvliaManagement OM = new OvliaManagement();
            ViewBag.Ovlia = OM.ComboBoxeOvlia(Tools.F_MadraseId());
            string result = sm.EditDaneshAmuz(model);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListDaneshAmooz", "DaneshAmooz", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            else
            {
                ViewBag.jsNotifyMessage = result;
                return View(model);
            }
        }

        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult ChangeDisplayDaneshAmooz(int? NemayandegiId, int? ModirId, int DaneshAmoozId)
        {

            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            DaneshAmoozManagement sm = new DaneshAmoozManagement();
            string result = sm.ChangeStatusDaneshAmuz(DaneshAmoozId, HttpContext.Items["ParrentId"] as string);
            TempData["Notification"] = result;
            return RedirectToAction("ListDaneshAmooz", "DaneshAmooz", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
        }


        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult DeleteDaneshAmooz(int? NemayandegiId, int? ModirId, int DaneshAmoozId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            DaneshAmoozManagement sm = new DaneshAmoozManagement();
            string result = sm.DeleteDaneshAmuz(DaneshAmoozId, HttpContext.Items["ParrentId"] as string);
            TempData["Notification"] = result;
            if (result == "success")
            {
                return RedirectToAction("ListDaneshAmooz", "DaneshAmooz", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            else
            {
                return RedirectToAction("DetailDaneshAmooz", "DaneshAmooz", new { NemayandegiId = NemayandegiId, ModirId = ModirId, DaneshAmoozId = DaneshAmoozId });
            }
        }
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "DaneshAmooz_DetailDaneshAmooz")]
        public ActionResult DetailDaneshAmooz(int? NemayandegiId, int? ModirId, int DaneshAmoozId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            DaneshAmoozManagement sm = new DaneshAmoozManagement();
            ViewBag.ListTarikh = Tools.Months();
            return View(sm.DetailDaneshAmuz(DaneshAmoozId, HttpContext.Items["ParrentId"] as string));
        }
        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult DaryafteKarname(int? NemayandegiId, int? ModirId, string Mah, int DaneshAmoozId, int KelasId,string Sal)
        {
            string ParrentId = HttpContext.Items["ParrentId"] as string;
            var db = new SCEntities();
            int? F_MadreseId = Tools.MadreseId(ParrentId);
            bool Allow = true;
            var mad = db.Madaares.FirstOrDefault(u => u.ID == F_MadreseId);
            if (mad != null && mad.Maghaate.NaameMaghta == "ابتدایی")
                Allow = false;
            DaneshAmoozManagement sm = new DaneshAmoozManagement();
            var DaneshAmuz = sm.DetailDaneshAmuz(DaneshAmoozId, ParrentId);
            KarnameModel model = new KarnameModel();
            PersianCalendar p = new PersianCalendar(); var now = DateTime.Now; DateTime date;
            if (mad == null || mad.SaleTahsili == null || string.IsNullOrEmpty(mad.SaleTahsili.Sal))
                Tools.GetJalaliDateReturnDateTime("1500/01/01 00:00:00", out date);
            else
                Tools.GetJalaliDateReturnDateTime("13" + Sal + "/" + Mah + "/01 00:00:00", out date);
            model.Mah = Tools.Months().FirstOrDefault(u => u.Value == Mah).Text;
            model.Madrese = DaneshAmuz.Ovlia.UserInformation.Madaares.NaameMadrese;
            model.LastName = DaneshAmuz.LastName;
            model.FirstName = DaneshAmuz.FirstName;
            model.CodeMelli = DaneshAmuz.CodeMelli;
            model.FatherName = DaneshAmuz.Ovlia.UserInformation.FirstName;
            model.Kelas = DaneshAmuz.Kelas.NaameKelas;
            model.Maghta = DaneshAmuz.Kelas.Paaye.Maghaate.NaameMaghta;
            model.Paaye = DaneshAmuz.Kelas.Paaye.NaamePaye;
            model.SaleTahsili = DaneshAmuz.Ovlia.UserInformation.Madaares.SaleTahsili.Sal;
            model.RotbeDarKelas = "";
            model.RotbeDarMadrese = "";
            var moaven = db.Karmandaan.FirstOrDefault(u => u.Semat == "Moaven" && u.F_ParrentID == ParrentId && u.UserInformation.F_MadaaresID == F_MadreseId && u.UserInformation.isDeleted == false && u.UserInformation.Status == true);
            if (moaven != null)
                model.MoavenFirstName = moaven.UserInformation.FirstName;
            int modirId = DaneshAmuz.Ovlia.UserInformation.Madaares.ModirID ?? default(int);
            model.MoavenLastName = moaven.UserInformation.LastName;
            var modir = db.Karmandaan.FirstOrDefault(u => u.Semat == "Modir" && u.UserInformation.F_MadaaresID == F_MadreseId && u.UserInformation.isDeleted == false && u.UserInformation.Status == true);
            if (modir != null)
            {
                model.ModirFirstName = modir.UserInformation.FirstName;
                model.ModirLastName = modir.UserInformation.LastName;
            }
            DoroosManagement DM = new DoroosManagement();
            var Doroos = DM.DoroosCombo(DaneshAmuz.F_KelasID ?? default(int), F_MadreseId ?? default(int));
            Doroos_DAL dal = new Doroos_DAL(new SCEntities());
            double score = 0;
            int count = 0;
            double Nothing;
            List<int> DrasIds = Doroos.Select(y => y.Value).Select(int.Parse).ToList();
            var BarnameHayeHaftegi = db.BarnameHaftegi.Where(u => DrasIds.Contains(u.Mapping_Moallem_Doroos.F_DoroosID ?? default(int))).Select(u => u.ID).ToList();
            var nomarat = db.NomreKelasi.Where(u => BarnameHayeHaftegi.Contains(u.F_BarnameHaftegiID ?? default(int)) && u.isDeleted == false).ToList();
            var temp1 = from st in db.BarnameHaftegi
                        join map in db.Mapping_Moallem_Doroos on st.F_MoallemDoroosID equals map.ID
                        join doroos in db.Doroos on map.F_DoroosID equals doroos.ID
                        join NomarateKelasi in db.NomreKelasi on st.ID equals NomarateKelasi.F_BarnameHaftegiID
                        select new Nomre_Model
                        {
                            F_DaneshAmuzID = NomarateKelasi.F_DaneshAmuzID,
                            F_kelasid = st.F_KelasID,
                            ID = doroos.ID,
                            TarikhDate = NomarateKelasi.Tarikh,
                            NomreDars = NomarateKelasi.Nomre,
                            NaameDars = doroos.NaameDars
                        };
            var barname = db.BarnameHaftegi.Include(u => u.Mapping_Moallem_Doroos);
            var DaneshAmuzan = db.DaneshAmuz.Include(u => u.Ovlia.UserInformation).Where(u => u.F_KelasID == KelasId && u.Status == true && u.isDeleted == false && u.Ovlia.UserInformation.F_MadaaresID == F_MadreseId);
            foreach (var item in Doroos)
            {
                var temm = dal.KarnameDarsNomre(F_MadreseId ?? default(int), DaneshAmuz, int.Parse(item.Value), date,temp1);
                KarnameNomreModel m = new KarnameNomreModel();
                m.NaameDars = item.Text;
                foreach (var item2 in temm)
                {
                    if (!string.IsNullOrEmpty(item2.NomreDars))
                    {
                        item2.NomreDars = Tools.ToLatin(item2.NomreDars);
                    }
                    m.NomreDars += item2.NomreDars + " , ";
                    if (double.TryParse(item2.NomreDars, out Nothing))
                    {
                        score += Convert.ToDouble(item2.NomreDars);
                        count++;
                    }
                }
                if (!string.IsNullOrEmpty(m.NomreDars))
                {
                    m.NomreDars = m.NomreDars.Remove(m.NomreDars.LastIndexOf(','));
                    if (Allow)
                    {
                        List<string> temp = sm.DaryafteRotbe(KelasId, int.Parse(item.Value), date, F_MadreseId ?? default(int), DaneshAmuz.ID, nomarat, barname,DaneshAmuz,DaneshAmuzan);
                        m.RotbeDarKelas = temp[0];
                        m.RotbeDarMadrese = temp[1];
                    }
                }
                model.Nomarat.Add(m);
            }
            if (count > 0 && Allow)
            {
                double meghdar = score / count;
                int meghdar2 = (int)(meghdar * 100); meghdar = meghdar2;
                model.MoaddeleKol = (meghdar / 100) + "";
            }
            if (Allow)
            {
                var RotbehayeKoll = sm.DaryafteRotbeKolli(KelasId, date, F_MadreseId ?? default(int), DaneshAmuz.ID, nomarat, barname, DaneshAmuz, DaneshAmuzan);
                model.RotbeDarKelas = RotbehayeKoll[0];
                model.RotbeDarMadrese = "-";// RotbehayeKoll[1];
            }
            return View(model);
        }
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "DaneshAmooz_GozaresheTahsili")]
        public ActionResult GozaresheTahsili(int? NemayandegiId, int? ModirId, int DaneshAmoozId, int KelasId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            int? MadreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            ViewBag.DaneshAmoozId = DaneshAmoozId;
            ViewBag.KelasId = KelasId;
            DoroosManagement DM = new DoroosManagement();
            ViewBag.ListDoroos = DM.DoroosCombo(KelasId, MadreseId ?? default(int));
            ViewBag.ListTarikh = Tools.Months();
            return View();
        }
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "DaneshAmooz_HuzurGhiabVaTashvighTanbih")]
        public ActionResult HuzurGhiabVaTashvighTanbih(int? NemayandegiId, int? ModirId, int DaneshAmoozId)
        {
            ViewBag.DaneshAmoozId = DaneshAmoozId;
            ViewBag.ListTarikh = Tools.Months();
            return View();
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "DaneshAmooz_DetailDaneshAmooz")]
        public ActionResult SabteNomre(int? NemayandegiId, int? ModirId, int DaneshAmoozId, int KelasId)
        {
            BarnameHaftegi_DAL dal = new BarnameHaftegi_DAL(new SCEntities());
            var Barname = ((List<AndroidBarnameHaftegi_Model>)dal.GetDaneshAmuzBarname(DaneshAmoozId)).GroupBy(x => new { x.NameDars }).Select(g => g.First()).ToList();
            if (Barname != null)
                ViewBag.Doroos = new SelectList(Barname.Select(u => new { Value = u.BarnameHaftegiId, Text = u.NameDars }), "Value", "Text");
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "هفته اول", Value = "1" });
            listItems.Add(new SelectListItem { Text = "هفته دوم", Value = "2" });
            listItems.Add(new SelectListItem { Text = "هفته سوم", Value = "3" });
            listItems.Add(new SelectListItem { Text = "هفته چهارم", Value = "4" });
            ViewBag.Hafte = listItems;
            SabteNomreModel model = new SabteNomreModel();
            model.DaneshAmoozId = DaneshAmoozId;
            return View(model);
        }
        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "DaneshAmooz_DetailDaneshAmooz")]
        public ActionResult SabteNomre(int? NemayandegiId, int? ModirId, SabteNomreModel model)
        {
            var asm = new DaneshAmoozManagement();
            string result = asm.SetClassScore(model);
            TempData["Notification"] = result;
            return RedirectToAction("ListDaneshAmooz", "DaneshAmooz", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
        }
    }
}