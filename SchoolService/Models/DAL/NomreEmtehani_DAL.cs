using SchoolService.Models.AndroidJsonModel;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DAL
{
    public class NomreEmtehani_DAL
    {
        private SCEntities db;
        public NomreEmtehani_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<NomreEmtehani> List()
        {
            var NomreEmtehani = db.NomreEmtehani.Where(u => u.isDeleted == false);
            return NomreEmtehani.ToList();
        }

        public NomreEmtehani Details(int id)
        {
            NomreEmtehani NomreEmtehani = db.NomreEmtehani.Find(id);
            if (NomreEmtehani == null)
            {
                return null;
            }
            return NomreEmtehani;
        }

        public void Create(NomreEmtehani NomreEmtehani)
        {
            db.NomreEmtehani.Add(NomreEmtehani);
            db.SaveChanges();
        }

        public void Edit(NomreEmtehani NomreEmtehani)
        {
            db.Entry(NomreEmtehani).State = EntityState.Modified;
            db.SaveChanges();
        }


        public void Delete(int id)
        {
            NomreEmtehani NomreEmtehani = db.NomreEmtehani.Find(id);
            if (NomreEmtehani != null)
            {
                NomreEmtehani.isDeleted = true;
                db.SaveChanges();
            }

        }


        public dynamic KarnameDaneshAmuz(int DaneshAmoozId)
        {
            var result = from nomre in db.NomreEmtehani
                         where nomre.F_DaneshAmuzID == DaneshAmoozId
                         join barname in db.BarnameEmtehani on nomre.F_BarnameEmtehani equals barname.ID
                         where barname.isDeleted == false
                         join map in db.Mapping_Moallem_Doroos on barname.F_MoallemDoroosID equals map.ID
                         join dars in db.Doroos on map.F_DoroosID equals dars.ID
                         where dars.isDeleted == false
                         select new KarnameDars_Model
                         {
                             NaameDars=dars.NaameDars,
                             Nomre=nomre.Nomre
                         };
            Karname_Model Result = new Karname_Model();
            Result.TermeAvval.AddRange(result);
            return Result;
        }
    }
}