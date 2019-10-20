using SchoolService.Models.DAL;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolService.Models.BLL
{
    public class PaayeManagement
    {
        public SelectList PaayeCombo(int MaghtaId,int? SelectedPaayeId=null)
        {
            //Paaye_DAL dal = new Paaye_DAL(new SCEntities());
            //var Paaye = dal.PaayeCombo(MaghtaId);
            //var listItems = new List<SelectListItem>();
            //foreach (var item in Paaye)
            //{
            //    listItems.Add(new SelectListItem { Text = item.Text, Value = item.Value + "" });
            //}
            //return listItems;
            Paaye_DAL dal = new Paaye_DAL(new SCEntities());
            var Paaye = dal.List(MaghtaId);
            return new SelectList(Paaye.Select(u => new { Value = u.ID, Text = u.NaamePaye }), "Value", "Text", SelectedPaayeId);
        }
        public string AddPaaye(Paaye model, ModelStateDictionary ModelState)
        {
            if (string.IsNullOrEmpty(model.NaamePaye))
            {
                ModelState.AddModelError("NaamePaye", Resource.Resource.View_ValidationError);
                return "error";
            }
            model.NaamePaye = model.NaamePaye.Trim();
            SCEntities db = new SCEntities();
            Paaye_DAL dal = new Paaye_DAL(db);
            if (dal.isExist(model) != null)
            {
                ModelState.AddModelError("NaamePaye", "فیلد مورد نظر تکراری می باشد");
                return "error";
            }
            model.isDeleted = false;
            dal.Create(model);
            return "success";

        }

        public Paaye DetailPaaye(int PaayeId)
        {
            Paaye_DAL dal = new Paaye_DAL(new SCEntities());
            return dal.Details(PaayeId);
        }

        public string EditPaaye(Paaye model, ModelStateDictionary ModelState)
        {
            if (string.IsNullOrEmpty(model.NaamePaye))
            {
                ModelState.AddModelError("NaamePaye", Resource.Resource.View_ValidationError);
                return "error";
            }
            var db = new SCEntities();
            Paaye_DAL dal = new Paaye_DAL(db);
            model.NaamePaye = model.NaamePaye.Trim();
            int? result = dal.isExist(model);
            if (result == null || (result != null && result == model.ID))
            {
                dal.Edit(model);
                return "success";
            }

            ModelState.AddModelError("NaamePaye", "نام مورد نظر تکراری است");
            return "error";
        }

        public string DeletePaaye(int PaayeId)
        {
            Paaye_DAL dal = new Paaye_DAL(new SCEntities());
            if (dal.Delete(PaayeId) == null)
                return "error";
            else
                return "success";
        }

        public List<Paaye> ListPaaye(int? MaghaateId = null)
        {
            Paaye_DAL dal = new Paaye_DAL(new SCEntities());
            return dal.List(MaghaateId);
        }

    }
}