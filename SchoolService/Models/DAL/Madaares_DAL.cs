using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;

namespace SchoolService.Models.DAL
{
    public class Madaares_DAL
    {
        private SCEntities db;
        public Madaares_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public int? isExist(Madaares model, string ParrentId)
        {
            var found = List(ParrentId).FirstOrDefault(u => u.NaameMadrese == model.NaameMadrese);
            if (found == null)
                return null;
            else
                db.Entry(found).State = EntityState.Detached;
                return found.ID;
        }
        public List<Madaares> List(string ParrentId)
        {
            var Madaares = (ParrentId == null) ? db.Madaares.Where(u => u.isDeleted == false) : db.Madaares.Where(u => u.isDeleted == false && u.F_ParrentID == ParrentId);
            return Madaares.ToList();
        }

        public Madaares Details(int id, string ParrentId)
        {
            Madaares Madaares = List(ParrentId).FirstOrDefault(u=>u.ID==id);
            if (Madaares == null)
            {
                return null;
            }
            return Madaares;
        }

        public int? hasModir(int id, string ParrentId)
        {
            Madaares Madaares = List(ParrentId).FirstOrDefault(u => u.ID == id);
            if (Madaares == null)
            {
                return null;
            }
            return Madaares.ModirID;
        }

        public void Create(Madaares Madaares)
        {
            db.Madaares.Add(Madaares);
            db.SaveChanges();
        }

        public int Edit(Madaares Madaares)
        {

                db.Entry(Madaares).State = EntityState.Modified;
                //db.Entry(Madaares).Property(u => u.isDeleted).IsModified = false;
                //db.Entry(Madaares).Property(u => u.Status).IsModified = false;
                //db.Entry(Madaares).Property(u => u.F_NemayandegiID).IsModified = false;
                return db.SaveChanges();

        }


        public int? Delete(int ID, string ParrentId)
        {
            Madaares Madaares = Details(ID, ParrentId);
            if (Madaares != null)
            {
                Madaares.isDeleted = true;
                db.SaveChanges();
                return Madaares.ID;
            }
            return null;
        }

        public int? ChangeStatus(int ID, string ParrentId)
        {
            Madaares Madaares = Details(ID,ParrentId);
            if (Madaares != null)
            {
                Madaares.Status = !Madaares.Status;
                 db.SaveChanges();
                 return Madaares.ID;
            }
            return null;
        }

    }
}