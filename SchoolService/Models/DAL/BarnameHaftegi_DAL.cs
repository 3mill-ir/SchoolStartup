using SchoolService.Models.AndroidJsonModel;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SchoolService.Models.DAL
{
    public class BarnameHaftegi_DAL
    {
        private SCEntities db;
        public BarnameHaftegi_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<BarnameHaftegi> List()
        {
            var BarnameHaftegi = db.BarnameHaftegi.Where(u => u.isDeleted == false);
            return BarnameHaftegi.ToList();
        }

        public dynamic GetMoallemBarname(int F_MoallemId)
        {
            var Result = new BarnameMoallem_Model();
            var temp = from barname in db.BarnameHaftegi
                       join Class in db.Kelas on barname.F_KelasID equals Class.ID
                       join map in db.Mapping_Moallem_Doroos on barname.F_MoallemDoroosID equals map.ID
                       where map.F_MoallemID == F_MoallemId && barname.isDeleted == false
                       join doroos in db.Doroos on map.F_DoroosID equals doroos.ID
                       select new
                         {
                             Kelas_ID = Class.ID,
                             NaameKelas = Class.NaameKelas,
                             BarnameHaftegi_ID = barname.ID,
                             barname.Ruz,
                             barname.Zang,
                             doroos.NaameDars,
                             barname.F_KelasID,
                             barname.F_MoallemDoroosID
                         };
            foreach (var item in temp)
            {
                var Kelas = new Class_Model();
                Kelas.ClassId = item.Kelas_ID;
                Kelas.ClassName = item.NaameKelas;
                foreach (var item2 in temp.Where(u => u.Kelas_ID == item.Kelas_ID))
                {
                    var barname = new AndroidBarnameHaftegi_Model();
                    barname.BarnameHaftegiId = item2.BarnameHaftegi_ID;
                    barname.NameDars = item2.NaameDars;
                    barname.RuzeHafte = item2.Ruz ?? default(int);
                    barname.Zang = item2.Zang ?? default(int);
                    Kelas.Barname.Add(barname);
                }
                Result.KelasHa.Add(Kelas);
            }
            Result.KelasHa = Result.KelasHa.GroupBy(x => new { x.ClassId }).Select(g => g.First()).ToList();
            return Result;
        }


        public dynamic  GetDaneshAmuzBarname(int F_DaneshAmuzId)
        {
            var Result = new List<AndroidBarnameHaftegi_Model>();
            var DaneshAmuz = db.DaneshAmuz.FirstOrDefault(u => u.ID == F_DaneshAmuzId && u.isDeleted == false);
            if (DaneshAmuz != null)
            {
                var temp = from barname in db.BarnameHaftegi
                           where barname.F_KelasID == DaneshAmuz.F_KelasID && barname.isDeleted == false
                           join Class in db.Kelas on barname.F_KelasID equals Class.ID
                           join map in db.Mapping_Moallem_Doroos on barname.F_MoallemDoroosID equals map.ID
                           join doroos in db.Doroos on map.F_DoroosID equals doroos.ID
                           where doroos.isDeleted == false
                           select new
                           {
                               barname.Ruz,
                               barname.Zang,
                               doroos.NaameDars,
                               barname.ID
                           };
                foreach (var item in temp)
                {
                    var barname = new AndroidBarnameHaftegi_Model();
                    barname.NameDars = item.NaameDars;
                    barname.RuzeHafte = item.Ruz ?? default(int);
                    barname.Zang = item.Zang ?? default(int);
                    barname.BarnameHaftegiId=item.ID;
                    Result.Add(barname);
                }
            }
            return Result;
        }

        public List<BarnameHaftegi_Model> List_Moallem_Doroos(int MadreseId,int PayeId)
        {

            var barname = from a in db.Paaye
                          where a.isDeleted == false && a.ID == PayeId
                          join b in db.Doroos on a.ID equals b.F_PayeID
                          where b.isDeleted == false
                          join c in db.Mapping_Moallem_Doroos on b.ID equals c.F_DoroosID
                          join d in db.Moallem on c.F_MoallemID equals d.ID
                          where d.UserInformation.isDeleted == false && d.F_MadaaresID==MadreseId
                          select new BarnameHaftegi_Model { Barnamehaftegi_MoallemID = d.ID, Barnamehaftegi_DoroosID = b.ID, Barnamehaftegi_MoallemFullName = d.UserInformation.FirstName + " " + d.UserInformation.LastName, Barnamehaftegi_DoroosName = b.NaameDars };
            return barname.ToList();
        }

        public void FreeBarnameHaftegi(int kelasId)
        {
            var barname = db.BarnameHaftegi.Where(u => u.F_KelasID == kelasId);
            foreach (var b in barname)
            {
                b.isDeleted = true;
            }
            db.SaveChanges();
        }

        public BarnameHaftegi_ModelList Details(int kelasId)
        {
            var barname = from a in db.BarnameHaftegi
                          where a.isDeleted == false && a.F_KelasID == kelasId
                          join b in db.Mapping_Moallem_Doroos on a.F_MoallemDoroosID equals b.ID
                          join c in db.Moallem on b.F_MoallemID equals c.ID
                          where c.UserInformation.isDeleted == false
                          join d in db.Doroos on b.F_DoroosID equals d.ID
                          where d.isDeleted == false
                          select new BarnameHaftegi_Model { Barnamehaftegi_ID = a.ID, Barnamehaftegi_ruz = a.Ruz, Barnamehaftegi_zang = a.Zang, Barnamehaftegi_MoallemFullName = c.UserInformation.FirstName + " " + c.UserInformation.LastName, Barnamehaftegi_MoallemID = c.ID, Barnamehaftegi_DoroosName = d.NaameDars, Barnamehaftegi_DoroosID = d.ID };

            if (barname.Count() == 0)
            {
                return null;
            }
            var kelas = db.Kelas.Find(kelasId);
            int Max_Zang = (kelas.MaxZang ?? default(int));

            BarnameHaftegi_ModelList BMList = new BarnameHaftegi_ModelList("Detail");


            for (int i = 0; i < 6; i++)
            {
                BMList.BarnamehaftegiList[i] = new BarnameHaftegi_Model[Max_Zang];
                for (int j = 0; j < Max_Zang; j++)
                {
                    BMList.BarnamehaftegiList[i][j] = new BarnameHaftegi_Model { Barnamehaftegi_MoallemFullName = "", Barnamehaftegi_DoroosName = "", Barnamehaftegi_MoallemID = 0, Barnamehaftegi_DoroosID = 0 };
                }
            }
            //DataTable table = new DataTable();
            //for (int i = 1; i <= Max_Zang; i++)
            //{
            //    table.Columns.Add(i.ToString(), typeof(string));
            //}

            //for (int i = 0; i < 6; i++)
            //{
            //    DataRow toInsert = table.NewRow();
            //    table.Rows.Add(toInsert);
            //}
            //foreach (var b in barname)
            //{
            //    table.Rows[(b.barnameHaftegi_Ruz) ?? default(int)][b.BarnameHafte_zang.ToString()] = b.BarnameHaftegi_ID + "#" + b.Moallem_FuLLName+ "#" + b.Moallem_ID + "#" + b.Doroos_ID + "#" + b.Doroos_Name;
            //}
            foreach (var b in barname)
            {
                int x = (b.Barnamehaftegi_ruz) ?? default(int);
                int y = (b.Barnamehaftegi_zang) ?? default(int);
                int z = x + y;
                BMList.BarnamehaftegiList[(b.Barnamehaftegi_ruz) ?? default(int)][(b.Barnamehaftegi_zang) ?? default(int)] = b;
            }
            BMList.MaxZang = Max_Zang;

            return BMList;
        }

        public void Create_viaList(List<BarnameHaftegi> BarnameHaftegi)
        {
            db.BarnameHaftegi.AddRange(BarnameHaftegi);
            db.SaveChanges();
        }

        public void Edit(BarnameHaftegi BarnameHaftegi)
        {
            db.Entry(BarnameHaftegi).State = EntityState.Modified;
            db.SaveChanges();
        }


        public void Delete(int id)
        {
            BarnameHaftegi BarnameHaftegi = db.BarnameHaftegi.Find(id);
            if (BarnameHaftegi != null)
            {
                BarnameHaftegi.isDeleted = true;
                db.SaveChanges();
            }

        }

    }
}