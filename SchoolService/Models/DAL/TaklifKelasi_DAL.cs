using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DAL
{
    public class TaklifKelasi_DAL
    { private SCEntities db;
        public TaklifKelasi_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<TaklifKelasi> List()
        {
            var TaklifKelasi = db.TaklifKelasi.Where(u=>u.isDeleted==false);
            return TaklifKelasi.ToList();
        }

        public TaklifKelasi Details(int id)
        {
            TaklifKelasi TaklifKelasi = db.TaklifKelasi.Find(id);
            if (TaklifKelasi == null)
            {
                return null;
            }
            return TaklifKelasi;
        }

        public void Create(TaklifKelasi TaklifKelasi)
        {
            db.TaklifKelasi.Add(TaklifKelasi);
            db.SaveChanges();
        }

        public void CreateJami(TaklifKelasiJami TaklifKelasi)
        {
            db.TaklifKelasiJami.Add(TaklifKelasi);
            db.SaveChanges();
        }

        public void Edit(TaklifKelasi TaklifKelasi)
        {
            db.Entry(TaklifKelasi).State = EntityState.Modified;
            db.SaveChanges();
        }


        public void Delete(int id) {
            TaklifKelasi TaklifKelasi = db.TaklifKelasi.Find(id);
            if (TaklifKelasi != null)
            {
                TaklifKelasi.isDeleted = true;
                db.SaveChanges();
            }

        }
    
    }
}