using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SchoolService.Areas.Admin3mill.Models;
using SchoolService.Models.AndroidJsonModel;
using SchoolService.Models.DAL;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.IO;
using LinqToExcel;
using OfficeOpenXml;

namespace SchoolService.Models.BLL
{
    public class DaneshAmoozManagement
    {
        public List<DaneshAmuz> ListDaneshAmuz(string ParrentId)
        {
            DaneshAmuz_DAL KD = new DaneshAmuz_DAL(new SCEntities());
            return KD.List(ParrentId);
        }

        public List<DaneshAmuz_Model> ListStudents(int KelasId)
        {
            DaneshAmuz_DAL dal = new DaneshAmuz_DAL(new SCEntities());
            var DaneshAmoozan = dal.ListDaneshAmuzaneKelas(KelasId);
            List<DaneshAmuz_Model> Result = new List<DaneshAmuz_Model>();
            foreach (var item in DaneshAmoozan)
            {
                var m = new DaneshAmuz_Model();
                m.FullName = item.FirstName + " " + item.LastName;
                m.ID = item.ID;
                Result.Add(m);
            }
            return Result;
        }

        public string AddDaneshAmuz(DaneshAmuz model, ModelStateDictionary ModelState, out int? Id)
        {
            Id = null;
            SCEntities db = new SCEntities();

            DaneshAmuz_DAL KD = new DaneshAmuz_DAL(db);
            model.isDeleted = false;
            model.Status = true;
            Id = KD.Create(model);
            return "success";
        }

        public string EditDaneshAmuz(DaneshAmuz model)
        {
            DaneshAmuz_DAL KD = new DaneshAmuz_DAL(new SCEntities());
            KD.Edit(model);
            return "success";

        }


        public string ChangeStatusDaneshAmuz(int ID, string ParrentId)
        {
            DaneshAmuz_DAL dal = new DaneshAmuz_DAL(new SCEntities());
            if (dal.ChangeStatus(ID, ParrentId) == null)
            {
                return "error";
            }
            else
            {
                return "success";
            }
        }
        public string DeleteDaneshAmuz(int ID, string ParrentId)
        {
            var db = new SCEntities();
            DaneshAmuz_DAL DD = new DaneshAmuz_DAL(db);
            if (DD.Delete(ID, ParrentId) == null)
            {
                return "error";
            }
            else
            {
                return "success";
            }
        }


        public DaneshAmuz DetailDaneshAmuz(int ID, string ParrentId)
        {
            DaneshAmuz_DAL KD = new DaneshAmuz_DAL(new SCEntities());
            return KD.Details(ID, ParrentId);
        }

        public List<string> DaryafteRotbe(int KelasId, int DarsId, DateTime date, int F_MadreseId, int DaneshAmoozId, List<NomreKelasi> nomarat,IQueryable<BarnameHaftegi> barnamehafteg,DaneshAmuz danesh,IQueryable<DaneshAmuz> daneshan)
        {
            DateTime date2 = date.AddMonths(1);
            List<double> KelasAverages = new List<double>();
            List<double> MadreseAverages = new List<double>();
          //  var db = new SCEntities();
            var BarnameHayeHaftegi = barnamehafteg.Where(u => u.Mapping_Moallem_Doroos.F_DoroosID == DarsId).Select(u => u.ID).ToList();
           // Doroos_DAL dal = new Doroos_DAL(db);
            var mynomarat = nomarat.Where(u => BarnameHayeHaftegi.Contains(u.F_BarnameHaftegiID ?? default(int)) && u.isDeleted == false).ToList();
            var temm2 = mynomarat.Where(u => (u.Tarikh > date || u.Tarikh == date) && u.Tarikh < date2 && u.F_DaneshAmuzID == DaneshAmoozId && u.DaneshAmuz.Ovlia.UserInformation.F_MadaaresID == F_MadreseId).Select(y => y.Nomre).ToList();
            double temp = Average(temm2);
         //   var DaneshAmuz = db.DaneshAmuz.Where(u => u.isDeleted == false && u.Ovlia.UserInformation.F_MadaaresID == F_MadreseId).Select(y => new { y.ID, y.F_KelasID, y.Kelas });
        //  var Paaye_Object = DaneshAmuz.FirstOrDefault(u => u.F_KelasID == KelasId);
            if (danesh != null)
            {
                var DaneshAmuzaneDars = daneshan.Where(u => u.Kelas.F_PayeID == danesh.Kelas.F_PayeID && u.Kelas.Paaye.F_MaghaateID == danesh.Kelas.Paaye.F_MaghaateID).ToList();
                foreach (var item in DaneshAmuzaneDars)
                {
                    var temm = mynomarat.Where(u => (u.Tarikh > date || u.Tarikh == date) && u.Tarikh < date2 && u.F_DaneshAmuzID == item.ID).Select(y => y.Nomre).ToList();
                    MadreseAverages.Add(Average(temm));
                }
            }
            var DaneshAmuzaneKelas = daneshan.Where(u => u.F_KelasID == KelasId).ToList();
            foreach (var item in DaneshAmuzaneKelas)
            {
                var temm = mynomarat.Where(u => (u.Tarikh > date || u.Tarikh == date) && u.Tarikh < date2 && u.F_DaneshAmuzID == item.ID).Select(y => y.Nomre).ToList();
                KelasAverages.Add(Average(temm));
            }
            KelasAverages = KelasAverages.GroupBy(x => x).Select(g => g.FirstOrDefault()).OrderByDescending(u => u).ToList();
            MadreseAverages = MadreseAverages.GroupBy(x => x).Select(g => g.FirstOrDefault()).OrderByDescending(u => u).ToList();
            return new List<string>() { (KelasAverages.IndexOf(temp) + 1) + "", (MadreseAverages.IndexOf(temp) + 1) + "" };
        }

