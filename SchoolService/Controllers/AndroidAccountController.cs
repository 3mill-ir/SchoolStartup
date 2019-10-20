using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using SchoolService.Areas.Admin3mill.Models;
using SchoolService.Models;
using SchoolService.Models.BLL;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data.Entity;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Security;

namespace SchoolService.Controllers
{
    [Authorize]
    public class AndroidAccountController : ApiController
    {
        UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        [HttpPost]
        [AllowAnonymous]
        public async Task<string> Login(string UserName, string Password, string AndroidId, string Type)
        {
            JsonResultModel response = new JsonResultModel();
            if (ModelState.IsValid)
            {
                var user = UserManager.Find(UserName, Password);
                using (var db = new SCEntities())
                {
                    if (user != null)
                    {
                        var UserInf = db.UserInformation.FirstOrDefault(u => u.ID == user.Id);
                        if (UserInf.Status == true)
                        {
                            await SignInAsync(user, false);
                            if (UserInf.AndroidID != AndroidId)
                            {
                                UserInf.AndroidID = AndroidId;
                                db.SaveChanges();
                            }
                            if (Type == "Moallem")
                                return Tools.GenerateJsonResponse("OK", "شما با موفقیت وارد سیستم شدید", UserInf.FirstName + " " + UserInf.LastName + ":" + db.Moallem.FirstOrDefault(u => u.F_UserInformation == UserInf.ID).ID + "#" + UserInf.ID);
                            else if (Type == "Ovlia")
                            {
                                var Vali = db.Ovlia.FirstOrDefault(u => u.F_UserInformationID == UserInf.ID);
                                return Tools.GenerateOvliaLoginResponse("OK", "شما با موفقیت وارد سیستم شدید", Vali.ID, UserInf.ID);
                            }
                            else if (Type == "Moaven")
                                return Tools.GenerateJsonResponse("OK", "شما با موفقیت وارد سیستم شدید", UserInf.FirstName + " " + UserInf.LastName + ":" + db.Karmandaan.FirstOrDefault(u => u.F_UserInfromation == UserInf.ID).ID + "#" + UserInf.ID);
                            else
                                return Tools.GenerateJsonResponse("OK", "شما با موفقیت وارد سیستم شدید");
                        }
                        return Tools.GenerateJsonResponse("NOK", "کد تایید هویت تاکنون ثبت نگردیده");
                    }
                    if (Type != "Ovlia")
                    return Tools.GenerateJsonResponse("WrongUserPass", "نام کاربری یا رمز عبور اشتباه است");
                    else
                        return Tools.GenerateOvliaLoginResponse("WrongUserPass", "نام کاربری یا رمز عبور اشتباه است", -1, "NotFound");
                }
            }
            return Tools.GenerateJsonResponse("Error", "خطا در پردازش عملیات");
        }

        [HttpPost]
        public string ChangePassword(string OldPassword, string NewPassword)
        {
            var asm = new AndroidServicesManagement();
            return asm.ChangePassword(OldPassword, NewPassword);
        }

        [HttpPost]
        public string LogOut()
        {
            AuthenticationManager.SignOut();
            return Tools.GenerateJsonResponse("OK", "شما با موفقیت از حساب کاربری خود خارج شدید ");
        }

        [AllowAnonymous]
        [HttpPost]
        public string ForgottenPassword(string Tell, string Type)
        {
            string Result = Tools.ForgottenPassword(Tell, Type);
            if (Result != "NotFound")
            {
                Tools.SendSmsToWithText(Tell, "رمز عبور جدید شما جهت ورود به سیستم : \n" + Result);
                return Tools.GenerateJsonResponse("OK", "رمز عبور جدید شما به شماره همراه ثبت شده شما در سیستم ارسال خواهد شد");
            }
            return Tools.GenerateJsonResponse("NOK", "خطا در عملیات بازیابی رمز عبور");
        }

        #region Helper
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }
        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        #endregion
    }
}
