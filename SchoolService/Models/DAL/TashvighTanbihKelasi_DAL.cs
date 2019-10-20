using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DAL
{
    public class TashvighTanbihKelasi_DAL
    { private SCEntities db;
        public TashvighTanbihKelasi_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<TashvighTanbihKelasi> List()
        {
            var TashvighTanbihKelasi = db.TashvighTanbihKelasi.Where(u=>u.isDeleted==false);
            return TashvighTanbihKelasi.ToList();
        }

        public TashvighTanbihKelasi Details(int id)
        {
            TashvighTanbihKelasi TashvighTanbihKelasi = db.TashvighTanbihKelasi.Find(id);
            if (TashvighTanbihKelasi == null)
            {
                return null;
            }
            return TashvighTanbihKelasi;
        }

        public void Create(TashvighTanbihKelasi TashvighTanbihKelasi)
        {
            db.TashvighTanbihKelasi.Add(TashvighTanbihKelasi);
            db.SaveChanges();
        }

        public void Edit(TashvighTanbihKelasi TashvighTanbihKelasi)
        {
            db.Entry(TashvighTanbihKelasi).State = EntityState.Modified;
            db.SaveChanges();
        }


        public void Delete(int id) {
            TashvighTanbihKelasi TashvighTanbihKelasi = db.TashvighTanbihKelasi.Find(id);
            if (TashvighTanbihKelasi != null)
            {
                TashvighTanbihKelasi.isDeleted = true;
                db.SaveChanges();
            }

        }
     
    }
}