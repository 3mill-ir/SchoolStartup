using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SchoolService.Models.DAL;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolService.Models.BLL
{
    public class MaghaateManagement
    {

        public string AddMaghaate(Maghaate model, ModelStateDictionary ModelState)
        {
            if (string.IsNullOrEmpty(model.NaameMaghta)) 
            {
                ModelState.AddModelError("NaameMaghta", Resource.Resource.View_ValidationError);
                return "error";
            }
            model.NaameMaghta = model.NaameMaghta.Trim();
            SCEntities db = new SCEntities();
            Maghaate_DAL dal = new Maghaate_DAL(db);
            if ( dal.isExist(model)!=null)
            {
                ModelState.AddModelError("NaameMaghta", "فیلد مورد نظر تکراری می باشد");
                return "error";
            }
            model.isDeleted = false;
            dal.Create(model);
            return "success";

        }

        public Maghaate DetailMaghaate(int MaghaateId)
        {
            Maghaate_DAL dal = new Maghaate_DAL(new SCEntities());
            return dal.Details(MaghaateId);
        }

        public string EditMaghaate(Maghaate model, ModelStateDictionary ModelState)
        {
            if (string.IsNullOrEmpty(model.NaameMaghta))
            {
                ModelState.AddModelError("NaameMaghta", Resource.Resource.View_ValidationError);
                return "error";
            }
            var db = new SCEntities();
            Maghaate_DAL KD = new Maghaate_DAL(db);
            model.NaameMaghta = model.NaameMaghta.Trim();
            int? result = KD.isExist(model);
            if (result == null || (result != null && result == model.ID)) 
            {
                KD.Edit(model);
                return "success";
            }
            ModelState.AddModelError("NaameMaghta", "نام مورد نظر تکراری است");
            return "error";
        }

        public string DeleteMaghaate(int MaghaateId)
        {
            Maghaate_DAL dal = new Maghaate_DAL(new SCEntities());
            if (dal.Delete(MaghaateId) ==null)
                return "error";
            else
            return "success";
        }
        public List<Maghaate> ListMaghaate()
        {
            Maghaate_DAL dal = new Maghaate_DAL(new SCEntities());
            return dal.List();
        }
        public SelectList MaghaateCombo(int? SelectedMaghtaId=null)
        {
            //Maghaate_DAL dal = new Maghaate_DAL(new SCEntities());
            //var Maghaate = dal.MaghaateCombo();
            //var listItems = new List<SelectListItem>();
            //foreach (var item in Maghaate)
            //{
            //    listItems.Add(new SelectListItem { Text = item.Text, Value = item.Value + "" });
            //}
            //return listItems;

            Maghaate_DAL dal = new Maghaate_DAL(new SCEntities());
            var Maghaate = dal.List();
            return new SelectList(Maghaate.Select(u => new { Value = u.ID, Text = u.NaameMaghta }), "Value", "Text", SelectedMaghtaId);
        }
    }
}