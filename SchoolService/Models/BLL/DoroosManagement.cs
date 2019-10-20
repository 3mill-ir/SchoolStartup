using SchoolService.Areas.Admin3mill.Models;
using SchoolService.Models.AndroidJsonModel;
using SchoolService.Models.DAL;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolService.Models.BLL
{
    public class DoroosManagement
    {
        public List<AndroidBarnameHaftegi_Model> ListeDorooseKelasha(int KelasId)
        {
            Doroos_DAL dal = new Doroos_DAL(new SCEntities());
            return dal.ListeDorooseKelasha(KelasId);
        }

        public SelectList ComboDorooseKelasha(int KelasId)
        {
            Doroos_DAL dal = new Doroos_DAL(new SCEntities());
            var temp=dal.ComboDorooseKelasha(KelasId);
            return new SelectList(temp.Select(u => new { Value = u.ID, Text = u.NameDars }), "Value", "Text");
        }
        public List<Doroos> ListDoroos(int? PaayeId = null)
        {
            Doroos_DAL KD = new Doroos_DAL(new SCEntities());
            return KD.List(PaayeId);
        }
        public List<Doroos> ListFovgholade(int MadreseId)
        {
            Doroos_DAL KD = new Doroos_DAL(new SCEntities());
            return KD.ListFovgholade(MadreseId);
        }
        public string AddDoroosFovgholade(Doroos model, int MadreseId, ModelStateDictionary ModelState)
        {
            if (string.IsNullOrEmpty(model.NaameDars))
            {
                ModelState.AddModelError("NaameDars", Resource.Resource.View_ValidationError);
                return "error";
            }
            model.NaameDars = model.NaameDars.Trim();
            SCEntities db = new SCEntities();
            Doroos_DAL dal = new Doroos_DAL(db);
            if (dal.isExistFovgholade(model, MadreseId) != null)
            {
                ModelState.AddModelError("NaameDars", "فیلد مورد نظر تکراری می باشد");
                return "error";
            }
            model.isDeleted = false;
            model.Sabet = false;
            dal.Create(model);
            return "success";
        }
        public string AddDoroos(Doroos model, ModelStateDictionary ModelState)
        {
            if (string.IsNullOrEmpty(model.NaameDars))
            {
                ModelState.AddModelError("NaameDars", Resource.Resource.View_ValidationError);
                return "error";
            }
            model.NaameDars = model.NaameDars.Trim();
            SCEntities db = new SCEntities();
            Doroos_DAL dal = new Doroos_DAL(db);
            if (dal.isExist(model) != null)
            {
                ModelState.AddModelError("NaameDars", "فیلد مورد نظر تکراری می باشد");
                return "error";
            }
            model.isDeleted = false;
            model.Sabet = true;
            dal.Create(model);
            return "success";
        }
        public string EditDoroosFovgholade(Doroos model, int MadreId, ModelStateDictionary ModelState)
        {
            if (string.IsNullOrEmpty(model.NaameDars))
            {
                ModelState.AddModelError("NaameDars", Resource.Resource.View_ValidationError);
                return "error";
            }
            var db = new SCEntities();
            Doroos_DAL dal = new Doroos_DAL(db);
            model.NaameDars = model.NaameDars.Trim();
            //int? result = dal.isExistFovgholade(model, MadreId);
            var dars = db.Doroos.FirstOrDefault(u => u.F_MadaaresID == MadreId && u.isDeleted == false && u.Sabet == false && u.NaameDars == model.NaameDars && u.F_PayeID == model.F_PayeID);
            //if (result == null || (result != null && result == model.ID))
            if (dars == null)
            {
                dal.Edit(model);
                return "success";
            }
            ModelState.AddModelError("NaameDars", "نام مورد نظر تکراری است");
            return "error";
        }

        public string EditDoroos(Doroos model, ModelStateDictionary ModelState)
        {
            if (string.IsNullOrEmpty(model.NaameDars))
            {
                ModelState.AddModelError("NaameDars", Resource.Resource.View_ValidationError);
                return "error";
            }
            var db = new SCEntities();
            Doroos_DAL dal = new Doroos_DAL(db);
            model.NaameDars = model.NaameDars.Trim();
            int? result = dal.isExist(model);
            if (result == null || (result != null && result == model.ID))
            {
                dal.Edit(model);
                return "success";
            }

            ModelState.AddModelError("NaameDars", "نام مورد نظر تکراری است");
            return "error";
        }

        public string DeleteDoroos(int darsId)
        {
            var db = new SCEntities();
            Doroos_DAL DD = new Doroos_DAL(db);
            if (DD.Delete(darsId) == null)
                return "error";
            else
                return "success";
        }
        public string DeleteDoroosFovgholade(int darsId, int MadreseId)
        {
            var db = new SCEntities();
            Doroos_DAL DD = new Doroos_DAL(db);
            if (DD.DeleteFovgholade(darsId, MadreseId) == null)
                return "error";
            else
                return "success";
        }

        public SelectList DoroosCombo(int KelasId, int MadreseId)
        {
            Doroos_DAL dal = new Doroos_DAL(new SCEntities());
            var Temp = dal.GetListDoroosForAdmin(KelasId, MadreseId);
            if (Temp != null)
                return new SelectList(Temp.Select(u => new { Value = u.ID, Text = u.NaameDars }), "Value", "Text");
            return null;
        }
        public SelectList DoroosComboAll(int MadreseId, int?[] Selected = null)
        { 

            Doroos_DAL dal = new Doroos_DAL(new SCEntities());
            var Temp = dal.GetListDoroosForAdminALL(MadreseId);
            return new SelectList(Temp.Select(u => new { Value = u.ID, Text = u.NaameDars + " ) پایه " + u.Paaye.NaamePaye + " ) " }), "Value", "Text", Selected);


        }

        public Doroos DetailDoroos(int ID)
        {
            Doroos_DAL KD = new Doroos_DAL(new SCEntities());
            return KD.Details(ID);
        }
        public Doroos DetailDoroosFovgholade(int ID, int MadreseId)
        {
            Doroos_DAL KD = new Doroos_DAL(new SCEntities());
            return KD.DetailsFovgholade(ID, MadreseId);
        }
    }
}