using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SchoolService.Areas.Admin3mill.Models;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolService.Models.DAL;
using System.Data.Entity;
using System.Threading.Tasks;
using SchoolService.Models.AndroidJsonModel;
using System.Globalization;
using System.Configuration;

namespace SchoolService.Models.BLL
{
    public class AndroidServicesManagement
    {

        public string ChangePassword(string OldPassword, string NewPassword)
        {
            using (var db = new SCEntities())
            {
                UserManager<ApplicationUser> manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var FoundedUser = manager.Find(Tools.F_UserName(), OldPassword);
                if (FoundedUser != null)
                {
                    var EditObject = db.UserInformation.FirstOrDefault(u => u.ID == FoundedUser.Id && u.isDeleted == false && u.Status == true);
                    if (EditObject != null)
                    {
                        IdentityResult result = manager.ChangePassword(FoundedUser.Id, OldPassword, NewPassword);
                        if (!result.Succeeded)
                            return Tools.GenerateJsonResponse("ChangePasswordError", "خطا در ویرایش رمز عبور");
                        else
                            return Tools.GenerateJsonResponse("Success", "رمز عبور با موفقیت ویرایش شد");
                    }
                }
                return Tools.GenerateJsonResponse("NoSuchUser", "رمز عبور فعلی وارد شده صحیح نمی باشد. لطفا مجددا تلاش کنید");
            }
        }

        public string SetClassScore(int DaneshAmoozId, int BarnameHaftegi_Id, string Tarikh, int Hafte, string Nomre, string Tozihat)
        {
            try
            {
                PersianCalendar p = new PersianCalendar(); DateTime date;
                if (Tools.GetJalaliDateReturnDateTime(p.GetYear(DateTime.Now) + "/" + Tarikh + " 00:00:00", out date))
                {
                    var NK = new NomreKelasi_DAL(new SCEntities());
                    NomreKelasi InsertObject = new NomreKelasi();
                    InsertObject.F_DaneshAmuzID = DaneshAmoozId;
                    InsertObject.Hafte = Hafte;
                    InsertObject.Tarikh = date;
                    InsertObject.Nomre = Nomre;
                    InsertObject.Tovzihat = Tozihat;
                    InsertObject.isDeleted = false;
                    InsertObject.F_BarnameHaftegiID = BarnameHaftegi_Id;
                    NK.Create(InsertObject);
                    return Tools.GenerateJsonResponse("OK", "نمره کلاسی با موفقیت ثبت شد");
                }
                throw new ArgumentNullException();
            }
            catch { return Tools.GenerateJsonResponse("NOK", "متاسفانه فرآیند ثبت نمره با خطا مواجه شده. لطفا مجددا تلاش کنید"); }
        }

        private int CheckHuzurGhiabeKelasi(int F_BarnameHaftegi, int Hafte, DateTime Tarikh, bool IsChecked)
        {
            try
            {
                var HG = new HozoorGhiab_DAL(new SCEntities());
                HozoorGhiab InsertObject = new HozoorGhiab();
                InsertObject.Hafte = Hafte;
                InsertObject.Tarikh = Tarikh;
                InsertObject.isChecked = IsChecked;
                InsertObject.isDeleted = false;
                InsertObject.F_BarnameHaftegi = F_BarnameHaftegi;
                HG.Create(InsertObject);
                return InsertObject.ID;
            }
            catch { return -1; }
        }

