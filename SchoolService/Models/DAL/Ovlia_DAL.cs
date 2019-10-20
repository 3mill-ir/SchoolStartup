using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;

namespace SchoolService.Models.DAL
{
    public class Ovlia_DAL
    {
        private SCEntities db;
        public Ovlia_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<Ovlia> List(string ParrentId)
        {
            var Ovlia = db.Ovlia.Include(b => b.UserInformation).Where(u => u.UserInformation.isDeleted == false && u.F_ParrentID==ParrentId).OrderBy(u => u.UserInformation.LastName);
            return Ovlia.ToList();
        }

        public SelectList ListMadreseOvlia(int MadaresId, int? F_OvliaID)
        {
            return (new SelectList(db.Ovlia.Include(b => b.UserInformation).Where(u => u.UserInformation.isDeleted == false && u.UserInformation.F_MadaaresID == MadaresId).Select(u => new { Value = u.ID, Text = u.UserInformation.FirstName + " " + u.UserInformation.LastName }), "Value", "Text", F_OvliaID));
            //var Ovlia = db.Ovlia.Include(b => b.UserInformation).Where(u => u.UserInformation.isDeleted == false && u.UserInformation.F_MadaaresID == MadaresId).Select(x => new { OvliaId = x.ID, Name = x.UserInformation.FirstName + " " + x.UserInformation.LastName });
            //return Ovlia.ToList();
        }

        public Ovlia Details(int id, string ParrentId)
        {
            Ovlia Ovlia = List(ParrentId).FirstOrDefault(u=>u.ID==id);
            if (Ovlia == null)
            {
                return null;
            }
            return Ovlia;
        }

        public int Create(Ovlia Ovlia)
        {
            db.Ovlia.Add(Ovlia);
            db.SaveChanges();
            return Ovlia.ID;
        }

        public int Edit(Ovlia Ovlia)
        {
            db.Entry(Ovlia).State = EntityState.Modified;
            db.Entry(Ovlia).Property(x => x.F_ParrentID).IsModified = false;
            db.Entry(Ovlia).Property(x => x.F_UserInformationID).IsModified = false;
            db.Entry(Ovlia.UserInformation).State = EntityState.Modified;
            db.Entry(Ovlia.UserInformation).Property(x => x.Status).IsModified = false;
            db.Entry(Ovlia.UserInformation).Property(x => x.isDeleted).IsModified = false;
            db.Entry(Ovlia.UserInformation).Property(x => x.F_MadaaresID).IsModified = false;
            db.Entry(Ovlia.UserInformation).Property(x => x.AndroidID).IsModified = false;
            return db.SaveChanges();
        }

        public int? ChangeStatus(int id, string ParrentID)
        {
            Ovlia ovlia = Details(id, ParrentID);
            if (ovlia != null)
            {
                ovlia.UserInformation.Status = !ovlia.UserInformation.Status;
                db.SaveChanges();
                return ovlia.ID;
            }
            return null;
        }


        public int? Delete(int id, string ParrentID)
        {
            Ovlia ovlia = Details(id, ParrentID);
            if (ovlia != null)
            {
                ovlia.UserInformation.isDeleted = true;
                db.SaveChanges();
                return ovlia.ID;
            }
            return null;

        }

    }
}