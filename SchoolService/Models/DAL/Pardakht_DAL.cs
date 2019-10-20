using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SchoolService.Models.DAL
{
    public class Pardakht_DAL
    {
        private SCEntities db;
        public Pardakht_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<Pardakht> List(int hazineId,string ParrentId)
        {
            var Pardakht = db.Pardakht.Where(u => u.IsDeleted == false &&  u.F_HazineId==hazineId && u.F_ParrentID==ParrentId);
            return Pardakht.ToList();
        }

        public Pardakht Details(int id,string ParrentId,int hazineId)
        {
            Pardakht Pardakht = List(hazineId, ParrentId).FirstOrDefault(u => u.ID == id);
            if (Pardakht == null)
            {
                return null;
            }
            return Pardakht;
        }

        public int Create(Pardakht Pardakht)
        {
            var temp = db.Hazine.FirstOrDefault(u => u.IsDeleted == false && u.ID == Pardakht.F_HazineId);
            if (Pardakht.MablaghePardakhti < temp.Bedehi || Pardakht.MablaghePardakhti == temp.Bedehi && temp != null)
            {
                db.Pardakht.Add(Pardakht);
                temp.Bedehi = temp.Bedehi - Pardakht.MablaghePardakhti;
                db.SaveChanges();
                return 1;
            }
            return -1;
        }

        public int Edit(Pardakht Pardakht)
        {
            try
            {
                var temp = db.Hazine.FirstOrDefault(u => u.IsDeleted == false && u.ID == Pardakht.F_HazineId);
                if (temp != null)
                {
                    var pa = db.Pardakht.FirstOrDefault(u => u.ID == Pardakht.ID && u.IsDeleted == false);
                    temp.Bedehi = temp.Bedehi + pa.MablaghePardakhti;
                    if (temp.Bedehi > Pardakht.MablaghePardakhti || temp.Bedehi == Pardakht.MablaghePardakhti)
                        temp.Bedehi = temp.Bedehi - Pardakht.MablaghePardakhti;
                    pa.MablaghePardakhti = Pardakht.MablaghePardakhti;
                }
                return db.SaveChanges();
            }
            catch { return -1; }
        }

        public int Delete(int id,string ParrentId,int HazineId)
        {
            Pardakht Pardakht = List(HazineId, ParrentId).FirstOrDefault(u => u.ID == id);
            if (Pardakht != null)
            {
                Pardakht.IsDeleted = true;
                return db.SaveChanges();
            }
            return -1;
        }
    }
}