        public List<string> DaryafteRotbeKolli(int KelasId, DateTime date, int F_MadreseId, int DaneshAmoozId, List<NomreKelasi> nomarat, IQueryable<BarnameHaftegi> barnamehafteg, DaneshAmuz danesh, IQueryable<DaneshAmuz> daneshan)
        {
            DateTime date2 = date.AddMonths(1);
            List<double> KelasAverages = new List<double>();
            List<double> MadreseAverages = new List<double>();
         //   var db = new SCEntities();
          //  Doroos_DAL dal = new Doroos_DAL(db);
            var temm2 = nomarat.Where(u => (u.Tarikh > date || u.Tarikh == date) && u.Tarikh < date2 && u.F_DaneshAmuzID == DaneshAmoozId).Select(y => y.Nomre).ToList();
            double temp = Average(temm2);
          //  var DaneshAmuz = db.DaneshAmuz.Where(u => u.isDeleted == false && u.Ovlia.UserInformation.F_MadaaresID == F_MadreseId).Select(y => new { y.ID, y.F_KelasID, y.Kelas });
          //  var Paaye_Object = DaneshAmuz.FirstOrDefault(u => u.F_KelasID == KelasId);
            if (danesh != null)
            {
                var DaneshAmuzaneDars = daneshan.Where(u => u.Kelas.F_PayeID == danesh.Kelas.F_PayeID && u.Kelas.Paaye.F_MaghaateID == danesh.Kelas.Paaye.F_MaghaateID).ToList();
                foreach (var item in DaneshAmuzaneDars)
                {
                    var temm = nomarat.Where(u => (u.Tarikh > date || u.Tarikh == date) && u.Tarikh < date2 && u.F_DaneshAmuzID == item.ID).Select(y => y.Nomre).ToList();
                    MadreseAverages.Add(Average(temm));
                }
            }
            var DaneshAmuzaneKelas = daneshan.Where(u => u.F_KelasID == KelasId).ToList();
            foreach (var item in DaneshAmuzaneKelas)
            {
                var temm = nomarat.Where(u => (u.Tarikh > date || u.Tarikh == date) && u.Tarikh < date2 && u.F_DaneshAmuzID == item.ID).Select(y => y.Nomre).ToList();
                KelasAverages.Add(Average(temm));
            }
            KelasAverages = KelasAverages.GroupBy(x => x).Select(g => g.FirstOrDefault()).OrderByDescending(u => u).ToList();
            MadreseAverages = MadreseAverages.GroupBy(x => x).Select(g => g.FirstOrDefault()).OrderByDescending(u => u).ToList();
            return new List<string>() { (KelasAverages.IndexOf(temp) + 1) + "", (MadreseAverages.IndexOf(temp) + 1) + "" };
        }
        public double Average(List<string> model)
        {
            double t;
            int count = model.Count;
            if (model.Count > 0)
            {
                double temp = 0;
                foreach (var item in model)
                {
                    if (double.TryParse(item, out t))
                    {
                        temp += Convert.ToDouble(item);
                    }
                    else
                        count--;
                }
                double meghdar = temp / count;
                int meghdar2 = (int)(meghdar * 100);
                meghdar = (double)meghdar2 / 100;
                return meghdar;
            }
            return 0;
        }
        public string SetClassScore(SabteNomreModel model)
        {
            try
            {
                var NK = new NomreKelasi_DAL(new SCEntities());
                foreach (var item in model.Nomarat)
                {
                    if (!string.IsNullOrEmpty(item.Nomre))
                    {
                        PersianCalendar p = new PersianCalendar(); DateTime date;
                        if (Tools.GetJalaliDateReturnDateTime(model.Tarikh, out date))
                        {
                            NomreKelasi InsertObject = new NomreKelasi();
                            InsertObject.F_DaneshAmuzID = model.DaneshAmoozId;
                            InsertObject.Hafte = model.Hafte;
                            InsertObject.Tarikh = date;
                            InsertObject.Nomre = item.Nomre;
                            InsertObject.Tovzihat = item.Tozih;
                            InsertObject.isDeleted = false;
                            InsertObject.F_BarnameHaftegiID = item.BarnameHaftegiId;
                            NK.Create(InsertObject);
                        }
                        throw new ArgumentNullException();
                    }
                }
                return "success";
            }
            catch { return "متاسفانه در فرآیند ثبت نمره خطایی پیش آمده است"; }
        }

