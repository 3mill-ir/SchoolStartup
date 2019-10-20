using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using SchoolService.Models.AndroidJsonModel;
using SchoolService.Areas.Admin3mill.Models;
using System.Web.Mvc;
using System.Data.Entity.SqlServer;
using SchoolService.Models.BLL;
using System.Globalization;

namespace SchoolService.Models.DAL
{
    public class Doroos_DAL
    {
        private SCEntities db;
        public Doroos_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public int? isExist(Doroos model)
        {

            var found = db.Doroos.FirstOrDefault(u => u.NaameDars == model.NaameDars && u.isDeleted == false && u.F_PayeID == model.F_PayeID);
            if (found == null)
                return null;
            else
                db.Entry(found).State = EntityState.Detached;
            return found.ID;
        }
        public int? isExistFovgholade(Doroos model, int MadreseId)
        {

            var found = ListFovgholade(MadreseId).FirstOrDefault(u => u.NaameDars == model.NaameDars);
            if (found == null)
                return null;
            else
                db.Entry(found).State = EntityState.Detached;
            return found.ID;
        }

        public List<Doroos> List(int? PaayeId = null)
        {
            var Doroos = PaayeId == null ? db.Doroos.Where(u => u.isDeleted == false && u.Sabet == true) : db.Doroos.Where(u => u.isDeleted == false && u.F_PayeID == PaayeId && u.Sabet == true);
            return Doroos.ToList();
        }
        public List<Doroos> ListFovgholade(int MadreseId)
        {
            var doroos = db.Doroos.Where(u => u.F_MadaaresID == MadreseId && u.isDeleted == false && u.Sabet == false);
            return doroos.ToList();
        }

        public dynamic ListTeacherDoroos(int id)
        {
            var Doroos = db.Doroos.Include(u => u.Mapping_Moallem_Doroos.Where(t => t.F_MoallemID == id)).Where(u => u.isDeleted == false).Select(x => new { x.ID, x.NaameDars });
            return Doroos;
        }

        public Doroos Details(int id)
        {
            Doroos Doroos = db.Doroos.Find(id);
            if (Doroos == null)
            {
                return null;
            }
            return Doroos;
        }
        public Doroos DetailsFovgholade(int id, int MadreseId)
        {
            Doroos Doroos = ListFovgholade(MadreseId).FirstOrDefault(u => u.ID == id);
            if (Doroos == null)
            {
                return null;
            }
            return Doroos;
        }

        public void Create(Doroos Doroos)
        {
            db.Doroos.Add(Doroos);
            db.SaveChanges();
        }

        public void Edit(Doroos Doroos)
        {
            db.Entry(Doroos).State = EntityState.Modified;
            db.Entry(Doroos).Property(u => u.F_MadaaresID).IsModified = false;
            db.Entry(Doroos).Property(u => u.F_PayeID).IsModified = false;
            db.Entry(Doroos).Property(u => u.F_ParrentID).IsModified = false;
            db.Entry(Doroos).Property(u => u.isDeleted).IsModified = false;
            db.Entry(Doroos).Property(u => u.Sabet).IsModified = false;
            db.SaveChanges();
        }


        public int? Delete(int id)
        {
            Doroos Doroos = db.Doroos.Find(id);
            if (Doroos != null)
            {
                Doroos.isDeleted = true;
                db.SaveChanges();
                return Doroos.ID;
            }
            return null;
        }

        public int? DeleteFovgholade(int id, int MadreseId)
        {
            Doroos Doroos = ListFovgholade(MadreseId).FirstOrDefault(u => u.ID == id);
            if (Doroos != null)
            {
                Doroos.isDeleted = true;
                db.SaveChanges();
                return Doroos.ID;
            }
            return null;
        }