        public string SetHuzurGhiabeKelasi(int DaneshAmoozId, int BarnameHaftegi_Id, float Takhir, string Tozih, int Hafte, string Tarikh)
        {
            try
            {
                PersianCalendar p = new PersianCalendar(); DateTime date;
                if (Tools.GetJalaliDateReturnDateTime(p.GetYear(DateTime.Now) + "/" + Tarikh + " 00:00:00", out date))
                {
                    var db = new SCEntities();
                    int HozoorGhiabId = -1;
                    if (DaneshAmoozId != -1)
                    {
                        HozoorGhiab_DAL HG = new HozoorGhiab_DAL(db);
                        var IsFound = db.HozoorGhiab.FirstOrDefault(u => u.Tarikh == date && u.F_BarnameHaftegi == BarnameHaftegi_Id && u.Hafte == Hafte);
                        if (IsFound == null)
                            HozoorGhiabId = CheckHuzurGhiabeKelasi(BarnameHaftegi_Id, Hafte, date, true);
                        var HGK = new HozoorGhiabKelasi_DAL(db);
                        HozoorGhiabKelasi obj = new HozoorGhiabKelasi();
                        obj.F_DaneshAmuzID = DaneshAmoozId;
                        if (Takhir != -1)
                            obj.Takhir = Takhir;
                        obj.Tovzihat = Tozih;
                        obj.isDeleted = false;
                        obj.F_HozoorGhiabID = IsFound != null ? IsFound.ID : HozoorGhiabId;
                        HGK.Create(obj);
                        return Tools.GenerateJsonResponse("OK", "حضور غیاب با موفقیت ثبت شد");
                    }
                    else
                    {
                        HozoorGhiab_DAL HG = new HozoorGhiab_DAL(db);
                        var IsFound = db.HozoorGhiab.FirstOrDefault(u => u.Tarikh == date && u.F_BarnameHaftegi == BarnameHaftegi_Id && u.Hafte == Hafte);
                        if (IsFound == null)
                        {
                            HozoorGhiabId = CheckHuzurGhiabeKelasi(BarnameHaftegi_Id, Hafte, date, true);
                            return Tools.GenerateJsonResponse("OK", "حضور غیاب با موفقیت ثبت شد");
                        }
                    }
                }
                return Tools.GenerateJsonResponse("NOK", "حضور غیاب قبلا برای این تاریخ ثبت گردیده");
            }
            catch { return Tools.GenerateJsonResponse("NOK", "متاسفانه فرآیند ثبت نمره با خطا مواجه شده. لطفا مجددا تلاش کنید"); }
        }

        public string SetTaklifeKelasi(int DaneshAmoozId, int BarnameHaftegiId, string Date, int Hafte, string OnvaneTaklif, string TozihateTaklif, HttpPostedFileBase File, string Type)
        {
            try
            {
                PersianCalendar p = new PersianCalendar(); DateTime date;
                if (Tools.GetJalaliDateReturnDateTime(p.GetYear(DateTime.Now) + "/" + Date + " 00:00:00", out date))
                {
                    if (Type == "Jami")
                    {
                        var db = new SCEntities();
                        var HGK = new TaklifKelasi_DAL(db);
                        TaklifKelasiJami InsertObject = new TaklifKelasiJami();
                        InsertObject.F_KelasID = db.DaneshAmuz.FirstOrDefault(u => u.ID == DaneshAmoozId && u.isDeleted == false).F_KelasID;
                        InsertObject.F_BarnameHaftegiID = BarnameHaftegiId;
                        InsertObject.FileTaklif = Tools.FileSave(File, "TaklifeKelasi");
                        InsertObject.TovzihateTaklif = TozihateTaklif;
                        InsertObject.OnvaneTaklif = OnvaneTaklif;
                        InsertObject.isDeleted = false;
                        InsertObject.Tarikh = date;
                        HGK.CreateJami(InsertObject);
                        return Tools.GenerateJsonResponse("OK", "تکلیف با موفقیت برای کلاس ثبت شد");
                    }
                    else
                    {
                        var HGK = new TaklifKelasi_DAL(new SCEntities());
                        TaklifKelasi InsertObject = new TaklifKelasi();
                        InsertObject.F_DaneshAmuzID = DaneshAmoozId;
                        InsertObject.F_BarnameHaftegiID = BarnameHaftegiId;
                        InsertObject.FileTaklif = Tools.FileSave(File, "TaklifeKelasi");
                        InsertObject.TovzihateTaklif = TozihateTaklif;
                        InsertObject.OnvaneTaklif = OnvaneTaklif;
                        InsertObject.isDeleted = false;
                        InsertObject.Tarikh = date;
                        HGK.Create(InsertObject);
                        return Tools.GenerateJsonResponse("OK", "تکلیف با موفقیت برای دانش آموز مورد نظر ثبت شد");
                    }
                }
                throw new ArgumentNullException();
            }
            catch { return Tools.GenerateJsonResponse("NOK", "متاسفانه فرآیند ثبت تکلیف با خطا مواجه شده. لطفا مجددا تلاش کنید"); }
        }

