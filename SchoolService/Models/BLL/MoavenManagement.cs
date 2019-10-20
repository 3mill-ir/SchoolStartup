using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolService.Models.DataModel;
using SchoolService.Models.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;

namespace SchoolService.Models.BLL
{
    public class MoavenManagement
    {
        public List<Karmandaan> ListMoaven(string ParrentId)
        {
            Karmandan_DAL KD = new Karmandan_DAL(new SCEntities());
            return KD.List("Moaven", ParrentId);
        }
        public string AddMoaven(Karmandaan model, string username, string password, string ConfirmPassword, ModelStateDictionary ModelState, out int? Id)
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

            UserManager.AddToRole(user.Id, "Moaven");
            UserInformation_DAL UD = new UserInformation_DAL(db);
            Karmandan_DAL KD = new Karmandan_DAL(db);
            model.Semat = "Moaven";
            model.UserInformation.isDeleted = false;
            model.UserInformation.Status = true;
            model.UserInformation.ID = user.Id;

            model.F_UserInfromation = UD.Create(model.UserInformation);

            Id = KD.Create(model);
            return "success";
        }

        public string EditMoaven(Karmandaan model)
        {
            Karmandan_DAL KD = new Karmandan_DAL(new SCEntities());
            KD.Edit(model);
            return "success";
        }


        public string ChangeStatusMoaven(int ID, string ParrentId)
        {

            Karmandan_DAL KD = new Karmandan_DAL(new SCEntities());
            if (KD.ChangeStatus(ID, "Moaven", ParrentId) == null)
            {
                return "error";
            }
            else
            {
                return "success";
            }
        }

        public string DeleteMoaven(int ID, string ParrentId)
        {

            Karmandan_DAL KD = new Karmandan_DAL(new SCEntities());
          if (KD.Delete(ID, "Moaven", ParrentId) == null)
            {
                return "error";
            }
            else
            {
                return "success";
            }
        }


        public Karmandaan DetailMoaven(int ID,string ParrentId)
        {
            Karmandan_DAL KD = new Karmandan_DAL(new SCEntities());
            return KD.Details(ID,"Moaven",ParrentId);
        }
    }
}