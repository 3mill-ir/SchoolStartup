using SchoolService.Areas.Admin3mill.Models;
using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolService.CustomFilters
{

    public class AccessPermissionOnPostEditAttributeActionFilter : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = base.AuthorizeCore(httpContext);
            if (!authorized)
            {
                return false;
            }
            if (System.Web.HttpContext.Current.User.IsInRole("Admin"))
            {
                return true;
            }
            var rd = httpContext.Request.RequestContext.RouteData;
            var id = rd.Values["F_ParrentID"].ToString();
            if (System.Web.HttpContext.Current.User.IsInRole("Nemayandegi"))
            {
                string F_UserID = Tools.F_UserID();
                if (F_UserID == id)
                {
                    return true;
                }
                SCEntities db = new SCEntities();
                var _users = db.Karmandaan.Where(u => u.Semat == "مدیر" && u.UserInformation.Madaares.Nemayandegi.F_UserID == F_UserID).Select(u => u.F_UserInfromation);
                if (_users != null && _users.Contains(id))
                {
                    return true;
                }
            }
            if (System.Web.HttpContext.Current.User.IsInRole("Modir"))
            {
                string F_UserID = Tools.F_UserID();
                if (F_UserID == id)
                {
                    return true;
                }
            }

            return false;

        }
    }

    public class AccessPermissionAttributeActionFilter : AuthorizeAttribute
    {
        public string Table { get; set; }
        public string RecordField { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = base.AuthorizeCore(httpContext);
            if (!authorized)
            {
                return false;
            }
            if (System.Web.HttpContext.Current.User.IsInRole("Admin"))
            {
                return true;
            }
            var rd = httpContext.Request.RequestContext.RouteData;
            var id = rd.Values[RecordField].ToString();
            SCEntities db=new SCEntities();
            string F_ParrentID = db.Database.SqlQuery<string>("Select F_ParrentID from " + Table + " where " + RecordField + " = " + id).FirstOrDefault<string>();
            if (System.Web.HttpContext.Current.User.IsInRole("Nemayandegi"))
            {
                string F_UserID = Tools.F_UserID();
                if (F_UserID == F_ParrentID)
                {
                    return true;
                }
   
                var _users = db.Karmandaan.Where(u => u.Semat == "مدیر" && u.UserInformation.Madaares.Nemayandegi.F_UserID == F_UserID).Select(u => u.F_UserInfromation);
                if (_users != null && _users.Contains(F_ParrentID))
                {
                    return true;
                }
            }
            if (System.Web.HttpContext.Current.User.IsInRole("Modir"))
            {
                string F_UserID = Tools.F_UserID();
                if (F_UserID == id)
                {
                    return true;
                }
            }

            return false;

        }
    }

}