        public string SetTashvighoTanbiheKelasi(int DaneshAmoozId, int BarnameHaftegiId, string Tarikh, int Hafte, int Emtiaz, string Tozihat)
        {
            try
            {
                PersianCalendar p = new PersianCalendar(); DateTime date;
                if (Tools.GetJalaliDateReturnDateTime(p.GetYear(DateTime.Now) + "/" + Tarikh + " 00:00:00", out date))
                {
                    var HGK = new TashvighTanbihKelasi_DAL(new SCEntities());
                    TashvighTanbihKelasi InsertObject = new TashvighTanbihKelasi();
                    InsertObject.F_DaneshAmuzID = DaneshAmoozId;
                    if (BarnameHaftegiId != -1)
                        InsertObject.F_BarnameHaftegiID = BarnameHaftegiId;
                    InsertObject.Tovzihat = Tozihat;
                    InsertObject.Tarikh = date;
                    InsertObject.Emtiaz = Emtiaz;
                    InsertObject.isDeleted = false;
                    HGK.Create(InsertObject);
                    return Tools.GenerateJsonResponse("OK", "امتیاز با موفقیت برای دانش آموز مورد نظر ثبت شد");
                }
                throw new ArgumentNullException();
            }
            catch { return Tools.GenerateJsonResponse("NOK", "متاسفانه فرآیند ثبت امتیاز با خطا مواجه شده. لطفا مجددا تلاش کنید"); }
        }

        public string SetExamScore(int DaneshAmoozId, int BarnameHaftegiId, string Nomre, string Tozih)
        {
            try
            {
                var db = new SCEntities();
                NomreEmtehani_DAL NE = new NomreEmtehani_DAL(db);
                NomreEmtehani obj = new NomreEmtehani();
                var temp = db.BarnameHaftegi.FirstOrDefault(u => u.ID == BarnameHaftegiId && u.isDeleted == false);
                var Found = db.BarnameEmtehani.Where(u => u.F_KelasID == temp.F_KelasID && u.F_MoallemDoroosID == temp.F_MoallemDoroosID && u.isDeleted == false).OrderByDescending(u => u.TarikhKamel).FirstOrDefault();
                if (Found != null)
                {
                    var nomre = db.NomreEmtehani.FirstOrDefault(u => u.F_BarnameEmtehani == Found.ID && u.F_DaneshAmuzID == DaneshAmoozId && u.isDeleted == false);
                    if (nomre == null)
                    {
                        obj.F_BarnameEmtehani = Found.ID;
                        obj.F_DaneshAmuzID = DaneshAmoozId;
                        obj.isDeleted = false;
                        obj.Nomre = Nomre;
                        obj.Tovzihat = Tozih;
                        NE.Create(obj);
                        return Tools.GenerateJsonResponse("OK", "نمره امتحانی با موفقیت ثبت شد");
                    }
                    else
                        return Tools.GenerateJsonResponse("Iterative", "معلم گرامی، برای این مورد امتحانی قبلا نمره ثبت شده است");
                }
                else
                    return Tools.GenerateJsonResponse("NotFound", "امتحان مربوطه برای این دانش آموز تعیین نشده است !");
            }
            catch { return Tools.GenerateJsonResponse("NOK", "متاسفانه فرآیند ثبت نمره امتحانی با خطا مواجه شده. لطفا مجددا تلاش کنید"); }
        }

        public string ListClasses(int MoallemId)
        {
            Doroos_DAL dal = new Doroos_DAL(new SCEntities());
            dynamic collectionWrapper = new { Root = dal.ListTeacherDoroos(MoallemId) };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }

