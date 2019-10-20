using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace SchoolService.Models.DAL
{
    public class BarnameEmtehani_DAL
    {
        private SCEntities db;
        public BarnameEmtehani_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<BarnameEmtehani> List()
        {
            var barnameemtehani = db.BarnameEmtehani.Where(u => u.isDeleted == false);
            return barnameemtehani.ToList();
        }

        public BarnameEmtehani Details(int id)
        {
            BarnameEmtehani barnameemtehani = db.BarnameEmtehani.Find(id);
            if (barnameemtehani == null)
            {
                return null;
            }
            return barnameemtehani;
        }

        public void Create(BarnameEmtehani barnameemtehani)
        {
            db.BarnameEmtehani.Add(barnameemtehani);
            db.SaveChanges();
        }

        public void Edit(BarnameEmtehani barnameemtehani)
        {
            db.Entry(barnameemtehani).State = EntityState.Modified;
            db.SaveChanges();
        }


        public void Delete(int id)
        {
            BarnameEmtehani barnameemtehani = db.BarnameEmtehani.Find(id);
            if (barnameemtehani != null)
            {
                barnameemtehani.isDeleted = true;
                db.SaveChanges();
            }

        }

        public BarnameEmtehani_ModelList ListDoroosEmtehani(int kelasId)
        {
            BarnameEmtehani_ModelList BML = new BarnameEmtehani_ModelList();
            var doroosList = from a in db.Kelas
                             where kelasId == a.ID && a.isDeleted == false
                             join b in db.BarnameHaftegi on a.ID equals b.F_KelasID
                             join c in db.Mapping_Moallem_Doroos on b.F_MoallemDoroosID equals c.ID
                             join d in db.Doroos on c.F_DoroosID equals d.ID
                             where d.isDeleted == false
                             select new { DarsName = d.NaameDars, MoallemDorosId = c.ID } into e
                             group e by e.DarsName into f
                             select new BarnameEmtehani_Model { NameDars = f.FirstOrDefault().DarsName, MoallemDoroosID = f.FirstOrDefault().MoallemDorosId };
            BML.BarnameEMtehaniList.AddRange(doroosList);

            return BML;
        }
        public string CreateListBarnameEmtehani(BarnameEmtehani_ModelList barnameemtehani, int kelasId, string ParentId)
        {
            List<BarnameEmtehani> list = new List<BarnameEmtehani>();
            BarnameEmtehani bm;
            foreach (var m in barnameemtehani.BarnameEMtehaniList)
            {
                bm = new BarnameEmtehani();
                bm.F_KelasID = kelasId;
                bm.F_MoallemDoroosID = m.MoallemDoroosID;
                bm.isDeleted = false;
                bm.TarikhKamel = m.Tarikh;
                bm.F_ParrentID = ParentId;
                list.Add(bm);
            }
            db.BarnameEmtehani.AddRange(list);
            db.SaveChanges();
            return "success";
        }

    }
}