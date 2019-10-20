using SchoolService.Areas.Admin3mill.Models;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DAL
{
    public class Karmandan_DAL
    {
        private SCEntities db;
        public Karmandan_DAL(SCEntities SCE)
        {
            db = SCE;
        }
        public List<Karmandaan> List(string Semat,string ParrentID)
        {
            var Karmandan = db.Karmandaan.Where(u => u.UserInformation.isDeleted == false && u.Semat == Semat && u.F_ParrentID==ParrentID);
            return Karmandan.ToList();
        }

        public Karmandaan Details(int id, string Semat, string ParrentID)
        {
            Karmandaan Karmandan = List(Semat,ParrentID).FirstOrDefault(u=>u.ID==id);
            if (Karmandan == null)
            {
                return null;
            }
            return Karmandan;
        }
        public string ModirMadresename(int id)
        {
            Karmandaan Karmandan = db.Karmandaan.FirstOrDefault(u => u.ID == id);
            if (Karmandan == null)
            {
                return null;
            }
            return Karmandan.UserInformation.Madaares.NaameMadrese;
        }
        public string AssignModirToMadrese(int modirId, int madreseId)
        {
            var karmand = db.Karmandaan.Find(modirId);
            if (karmand != null)
            {
                karmand.UserInformation.F_MadaaresID = madreseId;
                db.SaveChanges();
                karmand.UserInformation.Madaares.ModirID = modirId;
                db.SaveChanges();
                return "success";
            }
            return "error";
        }

        public int Create(Karmandaan Karmandan)
        {
            db.Karmandaan.Add(Karmandan);
            db.SaveChanges();
            return Karmandan.ID;
        }

        public void Edit(Karmandaan Karmandan)
        {
            db.Entry(Karmandan).State = EntityState.Modified;
            db.Entry(Karmandan).Property(u => u.F_UserInfromation).IsModified = false;
            db.Entry(Karmandan).Property(u => u.F_ParrentID).IsModified = false;
            db.Entry(Karmandan).Property(u => u.Semat).IsModified = false;
            db.Entry(Karmandan.UserInformation).State = EntityState.Modified;
            db.Entry(Karmandan.UserInformation).Property(u => u.AndroidID).IsModified = false;
            db.Entry(Karmandan.UserInformation).Property(u => u.F_MadaaresID).IsModified = false;
            db.Entry(Karmandan.UserInformation).Property(u => u.isDeleted).IsModified = false;
            db.Entry(Karmandan.UserInformation).Property(u => u.Status).IsModified = false;
            db.Entry(Karmandan.UserInformation.Madaares).State = EntityState.Modified;
            db.Entry(Karmandan.UserInformation.Madaares).Property(u => u.F_MadreseId).IsModified = false;
            db.Entry(Karmandan.UserInformation.Madaares).Property(u => u.F_MaghaateID).IsModified = false;
            db.Entry(Karmandan.UserInformation.Madaares).Property(u => u.F_NemayandegiID).IsModified = false;
            db.Entry(Karmandan.UserInformation.Madaares).Property(u => u.F_ParrentID).IsModified = false;
            db.Entry(Karmandan.UserInformation.Madaares).Property(u => u.F_SaleTahsiliId).IsModified = false;
            db.Entry(Karmandan.UserInformation.Madaares).Property(u => u.isDeleted).IsModified = false;
            db.Entry(Karmandan.UserInformation.Madaares).Property(u => u.ModirID).IsModified = false;
            db.Entry(Karmandan.UserInformation.Madaares).Property(u => u.Status).IsModified = false;


             db.SaveChanges();
        }


        public int? Delete(int id, string Semat, string ParrentID)
        {
            Karmandaan Karmandan = Details(id,Semat,ParrentID);
            if (Karmandan != null && Semat=="Modir")
            {
                Karmandan.UserInformation.isDeleted = true;
                Karmandan.UserInformation.Madaares.ModirID = null;
                db.SaveChanges();
                return Karmandan.UserInformation.Madaares.ID;
            }
            else if (Karmandan != null && Semat == "Moaven")
            {
                Karmandan.UserInformation.isDeleted = true;
                db.SaveChanges();
                return Karmandan.ID;
            }
            return null;
        }

        public int? ChangeStatus(int id, string Semat, string ParrentID)
        {
            Karmandaan Karmandan = Details(id, Semat, ParrentID);
             if (Karmandan != null )
            {
                Karmandan.UserInformation.Status = !Karmandan.UserInformation.Status;
                db.SaveChanges();
                return Karmandan.ID;
            }
            return null;
        }

    }
}