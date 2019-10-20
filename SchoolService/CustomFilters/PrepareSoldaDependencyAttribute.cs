using SchoolService.Areas.Admin3mill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolService.CustomFilters
{
    public class NemayandegiIdPrepareAttributeActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            int? Id;
            if (System.Web.HttpContext.Current.User.IsInRole("Nemayandegi"))
            {
                Id = Tools.NemayandegiId_Current();

            }
            else if (System.Web.HttpContext.Current.User.IsInRole("Modir"))
            {
                Id = Tools.NemayandegiId_CurrentModir(Tools.ModirParrent_Current());
            }
            else {
                Id = filterContext.ActionParameters["NemayandegiId"] as int?;
            }
            filterContext.Controller.ViewBag.NemayandegiId = Id;
            filterContext.ActionParameters["NemayandegiId"] = Id;

        }
    }
    public class ModirIdPreparePrepareAttributeActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            int? Id;
            if (System.Web.HttpContext.Current.User.IsInRole("Modir"))
            {
                Id =Tools.ModirId_Current();
                filterContext.HttpContext.Items["ParrentId"] = Tools.F_UserID();
            }
            else if (System.Web.HttpContext.Current.User.IsInRole("Nemayandegi"))
            {
                if (Tools.ModirParrent(filterContext.ActionParameters["ModirId"] as int? ?? default(int)) == Tools.F_UserID())
                {
                    Id = filterContext.ActionParameters["ModirId"] as int?;
                    filterContext.HttpContext.Items["ParrentId"] = Tools.F_UserID_Modir(filterContext.ActionParameters["ModirId"] as int? ?? default(int));
                }
                else
                {
                    filterContext.ActionParameters["ParrentId"] = null;
                    Id = null;
                }

            }
            else
            {
                Id = filterContext.ActionParameters["ModirId"] as int?;
                filterContext.HttpContext.Items["ParrentId"] = Tools.F_UserID_Modir(filterContext.ActionParameters["ModirId"] as int? ?? default(int));
            }
            filterContext.Controller.ViewBag.ModirId = Id;
            filterContext.ActionParameters["ModirId"] = Id;
        }
    }
}