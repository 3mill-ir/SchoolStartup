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
    public class NemayandegiManagement
    {

        public List<Nemayandegi> ListNemayandegi()
        {
            Nemayandegi_DAL dal = new Nemayandegi_DAL(new SCEntities());
            return dal.List();
        }
        public string AddNemayandegi(Nemayandegi model, string Username, string Password, string ConfirmPassword, ModelStateDictionary ModelState)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("Name", Resource.Resource.View_ValidationError);
                return "error";
            }
            if (Password == ConfirmPassword)
            {
                SCEntities db = new SCEntities();
                Nemayandegi_DAL nd = new Nemayandegi_DAL(db);
                if (nd.isExist(model) == null)
                {
                    UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                    var user = new ApplicationUser() { UserName = Username };
                    var result = UserManager.Create(user, Password);
                    if (result.Succeeded)
                    {
                        UserManager.AddToRoleAsync(user.Id, "Nemayandegi");
                        Nemayandegi_DAL dal = new Nemayandegi_DAL(db);
                        model.Name = model.Name.Trim();
                        model.isDeleted = false;
                        model.Status = true;
                        model.F_UserID = user.Id;
                        dal.Create(model);
                        return "success";
                    }
                    else
                    {
                        ModelState.AddModelError("Username", "خطا در ثبت نام کاربری مدیر");
                    }
                }
                else
                {
                    ModelState.AddModelError("Name", "نام نمایندگی تکراری است");
                }
            }
            else
            {
                ModelState.AddModelError("ConfirmPassword", "تایید کلمه عبور نادرست است.");
            }
            return "error";
        }

        public Nemayandegi DetailNemayandegi(int NemayandegiId)
        {
            Nemayandegi_DAL dal = new Nemayandegi_DAL(new SCEntities());
            return dal.Details(NemayandegiId);
        }

        public string EditNemayandegi(Nemayandegi model, ModelStateDictionary ModelState)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("Name", Resource.Resource.View_ValidationError);
                return "error";
            }
            Nemayandegi_DAL KD = new Nemayandegi_DAL(new SCEntities());
            model.Name = model.Name.Trim();
            int? result = KD.isExist(model);
            if (result == null || (result != null && result == model.ID))
            {
                KD.Edit(model);
                return "success";
            }
            ModelState.AddModelError("Name", "نام نمایندگی مورد نظر تکراری است");
            return "error";
        }

        public string ChangeStatusNemayandegi(int NemayandegiId)
        {
            Nemayandegi_DAL dal = new Nemayandegi_DAL(new SCEntities());
            if (dal.ChangeStatus(NemayandegiId) == null)
                return "error";
            else
                return "success";
        }

        public string DeleteNemayandegi(int NemayandegiId)
        {
            Nemayandegi_DAL dal = new Nemayandegi_DAL(new SCEntities());
            if (dal.Delete(NemayandegiId) == null)
                return "error";
            else
                return "success";
        }

    }
}