        public string ListStudents(int KelasId)
        {
            DaneshAmuz_DAL dal = new DaneshAmuz_DAL(new SCEntities());
            dynamic collectionWrapper = new { Root = dal.ListDaneshAmuzaneKelas(KelasId) };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }

        public string SetFarakhan(int KarmandId, int KelasId, string Movzoo, string Matn, string TarikheFarakhan)
        {
            try
            {
                PersianCalendar p = new PersianCalendar(); DateTime date;
                if (Tools.GetJalaliDateReturnDateTime(p.GetYear(DateTime.Now) + "/" + TarikheFarakhan + " 00:00:00", out date))
                {
                    var db = new SCEntities();
                    Farakhanha_DAL dal = new Farakhanha_DAL(db);
                    Farakhanha obj = new Farakhanha();
                    obj.F_KarmandanID = KarmandId;
                    obj.isDeleted = false;
                    obj.Matn = Matn;
                    obj.Movzoo = Movzoo;
                    obj.TarikheFarakhan = date;
                    dal.Create(obj);
                    Mapping_Farakhanha_Kelas obj2 = new Mapping_Farakhanha_Kelas();
                    obj2.F_FarakhanhaID = obj.ID;
                    obj2.F_KelasID = KelasId;
                    Mapping_Farakhanha_Kelas_DAL dal2 = new Mapping_Farakhanha_Kelas_DAL(db);
                    dal2.Create(obj2);
                    return Tools.GenerateJsonResponse("OK", "عملیات ثبت فراخوان با موفقیت انجام شد");
                }
                throw new ArgumentNullException();
            }
            catch { return Tools.GenerateJsonResponse("NOK", "متاسفانه فرآیند ثبت فراخوان با خطا مواجه شده. لطفا مجددا تلاش کنید"); }

        }

        public string GetFarakhan(int DaneshAmoozId)
        {
            Farakhanha_DAL dal = new Farakhanha_DAL(new SCEntities());
            List<Farakhan_Model> Result = new List<Farakhan_Model>();
            foreach (var item in dal.GetFarakhan(DaneshAmoozId))
            {
                Result.Add(new Farakhan_Model(item.Movzoo, item.Matn, Tools.GetDateTimeReturnJalaliDate(item.TarikheFarakhan)));
            }
            dynamic collectionWrapper = new { Root = Result };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }

        public string MoavenGetFarakhan(int KelasId)
        {
            Farakhanha_DAL dal = new Farakhanha_DAL(new SCEntities());
            dynamic collectionWrapper = new { Root = dal.MoavenGetFarakhan(KelasId) };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }

        public string ListDoroos(int DaneshAmuzId)
        {
            Doroos_DAL dal = new Doroos_DAL(new SCEntities());
            dynamic collectionWrapper = new { Root = dal.GetListDoroos(DaneshAmuzId) };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }

        public string SendChatMessageTo(string Text, string From_Id, string To_Id)
        {
            try
            {
                Payamha_DAL dal = new Payamha_DAL(new SCEntities());
                Payamha pm = new Payamha();
                pm.isDeleted = false;
                pm.Text = Text;
                pm.F_ToID = To_Id;
                pm.CreatedDateOnUTC = DateTime.Now;
                pm.F_FromID = From_Id;
                dal.Create(pm);
                return "OK";
            }
            catch { return "NOK"; }
        }

        public string GetChat(string From_Id, string To_Id, int pageNumber, int pageSize, out int total)
        {
            try
            {
                Payamha_DAL dal = new Payamha_DAL(new SCEntities());
                dynamic collectionWrapper = new { Root = dal.List(From_Id, To_Id, pageNumber, pageSize, out total) };
                return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
            }
            catch { total = 0; return Tools.GenerateJsonResponse("NOK", "متاسفانه فرآیند دریافت تاریخچه چت از سرور با خطا مواجه شده. لطفا مجددا تلاش کنید"); }
        }

