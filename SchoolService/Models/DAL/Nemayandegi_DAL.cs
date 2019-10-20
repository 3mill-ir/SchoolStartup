using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DAL
{
    public class Nemayandegi_DAL
    {
        private SCEntities db;
        public Nemayandegi_DAL(SCEntities SCE)
        {
            db = SCE;
        }
        public int? isExist(Nemayandegi model)
        {
            var found = db.Nemayandegi.Local.FirstOrDefault(u => u.Name == model.Name && u.isDeleted == false);
            if (found == null)
                return null;
            else
                return found.ID;
        }
        public List<Nemayandegi> List()
        {
            var Nemayandegi = db.Nemayandegi.Where(u => u.isDeleted == false);
            return Nemayandegi.ToList();
        }

        public Nemayandegi Details(int id)
        {
            Nemayandegi Nemayandegi = db.Nemayandegi.Find(id);
            if (Nemayandegi != null && Nemayandegi.isDeleted == false)
            {
                return Nemayandegi;
            }
            return null;
        }

        public void Create(Nemayandegi Nemayandegi)
        {
            db.Nemayandegi.Add(Nemayandegi);
            db.SaveChanges();
        }

        public void Edit(Nemayandegi Nemayandegi)
        {
            db.Entry(Nemayandegi).State = EntityState.Modified;
            db.Entry(Nemayandegi).Property(x => x.F_UserID).IsModified = false;
            db.Entry(Nemayandegi).Property(x => x.Status).IsModified = false;
            db.Entry(Nemayandegi).Property(x => x.isDeleted).IsModified = false;
            db.SaveChanges();

        }


        public int? Delete(int id)
        {
            Nemayandegi Nemayandegi = db.Nemayandegi.Find(id);
            if (Nemayandegi != null)
            {
                Nemayandegi.isDeleted = true;
                db.SaveChanges();
                return Nemayandegi.ID;
            }
            return null;
        }


        public int? ChangeStatus(int id)
        {
            Nemayandegi Nemayandegi = db.Nemayandegi.Find(id);
            if (Nemayandegi != null)
            {
                Nemayandegi.Status = !Nemayandegi.Status;
                db.SaveChanges();
                return Nemayandegi.ID;
            }
            return null;
        }

    }
}