using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
    public class ModirManagement
    {
        public List<Karmandaan> ListModir(string ParrentID)
        {
            Karmandan_DAL KD = new Karmandan_DAL(new SCEntities());
            return KD.List("Modir",ParrentID);
        }
        public string AssignModirToMadrese(int modirId,int madreseId){
            Karmandan_DAL dal = new Karmandan_DAL(new SCEntities());
           return dal.AssignModirToMadrese(modirId, madreseId);
    }
        public string AddModir(Karmandaan model, string username, string password, string ConfirmPassword, ModelStateDictionary ModelState,out int? Id)
        {
            Id = null;
            if (string.IsNullOrEmpty(username))
            {
                ModelState.AddModelError("username", Resource.Resource.View_ValidationError);
                return "error";
            }
            if (string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("password", Resource.Resource.View_ValidationError);
                return "error";
            }
            if (password != ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "تایید کلمه عبور نا معتبر است");
                return "error";
            }

            SCEntities db = new SCEntities();
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = new ApplicationUser() { UserName = username };
            var result = UserManager.Create(user, password);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("username", "خطا در ثبت نام کاربری");
                return "error";
            }

            UserManager.AddToRole(user.Id, "Modir");
            UserInformation_DAL UD = new UserInformation_DAL(db);
            Karmandan_DAL KD = new Karmandan_DAL(db);
            model.Semat = "Modir";
            model.UserInformation.isDeleted = false;
            model.UserInformation.Status = true;
            model.UserInformation.ID = user.Id;

            model.F_UserInfromation = UD.Create(model.UserInformation);

          Id=  KD.Create(model);
            return "success";
        }



        public string EditModir(Karmandaan model, ModelStateDictionary ModelState, string ParrentId)
        {
            if (string.IsNullOrEmpty(model.UserInformation.Madaares.NaameMadrese))
            {
                ModelState.AddModelError("NaameMadrese", Resource.Resource.View_ValidationError);
                return "error";
            } SCEntities db = new SCEntities();

            Madaares_DAL dal = new Madaares_DAL(db);
            int? isExist = dal.isExist(model.UserInformation.Madaares, ParrentId);
            if (isExist == null || (isExist != null && isExist == model.UserInformation.Madaares.ID))
            {
                Karmandan_DAL KD = new Karmandan_DAL(new SCEntities());
                KD.Edit(model);
                return "success";
            }
            else
            {
                ModelState.AddModelError("NaameMadrese", "نام مورد نظر تکراری است");
                return "error";
            }

       
        }


        public string ChangeStatusModir(int ID)
        {
            var db = new SCEntities();
            UserInformation_DAL UD = new UserInformation_DAL(db);
            if (UD.ChangeStatus(db.Karmandaan.Find(ID).F_UserInfromation) != 0)
                return "OK";
            return "NotFound";
        }

        public string DeleteModir(int ID, string ParrentId,out int? madreseId)
        {

            Karmandan_DAL KD = new Karmandan_DAL(new SCEntities());
            madreseId = KD.Delete(ID, "Modir", ParrentId);
            if (madreseId == null)
            {
                return "error";
            }
            else
            {
                return "success";
            }
        }


        public Karmandaan DetailModir(int ID,string ParrentId)
        {
            Karmandan_DAL KD = new Karmandan_DAL(new SCEntities());
            return KD.Details(ID,"Modir",ParrentId);
        }
        public string ModirMadreseName(int ID)
        {
            Karmandan_DAL KD = new Karmandan_DAL(new SCEntities());
            return KD.ModirMadresename(ID);
        }
    }
}