        public string ListOvliaContacts(int DaneshAmuzId)
        {
            try
            {
                DaneshAmuz_DAL dal = new DaneshAmuz_DAL(new SCEntities());
                dynamic collectionWrapper = new { Root = dal.ListOvliaContacts(DaneshAmuzId) };
                return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
            }
            catch { return Tools.GenerateJsonResponse("NOK", "متاسفانه فرآیند دریافت لیست مخاطبین از سرور با خطا مواجه شده. لطفا مجددا تلاش کنید"); }
        }

        public string OvliaPreview(int DaneshAmoozId)
        {
            var db = new SCEntities();
            OvliaMainJson_Model Result = new OvliaMainJson_Model();
            NomreKelasi_DAL Nomre_dal = new NomreKelasi_DAL(db);
            Farakhanha_DAL Farakhan_dal = new Farakhanha_DAL(db);
            DaneshAmuz_DAL DaneshAmuz_dal = new DaneshAmuz_DAL(db);
            Check_DAL Check_dal = new Check_DAL(db);
            BarnameHaftegi_DAL dal = new BarnameHaftegi_DAL(new SCEntities());
            Result.Barname = dal.GetDaneshAmuzBarname(DaneshAmoozId);
            Result.Farakhanha = Farakhan_dal.AndroidFarakhanPreview(DaneshAmoozId);
            Result.Nomarat = Nomre_dal.ListForAllDoroos(DaneshAmoozId);
            Result.NafarateBartar = Nomre_dal.NafarateBartar();
            Result.NameAndPaye = DaneshAmuz_dal.NameAndPaye(DaneshAmoozId);
            Check_dal.CheckAlarm(DaneshAmoozId);
            dynamic collectionWrapper = new { Root = Result };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }

        public string GozaresheHuzurGhiab(int DaneshAmoozId, string mah)
        {
            var db = new SCEntities();
            PersianCalendar p = new PersianCalendar();
            var now = DateTime.Now;
            DateTime date;
            Tools.GetJalaliDateReturnDateTime(p.GetYear(now) + "/" + mah + "/01 00:00:00", out date);
            DateTime date2 = date.AddMonths(1);
            var temp = db.HozoorGhiabKelasi.Include(t => t.HozoorGhiab).Where(u => (u.HozoorGhiab.Tarikh > date || u.HozoorGhiab.Tarikh == date) && (u.HozoorGhiab.Tarikh < date2) && u.isDeleted == false && u.F_DaneshAmuzID == DaneshAmoozId).Select(x => new { x.HozoorGhiab.Tarikh, x.Takhir, x.Tovzihat });
            var Result = new List<Huzurghiab_Model>();
            foreach (var item in temp)
                Result.Add(new Huzurghiab_Model(Tools.SpecialJalaliFormat(item.Tarikh ?? default(DateTime)), item.Takhir.ToString(), item.Tovzihat));
            dynamic collectionWrapper = new { Root = Result };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }

        public string TakalifeDarsi(int DaneshAmoozId, string mah)
        {
            var db = new SCEntities();
            PersianCalendar p = new PersianCalendar();
            var now = DateTime.Now;
            DateTime date;
            Tools.GetJalaliDateReturnDateTime(p.GetYear(now) + "/" + mah + "/01 00:00:00", out date);
            DateTime date2 = date.AddMonths(1);
            var DaneshAmuz = db.DaneshAmuz.FirstOrDefault(u => u.ID == DaneshAmoozId && u.isDeleted == false);
            var temp = db.TaklifKelasi.Where(u => (u.Tarikh > date || u.Tarikh == date) && (u.Tarikh < date2) && u.isDeleted == false && u.F_DaneshAmuzID == DaneshAmoozId).OrderByDescending(u => u.Tarikh).Select(x => new { x.Tarikh, x.FileTaklif, x.OnvaneTaklif, x.TovzihateTaklif }).ToList();
            var Result = new List<Takalif_Model>();
            if (DaneshAmuz != null)
            {
                var temp2 = db.TaklifKelasiJami.Where(u => (u.Tarikh > date || u.Tarikh == date) && (u.Tarikh < date2) && u.isDeleted == false && u.F_KelasID == DaneshAmuz.F_KelasID).Select(x => new { x.Tarikh, x.FileTaklif, x.OnvaneTaklif, x.TovzihateTaklif }).ToList();
                temp.AddRange(temp2);
            }
            foreach (var item in temp.OrderByDescending(u => u.Tarikh))
                Result.Add(new Takalif_Model(Tools.SpecialJalaliFormat(item.Tarikh ?? default(DateTime)), item.OnvaneTaklif, item.TovzihateTaklif, ConfigurationManager.AppSettings["WebSiteUrl"] + Tools.ReturnPath("TaklifeKelasi", "TakalifeDarsi") + item.FileTaklif));
            dynamic collectionWrapper = new { Root = Result };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }

