using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SchoolService.Areas.Admin3mill.Models;

namespace SchoolService.Models.DAL
{
    public class Check_DAL
    {
        private SCEntities db;
        public Check_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public List<Check> List(int hazineId,string ParrentId)
        {
            var Check = db.Check.Where(u => u.IsDeleted == false && u.F_HazineId==hazineId && u.F_ParrentID==ParrentId);
            return Check.ToList();
        }

        public void CheckAlarm(int DaneshAmuzId)
        {
            try
            {
                var DaneshAmuz = db.DaneshAmuz.Include(u => u.Ovlia).FirstOrDefault(u => u.ID == DaneshAmuzId && u.isDeleted == false);
                var Today = DateTime.Now;
                var CheckHa = db.Check.Where(u => u.VaziateVosul == "تعیین نشده" && u.IsDeleted == false && u.Hazine.F_OvliaId == DaneshAmuz.F_OvliaID);
                foreach (var item in CheckHa)
                {
                    if (item.TarikheCheck.Value.AddDays(-1).ToShortDateString() == Today.ToShortDateString())
                    {
                        Tools.PushNotification(DaneshAmuz.Ovlia.UserInformation.AndroidID, "توجه !", "تنها یک روز تا موعد وصول چک شما باقی مانده");
                    }
                }
            }
            catch { return; }
        }
        public Check Details(int id,int HazineId,string ParrentId)
        {
            Check Check = List(HazineId, ParrentId).FirstOrDefault(u => u.ID == id);
            if (Check == null)
            {
                return null;
            }
            return Check;
        }

        public int Create(Check Check)
        {
            var temp = db.Hazine.FirstOrDefault(u => u.IsDeleted == false && u.ID == Check.F_HazineId);
            if (Check.MablagheCheck < temp.Bedehi || Check.MablagheCheck == temp.Bedehi && temp != null)
            {
                db.Check.Add(Check);
                temp.Bedehi = temp.Bedehi - Check.MablagheCheck;
                db.SaveChanges();
                return 1;
            }
            return -1;
        }

        public int Edit(Check Check)
        {
            try
            {
                var temp = db.Hazine.FirstOrDefault(u => u.IsDeleted == false && u.ID == Check.F_HazineId);
                if (temp != null)
                {
                    var pa = db.Check.FirstOrDefault(u => u.ID == Check.ID && u.IsDeleted == false);
                    temp.Bedehi = temp.Bedehi + pa.MablagheCheck;
                    if (temp.Bedehi > Check.MablagheCheck || temp.Bedehi == Check.MablagheCheck)
                        temp.Bedehi = temp.Bedehi - Check.MablagheCheck;
                    pa.MablagheCheck = Check.MablagheCheck;
                    pa.TarikheCheck = Check.TarikheCheck;
                    pa.Bank = Check.Bank;
                }
                return db.SaveChanges();
            }
            catch { return -1; }
        }

        public int Delete(int id,string ParrentId,int hazineId)
        {
            Check Check = List(hazineId, ParrentId).FirstOrDefault(u => u.ID==id);
            if (Check != null)
            {
                Check.IsDeleted = true;
                return db.SaveChanges();
            }
            return -1;
        }

        public int VosuleCheck(int CheckId,string ParrentId,int HazineId)
        {
            Check Check = List(HazineId, ParrentId).FirstOrDefault(u => u.ID== CheckId);
            if (Check != null && Check.IsDeleted == false)
            {
                Check.VaziateVosul = "وصول شده";
                return db.SaveChanges();
            }
            return -1;
        }

        public int BargashteCheck(int CheckId, string ParrentId, int HazineId)
        {
            Check Check = List(HazineId, ParrentId).FirstOrDefault(u => u.ID == CheckId);
            if (Check != null && Check.IsDeleted == false)
            {
                Check.VaziateVosul = "برگشت خورده";
                return db.SaveChanges();
            }
            return -1;
        }
    }
}