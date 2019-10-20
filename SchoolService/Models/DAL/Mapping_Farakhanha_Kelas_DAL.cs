using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DAL
{
    public class Mapping_Farakhanha_Kelas_DAL
    { private SCEntities db;
        public Mapping_Farakhanha_Kelas_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<Mapping_Farakhanha_Kelas> List()
        {
            var Mapping_Farakhanha_Kelas = db.Mapping_Farakhanha_Kelas;
            return Mapping_Farakhanha_Kelas.ToList();
        }

        public Mapping_Farakhanha_Kelas Details(int id)
        {
            Mapping_Farakhanha_Kelas Mapping_Farakhanha_Kelas = db.Mapping_Farakhanha_Kelas.Find(id);
            if (Mapping_Farakhanha_Kelas == null)
            {
                return null;
            }
            return Mapping_Farakhanha_Kelas;
        }

        public void Create(Mapping_Farakhanha_Kelas Mapping_Farakhanha_Kelas)
        {
            db.Mapping_Farakhanha_Kelas.Add(Mapping_Farakhanha_Kelas);
            db.SaveChanges();
        }

        public void Edit(Mapping_Farakhanha_Kelas Mapping_Farakhanha_Kelas)
        {
            db.Entry(Mapping_Farakhanha_Kelas).State = EntityState.Modified;
            db.SaveChanges();
        }


        public void Delete(int id) {
            Mapping_Farakhanha_Kelas Mapping_Farakhanha_Kelas = db.Mapping_Farakhanha_Kelas.Find(id);
            if (Mapping_Farakhanha_Kelas != null)
            {
                db.Mapping_Farakhanha_Kelas.Remove(Mapping_Farakhanha_Kelas);
                db.SaveChanges();
            }

        }
    
    }
}