        public string TashvighVaTanbih(int DaneshAmoozId, string mah)
        {
            var db = new SCEntities();
            PersianCalendar p = new PersianCalendar();
            var now = DateTime.Now;
            DateTime date;
            Tools.GetJalaliDateReturnDateTime(p.GetYear(now) + "/" + mah + "/01 00:00:00", out date);
            DateTime date2 = date.AddMonths(1);
            var temp = db.TashvighTanbihKelasi.Where(u => (u.Tarikh > date || u.Tarikh == date) && (u.Tarikh < date2) && u.isDeleted == false && u.F_DaneshAmuzID == DaneshAmoozId).Select(x => new { x.Tarikh, x.Emtiaz, x.Tovzihat });
            var Result = new List<TashvighTanbih_Model>();
            foreach (var item in temp)
                Result.Add(new TashvighTanbih_Model(Tools.SpecialJalaliFormat(item.Tarikh ?? default(DateTime)), item.Tovzihat, item.Emtiaz.ToString()));
            dynamic collectionWrapper = new { Root = Result };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }

        public string BarnameHaftegi(int MoallemId)
        {
            BarnameHaftegi_DAL dal = new BarnameHaftegi_DAL(new SCEntities());
            dynamic collectionWrapper = new { Root = dal.GetMoallemBarname(MoallemId) };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }

        public string OvliaBarnameHaftegi(int DaneshAmuzId)
        {
            BarnameHaftegi_DAL dal = new BarnameHaftegi_DAL(new SCEntities());
            dynamic collectionWrapper = new { Root = dal.GetDaneshAmuzBarname(DaneshAmuzId) };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }

        public string ListNomarateDars(int DaneshAmoozId, int DarsId)
        {
            Doroos_DAL dal = new Doroos_DAL(new SCEntities());
            dynamic collectionWrapper = new { Root = dal.ListNomarateDars(DaneshAmoozId, DarsId) };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }

        public string KarnameDaneshAmuz(int DaneshAmoozId)
        {
            NomreEmtehani_DAL dal = new NomreEmtehani_DAL(new SCEntities());
            dynamic collectionWrapper = new { Root = dal.KarnameDaneshAmuz(DaneshAmoozId) };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }

        public string ListeDaneshAmoozaneMadrese(int MoavenId)
        {
            DaneshAmuz_DAL dal = new DaneshAmuz_DAL(new SCEntities());
            dynamic collectionWrapper = new { Root = dal.ListeDaneshAmoozaneMadrese(MoavenId) };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }

        public string ListeMaghateBeHamraheKelasHa(int MoavenId)
        {
            Maghaate_DAL dal = new Maghaate_DAL(new SCEntities());
            dynamic collectionWrapper = new { Root = dal.ListeMaghateBeHamraheKelasHa(MoavenId) };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }

        public string ListeDorooseKelasha(int KelasId)
        {
            Doroos_DAL dal = new Doroos_DAL(new SCEntities());
            dynamic collectionWrapper = new { Root = dal.ListeDorooseKelasha(KelasId) };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }

        public string OmureMali(int DaneshAmoozId)
        {
            PardakhtHa_DAL dal = new PardakhtHa_DAL(new SCEntities());
            dynamic collectionWrapper = new { Root = dal.OmureMali(DaneshAmoozId) };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }
    }
}