        public dynamic GetListDoroos(int DaneshAmuzId)
        {
            var DaneshAmuz = db.DaneshAmuz.FirstOrDefault(u => u.ID == DaneshAmuzId && u.isDeleted == false);
            if (DaneshAmuz != null)
            {
                var result = from st in db.BarnameHaftegi
                             where st.F_KelasID == DaneshAmuz.F_KelasID
                             join map in db.Mapping_Moallem_Doroos on st.F_MoallemDoroosID equals map.ID
                             join doroos in db.Doroos on map.F_DoroosID equals doroos.ID
                             join dabir in db.Moallem on map.F_MoallemID equals dabir.ID
                             join User in db.UserInformation on dabir.F_UserInformation equals User.ID
                             select new Dars_Model
                             {
                                 NameDars = doroos.NaameDars,
                                 NameMoallem = User.FirstName + " " + User.LastName,
                                 //osulan bayad namash DarsId mibud vali chon samte android ba in nam sabt shode agar taghire nam dahim baraye app ha moshkel be vujud khahad amad
                                 BarnameHaftegiId = doroos.ID
                             };
                return result.GroupBy(x => new { x.BarnameHaftegiId }).Select(g => g.FirstOrDefault());
            }
            return new List<Dars_Model>();
        }
        public List<Doroos> GetListDoroosForAdmin(int KelasId, int MadreseId)
        {
            var kelas = db.Kelas.Where(u => u.F_MadaresID == MadreseId && u.ID == KelasId && u.isDeleted == false).FirstOrDefault();
            List<Doroos> listdoroos = new List<Doroos>();
            if (kelas != null)
            {
                listdoroos.AddRange(kelas.Paaye.Doroos.Where(u => u.isDeleted == false));
                //var madrese = db.Madaares.Where(u => u.ID == MadreseId && u.isDeleted == false).FirstOrDefault();
                //if (madrese != null)
                //{
                //    listdoroos.AddRange(madrese.Doroos.Where(u => u.isDeleted == false && u.F_PayeID == kelas.F_PayeID));
                //}
            }
            return listdoroos;
            //var DaneshAmuz = db.DaneshAmuz.FirstOrDefault(u => u.ID == DaneshAmuzId && u.isDeleted == false && u.Ovlia.UserInformation.F_MadaaresID == MadreseId);
            //if (DaneshAmuz != null)
            //{
            //    var result = from st in db.BarnameHaftegi
            //                 where st.F_KelasID == DaneshAmuz.F_KelasID
            //                 join map in db.Mapping_Moallem_Doroos on st.F_MoallemDoroosID equals map.ID
            //                 join doroos in db.Doroos on map.F_DoroosID equals doroos.ID
            //                 select new AdminDars_Model
            //                 {
            //                     NaameDars = doroos.NaameDars,
            //                     DarsId = doroos.ID
            //                 };
            //    return result.GroupBy(x => new { x.DarsId }).Select(g => g.FirstOrDefault());
            //}
            //return null;
        }
        public List<Doroos> GetListDoroosForAdminALL(int MadreseId)
        {
            List<Doroos> listdoroos = new List<Doroos>();
            var klas = db.Kelas.Where(u => u.isDeleted == false && u.F_MadaresID == MadreseId).Select(u => u.F_PayeID).Distinct();
            var doroos = from a in klas
                         join b in db.Paaye on a equals b.ID
                         where b.isDeleted == false
                         join c in db.Doroos on b.ID equals c.F_PayeID
                         where (c.isDeleted == false && c.Sabet == true && c.F_MadaaresID == null) || (c.isDeleted == false && c.Sabet == false && c.F_MadaaresID == MadreseId)
                         select c;
            if (doroos != null)
            {
                listdoroos.AddRange(doroos);
            }

            //var madrese = db.Madaares.Where(u => u.ID == MadreseId && u.isDeleted == false).FirstOrDefault();
            //listdoroos.AddRange(madrese.Doroos.Where(u => u.isDeleted == false));
            return listdoroos;

        }
        public dynamic ListNomarateDars(int DaneshAmoozId, int DarsId)
        {
            var Result = new GozaresheNomreKelasi_Model();
            var DaneshAmuz = db.DaneshAmuz.FirstOrDefault(u => u.ID == DaneshAmoozId && u.isDeleted == false);
            var BarnamehayeHaftegiId = db.BarnameHaftegi.Where(u => u.Mapping_Moallem_Doroos.F_DoroosID == DarsId && u.isDeleted == false).Select(y => y.ID).ToList();
            if (DaneshAmuz != null)
            {
                var temp = db.NomreKelasi.Where(u => BarnamehayeHaftegiId.Contains(u.F_BarnameHaftegiID ?? default(int)) && u.F_DaneshAmuzID == DaneshAmoozId && u.isDeleted == false).Select(x => new { x.Tarikh, x.Nomre }).ToList();
                var temp2 = db.NomreKelasi.Where(u => u.F_DaneshAmuzID == DaneshAmoozId && u.isDeleted == false).Select(x => new { x.Tarikh, x.Nomre ,x.F_BarnameHaftegiID}).ToList();
                foreach (var item in temp)
                {
                    var nomre = new Nomre_Model();
                    nomre.Tarikh = Tools.JalaliDateWithoutHour(item.Tarikh ?? default(DateTime));
                    nomre.NomreDars = item.Nomre;
                    Result.Nomarat.Add(nomre);
                }
                Result.Chart = GozaresheFilterShode(DarsId, "", DaneshAmoozId);
                return Result;
            }
            return new List<Nomre_Model>();
        }



