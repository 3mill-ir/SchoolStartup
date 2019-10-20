using SchoolService.Areas.Admin3mill.Models;
using SchoolService.Models.DAL;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolService.Models.BLL
{
    public class OmureMaliManagement
    {
        #region Service
        public string AddService(Service model)
        {
            SCEntities db = new SCEntities();
            var found = db.Service.FirstOrDefault(u => u.ServiceName == model.ServiceName && u.IsDeleted == false);
            if (found == null)
            {
                Service_DAL dal = new Service_DAL(db);
                model.IsDeleted = false;
                dal.Create(model);
                return "OK";
            }
            return "Iterative";
        }

        public object DetailService(int madreseId,int ServiceId,string ParrentId)
        {
            Service_DAL dal = new Service_DAL(new SCEntities());
            return dal.Details(madreseId,ServiceId,ParrentId);
        }

        public string EditService(Service model)
        {
            var db = new SCEntities();
            Service_DAL KD = new Service_DAL(db);
            var Found = db.Service.FirstOrDefault(u => u.ServiceName == model.ServiceName && u.IsDeleted == false);
            if (Found == null)
            {
                if (KD.Edit(model) != -1)
                    return "OK";
                return "NOK";
            }
            return "Iterative";
        }

        public string DeleteService(int madreseId,int ServiceId,string ParrentId)
        {

            Service_DAL dal = new Service_DAL(new SCEntities());
            if (dal.Delete(madreseId,ServiceId,ParrentId) != -1)
                return "OK";
            return "NotFound";
        }
        public List<Service> ListService(int madreseId,string ParrentId)
        {
            Service_DAL dal = new Service_DAL(new SCEntities());
            return dal.List(madreseId,ParrentId);
        }

        public List<SelectListItem> ServiceCombo(int madreseId)
        {
            Service_DAL dal = new Service_DAL(new SCEntities());
            var Services = dal.ServiceCombo(madreseId);
            var listItems = new List<SelectListItem>();
            foreach (var item in Services)
            {
                listItems.Add(new SelectListItem { Text = item.Text, Value = item.Value + "" });
            }
            return listItems;
        }

        #endregion

        #region Hazine

        public string AddHazine(Hazine model)
        {
            Hazine_DAL dal = new Hazine_DAL(new SCEntities());
            model.IsDeleted = false;
            model.Tarikh = DateTime.Now;
            dal.Create(model);
            return "OK";
        }

        public object DetailHazine(int HazineId, int OvliaId, string parrentId)
        {
            Hazine_DAL dal = new Hazine_DAL(new SCEntities());
            return dal.Details(HazineId,OvliaId,parrentId);
        }

        public string EditHazine(Hazine model)
        {
            var db = new SCEntities();
            Hazine_DAL KD = new Hazine_DAL(db);
            if (KD.Edit(model) != -1)
                return "OK";
            return "NOK";
        }

        public string DeleteHazine(int HazineId,int OvliaId,string ParrentId)
        {
            Hazine_DAL dal = new Hazine_DAL(new SCEntities());
            if (dal.Delete(HazineId, OvliaId, ParrentId) ==null)
                return "error";
            return "success";
        }
        public List<Hazine> ListHazine(int F_OvliaId,string ParrentId)
        {
            Hazine_DAL dal = new Hazine_DAL(new SCEntities());
            return dal.List(F_OvliaId,ParrentId);
        }

        #endregion

        #region Pardakht

        public string AddPardakht(Pardakht model)
        {
            Pardakht_DAL dal = new Pardakht_DAL(new SCEntities());
            model.IsDeleted = false;
            model.Tarikh = DateTime.Now;
            if (dal.Create(model) == 1)
                return "OK";
            return "NOK";
        }

        public object DetailPardakht(int PardakhtId,string ParrentId,int hazineId)
        {
            Pardakht_DAL dal = new Pardakht_DAL(new SCEntities());
            return dal.Details(PardakhtId,ParrentId,hazineId);
        }

        public string EditPardakht(Pardakht model)
        {
            var db = new SCEntities();
            Pardakht_DAL KD = new Pardakht_DAL(db);
            if (KD.Edit(model) != -1)
                return "OK";
            return "NOK";
        }

        public string DeletePardakht(int PardakhtId,string ParrentId,int HazineId)
        {
            Pardakht_DAL dal = new Pardakht_DAL(new SCEntities());
            if (dal.Delete(PardakhtId,ParrentId,HazineId) != -1)
                return "OK";
            return "NotFound";
        }
        public List<Pardakht> ListPardakht(int HazineId,string ParrentId)
        {
            Pardakht_DAL dal = new Pardakht_DAL(new SCEntities());
            return dal.List(HazineId,ParrentId);
        }

        #endregion

        #region Check

        public string AddCheck(Check model)
        {
            Check_DAL dal = new Check_DAL(new SCEntities());
            model.IsDeleted = false;
            model.VaziateVosul = "تعیین نشده";
            if (dal.Create(model) == 1)
                return "OK";
            return "NOK";
        }

        public object DetailCheck(int CheckId,int HazineId,string ParrentId)
        {
            Check_DAL dal = new Check_DAL(new SCEntities());
            return dal.Details(CheckId,HazineId,ParrentId);
        }

        public string EditCheck(Check model)
        {
            var db = new SCEntities();
            Check_DAL KD = new Check_DAL(db);
            if (KD.Edit(model) != -1)
                return "OK";
            return "NOK";
        }

        public string DeleteCheck(int CheckId,string ParrentId,int hazineId)
        {
            Check_DAL dal = new Check_DAL(new SCEntities());
            if (dal.Delete(CheckId,ParrentId,hazineId) != -1)
                return "OK";
            return "NotFound";
        }
        public string VosuleCheck(int CheckId,string ParrentId,int HazineId)
        {
            Check_DAL dal = new Check_DAL(new SCEntities());
            if (dal.VosuleCheck(CheckId,ParrentId,HazineId) != -1)
                return "OK";
            return "NotFound";
        }
        public string BargashteCheck(int CheckId, string ParrentId, int HazineId)
        {
            Check_DAL dal = new Check_DAL(new SCEntities());
            if (dal.BargashteCheck(CheckId, ParrentId, HazineId) != -1)
                return "OK";
            return "NotFound";
        }
        public List<Check> ListCheck(int HazineId,string ParrentId)
        {
            Check_DAL dal = new Check_DAL(new SCEntities());
            return dal.List(HazineId,ParrentId);
        }

        #endregion
    }
}