using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SchoolService.Models.DAL
{
    public class Hazine_DAL
    {
        private SCEntities db;
        public Hazine_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<Hazine> List(int F_OvliaId,string ParrentId)
        {
            var Hazine = db.Hazine.Include(u=>u.Service).Where(u => u.IsDeleted == false && u.F_OvliaId == F_OvliaId && u.F_ParrentID==ParrentId);
            return Hazine.ToList();
        }

        public Hazine Details(int id,int OvliaId,string ParrentId)
        {
            Hazine Hazine = List(OvliaId,ParrentId).FirstOrDefault(u => u.ID == id);
            if (Hazine == null)
            {
                return null;
            }
            return Hazine;
        }

        public void Create(Hazine Hazine)
        {
            db.Hazine.Add(Hazine);
            db.SaveChanges();
        }

        public int Edit(Hazine Hazine)
        {

                db.Entry(Hazine).State = EntityState.Modified;
                db.Entry(Hazine).Property(x => x.IsDeleted).IsModified = false;
                db.Entry(Hazine).Property(x => x.F_OvliaId).IsModified = false;
                db.Entry(Hazine).Property(x => x.Tarikh).IsModified = false;
                return db.SaveChanges();
 
        }

        public int? Delete(int id,int OvliadId,string parrentId)
        {
            Hazine Hazine = List(OvliadId, parrentId).FirstOrDefault(u => u.ID == id);
            if (Hazine != null)
            {
                Hazine.IsDeleted = true;
                 db.SaveChanges();
                 return Hazine.ID;
            }
            return null;
        }
    }
}