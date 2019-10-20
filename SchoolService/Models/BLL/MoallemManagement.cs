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
    public class MoallemManagement
    {
        public List<Moallem> ListMoallem(string ParrentId)
        {
            Moallem_DAL KD = new Moallem_DAL(new SCEntities());
            return KD.List(ParrentId);
        }
        public string AssignMoallemToDars(int MoallemId, int?[] doroosId, string ParrentId)
        {
            Moallem_DAL KD = new Moallem_DAL(new SCEntities());
            KD.AssignMoallemToDars(MoallemId, doroosId, ParrentId);
            return "success";
        }
        public string ReAssignMoallemToDars(int MoallemId, int?[] doroosId, string ParrentId)
        {
            Moallem_DAL KD = new Moallem_DAL(new SCEntities());
            KD.FreeMoallemFromDars(MoallemId);
            KD.AssignMoallemToDars(MoallemId, doroosId, ParrentId);
            return "success";
        }
        public string AddMoallem(Moallem_Dars model, string username, string password, string ConfirmPassword, ModelStateDictionary ModelState, out int? Id)
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
            if (model.DarsId == null)
            {
                ModelState.AddModelError("DarsId", Resource.Resource.View_ValidationError);
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

            UserManager.AddToRole(user.Id, "Dabir");

            UserInformation_DAL UD = new UserInformation_DAL(db);
            Moallem_DAL KD = new Moallem_DAL(db);
            model.moallem.UserInformation.isDeleted = false;
            model.moallem.UserInformation.Status = true;
            model.moallem.UserInformation.ID = user.Id;

            model.moallem.F_UserInformation = UD.Create(model.moallem.UserInformation);

            Id = KD.Create(model.moallem);
            return "success";

        }

        public string EditMoallem(Moallem model)
        {
            Moallem_DAL KD = new Moallem_DAL(new SCEntities());
            KD.Edit(model);
            return "success";
        }


        public string ChangeStatusMoallem(int ID, string ParrentId)
        {
            Moallem_DAL dal = new Moallem_DAL(new SCEntities());
            if (dal.ChangeStatus(ID, ParrentId) == null)
            {
                return "error";
            }
            else
            {
                return "success";
            }
        }

        public string DeleteMoallem(int ID, string ParrentId)
        {
            Moallem_DAL dal = new Moallem_DAL(new SCEntities());
            if (dal.Delete(ID, ParrentId) == null)
            {
                return "error";
            }
            else
            {
                return "success";
            }
        }


        public Moallem DetailMoallem(int ID, string ParrentId)
        {
            Moallem_DAL KD = new Moallem_DAL(new SCEntities());
            return KD.Details(ID, ParrentId);
        }
    }
}