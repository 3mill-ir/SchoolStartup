using SchoolService.Models.DAL;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.BLL
{
    public class BarnameHaftegi_BLL
    {

        public void FreeBarnameHaftegi(int kelasId)
        {
            SCEntities db = new SCEntities();
            BarnameHaftegi_DAL dal = new BarnameHaftegi_DAL(db);
            dal.FreeBarnameHaftegi(kelasId);
        }
        public string BarnameHaftegi_Update(BarnameHaftegi_ModelList barnamehaftegi, int kelasId, int PayeId,string F_ParrentId)
        {
            SCEntities db = new SCEntities();
              BarnameHaftegi_DAL BD = new BarnameHaftegi_DAL(db);
            Mapping_Moallem_Doroos_DAL MMDD=new Mapping_Moallem_Doroos_DAL(db);

            List<BarnameHaftegi> listbanrame = new List<BarnameHaftegi>();
            BarnameHaftegi barname ;
            int? MoallemDoroosID;
            var kelas = db.Kelas.Find(kelasId);
            int MaxZang = kelas.MaxZang ?? default(int);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < MaxZang; j++)
                {
                    MoallemDoroosID=MMDD.GetRecordID(barnamehaftegi.BarnamehaftegiList[i][j].Barnamehaftegi_MoallemID,barnamehaftegi.BarnamehaftegiList[i][j].Barnamehaftegi_DoroosID);
                    if (MoallemDoroosID != null)
                    {
                        barname = new BarnameHaftegi();
                        barname.F_KelasID = kelasId;
                        barname.F_MoallemDoroosID = MoallemDoroosID;
                        barname.isDeleted = false;
                        barname.Ruz = i;
                        barname.Zang = j;
                        barname.F_ParrentID = F_ParrentId;
                        barname.Fovgholade = false;
                        listbanrame.Add(barname);
                    }

                }
            }
      

          
            BD.Create_viaList(listbanrame);
            return "success";
        }
    }
}