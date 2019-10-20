using SchoolService.Areas.Admin3mill.Models;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;


namespace SchoolService.Models.DAL
{
    public class Kelas_DAL
    {
        private SCEntities db;
        public Kelas_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<Kelas> List(int MadreseId)
        {
            var Kelas = db.Kelas.Where(u => u.isDeleted == false && u.F_MadaresID == MadreseId).OrderBy(u => u.NaameKelas);
            return Kelas.ToList();
        }
     

        public int? isExist(Kelas model, int MadreseId)
        {

            var found = List(MadreseId).FirstOrDefault(u => u.NaameKelas == model.NaameKelas);
            if (found == null)
                return null;
            else
                return found.ID;
        }
        public dynamic ListSchoolClasses(int MadreseId)
        {
            var Kelas = db.Kelas.Where(u => u.isDeleted == false && u.F_MadaresID == MadreseId).Select(x => new { Text = x.NaameKelas, Value = x.ID }).ToList();
            return Kelas;
        }
        public Kelas Details(int id, int MadreseId)
        {
            Kelas Kelas =List(MadreseId).FirstOrDefault(u=>u.ID==id);
            if (Kelas == null)
            {
                return null;
            }
            return Kelas;
        }

        public void Create(Kelas Kelas)
        {
            db.Kelas.Add(Kelas);
            db.SaveChanges();
        }

        public int Edit(Kelas Kelas)
        {

                db.Entry(Kelas).State = EntityState.Modified;
                db.Entry(Kelas).Property(u => u.isDeleted).IsModified = false;
                db.Entry(Kelas).Property(u => u.F_MadaresID).IsModified = false;
                return db.SaveChanges();
 
        }


        public int Delete(int id, int MadreseId)
        {
            Kelas Kelas = List(MadreseId).FirstOrDefault(u=>u.ID==id);
            if (Kelas != null)
            {
                Kelas.isDeleted = true;
                return db.SaveChanges();
            }
            return -1;
        }

    }
}