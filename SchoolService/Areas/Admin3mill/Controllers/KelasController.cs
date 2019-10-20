using SchoolService.Areas.Admin3mill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SchoolService.Models.DataModel;
using SchoolService.Models.BLL;
using SchoolService.CustomFilters;
using SchoolService.Models.DAL;
using SchoolService.Models.AndroidJsonModel;
using System.Globalization;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace SchoolService.Areas.Admin3mill.Controllers
{
    [AuthLog(Roles = "Admin,Nemayandegi,Modir")]
    public class KelasController : Controller
    {

        public KelasController()
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
        [PageTittleAttributeActionFilter(Function = "Kelas_ListKelas")]
        public ActionResult ListKelas(int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            int? MadreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            ViewBag.F_MadreseId = MadreseId;
            ViewBag.jsNotifyMessage = TempData["Notification"];
            KelasManagement sm = new KelasManagement();
            ViewBag.ListTarikh = Tools.Months();
            return View(sm.ListKelas(MadreseId ?? default(int)));
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Kelas_AddKelas")]
        public ActionResult AddKelas(int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            int? MadreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            MadaresManagement mm = new MadaresManagement();
            var madrese = mm.DetailMadares(MadreseId ?? default(int), Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int)));
            PaayeManagement pg = new PaayeManagement();
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "1", Value = "1" });
            listItems.Add(new SelectListItem { Text = "2", Value = "2" });
            listItems.Add(new SelectListItem { Text = "3", Value = "3" });
            listItems.Add(new SelectListItem { Text = "4", Value = "4" });
            listItems.Add(new SelectListItem { Text = "5", Value = "5" });
            ViewBag.Zang = listItems;
            ViewBag.Paaye = pg.PaayeCombo(madrese.F_MaghaateID ?? default(int));
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Kelas_AddKelas")]
        public ActionResult AddKelas(Kelas model, int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            int? MadreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            MadaresManagement mad = new MadaresManagement();
            var madrese = mad.DetailMadares(MadreseId ?? default(int), Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int)));


            KelasManagement mm = new KelasManagement();
            PaayeManagement pg = new PaayeManagement();
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "1", Value = "1" });
            listItems.Add(new SelectListItem { Text = "2", Value = "2" });
            listItems.Add(new SelectListItem { Text = "3", Value = "3" });
            listItems.Add(new SelectListItem { Text = "4", Value = "4" });
            listItems.Add(new SelectListItem { Text = "5", Value = "5" });
            ViewBag.Zang = listItems;
            ViewBag.Paaye = pg.PaayeCombo(madrese.F_MaghaateID ?? default(int));
            model.F_ParrentID = HttpContext.Items["ParrentId"] as string;
            model.F_MadaresID = MadreseId;
            string result = mm.AddKelas(model, MadreseId ?? default(int), ModelState);
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListKelas", "Kelas", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            else
            {
                ViewBag.jsNotifyMessage = result;
                return View(model);
            }
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Kelas_EditKelas")]
        public ActionResult EditKelas(int? NemayandegiId, int? ModirId, int KelasId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            int? MadreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            MadaresManagement mad = new MadaresManagement();
            var madrese = mad.DetailMadares(MadreseId ?? default(int), Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int)));

            KelasManagement mm = new KelasManagement();
            PaayeManagement pg = new PaayeManagement();
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "1", Value = "1" });
            listItems.Add(new SelectListItem { Text = "2", Value = "2" });
            listItems.Add(new SelectListItem { Text = "3", Value = "3" });
            listItems.Add(new SelectListItem { Text = "4", Value = "4" });
            listItems.Add(new SelectListItem { Text = "5", Value = "5" });
            ViewBag.Zang = listItems;
            ViewBag.Paaye = pg.PaayeCombo(madrese.F_MaghaateID ?? default(int));
            var model = mm.DetailKelas(KelasId, MadreseId ?? default(int));
            if (model != null)
            {
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
        [PageTittleAttributeActionFilter(Function = "Kelas_EditKelas")]
        public ActionResult EditKelas(Kelas model, int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            int? MadreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            MadaresManagement mad = new MadaresManagement();
            var madrese = mad.DetailMadares(MadreseId ?? default(int), Tools.F_UserID_Nemayandegi(NemayandegiId ?? default(int)));
            if (madrese == null)
            {
                return View("NotFound");
            }
            KelasManagement mm = new KelasManagement();
            PaayeManagement pg = new PaayeManagement();
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "1", Value = "1" });
            listItems.Add(new SelectListItem { Text = "2", Value = "2" });
            listItems.Add(new SelectListItem { Text = "3", Value = "3" });
            listItems.Add(new SelectListItem { Text = "4", Value = "4" });
            listItems.Add(new SelectListItem { Text = "5", Value = "5" });
            ViewBag.Zang = listItems;
            ViewBag.Paaye = pg.PaayeCombo(madrese.F_MaghaateID ?? default(int));
            string result = mm.EditKelas(model);

            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListKelas", "Kelas", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
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
        public ActionResult DeleteKelas(int KelasId, int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            int? MadreseId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);

            KelasManagement sm = new KelasManagement();
            string result = sm.DeleteKelas(KelasId, MadreseId ?? default(int));
            if (result == "success")
            {
                TempData["Notification"] = result;
                return RedirectToAction("ListKelas", "Kelas", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
            }
            else
            {
                ViewBag.jsNotifyMessage = result;
                return RedirectToAction("DetailKelas", "Kelas", new { NemayandegiId = NemayandegiId, ModirId = ModirId, KelasId = KelasId });
            }
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Kelas_DetailKelas")]
        public ActionResult DetailKelas(int KelasId, int? NemayandegiId, int? ModirId)
        {
            //string ParrentId;
            //NemayandegiId = NemayandegiIdPrepare(NemayandegiId);
            //ModirId = ModirIdPrepare(ModirId, out ParrentId);
            int? MadaresId = Tools.MadreseId(HttpContext.Items["ParrentId"] as string);
            KelasManagement sm = new KelasManagement();
            var model = sm.DetailKelas(KelasId, MadaresId ?? default(int));
            if (model != null)
                return View(model);
            else
                return View("NotFound");
        }

        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Kelas_SabteNomre")]
        public ActionResult SabteNomre(int? NemayandegiId, int? ModirId, int KelasId)
        {
            DoroosManagement dm = new DoroosManagement();
            var Doroos = dm.ListeDorooseKelasha(KelasId);
            DaneshAmoozManagement dam = new DaneshAmoozManagement();
            ViewBag.DaneshAmuzan = dam.ListStudents(KelasId);
            if (Doroos != null)
                ViewBag.Doroos = new SelectList(Doroos.Select(u => new { Value = u.BarnameHaftegiId, Text = u.NameDars }), "Value", "Text");
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "هفته اول", Value = "1" });
            listItems.Add(new SelectListItem { Text = "هفته دوم", Value = "2" });
            listItems.Add(new SelectListItem { Text = "هفته سوم", Value = "3" });
            listItems.Add(new SelectListItem { Text = "هفته چهارم", Value = "4" });
            ViewBag.Hafte = listItems;
            return View();
        }
        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Kelas_SabteNomre")]
        public ActionResult SabteNomre(int? NemayandegiId, int? ModirId, SabteNomreModel model)
        {
            var asm = new DaneshAmoozManagement();
            string result = asm.SetClassScoreForMany(model);
            TempData["Notification"] = result;
            return RedirectToAction("ListKelas", "Kelas", new { NemayandegiId = NemayandegiId, ModirId = ModirId });
        }
        public static double? Parsedouble(string str)
        {
            double? defaultValue = null;
            double result;
            return double.TryParse(str, out result) ? result : defaultValue;
        }
        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        public ActionResult DaryafteKarname(int? NemayandegiId, int? ModirId, int KelasId,string Sal, string Mah = "07")
        {
            //  System.Web.HttpContext.Current.Server.ScriptTimeout = 1000;
            var db = new SCEntities();
            // db.Database.CommandTimeout = 600;
            string ParrentId = HttpContext.Items["ParrentId"] as string;

            db.Configuration.LazyLoadingEnabled = true;
            int? F_MadreseId = Tools.MadreseId(ParrentId);
            bool Allow = true;
            var mad = db.Madaares.FirstOrDefault(u => u.ID == F_MadreseId);
            if (mad != null && mad.Maghaate.NaameMaghta == "ابتدایی")
                Allow = false;

            var moaven = db.Karmandaan.Include(u => u.UserInformation).FirstOrDefault(u => u.Semat == "Moaven" && u.F_ParrentID == ParrentId && u.UserInformation.F_MadaaresID == F_MadreseId && u.UserInformation.isDeleted == false && u.UserInformation.Status == true);
            var modir = db.Karmandaan.Include(u => u.UserInformation).FirstOrDefault(u => u.Semat == "Modir" && u.UserInformation.F_MadaaresID == F_MadreseId && u.UserInformation.isDeleted == false && u.UserInformation.Status == true);
            string MoavenFirstName = "";
            string MoavenLastName = "";
            string ModirFirstName = "";
            string ModirLastName = "";
            if (moaven != null)
            {
                MoavenFirstName = moaven.UserInformation.FirstName;
                MoavenLastName = moaven.UserInformation.LastName;
            }
            if (modir != null)
            {
                ModirFirstName = modir.UserInformation.FirstName;
                ModirLastName = modir.UserInformation.LastName;
            }
         int? paye=   db.Kelas.Find(KelasId).F_PayeID;
            PersianCalendar p = new PersianCalendar(); var now = DateTime.Now; DateTime date;
            if (mad == null || mad.SaleTahsili == null || string.IsNullOrEmpty(mad.SaleTahsili.Sal))
                Tools.GetJalaliDateReturnDateTime("1500/01/01 00:00:00", out date);
            else
                Tools.GetJalaliDateReturnDateTime("13" + Sal + "/" + Mah + "/01 00:00:00", out date);
            string Maah = Tools.Months().FirstOrDefault(u => u.Value == Mah).Text;
            DateTime date2 = date.AddMonths(1);
            var Query = (from st in db.BarnameHaftegi
                         join map in db.Mapping_Moallem_Doroos on st.F_MoallemDoroosID equals map.ID
                         join doroos in db.Doroos on map.F_DoroosID equals doroos.ID
                         where doroos.isDeleted == false
                         join NomarateKelasi in db.NomreKelasi on st.ID equals NomarateKelasi.F_BarnameHaftegiID
                         where NomarateKelasi.Tarikh >= date && NomarateKelasi.Tarikh < date2          
                         join daneshamuz in db.DaneshAmuz on NomarateKelasi.F_DaneshAmuzID equals daneshamuz.ID
                         where daneshamuz.isDeleted == false && daneshamuz.Ovlia.UserInformation.F_MadaaresID == F_MadreseId && daneshamuz.Kelas.F_PayeID==paye
                         select new
                         {
                             daneshamuz_ID = daneshamuz.ID,
                             daneshamuz_firstname = daneshamuz.FirstName,
                             daneshamuz_lastname = daneshamuz.LastName,
                             daneshamuz_CodeMelli = daneshamuz.CodeMelli,
                             daneshamuz_FatherName = daneshamuz.Ovlia.UserInformation.FirstName,

                             kelas_id = daneshamuz.Kelas.ID,
                             kelas_name = daneshamuz.Kelas.NaameKelas,
                             maghta = daneshamuz.Kelas.Paaye.Maghaate.NaameMaghta,
                             paye = daneshamuz.Kelas.Paaye.NaamePaye,

                             dars_id = doroos.ID,
                             dars_name = doroos.NaameDars,

                             nomre = NomarateKelasi.Nomre,
                             nomre_tarikh = NomarateKelasi.Tarikh,

                             madaresname = daneshamuz.Ovlia.UserInformation.Madaares.NaameMadrese,
                             saletahsili = daneshamuz.Ovlia.UserInformation.Madaares.SaleTahsili.Sal
                         }).ToList();

            var pp = (from que in Query
                      group que by new { que.daneshamuz_ID, que.kelas_id, que.dars_id } into poi
                      select new Ranking
                      {
                          darsID = poi.FirstOrDefault().dars_id,
                          KelasID = poi.FirstOrDefault().kelas_id,
                          daneshamuzID = poi.FirstOrDefault().daneshamuz_ID,
                          avg = (from a in poi let intnomre = Parsedouble(a.nomre) where intnomre != null select intnomre).Average()// poi.Where(u => SqlFunctions.IsNumeric(u.nomre)==1).Average(u => Parsedouble(u.nomre))

                      }).ToList();
            
            var rankdarsbyeklas = from s in pp
                              group s by s.KelasID into soi
                              select new
                              {
                                  klasid = soi.FirstOrDefault().KelasID,

                                  rankdars = from oi in soi
                                             group oi by oi.darsID into woi
                                             select new
                                             {
                                                 darsid = woi.FirstOrDefault().darsID,
                                                 avglist = woi.Where(u=>u.avg!=null).OrderByDescending(u => u.avg).Select(u => u.avg).Distinct().ToList()
                                             }
                              };
            var rankdarsbymadrese = from oi in pp
                                group oi by oi.darsID into woi
                                select new
                                {
                                    darsid = woi.FirstOrDefault().darsID,
                                    avglist = woi.Where(u => u.avg != null).OrderByDescending(u => u.avg).Select(u => u.avg).Distinct().ToList()
                                };

          
            var Query_Daneshamuzankelas = Query.Where(u => u.kelas_id == KelasId);
            var daneshamuzan = (from a in Query
                                group a by a.daneshamuz_ID into gp
                                select new
                                {
                                    firstname = gp.FirstOrDefault().daneshamuz_firstname,
                                    lastname = gp.FirstOrDefault().daneshamuz_lastname,
                                    codemeli = gp.FirstOrDefault().daneshamuz_CodeMelli,
                                    fathername = gp.FirstOrDefault().daneshamuz_FatherName,
                                    kelas = gp.FirstOrDefault().kelas_name,
                                    madrese = gp.FirstOrDefault().madaresname,
                                    maghta = gp.FirstOrDefault().maghta,
                                    paye = gp.FirstOrDefault().paye,
                                    sal = gp.FirstOrDefault().saletahsili,
                                    kelasid = gp.FirstOrDefault().kelas_id,

                                    daneshamuzID = gp.FirstOrDefault().daneshamuz_ID,
                                    nomre = from b in gp
                                            group b by b.dars_id into qw
                                            select new
                                            {
                                                namedars = qw.FirstOrDefault().dars_name,
                                                nomreha = qw.Select(u => u.nomre).ToList(),
                                                avgdars = (from a in qw let intnomre = Parsedouble(a.nomre) where intnomre != null select intnomre).Average(),//qw.Where(u => SqlFunctions.IsNumeric(u.nomre) == 1).Average(u => Parsedouble(u.nomre)),
                                                iddars = qw.FirstOrDefault().dars_id
                                            },
                                    moaddel = (from a in gp let intnomre = Parsedouble(a.nomre) where intnomre != null select intnomre).Average()//gp.Where(u => SqlFunctions.IsNumeric(u.nomre)==1).Average(u => Parsedouble(u.nomre)),
                                });

            var daneshamuznklas = daneshamuzan.Where(u => u.kelasid == KelasId);
            List<KarnameModel> Result = new List<KarnameModel>();
            foreach (var pipo in daneshamuznklas)
            {
                KarnameModel model = new KarnameModel();
                model.LastName = pipo.firstname;
                model.FirstName = pipo.lastname;
                model.CodeMelli = pipo.codemeli;
                model.FatherName = pipo.fathername;
                model.Kelas = pipo.kelas;
                model.Madrese = pipo.madrese;
                model.Maghta = pipo.maghta;
                model.Mah = Mah;
                model.MoavenFirstName = MoavenFirstName;
                model.MoavenLastName = MoavenLastName;
                model.ModirFirstName = ModirFirstName;
                model.ModirLastName = ModirLastName;
                model.Paaye = pipo.paye;
                model.SaleTahsili = pipo.sal;

                foreach (var q in pipo.nomre)
                {
                    KarnameNomreModel m = new KarnameNomreModel();
                    m.NaameDars = q.namedars;
                    foreach (var uy in q.nomreha)
                    {
                        m.NomreDars = m.NomreDars + uy + ",";
                    }
                    m.NomreDars = m.NomreDars.TrimEnd(',');
                    //var qa=daneshamuzan.Where(u=>u.kelasid==pipo.kelasid);
                    //from a qa.
                    if (q.avgdars != null)
                    {
                        m.RotbeDarKelas = (rankdarsbyeklas.FirstOrDefault(u => u.klasid == pipo.kelasid).rankdars.FirstOrDefault(u => u.darsid == q.iddars).avglist.FindIndex(u => u.Value == q.avgdars) + 1).ToString();
                        m.RotbeDarMadrese = (rankdarsbymadrese.FirstOrDefault(u => u.darsid == q.iddars).avglist.FindIndex(u => u.Value == q.avgdars) + 1).ToString();
                    }
                    model.Nomarat.Add(m);
                }
                if (pipo.moaddel!= null)
                {

                    model.MoaddeleKol = Math.Round((pipo.moaddel ?? default(double)), 2).ToString();
                    model.RotbeDarKelas = (daneshamuznklas.OrderByDescending(u => u.moaddel).Select(u => u.moaddel).Distinct().ToList().FindIndex(u => u.Value == pipo.moaddel) + 1).ToString(); //(rankdaneshjoobyeklas.FirstOrDefault(u => u.klasid == pipo.kelasid).rankdaneshjoo.FirstOrDefault(u => u.daneshjooid == pipo.daneshamuzID).avglist.FindIndex(u => u.Value == pipo.moaddel) + 1).ToString();

                    model.RotbeDarMadrese = (daneshamuzan.OrderByDescending(u => u.moaddel).Select(u => u.moaddel).Distinct().ToList().FindIndex(u => u.Value == pipo.moaddel) + 1).ToString();
                }
                

                Result.Add(model);
            }


            //  //foreach (var daneshamuz in Query_Daneshamuzan)
            //  //{
            //  //    daneshamuz.
            //  //}
            //  foreach (var pipo in Query_Daneshamuzankelas)
            //  {

            //      KarnameModel model = new KarnameModel();
            //      model.Mah = Mah;
            //      model.Madrese = pipo.madaresname;
            //      model.LastName = pipo.daneshamuz_lastname;
            //      model.FirstName = pipo.daneshamuz_firstname;
            //      model.CodeMelli = pipo.daneshamuz_CodeMelli;
            //      model.FatherName = pipo.daneshamuz_FatherName;
            //      model.Kelas = pipo.kelas_name;
            //      model.Maghta = pipo.maghta;
            //      model.Paaye = pipo.paye;
            //      model.SaleTahsili = pipo.saletahsili;
            //      model.RotbeDarKelas = "";
            //      model.RotbeDarMadrese = "";

            //      model.MoavenFirstName =MoavenFirstName;
            //      model.MoavenLastName = MoavenLastName;
            //      model.ModirFirstName = ModirFirstName;
            //      model.ModirLastName = ModirLastName;
            //    //  var daneshamuz_dars=pipo.NomarateKelasi.. ;
            //    //  var Query_Daneshamuz_bydars = from c in daneshamuz_dars group c by p

            //      KarnameNomreModel m = new KarnameNomreModel();


            //  }
            //  DaneshAmoozManagement sm = new DaneshAmoozManagement();
            //  var DaneshAmuzan = db.DaneshAmuz.Include(u=>u.Ovlia.UserInformation).Where(u => u.F_KelasID == KelasId && u.Status == true && u.isDeleted == false && u.Ovlia.UserInformation.F_MadaaresID == F_MadreseId);
            ////  List<KarnameModel> Result = new List<KarnameModel>();

            //  var temp1 = from st in db.BarnameHaftegi
            //              join map in db.Mapping_Moallem_Doroos on st.F_MoallemDoroosID equals map.ID
            //              join doroos in db.Doroos on map.F_DoroosID equals doroos.ID
            //              join NomarateKelasi in db.NomreKelasi on st.ID equals NomarateKelasi.F_BarnameHaftegiID
            //              select new Nomre_Model
            //              {
            //                  F_DaneshAmuzID=NomarateKelasi.F_DaneshAmuzID,
            //                  F_kelasid=st.F_KelasID,
            //                  ID=doroos.ID,
            //                  TarikhDate=NomarateKelasi.Tarikh,
            //                  NomreDars = NomarateKelasi.Nomre,
            //                  NaameDars = doroos.NaameDars
            //              };

            //  DoroosManagement DM = new DoroosManagement();
            //  var Doroos = DM.DoroosCombo(KelasId , F_MadreseId ?? default(int));
            //  List<int> DrasIds = Doroos.Select(y => y.Value).Select(int.Parse).ToList();
            //  var BarnameHayeHaftegi = db.BarnameHaftegi.Where(u => DrasIds.Contains(u.Mapping_Moallem_Doroos.F_DoroosID ?? default(int))).Select(u => u.ID).ToList();
            //  var nomarat = db.NomreKelasi.Where(u => BarnameHayeHaftegi.Contains(u.F_BarnameHaftegiID ?? default(int)) && u.isDeleted == false);
            //  var barname = db.BarnameHaftegi.Include(u => u.Mapping_Moallem_Doroos);
            //  //var kelas = db.Kelas.FirstOrDefault(u => u.ID == KelasId);
            //  //var DaneshAmuzaneDars = DaneshAmuzan.Where(u => u.Kelas.F_PayeID == kelas.F_PayeID && u.Kelas.Paaye.F_MaghaateID == kelas.Paaye.F_MaghaateID).ToList();
            //  //foreach (var item in DaneshAmuzaneDars)
            //  //{
            //  //    var temm = mynomarat.Where(u => (u.Tarikh > date || u.Tarikh == date) && u.Tarikh < date2 && u.F_DaneshAmuzID == item.ID).Select(y => y.Nomre).ToList();
            //  //    MadreseAverages.Add(Average(temm));
            //  //}
            //  foreach (var DaneshAmuz in DaneshAmuzan)
            //  {
            //      KarnameModel model = new KarnameModel();
            //      model.Mah = Maah;
            //      model.Madrese = DaneshAmuz.Ovlia.UserInformation.Madaares.NaameMadrese;
            //      model.LastName = DaneshAmuz.LastName;
            //      model.FirstName = DaneshAmuz.FirstName;
            //      model.CodeMelli = DaneshAmuz.CodeMelli;
            //      model.FatherName = DaneshAmuz.Ovlia.UserInformation.FirstName;
            //      model.Kelas = DaneshAmuz.Kelas.NaameKelas;
            //      model.Maghta = DaneshAmuz.Kelas.Paaye.Maghaate.NaameMaghta;
            //      model.Paaye = DaneshAmuz.Kelas.Paaye.NaamePaye;
            //      model.SaleTahsili = DaneshAmuz.Ovlia.UserInformation.Madaares.SaleTahsili.Sal;
            //      model.RotbeDarKelas = "";
            //      model.RotbeDarMadrese = "";

            //       model.MoavenFirstName =MoavenFirstName;
            //      model.MoavenLastName=MoavenLastName;
            //      model.ModirFirstName = ModirFirstName;
            //      model.ModirLastName = ModirLastName;
            //      //var moaven = db.Karmandaan.FirstOrDefault(u => u.Semat == "Moaven" && u.F_ParrentID == ParrentId && u.UserInformation.F_MadaaresID == F_MadreseId && u.UserInformation.isDeleted == false && u.UserInformation.Status == true);
            //      //if (moaven != null)
            //      //    model.MoavenFirstName = moaven.UserInformation.FirstName;
            //      ////int modirId = DaneshAmuz.Ovlia.UserInformation.Madaares.ModirID ?? default(int);
            //      //model.MoavenLastName = moaven.UserInformation.LastName;
            //      ////var modir = db.Karmandaan.FirstOrDefault(u => u.Semat == "Modir" && u.UserInformation.F_MadaaresID == F_MadreseId && u.UserInformation.isDeleted == false && u.UserInformation.Status == true);
            //      //if (modir != null)
            //      //{
            //      //    model.ModirFirstName = modir.UserInformation.FirstName;
            //      //    model.ModirLastName = modir.UserInformation.LastName;
            //      //}
            //      double Nothing;
            //      //DoroosManagement DM = new DoroosManagement();
            //      //var Doroos = DM.DoroosCombo(DaneshAmuz.F_KelasID ?? default(int), F_MadreseId ?? default(int));
            //      Doroos_DAL dal = new Doroos_DAL(new SCEntities());
            //      double score = 0;
            //      int count = 0;
            //      //List<int> DrasIds = Doroos.Select(y => y.Value).Select(int.Parse).ToList();
            //      //var BarnameHayeHaftegi = db.BarnameHaftegi.Where(u => DrasIds.Contains(u.Mapping_Moallem_Doroos.F_DoroosID ?? default(int))).Select(u => u.ID).ToList();
            //      //var nomarat = db.NomreKelasi.Where(u => BarnameHayeHaftegi.Contains(u.F_BarnameHaftegiID ?? default(int)) && u.isDeleted == false);
            //      foreach (var item in Doroos)
            //      {
            //          var temm = dal.KarnameDarsNomre(F_MadreseId ?? default(int), DaneshAmuz, int.Parse(item.Value), date, temp1);
            //          KarnameNomreModel m = new KarnameNomreModel();
            //          m.NaameDars = item.Text;
            //          foreach (var item2 in temm)
            //          {
            //              if (!string.IsNullOrEmpty(item2.NomreDars)) {
            //              item2.NomreDars = Tools.ToLatin(item2.NomreDars);
            //              }
            //              m.NomreDars += item2.NomreDars + " , ";
            //              if (double.TryParse(item2.NomreDars, out Nothing))
            //              {
            //                  score += Convert.ToDouble(item2.NomreDars);
            //                  count++;
            //              }
            //          }
            //          if (!string.IsNullOrEmpty(m.NomreDars))
            //          {
            //              m.NomreDars = m.NomreDars.Remove(m.NomreDars.LastIndexOf(','));
            //              if (Allow)
            //              {
            //                 // List<string> temp = sm.DaryafteRotbe(KelasId, int.Parse(item.Value), date, F_MadreseId ?? default(int), DaneshAmuz.ID, nomarat.ToList(), barname, DaneshAmuz, DaneshAmuzan);
            //                  m.RotbeDarKelas = "-";// temp[0];
            //                  m.RotbeDarMadrese = "-";// temp[1];
            //              }
            //          }
            //          model.Nomarat.Add(m);
            //      }
            //      if (count > 0 && Allow)
            //      {
            //          double meghdar = score / count;
            //          int meghdar2 = (int)(meghdar * 100); meghdar = meghdar2;
            //          model.MoaddeleKol = (meghdar / 100) + "";
            //      }
            //      if (Allow)
            //      {
            //          var RotbehayeKoll = sm.DaryafteRotbeKolli(KelasId, date, F_MadreseId ?? default(int), DaneshAmuz.ID, nomarat.ToList(), barname, DaneshAmuz, DaneshAmuzan);
            //          model.RotbeDarKelas =  RotbehayeKoll[0];
            //          model.RotbeDarMadrese = RotbehayeKoll[1];
            //      }
            //      Result.Add(model);
            //  }
            Result = Result.OrderBy(u =>Parsedouble(u.RotbeDarKelas)).ToList();
            return View(Result);
        }



        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Kelas_RizNomarat")]
        public ActionResult RizNomarat(int? NemayandegiId, int? ModirId, int KelasId, int DarsId = -1)
        {
            DoroosManagement dm = new DoroosManagement();
            ViewBag.Doroos = dm.ComboDorooseKelasha(KelasId);
            ViewBag.KelasId = KelasId;
            ViewBag.NemayandegiId = NemayandegiId;
            ViewBag.ModirId = ModirId;
            return View(DarsId);
        }
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Kelas_RizNomarat")]
        public ActionResult _RizNomaratLoad(int? NemayandegiId, int? ModirId, int KelasId, int DarsId = -1, string DateFrom = "1396/07/01", string DateTo = "1450/09/01")
        {
            if (string.IsNullOrEmpty(DateFrom)) DateFrom = "1396/07/01";
            if (string.IsNullOrEmpty(DateTo)) DateTo = "1450/09/01";
            DateTime From;
            Tools.GetJalaliDateReturnDateTime(DateFrom + " 00:00:00", out From);
            DateTime To;
            Tools.GetJalaliDateReturnDateTime(DateTo + " 00:00:00", out To);
            var db = new SCEntities();
            NomreKelasi_DAL dal = new NomreKelasi_DAL(db);
            var model = dal.RizNomreKelasi(KelasId, From, To, DarsId);
            ViewBag.KelasId = KelasId;
            ViewBag.DarsId = DarsId;
            ViewBag.NemayandegiId = NemayandegiId;
            ViewBag.ModirId = ModirId;
            return PartialView(model);
        }
        [HttpPost]
        [NemayandegiIdPrepareAttributeActionFilter]
        [ModirIdPreparePrepareAttributeActionFilter]
        [PageTittleAttributeActionFilter(Function = "Kelas_RizNomarat")]
        public ActionResult RizNomaratEdit(int? NemayandegiId, int? ModirId, List<Nomre_Model> model, int KelasId, int DarsId)
        {
            string ParrentId = HttpContext.Items["ParrentId"] as string;
            int F_MadreseId = Tools.MadreseId(ParrentId) ?? default(int);
            var asm = new KelasManagement();
            string result = asm.ChangeRizNomreKelasi(model, F_MadreseId);
            TempData["Notification"] = result;
            return RedirectToAction("RizNomarat", "Kelas", new { NemayandegiId = NemayandegiId, ModirId = ModirId, KelasId = KelasId, DarsId = DarsId });
        }
    }
}