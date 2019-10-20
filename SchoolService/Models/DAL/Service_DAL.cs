using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SchoolService.Areas.Admin3mill.Models;

namespace SchoolService.Models.DAL
{
    public class Service_DAL
    {
        private SCEntities db;
        public Service_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<Service> List(int madreseId, string ParrentId)
        {
            var Service = db.Service.Where(u => u.IsDeleted == false && u.F_ParrentID == ParrentId && u.F_MadraseId==madreseId);
            return Service.ToList();
        }

        public dynamic ServiceCombo(int madreseId)
        {
            var Service = db.Service.Where(u => u.IsDeleted == false && u.F_MadraseId == madreseId).Select(x => new { Value = x.ID, Text = x.ServiceName });
            return Service;
        }

        public Service Details(int madreseId,int id,string ParrentId)
        {
            Service Service = List(madreseId,ParrentId).FirstOrDefault(u => u.ID == id);
            if (Service == null)
            {
                return null;
            }
            return Service;
        }

        public void Create(Service Service)
        {
            db.Service.Add(Service);
            db.SaveChanges();
        }

        public int Edit(Service Service)
        {
        
                db.Entry(Service).State = EntityState.Modified;
                db.Entry(Service).Property(x => x.F_ParrentID).IsModified = false;
                db.Entry(Service).Property(x => x.IsDeleted).IsModified = false;
                db.Entry(Service).Property(x => x.F_MadraseId).IsModified = false;
                return db.SaveChanges();
   
        }

        public int Delete(int madreseId, int id, string ParrentId)
        {
            Service Service = List(madreseId,ParrentId).FirstOrDefault(u => u.ID == id);
            if (Service != null)
            {
                Service.IsDeleted = true;
                return db.SaveChanges();
            }
            return -1;
        }
    }
}