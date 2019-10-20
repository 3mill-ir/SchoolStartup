using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DAL
{
    public class Mapping_Moallem_Doroos_DAL
    {
        private SCEntities db;
        public Mapping_Moallem_Doroos_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<Mapping_Moallem_Doroos> List()
        {
            var Mapping_Moallem_Doroos = db.Mapping_Moallem_Doroos;
            return Mapping_Moallem_Doroos.ToList();
        }

        public Mapping_Moallem_Doroos Details(int id)
        {
            Mapping_Moallem_Doroos Mapping_Moallem_Doroos = db.Mapping_Moallem_Doroos.Find(id);
            if (Mapping_Moallem_Doroos == null)
            {
                return null;
            }
            return Mapping_Moallem_Doroos;
        }

        public void Create(Mapping_Moallem_Doroos Mapping_Moallem_Doroos)
        {
            db.Mapping_Moallem_Doroos.Add(Mapping_Moallem_Doroos);
            db.SaveChanges();
        }

        public void Edit(Mapping_Moallem_Doroos Mapping_Moallem_Doroos)
        {
            db.Entry(Mapping_Moallem_Doroos).State = EntityState.Modified;
            db.SaveChanges();
        }


        public void Delete(int id)
        {
            Mapping_Moallem_Doroos Mapping_Moallem_Doroos = db.Mapping_Moallem_Doroos.Find(id);
            if (Mapping_Moallem_Doroos != null)
            {
                db.Mapping_Moallem_Doroos.Remove(Mapping_Moallem_Doroos);
                db.SaveChanges();
            }

        }

        public int? GetRecordID(int MoallemId,int DoroosId)
        {
            Mapping_Moallem_Doroos Mapping_Moallem_Doroos = db.Mapping_Moallem_Doroos.FirstOrDefault(m=>m.F_DoroosID==DoroosId && m.F_MoallemID==MoallemId);
            if (Mapping_Moallem_Doroos != null)
            {
              return  Mapping_Moallem_Doroos.ID;
            }
            return null;
        }
    }
}