using SchoolService.Areas.Admin3mill.Models;
using SchoolService.Models.AndroidJsonModel;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DAL
{
    public class Farakhanha_DAL
    {
        private SCEntities db;
        public Farakhanha_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<Farakhanha> List()
        {
            var Farakhanha = db.Farakhanha.Where(u => u.isDeleted == false);
            return Farakhanha.ToList();
        }

        public List<string> AndroidFarakhanPreview(int DaneshAmoozId)
        {
            var DaneshAmuz = db.DaneshAmuz.FirstOrDefault(u => u.ID == DaneshAmoozId && u.isDeleted == false);
            if (DaneshAmuz != null)
            {
                var Farakhanha = db.Mapping_Farakhanha_Kelas.Include(u => u.Farakhanha).Where(u => u.F_KelasID == DaneshAmuz.F_KelasID && u.Farakhanha.isDeleted == false).Take(5).Select(y => y.Farakhanha.Movzoo);
                return Farakhanha.ToList();
            }
            return new List<string>();
        }

        public dynamic GetFarakhan(int DaneshAmoozId)
        {
            var DaneshAmooz = db.DaneshAmuz.FirstOrDefault(u => u.ID == DaneshAmoozId && u.isDeleted == false);
            if (DaneshAmooz != null)
            {
                var Farakhan = db.Mapping_Farakhanha_Kelas.Include(u => u.Farakhanha).Where(u => u.F_KelasID == DaneshAmooz.F_KelasID).OrderByDescending(u => u.Farakhanha.TarikheFarakhan).Select(x => new { Matn = x.Farakhanha.Matn, Movzoo = x.Farakhanha.Movzoo, TarikheFarakhan = x.Farakhanha.TarikheFarakhan });
                return Farakhan.ToList();
            }
            return null;
        }

        public dynamic MoavenGetFarakhan(int KelasId)
        {
            var Farakhan = db.Mapping_Farakhanha_Kelas.Include(u => u.Farakhanha).Where(u => u.F_KelasID == KelasId).OrderByDescending(u => u.Farakhanha.TarikheFarakhan).Select(x => new { Matn = x.Farakhanha.Matn, Movzoo = x.Farakhanha.Movzoo, TarikheFarakhan = x.Farakhanha.TarikheFarakhan });
            var Result = new List<Farakhan_Model>();
            foreach (var item in Farakhan)
            {
                Result.Add(new Farakhan_Model(item.Movzoo, item.Matn, Tools.JalaliDateWithoutHour(item.TarikheFarakhan ?? default(DateTime))));
            }
            return Result;
        }

        public Farakhanha Details(int id)
        {
            Farakhanha Farakhanha = db.Farakhanha.Find(id);
            if (Farakhanha == null)
            {
                return null;
            }
            return Farakhanha;
        }

        public void Create(Farakhanha Farakhanha)
        {
            db.Farakhanha.Add(Farakhanha);
            db.SaveChanges();
        }

        public void Edit(Farakhanha Farakhanha)
        {
            db.Entry(Farakhanha).State = EntityState.Modified;
            db.SaveChanges();
        }


        public void Delete(int id)
        {
            Farakhanha Farakhanha = db.Farakhanha.Find(id);
            if (Farakhanha != null)
            {
                Farakhanha.isDeleted = true;
                db.SaveChanges();
            }

        }

    }
}