using SchoolService.Areas.Admin3mill.Models;
using SchoolService.Models.BLL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolService.CustomFilters
{
    public class PageTittleAttributeActionFilter : ActionFilterAttribute
    {
        public string Function { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            System.Resources.ResourceManager rm = Resource.Resource.ResourceManager;
            filterContext.Controller.ViewBag.PageTittle_Tittle = rm.GetString(Function + "_Tittle_PageTittle");
            filterContext.Controller.ViewBag.PageTittle_Description = rm.GetString(Function + "_Description_PageTittle");
            filterContext.Controller.ViewBag.PageTittle_ContactUS = rm.GetString(Function + "_ContactUs_PageTittle");
            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("PagetittleSection");
            var tempPath = new List<string>(section[Function + "_PathLog"].Split(new char[] { ';' }));
            List<PageTitle> PathLog = new List<PageTitle>();
            foreach (var temp in tempPath)
            {
                string[] path = temp.Split(',');
                string _arguman = null;
                if (path.Count() == 5 && path[4] != "null")
                {
                    string[] argumans = path[4].Split('-');
                    for (int i = 0; i < argumans.Count(); i++)
                    {

                        if (filterContext.ActionParameters.ContainsKey(argumans[i]) && filterContext.ActionParameters[argumans[i]] != null)
                        {
                            if (_arguman == null)
                            {
                                _arguman = "?" + string.Format("{0}={1}", argumans[i], filterContext.ActionParameters[argumans[i]].ToString());
                            }
                            else
                            {
                                _arguman = _arguman + "&" + string.Format("{0}={1}", argumans[i], filterContext.ActionParameters[argumans[i]].ToString());
                            }
                        }
                    }
                }
                PathLog.Add(new PageTitle(rm.GetString(path[0]), path[1], path[2], path[3], _arguman));
            }
            filterContext.Controller.ViewBag.PathLog = PathLog;

            PrePareTittleParrent(Function, filterContext);

        }


        public string PrePareTittleParrent(string func, ActionExecutingContext filterContext)
        {
            string[] Key = func.Split('_');
            switch (Key[0]){
                case "Paaye":
                    if (filterContext.ActionParameters.ContainsKey("MaghaateId") && filterContext.ActionParameters["MaghaateId"] != null)
                    {
                        MaghaateManagement mg = new MaghaateManagement();
                        string temp = filterContext.ActionParameters["MaghaateId"].ToString();
                        int result=0;
                        if(int.TryParse(temp,out result )){
                         var det=   mg.DetailMaghaate(result);
                         if (det != null)
                         {
                             filterContext.Controller.ViewBag.PageTittle_TittleExtra = " <i class=\"fa fa-angle-double-left\"></i> مقطع " + det.NaameMaghta;
                         }
                        }
                    }
                    break;

                case "Doroos":
                    if (filterContext.ActionParameters.ContainsKey("PaayeId") && filterContext.ActionParameters["PaayeId"] != null)
                    {
                        PaayeManagement mg = new PaayeManagement();
                        string temp = filterContext.ActionParameters["PaayeId"].ToString();
                        int result = 0;
                        if (int.TryParse(temp, out result))
                        {
                            var det = mg.DetailPaaye(result);
                            if (det != null)
                            {
                                filterContext.Controller.ViewBag.PageTittle_TittleExtra = " <i class=\"fa fa-angle-double-left\"></i> پایه " + det.NaamePaye + " <i class=\"fa fa-angle-double-left\"></i> مقطع " + det.Maghaate.NaameMaghta; 
                            }
                        }
                    }
                    break;
                case "Madares":
                case "Modir":
                case "Account":
                case "Moaven":
                case "Moallem":
                case "Ovlia":
                case "DaneshAmooz":
                case "DoroosFogholade":
                case "Kelas":
                case "BarnameHaftegi":
                case "OmureMali":      
                    if (filterContext.ActionParameters.ContainsKey("ModirId") && filterContext.ActionParameters["ModirId"] != null)
                    {
                        ModirManagement mg = new ModirManagement();
                        string temp = filterContext.ActionParameters["ModirId"].ToString();
                        int result = 0;
                        if (int.TryParse(temp, out result))
                        {
                            var det = mg.ModirMadreseName(result);
                            if (det != null)
                            {
                                filterContext.Controller.ViewBag.PageTittle_TittleExtra = "<div role=\"tooltip\"class=\"tooltip left\"><div class=\"tooltip-arrow\"></div><div class=\"tooltip-inner text-large\"> مدرسه " + det + "</div></div>";
                            }
                        }
                    }                    if (filterContext.ActionParameters.ContainsKey("NemayandegiId") && filterContext.ActionParameters["NemayandegiId"] != null)
                    {
                        NemayandegiManagement mg = new NemayandegiManagement();
                        string temp = filterContext.ActionParameters["NemayandegiId"].ToString();
                        int result = 0;
                        if (int.TryParse(temp, out result))
                        {
                            var det = mg.DetailNemayandegi(result);
                            if (det != null)
                            {
                                filterContext.Controller.ViewBag.PageTittle_TittleExtra = filterContext.Controller.ViewBag.PageTittle_TittleExtra + "<div role=\"tooltip\"class=\"tooltip left\"><div class=\"tooltip-arrow\"></div><div class=\"tooltip-inner text-large\"> نمایندگی " + det.Name + "</div></div>";
                            }
                        }
                    }
                    break;
            }
            return null;
        }
    }
}