        public dynamic ListNomarateDarsForAdmin(int MadreseId, int DaneshAmoozId, int DarsId)
        {
            var Result = new AdminNomreReport_Model();
            var DaneshAmuz = db.DaneshAmuz.FirstOrDefault(u => u.ID == DaneshAmoozId && u.isDeleted == false && u.Ovlia.UserInformation.F_MadaaresID == MadreseId);
            if (DaneshAmuz != null)
            {
                var temp1 = from st in db.BarnameHaftegi
                            where st.F_KelasID == DaneshAmuz.F_KelasID
                            join map in db.Mapping_Moallem_Doroos on st.F_MoallemDoroosID equals map.ID
                            join doroos in db.Doroos on map.F_DoroosID equals doroos.ID
                            where doroos.ID == DarsId
                            join NomarateKelasi in db.NomreKelasi on st.ID equals NomarateKelasi.F_BarnameHaftegiID
                            where NomarateKelasi.F_DaneshAmuzID == DaneshAmuz.ID
                            select new AdminNomre_Model
                            {
                                NomreDars = NomarateKelasi.Nomre,
                                Tarikh = NomarateKelasi.Tarikh.Value
                            };
                var temp2 = from st in db.BarnameEmtehani
                            where st.F_KelasID == DaneshAmuz.F_KelasID
                            join map in db.Mapping_Moallem_Doroos on st.F_MoallemDoroosID equals map.ID
                            join doroos in db.Doroos on map.F_DoroosID equals doroos.ID
                            where doroos.ID == DarsId
                            join NomarateEmtehani in db.NomreEmtehani on st.ID equals NomarateEmtehani.F_BarnameEmtehani
                            where NomarateEmtehani.F_DaneshAmuzID == DaneshAmuz.ID
                            select new AdminNomre_Model
                            {
                                NomreDars = NomarateEmtehani.Nomre,
                                Tarikh = st.TarikhKamel.Value
                            };
                Result.NomarateKelasi = temp1.ToList();
                Result.NomarateEmtehani = temp2.ToList();
            }
            return Result;
        }
        public List<Nomre_Model> KarnameDarsNomre(int MadreseId, DaneshAmuz daneshmuz, int DarsId, DateTime date,IQueryable<Nomre_Model> nommodel)
        {
            DateTime date2 = date.AddMonths(1);
            var Result = new List<Nomre_Model>();
            //var DaneshAmuz = db.DaneshAmuz.FirstOrDefault(u => u.ID == DaneshAmoozId && u.isDeleted == false && u.Ovlia.UserInformation.F_MadaaresID == MadreseId);
            if (daneshmuz != null)
            {
                var temp1 = from u in nommodel where u.F_kelasid == daneshmuz.F_KelasID && u.ID == DarsId && u.F_DaneshAmuzID == daneshmuz.ID && (u.TarikhDate > date || u.TarikhDate == date) && u.TarikhDate < date2 select u;


                //var temp1 = from st in db.BarnameHaftegi
                //            where st.F_KelasID == DaneshAmuz.F_KelasID
                //            join map in db.Mapping_Moallem_Doroos on st.F_MoallemDoroosID equals map.ID
                //            join doroos in db.Doroos on map.F_DoroosID equals doroos.ID
                //            where doroos.ID == DarsId
                //            join NomarateKelasi in db.NomreKelasi on st.ID equals NomarateKelasi.F_BarnameHaftegiID
                //            where NomarateKelasi.F_DaneshAmuzID == DaneshAmuz.ID && (NomarateKelasi.Tarikh > date || NomarateKelasi.Tarikh == date) && NomarateKelasi.Tarikh < date2
                //            select new Nomre_Model
                //            {
                //                NomreDars = NomarateKelasi.Nomre,
                //                NaameDars = doroos.NaameDars
                //            };
                Result = temp1.ToList();
            }
            return Result;
        }


        public List<double> DaryafteNomarateDars(int MadreseId, int DaneshAmoozId, int DarsId, DateTime date)
        {
            DateTime date2 = date.AddMonths(1);
            var Result = new List<double>();
            var DaneshAmuz = db.DaneshAmuz.FirstOrDefault(u => u.ID == DaneshAmoozId && u.isDeleted == false && u.Ovlia.UserInformation.F_MadaaresID == MadreseId);
            if (DaneshAmuz != null)
            {
                var temp1 = from st in db.BarnameHaftegi
                            where st.F_KelasID == DaneshAmuz.F_KelasID
                            join map in db.Mapping_Moallem_Doroos on st.F_MoallemDoroosID equals map.ID
                            join doroos in db.Doroos on map.F_DoroosID equals doroos.ID
                            where doroos.ID == DarsId
                            join NomarateKelasi in db.NomreKelasi on st.ID equals NomarateKelasi.F_BarnameHaftegiID
                            where NomarateKelasi.F_DaneshAmuzID == DaneshAmuz.ID && (NomarateKelasi.Tarikh > date || NomarateKelasi.Tarikh == date) && NomarateKelasi.Tarikh < date2
                            select new Nomre_Model
                            {
                                NomreDars = NomarateKelasi.Nomre,
                            };
                int aa;
                foreach (var item in temp1)
                {
                    if (int.TryParse(item.NomreDars, out aa))
                        Result.Add(Convert.ToDouble(item.NomreDars));
                }
            }
            return Result;
        }

