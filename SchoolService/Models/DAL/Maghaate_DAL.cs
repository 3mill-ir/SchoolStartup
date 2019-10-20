using SchoolService.Models.AndroidJsonModel;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DAL
{
    public class Maghaate_DAL
    {
        private SCEntities db;
        public Maghaate_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public int? isExist(Maghaate model)
        {
            var found = db.Maghaate.FirstOrDefault(u => u.NaameMaghta == model.NaameMaghta && u.isDeleted == false);
            if (found == null)
                return null;
            else
                db.Entry(found).State = EntityState.Detached;
            return found.ID;
        }

        public List<Maghaate> List()
        {
            var Maghaate = db.Maghaate.Where(u => u.isDeleted == false);
            return Maghaate.ToList();
        }

        //public dynamic MaghaateCombo()
        //{
        //    var Maghaate = db.Maghaate.Where(u => u.isDeleted == false).Select(x => new { Value = x.ID, Text = x.NaameMaghta });
        //    return Maghaate;
        //}

        public Maghaate Details(int id)
        {
            Maghaate Maghaate = Find(id);
            if (Maghaate != null && Maghaate.isDeleted == false)
            {
                return Maghaate;
            }
            return null;
        }

        public void Create(Maghaate Maghaate)
        {
            db.Maghaate.Add(Maghaate);
            db.SaveChanges();
        }

        public void Edit(Maghaate Maghaate)
        {

            db.Entry(Maghaate).State = EntityState.Modified;
            db.Entry(Maghaate).Property(x => x.isDeleted).IsModified = false;
            db.SaveChanges();

        }


        public int? Delete(int id)
        {
            Maghaate Maghaate = Find(id);
            if (Maghaate != null && Maghaate.isDeleted == false)
            {
                Maghaate.isDeleted = true;
                db.SaveChanges();
                return Maghaate.ID;
            }
            return null;
        }

        private Maghaate Find(int id)
        {
            return db.Maghaate.Find(id);
        }


        public dynamic ListeMaghateBeHamraheKelasHa(int MoavenId)
        {
            List<Maghate_Model> Result = new List<Maghate_Model>();
            var Moaven = db.Karmandaan.FirstOrDefault(u => u.ID == MoavenId && u.Semat == "Moaven" && u.UserInformation.isDeleted == false);
            if (Moaven != null)
            {
                var temp = from st in db.Kelas
                           where st.F_MadaresID == Moaven.UserInformation.F_MadaaresID
                           join Paye in db.Paaye on st.F_PayeID equals Paye.ID
                           join Maghta in db.Maghaate on Paye.F_MaghaateID equals Maghta.ID
                           where Maghta.isDeleted == false && Paye.isDeleted == false
                           select new
                           {
                               NameKelas = st.NaameKelas,
                               KelasId = st.ID,
                               MaghtaName = Paye.NaamePaye + " " + Maghta.NaameMaghta,
                           };
                foreach (var item in temp.GroupBy(t => t.MaghtaName))
                {
                    var m = new Maghate_Model();
                    foreach (var item2 in item)
                    {
                        m.MaghtaName = item2.MaghtaName;
                        var mk = new MaghateClass_Model(item2.NameKelas, item2.KelasId, 0);
                        m.KelasHa.Add(mk);
                    }
                    Result.Add(m);
                }
            }
            return Result.GroupBy(x => new { x.MaghtaName }).Select(g => g.First()).ToList();
        }
    }
}