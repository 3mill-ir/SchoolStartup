using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DAL
{
    public class HozoorGhiab_DAL
    { private SCEntities db;
        public HozoorGhiab_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<HozoorGhiab> List()
        {
            var HozoorGhiab = db.HozoorGhiab.Where(u=>u.isDeleted==false);
            return HozoorGhiab.ToList();
        }

        public HozoorGhiab Details(int id)
        {
            HozoorGhiab HozoorGhiab = db.HozoorGhiab.Find(id);
            if (HozoorGhiab == null)
            {
                return null;
            }
            return HozoorGhiab;
        }

        public void Create(HozoorGhiab HozoorGhiab)
        {
            db.HozoorGhiab.Add(HozoorGhiab);
            db.SaveChanges();
        }

        public void Edit(HozoorGhiab HozoorGhiab)
        {
            db.Entry(HozoorGhiab).State = EntityState.Modified;
            db.SaveChanges();
        }


        public void Delete(int id) {
            HozoorGhiab HozoorGhiab = db.HozoorGhiab.Find(id);
            if (HozoorGhiab != null)
            {
                HozoorGhiab.isDeleted = true;
                db.SaveChanges();
            }

        }
 
    }
}