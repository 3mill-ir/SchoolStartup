using SchoolService.Areas.Admin3mill.Models;
using SchoolService.Models.AndroidJsonModel;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DAL
{
    public class PardakhtHa_DAL
    {
        private SCEntities db;
        public PardakhtHa_DAL(SCEntities SCE)
        {
            db = SCE;
        }

        public dynamic OmureMali(int DaneshAmoozId)
        {
            var DaneshAmuz = db.DaneshAmuz.FirstOrDefault(u => u.ID == DaneshAmoozId && u.isDeleted == false);
            if (DaneshAmuz != null)
            {
                List<PardakhtHa_Model> model = new List<PardakhtHa_Model>();
                var PardakhtHa = db.Pardakht.Where(u => u.IsDeleted == false && u.Hazine.F_OvliaId == DaneshAmuz.F_OvliaID).Select(y => new { AzBabate = y.Hazine.Service.ServiceName, MablaghePardakhti = y.MablaghePardakhti, Tarikh = y.Tarikh });
                var CheckHa = db.Check.Where(u => u.IsDeleted == false && u.Hazine.F_OvliaId == DaneshAmuz.F_OvliaID).Select(y => new { AzBabate = y.Hazine.Service.ServiceName, TarikheCheck = y.TarikheCheck, MablagheCheck = y.MablagheCheck, Banke = y.Bank, VaziateVosul = y.VaziateVosul });
                PardakhtHa_Model Result = new PardakhtHa_Model();
                foreach (var pardakht in PardakhtHa)
                {
                    var m = new PardakhteNaghdi_Model();
                    m.Tarikh = Tools.JalaliDateWithoutHour(pardakht.Tarikh ?? default(DateTime));
                    m.AzBabate = pardakht.AzBabate;
                    m.MablaghePardakhti = pardakht.MablaghePardakhti.ToString();
                    Result.PardakhthayeNaghdi.Add(m);
                }
                foreach (var check in CheckHa)
                {
                    var c = new PardakhteChecki_Model();
                    c.TarikheCheck = Tools.JalaliDateWithoutHour(check.TarikheCheck ?? default(DateTime));
                    c.AzBabate = check.AzBabate;
                    c.MablagheCheck = check.MablagheCheck.ToString();
                    c.Banke = check.Banke;
                    c.VaziateVosul = check.VaziateVosul;
                    Result.PardakhthayeChecki.Add(c);
                }
                model.Add(Result);
                return model;
            }
            return new List<PardakhtHa_Model>();
        }
    }
}