        public string SetClassScoreForMany(SabteNomreModel model)
        {
            try
            {
                var NK = new NomreKelasi_DAL(new SCEntities());
                var NomaratObject = new List<NomreKelasi>();
                foreach (var item in model.Nomarat)
                {
                    if (!string.IsNullOrEmpty(item.Nomre))
                    {
                        PersianCalendar p = new PersianCalendar(); DateTime date;
                        if (Tools.GetJalaliDateReturnDateTime(model.Tarikh, out date))
                        {
                            NomreKelasi InsertObject = new NomreKelasi();
                            InsertObject.F_DaneshAmuzID = item.DaneshAmoozId;
                            InsertObject.Hafte = model.Hafte;
                            InsertObject.Tarikh = date;
                            InsertObject.Nomre = item.Nomre;
                            InsertObject.Tovzihat = item.Tozih;
                            InsertObject.isDeleted = false;
                            InsertObject.F_BarnameHaftegiID = model.BarnameHaftegiId;
                            NomaratObject.Add(InsertObject);
                        }
                        else
                            throw new ArgumentNullException();
                    }
                }
                NK.CreateMany(NomaratObject);
                return "success";
            }
            catch { return "متاسفانه در فرآیند ثبت نمرات خطایی پیش آمده است"; }
        }
        public string AddDaneshAmuzWithExcel(HttpPostedFileBase ExcelFile, string F_ParrentID, ModelStateDictionary ModelState, out int? Id)
        {
            Id = null;
            if (ExcelFile == null)
                return "هیچ فایلی انتخاب نشده است";
            var allowedExtensions = new[] { ".xlsx", ".xls" };
            var extension = Path.GetExtension(ExcelFile.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                return "قالب فایل انتخابی صحیح نمی باشد";
            string fileName = ExcelFile.FileName;
            string fileContentType = ExcelFile.ContentType;
            byte[] fileBytes = new byte[ExcelFile.ContentLength];
            var data = ExcelFile.InputStream.Read(fileBytes, 0, Convert.ToInt32(ExcelFile.ContentLength));
            var usersList = new List<DaneshAmuz>();
            int? F_MadraseId = Tools.MadreseId(F_ParrentID);
            using (var package = new ExcelPackage(ExcelFile.InputStream))
            {
                var currentSheet = package.Workbook.Worksheets;
                var workSheet = currentSheet.First();
                var noOfCol = workSheet.Dimension.End.Column;
                var noOfRow = workSheet.Dimension.End.Row;
                using (SCEntities db = new SCEntities())
                {
                    var Ovlia = db.Ovlia.Where(u => u.UserInformation.F_MadaaresID == F_MadraseId && u.UserInformation.Status == true && u.UserInformation.isDeleted == false);
                    var Kelasha = db.Kelas.Where(u => u.F_MadaresID == F_MadraseId && u.isDeleted == false);
                    var ListCodeMelli = db.DaneshAmuz.Where(u => u.Status == true).Select(q => q.CodeMelli);
                    for (int rowIterator = 1; rowIterator <= noOfRow; rowIterator++)
                    {
                        if (workSheet.Cells[rowIterator, 7].Value != null)
                        {
                            var dd = workSheet.Cells[rowIterator, 7].Value;
                            string Temp = "";
                            if (dd != null)
                                Temp = dd.ToString();
                            var KelasName = workSheet.Cells[rowIterator, 2].Value.ToString();
                            var FoundedKelas = Kelasha.FirstOrDefault(u => u.NaameKelas == KelasName);
                            if (ListCodeMelli.FirstOrDefault(u => u == Temp) == null && FoundedKelas != null)
                            {
                                if (!string.IsNullOrEmpty(Temp))
                                {
                                    var DaneshAmooz = new DaneshAmuz();
                                    DaneshAmooz.LastName = workSheet.Cells[rowIterator, 11].Value.ToString();
                                    DaneshAmooz.FirstName = workSheet.Cells[rowIterator, 10].Value.ToString();
                                    DaneshAmooz.NesbateVali = "پدر";
                                    var jensiat = workSheet.Cells[rowIterator, 9].Value.ToString();
                                    if (jensiat == "دختر")
                                        DaneshAmooz.Jensiat = false;
                                    else
                                        DaneshAmooz.Jensiat = true;
                                    var OvliaName = workSheet.Cells[rowIterator, 8].Value.ToString();
                                    var FoundedOvlia = Ovlia.FirstOrDefault(u => u.UserInformation.FirstName == OvliaName && u.UserInformation.LastName == DaneshAmooz.LastName && u.UserInformation.isDeleted == false && u.UserInformation.Status == true);
                                    if (FoundedOvlia != null)
                                        DaneshAmooz.F_OvliaID = FoundedOvlia.ID;
                                    else
                                    {
                                        UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                                        var user = new ApplicationUser() { UserName = Temp };
                                        var result = UserManager.Create(user, Temp);
                                        if (!result.Succeeded)
                                        {
                                            ModelState.AddModelError("username", "خطا در ثبت نام کاربری");
                                            return "error";
                                        }
                                        UserManager.AddToRole(user.Id, "Dabir");
                                        Ovlia_DAL ovlia_dal = new Ovlia_DAL(db);
                                        Ovlia model = new Ovlia();
                                        UserInformation myuser = new UserInformation();
                                        myuser.ID = user.Id;
                                        myuser.Status = true;
                                        myuser.isDeleted = false;
                                        myuser.FirstName = OvliaName;
                                        myuser.LastName = DaneshAmooz.LastName;
                                        myuser.F_MadaaresID = F_MadraseId;
                                        UserInformation_DAL User_dal = new UserInformation_DAL(db);
                                        model.F_UserInformationID = User_dal.Create(myuser);
                                        model.F_ParrentID = F_ParrentID;
                                        DaneshAmooz.F_OvliaID = ovlia_dal.Create(model);
                                    }
                                    DaneshAmooz.CodeMelli = Temp;
                                    DaneshAmooz.ShomareShenasname = workSheet.Cells[rowIterator, 6].Value.ToString() + workSheet.Cells[rowIterator, 5].Value.ToString() + workSheet.Cells[rowIterator, 4].Value.ToString();
                                    DaneshAmooz.TarikhTavallod = workSheet.Cells[rowIterator, 3].Value.ToString();
                                    DateTime tem = new DateTime();
                                    Tools.GetJalaliDateReturnDateTime(DaneshAmooz.TarikhTavallod + " 00:00:00", out tem);
                                    DaneshAmooz.TarikhTavallod = tem.ToString();
                                    DaneshAmooz.F_KelasID = FoundedKelas.ID;
                                    DaneshAmooz.MahalleSodoor = workSheet.Cells[rowIterator + 1, 3].Value.ToString();
                                    DaneshAmooz.Tovzihat = workSheet.Cells[rowIterator, 1].Value.ToString();
                                    DaneshAmooz.Status = true;
                                    DaneshAmooz.isDeleted = false;
                                    DaneshAmooz.F_ParrentID = F_ParrentID;
                                    usersList.Add(DaneshAmooz);
                                    rowIterator++;
                                    db.DaneshAmuz.Add(DaneshAmooz);
                                }
                            }
                        }
                    }
                    db.SaveChanges();
                }
            }
            Id = 1;
            return "OK";
        }
    }
}