        public List<Chart_Model> NemudarePishrafteTahsili(int DaneshAmoozId)
        {
            var Result = new List<Chart_Model>();
            var DaneshAmuz = db.DaneshAmuz.FirstOrDefault(u => u.ID == DaneshAmoozId && u.isDeleted == false);
            if (DaneshAmuz != null)
            {
                List<string> Months = new List<string>() { "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور" };
                List<double> Average = new List<double>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                for (int i = 0; i < 12; i++)
                    Result.Add(new Chart_Model(Average[i], Months[i]));
            }
            return Result;
        }

        public List<Chart_Model> GozaresheFilterShode(int DarsId, string Mah, int DaneshAmoozId)
        {
            var Result = new List<Chart_Model>();
            var DaneshAmuz = db.DaneshAmuz.FirstOrDefault(u => u.ID == DaneshAmoozId && u.isDeleted == false);
            int MadreseId = DaneshAmuz.Ovlia.UserInformation.F_MadaaresID ?? default(int);
            List<string> Months = new List<string>() { "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور" };
            List<double> Average = new List<double>();
            double score = 0;
            int count = 0;
            double Nothing;
            var BarnameHayeHaftegi = db.BarnameHaftegi.Where(u => u.Mapping_Moallem_Doroos.F_DoroosID == DarsId).Select(u => u.ID).ToList();
            var nomarat = db.NomreKelasi.Where(u => BarnameHayeHaftegi.Contains(u.F_BarnameHaftegiID ?? default(int)) && u.isDeleted == false && u.F_DaneshAmuzID == DaneshAmuz.ID).ToList();
            PersianCalendar p = new PersianCalendar(); var now = DateTime.Now; DateTime date;
            Tools.GetJalaliDateReturnDateTime(p.GetYear(now) + "/" + "07" + "/01 00:00:00", out date);
            for (int i = 0; i < 12; i++)
            {
                count = 0;
                score = 0;
                foreach (var item2 in nomarat.Where(u => (u.Tarikh > date || u.Tarikh == date) && u.Tarikh < date.AddMonths(1)))
                {
                    if (double.TryParse(item2.Nomre, out Nothing))
                    {
                        score += Convert.ToDouble(item2.Nomre);
                        count++;
                    }
                }
                date = date.AddMonths(1);
                if (count != 0)
                {
                    double meghdar = score / count;
                    int meghdar2 = (int)(meghdar * 100); meghdar = meghdar2;
                    Average.Add((meghdar / 100));
                }
                else
                    Average.Add(0);
            }
            for (int i = 0; i < 12; i++)
                Result.Add(new Chart_Model(Average[i], Months[i]));
            return Result;
        }

        public dynamic ListeDorooseKelasha(int KelasId)
        {
            var result = from st in db.BarnameHaftegi
                         where st.F_KelasID == KelasId
                         join map in db.Mapping_Moallem_Doroos on st.F_MoallemDoroosID equals map.ID
                         join doroos in db.Doroos on map.F_DoroosID equals doroos.ID
                         select new Dars_Model
                         {
                             NameDars = doroos.NaameDars,
                             BarnameHaftegiId = st.ID,
                         };
            var Result = new List<AndroidBarnameHaftegi_Model>();
            foreach (var item in result)
            {
                var m = new AndroidBarnameHaftegi_Model();
                m.BarnameHaftegiId = item.BarnameHaftegiId;
                m.NameDars = item.NameDars;
                Result.Add(m);
            }
            return Result.GroupBy(x => new { x.NameDars }).Select(g => g.FirstOrDefault()).ToList();
        }

        public List<DarsCombo_Model> ComboDorooseKelasha(int KelasId)
        {
            var result = from st in db.BarnameHaftegi
                         where st.F_KelasID == KelasId
                         join map in db.Mapping_Moallem_Doroos on st.F_MoallemDoroosID equals map.ID
                         join doroos in db.Doroos on map.F_DoroosID equals doroos.ID
                         select new Dars_Model
                         {
                             NameDars = doroos.NaameDars,
                             BarnameHaftegiId = doroos.ID,
                         };
            var Result = new List<DarsCombo_Model>();
            foreach (var item in result)
            {
                var m = new DarsCombo_Model();
                m.ID = item.BarnameHaftegiId;
                m.NameDars = item.NameDars;
                Result.Add(m);
            }
            return Result.GroupBy(x => new { x.NameDars }).Select(g => g.FirstOrDefault()).ToList();
        }
    }
}