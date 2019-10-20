using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DAL
{
    public class Paaye_DAL
    {
        private SCEntities db;
        public Paaye_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        //public dynamic PaayeCombo(int MaghtaId)
        //{
        //    var Paaye = db.Paaye.Where(u => u.isDeleted == false && u.F_MaghaateID == MaghtaId).Select(x => new { Value = x.ID, Text = x.NaamePaye });
        //    return Paaye;
        //}

        public int? isExist(Paaye model)
        {

            var found = db.Paaye.FirstOrDefault(u => u.NaamePaye == model.NaamePaye && u.isDeleted == false && u.F_MaghaateID==model.F_MaghaateID);
            if (found == null)
                return null;
            else
                db.Entry(found).State = EntityState.Detached;
                return found.ID;
        }
        public List<Paaye> List(int? MaghaateId = null)
        {
            var Paaye = MaghaateId == null ? db.Paaye.Where(u => u.isDeleted == false) : db.Paaye.Where(u => u.isDeleted == false && u.F_MaghaateID == MaghaateId);
            return Paaye.ToList();
        }

        public Paaye Details(int id)
        {
            Paaye Paaye = db.Paaye.Find(id);
            if (Paaye !=null && Paaye.isDeleted==false)
            {
                return Paaye;
            }
            return null;
        }

        public void Create(Paaye Paaye)
        {
            db.Paaye.Add(Paaye);
            db.SaveChanges();
        }

        public void Edit(Paaye Paaye)
        {

                db.Entry(Paaye).State = EntityState.Modified;
                db.Entry(Paaye).Property(x => x.isDeleted).IsModified = false;
                 db.SaveChanges();

        }


        public int? Delete(int id)
        {
            Paaye Paaye = db.Paaye.Find(id);
            if (Paaye != null && Paaye.isDeleted==false)
            {
                Paaye.isDeleted = true;
                 db.SaveChanges();
                 return Paaye.ID;
            }
            else
                return null;
        }

    }
}