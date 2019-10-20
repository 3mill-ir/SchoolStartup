using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DAL
{
    public class Moallem_DAL
    {
        private SCEntities db;
        public Moallem_DAL(SCEntities SCE)
        {
            db = SCE;
        }
        public void AssignMoallemToDars(int MoallemId, int?[] doroosId, string ParrentId)
        {
            List<Mapping_Moallem_Doroos> map = new List<Mapping_Moallem_Doroos>();

            for (int i = 0; i < doroosId.Count(); i++)
            {
                Mapping_Moallem_Doroos mm = new Mapping_Moallem_Doroos();
                mm.F_MoallemID = MoallemId;
                mm.F_DoroosID = doroosId[i];
                mm.F_ParrentID = ParrentId;
                map.Add(mm);
            }

            db.Mapping_Moallem_Doroos.AddRange(map);
            db.SaveChanges();

        }
        public void FreeMoallemFromDars(int MoallemId)
        {

            var map = db.Mapping_Moallem_Doroos.Include(y => y.BarnameHaftegi).Include(y => y.BarnameEmtehani).Where(u => u.F_MoallemID == MoallemId).ToList();
            db.Mapping_Moallem_Doroos.RemoveRange(map);
            db.SaveChanges();

        }
        public List<Moallem> List(string ParrentId)
        {
            var Moallem = db.Moallem.Include(b => b.UserInformation).Where(u => u.UserInformation.isDeleted == false && u.F_ParrentID == ParrentId);
            return Moallem.ToList();
        }

        public Moallem Details(int id, string ParrentId)
        {
            Moallem Moallem = List(ParrentId).FirstOrDefault(u => u.ID == id);
            if (Moallem == null)
            {
                return null;
            }
            return Moallem;
        }

        public int Create(Moallem Moallem)
        {
            db.Moallem.Add(Moallem);
            db.SaveChanges();
            return Moallem.ID;
        }
        public int? ChangeStatus(int id, string ParrentID)
        {
            Moallem moallem = Details(id, ParrentID);
            if (moallem != null)
            {
                moallem.UserInformation.Status = !moallem.UserInformation.Status;
                db.SaveChanges();
                return moallem.ID;
            }
            return null;
        }

        public void Edit(Moallem Moallem)
        {

            db.Entry(Moallem).State = EntityState.Modified;
            db.Entry(Moallem).Property(x => x.F_UserInformation).IsModified = false;
            db.Entry(Moallem).Property(x => x.F_MadaaresID).IsModified = false;
            db.Entry(Moallem).Property(x => x.F_ParrentID).IsModified = false;
            db.Entry(Moallem.UserInformation).State = EntityState.Modified;
            db.Entry(Moallem.UserInformation).Property(x => x.Status).IsModified = false;
            db.Entry(Moallem.UserInformation).Property(x => x.isDeleted).IsModified = false;
            db.Entry(Moallem.UserInformation).Property(x => x.AndroidID).IsModified = false;
            db.Entry(Moallem.UserInformation).Property(x => x.F_MadaaresID).IsModified = false;
            db.SaveChanges();
        }


        public int? Delete(int id, string ParrentID)
        {
            Moallem moallem = Details(id, ParrentID);
            if (moallem != null)
            {
                moallem.UserInformation.isDeleted = true;
                db.SaveChanges();
                return moallem.ID;
            }
            return null;

        }

    }
}