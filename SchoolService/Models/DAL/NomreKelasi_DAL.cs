using SchoolService.Areas.Admin3mill.Models;
using SchoolService.Models.AndroidJsonModel;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
namespace SchoolService.Models.DAL
{
    public class NomreKelasi_DAL
    {
        private SCEntities db;
        public NomreKelasi_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<NomreKelasi> List()
        {
            var NomreKelasi = db.NomreKelasi.Where(u => u.isDeleted == false);
            return NomreKelasi.ToList();
        }

        public List<Nomre_Model> ListForAllDoroos(int DaneshAmuzId)
        {
            var result = from st in db.NomreKelasi
                         where st.F_DaneshAmuzID == DaneshAmuzId
                         join barname in db.BarnameHaftegi on st.F_BarnameHaftegiID equals barname.ID
                         join map in db.Mapping_Moallem_Doroos on barname.F_MoallemDoroosID equals map.ID
                         join doroos in db.Doroos on map.F_DoroosID equals doroos.ID
                         select new Nomre_Model
                         {
                             NaameDars = doroos.NaameDars,
                             NomreDars = st.Nomre,
                             ID = st.ID
                         };
            return result.ToList();
        }

        public RizNomre_Model RizNomreKelasi(int KelasId, DateTime From, DateTime To, int DarsId)
        {
            var query = from st in db.NomreKelasi
                        where (st.Tarikh > From || st.Tarikh == From) && st.Tarikh < To
                        join DaneshAmooz in db.DaneshAmuz on st.F_DaneshAmuzID equals DaneshAmooz.ID
                        where DaneshAmooz.F_KelasID == KelasId
                        join barname in db.BarnameHaftegi on st.F_BarnameHaftegiID equals barname.ID
                        join map in db.Mapping_Moallem_Doroos on barname.F_MoallemDoroosID equals map.ID
                        join doroos in db.Doroos on map.F_DoroosID equals doroos.ID
                        select new
                        {
                            BarnameHaftegiID = barname.ID,
                            NomreDars = st.Nomre,
                            ID = st.ID,
                            Tarikh = st.Tarikh,
                            DaneshAmoozFullName = DaneshAmooz.FirstName + " " + DaneshAmooz.LastName
                        };
            if (DarsId != -1)
            {
                var BarnameHayeHaftegi = db.BarnameHaftegi.Where(u => u.Mapping_Moallem_Doroos.F_DoroosID == DarsId).Select(u => u.ID).ToList();
                query = query.Where(u => BarnameHayeHaftegi.Contains(u.BarnameHaftegiID));
            }
            var Result = new RizNomre_Model();
            Result.Dates = query.GroupBy(x => new { x.Tarikh }).Select(g => g.FirstOrDefault().Tarikh).ToList();
            var Names = query.GroupBy(x => new { x.DaneshAmoozFullName }).Select(g => g.FirstOrDefault().DaneshAmoozFullName).ToList();
            foreach (var item in Names)
            {
                DaneshAmoozNomreHelper model = new DaneshAmoozNomreHelper();
                model.DaneshAmoozFullName = item;
                var Nomarat = query.Where(u => u.DaneshAmoozFullName == item);
                for (int i = 0; i < Result.Dates.Count; i++)
                {
                    DateTime tt = Result.Dates[i] ?? default(DateTime);
                    var temp = Nomarat.FirstOrDefault(y => y.Tarikh == tt);
                    var m = new NomreHelper();
                    if (temp != null)
                    {
                        m.ID = temp.ID;
                        m.NomreDars = temp.NomreDars;
                        m.Tarikh = temp.Tarikh;
                    }
                    model.Nomarat.Add(m);
                }
                Result.DaneshAmoozan.Add(model);
            }
            return Result;
        }

        public List<Chart_Model> AveragePerMonth(int DaneshAmuzId)
        {
            var Result = new List<Chart_Model>();
            var scores = db.NomreKelasi.Where(u => u.F_DaneshAmuzID == DaneshAmuzId);
            List<string> Months = new List<string>() { "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", "فروردین", "اردیبهشت", "خرداد" };
            List<double> Average = new List<double>() { 12.5, 17.73, 15.25, 16.0, 14.03, 12, 19, 18.75, 19.36 };
            var NowDate = DateTime.Now;
            DateTime date;
            Tools.GetJalaliDateReturnDateTime("1396/05/10 00:00:00", out date);
            for (int i = 0; i < 9; i++)
            {
                //var Average = scores.Where(q => q.Tarikh.Value>date&&q.Tarikh.Value<date.AddMonths(1)).Average(y=>y.Nomre);
                Result.Add(new Chart_Model(Average[i], Months[i]));
            }
            return Result;
        }

        public List<NafarateBartar_Model> NafarateBartar()
        {
            var Result = new List<NafarateBartar_Model>();
            Result.Add(new NafarateBartar_Model(1, "علی ولی زاده"));
            Result.Add(new NafarateBartar_Model(2, "امیر عباس مومیوند"));
            Result.Add(new NafarateBartar_Model(3, "یاشار بی باک"));
            return Result;
        }

        public NomreKelasi Details(int id)
        {
            NomreKelasi NomreKelasi = db.NomreKelasi.Find(id);
            if (NomreKelasi == null)
            {
                return null;
            }
            return NomreKelasi;
        }

        public void Create(NomreKelasi NomreKelasi)
        {
            db.NomreKelasi.Add(NomreKelasi);
            db.SaveChanges();
        }

        public void CreateMany(List<NomreKelasi> NomarateKelasi)
        {
            db.NomreKelasi.AddRange(NomarateKelasi);
            db.SaveChanges();
        }

        public void Edit(NomreKelasi NomreKelasi)
        {
            db.Entry(NomreKelasi).State = EntityState.Modified;
            db.SaveChanges();
        }


        public void Delete(int id)
        {
            NomreKelasi NomreKelasi = db.NomreKelasi.Find(id);
            if (NomreKelasi != null)
            {
                NomreKelasi.isDeleted = true;
                db.SaveChanges();
            }

        }

        public int ChangeRizNomreKelasi(List<Nomre_Model> model,int MId)
        {
            try
            {
                var Ids = model.Where(u => u.ID != 0).Select(q => q.ID).ToList();
                var Nomarat = db.NomreKelasi.Where(u => Ids.Contains(u.ID) && u.BarnameHaftegi.Kelas.F_MadaresID == MId);
                foreach (var item in Nomarat)
                {
                    var temp = model.FirstOrDefault(u => u.ID == item.ID);
                    if (temp != null)
                        item.Nomre = temp.NomreDars;
                }
                db.SaveChanges();
                return 1;
            }
            catch { return -1; }
        }

    }
}