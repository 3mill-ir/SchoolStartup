using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using SchoolService.Models.AndroidJsonModel;

namespace SchoolService.Models.DAL
{
    public class DaneshAmuz_DAL
    {
        private SCEntities db;
        public DaneshAmuz_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<DaneshAmuz> List(string ParrentId)
        {
            var DaneshAmuz = db.DaneshAmuz.Where(u => u.isDeleted == false && u.F_ParrentID == ParrentId).OrderBy(u => u.LastName);
            return DaneshAmuz.ToList();
        }

        public string NameAndPaye(int DaneshAmuzId)
        {
            var DaneshAmuz = db.DaneshAmuz.FirstOrDefault(u => u.ID == DaneshAmuzId && u.isDeleted == false);
            var Kelas = db.Kelas.FirstOrDefault(u => u.ID == DaneshAmuz.F_KelasID);
            return DaneshAmuz.FirstName + " " + DaneshAmuz.LastName + ":" + Kelas.Paaye.NaamePaye + " " + Kelas.Paaye.Maghaate.NaameMaghta;
        }

        public dynamic ListDaneshAmuzaneKelas(int KelasId)
        {
            var DaneshAmuz = db.DaneshAmuz.Where(u => u.isDeleted == false && u.F_KelasID == KelasId).OrderBy(u => u.LastName).Select(x => new { x.FirstName, x.LastName, x.ID, OvliaChatId = x.Ovlia.F_UserInformationID });
            return DaneshAmuz.ToList();
        }

        public DaneshAmuz Details(int id,string ParrentId)
        {
            DaneshAmuz DaneshAmuz = List(ParrentId).FirstOrDefault(u=>u.ID==id);
            if (DaneshAmuz == null)
            {
                return null;
            }
            return DaneshAmuz;
        }

        public int Create(DaneshAmuz DaneshAmuz)
        {
            db.DaneshAmuz.Add(DaneshAmuz);
            db.SaveChanges();
            return DaneshAmuz.ID;
        }

        public int Edit(DaneshAmuz DaneshAmuz)
        {
            db.Entry(DaneshAmuz).State = EntityState.Modified;
            db.Entry(DaneshAmuz).Property(u => u.isDeleted).IsModified = false;
            db.Entry(DaneshAmuz).Property(u => u.Status).IsModified = false;
            db.Entry(DaneshAmuz).Property(u => u.F_ParrentID).IsModified = false;

            return db.SaveChanges();
        }


        public int? Delete(int id, string ParrentId)
        {
            DaneshAmuz DaneshAmuz = List(ParrentId).FirstOrDefault(u => u.ID == id);
            if (DaneshAmuz != null)
            {
                DaneshAmuz.isDeleted = true;
                db.SaveChanges();
                return DaneshAmuz.ID;
            }
            return null;
        }

        public int? ChangeStatus(int id,string ParrentId)
        {
            DaneshAmuz DaneshAmuz = List(ParrentId).FirstOrDefault(u=>u.ID==id);
            if (DaneshAmuz != null)
            {
                DaneshAmuz.Status = !DaneshAmuz.Status;
                db.SaveChanges();
                return DaneshAmuz.ID;
            }
            return null;
        }


        public dynamic ListOvliaContacts(int DaneshAmuzId)
        {
            var DaneshAmuz = db.DaneshAmuz.FirstOrDefault(u => u.isDeleted == false && u.ID == DaneshAmuzId);
            var Result = new List<Contact_Model>();
            if (DaneshAmuz != null)
            { 
                int daneshamoozID=DaneshAmuz.F_KelasID  ?? default (int);
                var result = from a in db.Kelas 
                             where a.ID==daneshamoozID
                             join b in db.BarnameHaftegi on a.ID equals b.F_KelasID 
                             join c in db.Mapping_Moallem_Doroos on b.F_MoallemDoroosID equals c.ID
                             join d in db.Moallem on c.F_MoallemID equals d.ID 
                          select new Contact_Model
                             {
                                 FullName = d.UserInformation.FirstName + " " + d.UserInformation.LastName + " (معلم)",
                                 ID = d.UserInformation.ID,
                                
                             };
                //var result = from st in db.Kelas
                //             where st.ID == DaneshAmuz.F_KelasID
                //             join doroos in db.Doroos on st.F_PayeID equals doroos.F_PayeID
                //             join map in db.Mapping_Moallem_Doroos on doroos.ID equals map.F_DoroosID
                //             join moallem in db.Moallem on map.F_MoallemID equals moallem.ID
                //             join user in db.UserInformation on moallem.F_UserInformation equals user.ID
                //             where user.isDeleted == false
                //             select new Contact_Model
                //             {
                //                 FullName = user.FirstName + " " + user.LastName + " (معلم)",
                //                 ID = user.ID
                //             };
                var result2 = from user in db.UserInformation
                              join Moaven in db.Karmandaan on user.ID equals Moaven.F_UserInfromation
                              where Moaven.Semat == "Moaven" && user.F_MadaaresID == DaneshAmuz.Kelas.F_MadaresID && user.isDeleted == false
                              select new Contact_Model
                              {
                                  FullName = user.FirstName + " " + user.LastName + " (معاون)",
                                  ID = user.ID
                              };
                Result.AddRange(result); Result.AddRange(result2);

                return Result.GroupBy(x => new { x.ID }).Select(g => g.FirstOrDefault()).ToList();
            }
            return new List<Contact_Model>();
        }

        public dynamic ListeDaneshAmoozaneMadrese(int MoavenId)
        {
            var temp = db.Karmandaan.FirstOrDefault(u => u.ID == MoavenId);
            var result = from DaneshAmooz in db.DaneshAmuz
                         join Ovliaa in db.Ovlia on DaneshAmooz.F_OvliaID equals Ovliaa.ID
                         join Kelass in db.Kelas on DaneshAmooz.F_KelasID equals Kelass.ID
                         where Kelass.F_MadaresID == temp.UserInformation.F_MadaaresID
                         select new
                         {
                             FullName = DaneshAmooz.FirstName + ":" + DaneshAmooz.LastName,
                             ChatId = Ovliaa.F_UserInformationID,
                             ID = DaneshAmooz.ID
                         };
            return result;
        }
    }
}