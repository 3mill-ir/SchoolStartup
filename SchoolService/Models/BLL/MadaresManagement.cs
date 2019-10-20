using Microsoft.AspNet.Identity;
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
    public class MadaresManagement
    {
        public List<Madaares> ListMadares(string ParrentId)
        {
            Madaares_DAL KD = new Madaares_DAL(new SCEntities());
            return KD.List(ParrentId);
        }
        public string AddMadares(Madaares model, ModelStateDictionary ModelState, string ParrentId, out int? madreseId)
        {
            madreseId = null;
            if (string.IsNullOrEmpty(model.NaameMadrese))
            {
                ModelState.AddModelError("madrese.NaameMadrese", Resource.Resource.View_ValidationError);
                return "error";
            }
            SCEntities db = new SCEntities();
            Madaares_DAL dal = new Madaares_DAL(db);
            if (dal.isExist(model, ParrentId) == null)
            {
                Madaares_DAL KD = new Madaares_DAL(db);
                model.isDeleted = false;
                model.Status = true;
                KD.Create(model);
                madreseId = model.ID;
                return "success";
            }
            else
            {
                ModelState.AddModelError("madrese.NaameMadrese", "نام مدرسه تکراری است");
            }
            return "error";
        }

        public int? hasModir(int id, string ParrentId)
        {
            SCEntities db = new SCEntities();
            Madaares_DAL dal = new Madaares_DAL(db);
            return dal.hasModir(id, ParrentId);
        }

        public string EditMadares(Madaares model, ModelStateDictionary ModelState, string ParrentId)
        {
            if (string.IsNullOrEmpty(model.NaameMadrese))
            {
                ModelState.AddModelError("NaameMadrese", Resource.Resource.View_ValidationError);
                return "error";
            } SCEntities db = new SCEntities();

            Madaares_DAL dal = new Madaares_DAL(db);
            int? isExist = dal.isExist(model, ParrentId);
            if (isExist == null || (isExist != null && isExist == model.ID))
            {
                dal.Edit(model);
                return "success";
            }
            else
            {
                ModelState.AddModelError("NaameMadrese", "نام مورد نظر تکراری است");
                return "success";
            }

        }


        public string ChangeStatusMadares(int ID, string ParrentId)
        {
            Madaares_DAL KD = new Madaares_DAL(new SCEntities());

            if (KD.ChangeStatus(ID, ParrentId) != null)
            {
                return "success";
            }
            return "error";

        }

        public string DeleteMadares(int ID, string ParrentId)
        {
            Madaares_DAL KD = new Madaares_DAL(new SCEntities());

            if (KD.Delete(ID, ParrentId) != null)
            {
                return "success";
            }
            return "error";
        }


        public Madaares DetailMadares(int ID, string ParrentId)
        {

            Madaares_DAL KD = new Madaares_DAL(new SCEntities());
            return KD.Details(ID, ParrentId);
        }
    }
}