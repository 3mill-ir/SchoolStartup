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
    public class OvliaManagement
    {
        public List<Ovlia> ListOvlia(string ParrentId)
        {
            Ovlia_DAL KD = new Ovlia_DAL(new SCEntities());
            return KD.List(ParrentId);
        }
        public string AddOvlia(Ovlia model, string username, string password, string ConfirmPassword, ModelStateDictionary ModelState, out int? Id)
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

            UserManager.AddToRole(user.Id, "Dabir");
            UserInformation_DAL UD = new UserInformation_DAL(db);
            Ovlia_DAL KD = new Ovlia_DAL(db);
            model.UserInformation.isDeleted = false;
            model.UserInformation.Status = true;
            model.UserInformation.ID = user.Id;
            model.F_UserInformationID = UD.Create(model.UserInformation);

            Id = KD.Create(model);
            return "success";
        }

        public string EditOvlia(Ovlia model)
        {
            Ovlia_DAL KD = new Ovlia_DAL(new SCEntities());
            KD.Edit(model);
                return "success";
           


        }


        public string ChangeStatusOvlia(int ID, string ParrentId)
        {
            Ovlia_DAL dal = new Ovlia_DAL(new SCEntities());
            if (dal.ChangeStatus(ID,  ParrentId) == null)
            {
                return "error";
            }
            else
            {
                return "success";
            }
    }

        public string DeleteOvlia(int ID,string ParrentId)
        {
            Ovlia_DAL dal = new Ovlia_DAL(new SCEntities());
            if (dal.Delete(ID, ParrentId) == null)
            {
                return "error";
            }
            else
            {
                return "success";
            }
        }


        public Ovlia DetailOvlia(int ID, string ParrentId)
        {
            Ovlia_DAL KD = new Ovlia_DAL(new SCEntities());
            return KD.Details(ID, ParrentId);
        }

        public SelectList  ComboBoxeOvlia(int MadaresId ,int? F_OvliaID=null)
        {
            Ovlia_DAL OD = new Ovlia_DAL(new SCEntities());
            return OD.ListMadreseOvlia(MadaresId, F_OvliaID);
            //new SelectList(Ovlia.Select(u => new { Value = u.Id, Text = u.Name }), "Value", "Text"));
            //List<SelectListItem> Result = new List<SelectListItem>();
            //foreach (var item in Ovlia)
            //{
            //    var m = new SelectListItem();
            //    m.Value = item.OvliaId + "";
            //    m.Text = item.Name;
            //    Result.Add(m);
            //}
            //return Result;
        }
    }
}