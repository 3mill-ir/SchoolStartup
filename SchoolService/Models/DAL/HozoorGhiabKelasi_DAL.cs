using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DAL
{
    public class HozoorGhiabKelasi_DAL
    { private SCEntities db;
        public HozoorGhiabKelasi_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<HozoorGhiabKelasi> List()
        {
            var HozoorGhiabKelasi = db.HozoorGhiabKelasi.Where(u=>u.isDeleted==false);
            return HozoorGhiabKelasi.ToList();
        }

        public HozoorGhiabKelasi Details(int id)
        {
            HozoorGhiabKelasi HozoorGhiabKelasi = db.HozoorGhiabKelasi.Find(id);
            if (HozoorGhiabKelasi == null)
            {
                return null;
            }
            return HozoorGhiabKelasi;
        }

        public void Create(HozoorGhiabKelasi HozoorGhiabKelasi)
        {
            db.HozoorGhiabKelasi.Add(HozoorGhiabKelasi);
            db.SaveChanges();
        }

        public void Edit(HozoorGhiabKelasi HozoorGhiabKelasi)
        {
            db.Entry(HozoorGhiabKelasi).State = EntityState.Modified;
            db.SaveChanges();
        }


        public void Delete(int id) {
            HozoorGhiabKelasi HozoorGhiabKelasi = db.HozoorGhiabKelasi.Find(id);
            if (HozoorGhiabKelasi != null)
            {
                HozoorGhiabKelasi.isDeleted = true;
                db.SaveChanges();
            }

        }
   
    }
}