using SchoolService.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using Microsoft.AspNet.Identity;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Drawing;
using System.Web.Configuration;
using System.Net;
using SchoolService.Models.DataModel;
using System.Web.Mvc;

namespace SchoolService.Areas.Admin3mill.Models
{
    public static class Tools
    {
        public static void SendSmsToWithText(string Tell, string MessageText)
        {
            SMSIranianWebService.Send sms = new SMSIranianWebService.Send();
            long[] rec = null;
            byte[] status = null;
            string[] ReplyNum = { Tell };
            string ResultSMS = MessageText;
            sms.SendSms(WebConfigurationManager.AppSettings["SMSPannelUser"], WebConfigurationManager.AppSettings["SMSPannelPass"], ReplyNum, WebConfigurationManager.AppSettings["SMSPannel"], ResultSMS, false, "", ref rec, ref status);
        }


        public static void PushNotification(string AndroidId, string Title, string Matn)
        {
            string Body = "{\"applications\": [\"com.hezare.mmv\"],\"notification\":{\"show_app\": false},\"filter\": {\"device_id\": [\"" + AndroidId + "\"]},\"custom_content\": {\"Type\": \"Mali\",\"Title\": \"" + Title + "\",\"Matn\": \"" + Matn + "\"}}";
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", "Token " + WebConfigurationManager.AppSettings["PushNotificationToken"]);
                client.Headers.Add("Content-Type", "application/json;charset=utf-8");
                client.Headers.Add("Accept", "application/json");
                client.Encoding = System.Text.UTF8Encoding.UTF8;
                string responsebody = client.UploadString("http://panel.pushe.co/api/v1/notifications/", "POST", Body);
            }
        }

        public static string GenerateJsonResponse(string AKey, string AText, string AOption = "")
        {
            dynamic collectionWrapper = new { Root = new List<JsonResultModel> { new JsonResultModel { Key = AKey, Text = AText, Option = AOption } } };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }

        public static string GenerateOvliaLoginResponse(string AKey, string AText, int OvliaId, string UserInfId)
        {
            var db = new SCEntities();
            var temp = db.DaneshAmuz.Where(u => u.F_OvliaID == OvliaId).Select(y => new DaneshAmuz_Model { ID = y.ID, FullName = y.FirstName + " " + y.LastName });
            var Result = new OvliaJsonResultModel();
            Result.Key = AKey;
            Result.Text = AText;
            Result.OvliaChatID = UserInfId;
            if (temp != null)
                Result.DaneshAmuzan = temp.ToList();
            dynamic collectionWrapper = new { Root = new List<OvliaJsonResultModel>() { Result } };
            return Newtonsoft.Json.JsonConvert.SerializeObject(collectionWrapper);
        }
        public static int F_MadraseId()
        {
            string F_userId = Tools.F_UserID();
            using (var db = new SCEntities())
            {
                var User = db.UserInformation.FirstOrDefault(u => u.ID == F_userId);
                if (User != null)
                    return User.F_MadaaresID ?? default(int);
                else
                    return -1;
            }
        }

        public static SelectList SaleTahsiliCombo(int F_MadreseID)
        {
            using (var db = new SCEntities())
            {
                var mad = db.Madaares.FirstOrDefault(u => u.ID == F_MadreseID);
                if (mad != null)
                    return new SelectList(mad.SaleTahsili.Sal.Split('-').ToList());
                else
                    return new SelectList(new List<string>() {"00" });
            }
        }



        public static string F_UserID_Nemayandegi(int NemayandegiId)
        {
            using (var db = new SCEntities())
            {
                var nemyandegi = db.Nemayandegi.Find(NemayandegiId);
                if (nemyandegi != null)
                {
                    return nemyandegi.F_UserID;
                }
                return null;
            }
        }
        public static string F_UserID_Modir(int ModirId)
        {
            using (var db = new SCEntities())
            {
                var karmandan = db.Karmandaan.Find(ModirId);
                if (karmandan != null)
                {
                    return karmandan.F_UserInfromation;
                }
                return null;
            }
        }

        public static int? MadreseId(string ModirParrentId)
        {
            using (var db = new SCEntities())
            {
                var userinfo = db.UserInformation.Find(ModirParrentId);// (u => u.F_UserInfromation == ModirParrentId);
                if (userinfo != null)
                {
                    return userinfo.F_MadaaresID;
                }
                return null;
            }
        }
        public static int NemayandegiId_Current()
        {
            string F_userId = Tools.F_UserID();
            using (var db = new SCEntities())
            {
                return db.Nemayandegi.FirstOrDefault(u => u.F_UserID == F_userId).ID;
            }
        }

        public static int ModirId_Current()
        {
            string F_userId = Tools.F_UserID();
            using (var db = new SCEntities())
            {
                return db.Karmandaan.FirstOrDefault(u => u.UserInformation.ID == F_userId).ID;
            }
        }
        public static string ModirParrent_Current()
        {
            string F_userId = Tools.F_UserID();
            using (var db = new SCEntities())
            {
                return db.Karmandaan.FirstOrDefault(u => u.UserInformation.ID == F_userId).F_ParrentID;
            }
        }
        public static string ModirParrent(int ModirId)
        {
            using (var db = new SCEntities())
            {
                var karmandan = db.Karmandaan.Find(ModirId);
                if (karmandan != null)
                {
                    return karmandan.F_ParrentID;
                }
                return null;
            }
        }

        public static int ModirId()
        {
            using (var db = new SCEntities())
            {
                string F_userId = Tools.F_UserID();
                var karmandan = db.Karmandaan.FirstOrDefault(u => u.F_UserInfromation == F_userId);
                if (karmandan != null)
                {
                    return karmandan.ID;
                }
                return 0;
            }
        }
        public static int NemayandegiId_CurrentModir(string ParrentId)
        {
            using (var db = new SCEntities())
            {
                return db.Nemayandegi.FirstOrDefault(u => u.F_UserID == ParrentId).ID;
            }
        }

        public static string NemayandegiParrent_CurrentModir(string ParrentId)
        {
            using (var db = new SCEntities())
            {
                return db.Nemayandegi.FirstOrDefault(u => u.F_UserID == ParrentId).F_UserID;
            }
        }


        public static string F_UserName(string F_UserId)
        {
            using (var db = new SCEntities())
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var currentUser = manager.FindById(F_UserId);
                return currentUser.UserName;
            }
        }

        public static string F_UserName()
        {
            using (var db = new SCEntities())
            {
                string userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var currentUser = manager.FindById(userId);
                return currentUser.UserName;
            }
        }

        public static string AndroidId()
        {
            using (var db = new SCEntities())
            {
                string UserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
                var UserInf = db.UserInformation.FirstOrDefault(u => u.ID == UserId && u.isDeleted == false && u.Status == true);
                if (UserInf != null)
                    return UserInf.AndroidID;
                else
                    return "NOK";
            }
        }

        public static string AndroidId(string Id)
        {
            using (var db = new SCEntities())
            {
                var UserInf = db.UserInformation.FirstOrDefault(u => u.ID == Id && u.isDeleted == false && u.Status == true);
                if (UserInf != null)
                    return UserInf.AndroidID;
                else
                    return "NOK";
            }
        }

        public static string F_UserID()
        {
            using (var db = new SCEntities())
            {
                return System.Web.HttpContext.Current.User.Identity.GetUserId();
            }
        }

        public static string ForgottenPassword(string Tell, string Type)
        {
            using (var db = new SCEntities())
            {
                string ID = "";
                if (Type == "Ovlia")
                {
                    var temp = db.Ovlia.FirstOrDefault(u => u.Mobile == Tell && u.UserInformation.isDeleted == false);
                    if (temp != null)
                        ID = temp.F_UserInformationID;
                    else
                        return "NotFound";
                }
                else if (Type == "Moallem")
                {
                    var temp = db.Moallem.FirstOrDefault(u => u.Mobile == Tell && u.UserInformation.isDeleted == false);
                    if (temp != null)
                        ID = temp.F_UserInformation;
                    else
                        return "NotFound";
                }
                else
                {
                    var temp = db.Karmandaan.FirstOrDefault(u => u.Mobile == Tell && u.UserInformation.isDeleted == false);
                    if (temp != null)
                        ID = temp.F_UserInfromation;
                    else
                        return "NotFound";
                }
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var currentUser = manager.FindById(ID);
                manager.RemovePassword(currentUser.Id);
                string NewPass = Tools.PasswordGenerator(8);
                manager.AddPassword(currentUser.Id, NewPass);
                return NewPass;
            }
        }

        public static List<string> AllowedExtentions(string Type = "Default")
        {
            if (Type == "Img")
                return new List<string> { ".gif", ".jpg", ".jpeg", ".bmp", ".png" };
            return new List<string> { ".doc", ".xlsx", ".txt", ".pdf", ".ppt", ".gif", ".jpg", ".jpeg", ".bmp", ".png", ".m4a", ".mp3", ".wav" };
        }

        public static List<SelectListItem> Months()
        {
            var listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem { Text = "فروردین", Value = "01" });
            listItems.Add(new SelectListItem { Text = "اردیبهشت", Value = "02" });
            listItems.Add(new SelectListItem { Text = "خرداد", Value = "03" });
            listItems.Add(new SelectListItem { Text = "تیر", Value = "04" });
            listItems.Add(new SelectListItem { Text = "مرداد", Value = "05" });
            listItems.Add(new SelectListItem { Text = "شهریور", Value = "06" });
            listItems.Add(new SelectListItem { Text = "مهر", Value = "07" });
            listItems.Add(new SelectListItem { Text = "آبان", Value = "08" });
            listItems.Add(new SelectListItem { Text = "آذر", Value = "09" });
            listItems.Add(new SelectListItem { Text = "دی", Value = "10" });
            listItems.Add(new SelectListItem { Text = "بهمن", Value = "11" });
            listItems.Add(new SelectListItem { Text = "اسفند", Value = "12" });
            return listItems;
        }
        public static string FileSave(HttpPostedFileBase Content_Two, string PathForSave = "PostImagesPath")
        {

            string path = Tools.ReturnPathPhysicalMode(PathForSave, "FileSave()");
            string extension = Path.GetExtension(Content_Two.FileName);
            string curFile = "";
            string RandomValueString;
            Random rnd = new Random();
            do
            {

                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                RandomValueString = new string(Enumerable.Repeat(chars, 12)
                 .Select(s => s[rnd.Next(s.Length)]).ToArray());
                curFile = path + "/" + RandomValueString + extension;  //Your path
            } while (File.Exists(curFile));
            //var allowedExtensions = Tools.AllowedExtentions("Img");
            var extensions = Path.GetExtension(Content_Two.FileName).ToLower();
            //if (allowedExtensions.Contains(extensions))
            //{
            //    string pic = System.IO.Path.GetFileName(RandomValueString + extension);
            //    WebImage img = new WebImage(Content_Two.InputStream);
            //    if (img.Width < 790 || img.Height < 460)
            //    {
            //        int wi;
            //        int hi;
            //        // maintain the aspect ratio despite the thumbnail size parameters
            //        if (img.Width > img.Height)
            //        {
            //            wi = 790;
            //            hi = (int)(img.Height * ((decimal)790 / img.Width));
            //        }
            //        else
            //        {
            //            hi = 460;
            //            wi = (int)(img.Width * ((decimal)460 / img.Height));
            //        }
            //        img.Resize(wi, hi);
            //    }
            //    img.Save(path + pic);
            //}
            string pic = System.IO.Path.GetFileName(RandomValueString + extension);
            byte[] tempFile = new byte[Content_Two.ContentLength];
            Content_Two.InputStream.Read(tempFile, 0, Content_Two.ContentLength);

            System.IO.File.WriteAllBytes(path + pic, tempFile);
            return RandomValueString + extension;
        }
        public static string ConvertNativeDigits(this string text)
        {

            if (text == null)
                return null;
            if (text.Length == 0)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            foreach (char character in text)
            {
                if (char.IsDigit(character))
                    sb.Append(char.GetNumericValue(character));
                else
                    sb.Append(character);
            }
            return sb.ToString();


        }
        private static readonly CultureInfo arabic = new CultureInfo("fa-IR");
        private static readonly CultureInfo latin = new CultureInfo("en-US");

        /// <summary>
        /// in tabe jahate tabdile zabane english be arabic ( ta hududi farsi ) estefade mishavad
        /// </summary>
        /// <param name="input">reshteye morede nazar baraye tabdil</param>
        /// <returns>
        /// string
        /// reshteye tabdil shode
        /// </returns>
        public static string ToArabic(string input)
        {
            var arabicDigits = arabic.NumberFormat.NativeDigits;
            for (int i = 0; i < arabicDigits.Length; i++)
            {
                input = input.Replace(i.ToString(), arabicDigits[i]);
            }
            return input;
        }

        public static string ToLatin(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                var latinDigits = latin.NumberFormat.NativeDigits;
                var arabicDigits = arabic.NumberFormat.NativeDigits;
                for (int i = 0; i < latinDigits.Length; i++)
                {
                    input = input.Replace(arabicDigits[i], latinDigits[i]);
                }
            }
            return input;
        }

        /// <summary>
        /// in tabe tarikhe miladi ra dar forme datetime gerefte va tarikhe jalali ra dar forme string baz migardanad
        /// </summary>
        /// <param name="date">tarikhe morede nazar jahate tabdil</param>
        /// <returns>
        /// string
        /// tarikhe tabdil shode be jalali
        /// </returns>
        public static string GetDateTimeReturnJalaliDate(DateTime date)
        {
            if (date != null && date.ToShortDateString() != "1/1/0001")
            {
                PersianCalendar p = new PersianCalendar();
                int Month = p.GetMonth(date);
                int Year = p.GetYear(date);
                int Day = p.GetDayOfMonth(date);
                int Hour = p.GetHour(date);
                int Minute = p.GetMinute(date);
                int Second = p.GetSecond(date);
                string result1 = "";
                string result = Tools.ToArabic(Year.ToString()) + '/';
                if (Month.ToString().Count() == 2)
                    result += Tools.ToArabic(Month.ToString()) + '/';
                else
                    result += '۰' + Tools.ToArabic(Month.ToString()) + '/';
                if (Day.ToString().Count() == 2)
                    result += Tools.ToArabic(Day.ToString());
                else
                    result += '۰' + Tools.ToArabic(Day.ToString());
                if (Hour.ToString().Count() == 2)
                    result1 += Tools.ToArabic(Hour.ToString()) + ':';
                else
                    result1 += '۰' + Tools.ToArabic(Hour.ToString()) + ':';
                if (Minute.ToString().Count() == 2)
                    result1 += Tools.ToArabic(Minute.ToString()) + ':';
                else
                    result1 += '۰' + Tools.ToArabic(Minute.ToString()) + ':';
                if (Second.ToString().Count() == 2)
                    result1 += Tools.ToArabic(Second.ToString());
                else
                    result1 += '۰' + Tools.ToArabic(Second.ToString());
                string FinalResult = result + " " + result1;
                return FinalResult;
            }
            else
                return "";
        }


        public static string SpecialJalaliFormat(DateTime date)
        {
            if (date != null && date.ToShortDateString() != "1/1/0001")
            {
                PersianCalendar p = new PersianCalendar();
                int Month = p.GetMonth(date);
                string Week = p.GetDayOfWeek(date).ToString();
                int Year = p.GetYear(date);
                int Day = p.GetDayOfMonth(date);
                List<string> WeekDay = new List<string>() { "شنبه", "یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنجشنبه", "جمعه" };
                List<string> EnWeekDay = new List<string>() { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
                List<string> Months = new List<string>() { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
                return WeekDay[EnWeekDay.IndexOf(Week)] + " " + Day + " " + Months[Month - 1];
            }
            else
                return "";
        }
        public static string JalaliDateWithoutHour(DateTime date)
        {
            PersianCalendar p = new PersianCalendar();
            int Month = p.GetMonth(date);
            int Year = p.GetYear(date);
            int Day = p.GetDayOfMonth(date);
            string result = Tools.ToArabic(Year.ToString()) + '/';
            if (Month.ToString().Count() == 2)
                result += Tools.ToArabic(Month.ToString()) + '/';
            else
                result += '۰' + Tools.ToArabic(Month.ToString()) + '/';
            if (Day.ToString().Count() == 2)
                result += Tools.ToArabic(Day.ToString());
            else
                result += '۰' + Tools.ToArabic(Day.ToString());

            return result;
        }

        /// <summary>
        /// in tabe tarikhe jalali ra dar forme string gerefte va tarikhe miladi ra dar forme datetime baz migardanad
        /// </summary>
        /// <param name="date">tarikhe jalaliye morede nazar dar forme string</param>
        /// <param name="_Date"></param>
        /// <returns>
        ///  
        /// </returns>
        public static bool GetJalaliDateReturnDateTime(string date, out DateTime _Date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                Regex rex = new Regex(@"^[۰-۹0-9]{4}\/[۰-۹0-9]{2}\/[۰-۹0-9]{2} [۰-۹0-9]{2}:[۰-۹0-9]{2}:[۰-۹0-9]{2}$");
                if (rex.Match(date).Success)
                {
                    string firstpart = date.Substring(0, date.IndexOf(':') - 2);
                    string SecondPart = date.Substring(date.IndexOf(':') - 2);
                    string[] persianDatePartsStart = firstpart.Replace(" ", "").Split('/');
                    string[] persianDatePartsStartHour = SecondPart.Replace(" ", "").Split(':');


                    int persianYearStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStart[0]));
                    int persianMonthStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStart[1]));
                    int persianDayStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStart[2]));
                    int persianHourStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStartHour[0]));
                    int persianMinStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStartHour[1]));
                    int persianSecondStart = int.Parse(Tools.ConvertNativeDigits(persianDatePartsStartHour[2]));

                    string datetimeString = string.Format("{0}-{1}-{2} {3}:{4}:{5}", persianYearStart, persianMonthStart, persianDayStart, persianHourStart, persianMinStart, persianSecondStart);

                    PersianCalendar pc = new PersianCalendar();
                    try
                    {
                        DateTime start = new DateTime(persianYearStart, persianMonthStart, persianDayStart, persianHourStart, persianMinStart, persianSecondStart, pc);
                        _Date = start;
                        return true; ;
                    }
                    catch
                    {
                        _Date = DateTime.Now;
                        return false;
                    }
                }
            }
            _Date = DateTime.Now;
            return false; ;
        }

        public static string ImageSave_MaintainAspect(HttpPostedFileBase Content_Two, string F_UserName, string PathForSave = "PostImagesPath")
        {
            string path = Tools.ReturnPathPhysicalMode(PathForSave, "ImageSave_MaintainAspect()");
            string extension = Path.GetExtension(Content_Two.FileName);
            string curFile = "";
            string RandomValueString;
            Random rnd = new Random();
            do
            {

                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                RandomValueString = new string(Enumerable.Repeat(chars, 12)
                 .Select(s => s[rnd.Next(s.Length)]).ToArray());
                curFile = path + "/" + RandomValueString + extension;  //Your path
            } while (File.Exists(curFile));

            string pic = System.IO.Path.GetFileName(RandomValueString + extension);
            WebImage img = new WebImage(Content_Two.InputStream);
            if (img.Width < 790 || img.Height < 460)
            {
                int wi;
                int hi;
                // maintain the aspect ratio despite the thumbnail size parameters
                if (img.Width > img.Height)
                {
                    wi = 790;
                    hi = (int)(img.Height * ((decimal)790 / img.Width));
                }
                else
                {
                    hi = 460;
                    wi = (int)(img.Width * ((decimal)460 / img.Height));
                }
                img.Resize(wi, hi);
            }
            img.Save(path + pic);
            return RandomValueString + extension;
        }



        public static string ImageSave_Gallery(HttpPostedFileBase Content_Two, string PathForSave)
        {

            string path = PathForSave;

            string extension = Path.GetExtension(Content_Two.FileName);
            string curFile = "";
            string RandomValueString;
            Random rnd = new Random();
            do
            {

                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                RandomValueString = new string(Enumerable.Repeat(chars, 12)
                 .Select(s => s[rnd.Next(s.Length)]).ToArray());
                curFile = path + "/" + RandomValueString + extension;  //Your path
            } while (File.Exists(curFile));



            //string pic = System.IO.Path.GetFileName(RandomValueString + extension);

            WebImage img = new WebImage(Content_Two.InputStream);
            string newextension = img.ImageFormat;
            if (newextension.ToLower() == "jpeg")
            {
                newextension = "jpg";
            }
            if (img.Width < 790 || img.Height < 460)
            {
                int wi;
                int hi;
                // maintain the aspect ratio despite the thumbnail size parameters
                if (img.Width > img.Height)
                {
                    wi = 790;
                    hi = (int)(img.Height * ((decimal)790 / img.Width));
                }
                else
                {
                    hi = 460;
                    wi = (int)(img.Width * ((decimal)460 / img.Height));
                }
                img.Resize(wi, hi);
            }
            img.Save(path + "/" + RandomValueString + "." + newextension);

            return RandomValueString + "." + newextension;
        }


        public static string ContentFour_Save(string ContentFour, int ID)
        {

            string path = Tools.ReturnPathPhysicalMode("ContentFourPath", "ContentFour_Save()");
            string RandomValueString = "Description_" + ID;
            Random rnd = new Random();
            string extension = ".txt";
            string curFile = path + "/" + RandomValueString + extension;
            int i = 1;
            while (File.Exists(curFile))
            {

                const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                RandomValueString = new string(Enumerable.Repeat(chars, 12)
                 .Select(s => s[rnd.Next(s.Length)]).ToArray());
                curFile = path + "/" + RandomValueString + "_" + i + extension;  //Your path
                i++;
            }
            System.IO.File.WriteAllText(curFile, ContentFour);
            return RandomValueString + extension;
        }

        public static void ContentFour_Edit(string ContentFour, string ContentFour_Path)
        {
            string path = Tools.ReturnPathPhysicalMode("ContentFourPath", "ContentFour_Edit()");
            path = path + "/" + ContentFour_Path;
            System.IO.File.WriteAllText(path, ContentFour);

        }
        public static string ContentFour_Get(string ContentFour)
        {
            string path = Tools.ReturnPathPhysicalMode("ContentFourPath", "ContentFour_Get()");

            try
            {
                return System.IO.File.ReadAllText(path + ContentFour);
            }
            catch
            {
                return "خطا در عملیات پردازش متن";
            }


        }
        public static string PasswordGenerator(int size)
        {
            string RandomValueString = "";
            Random rnd = new Random(DateTime.Now.Millisecond * DateTime.Now.Day * DateTime.Now.Second);
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            RandomValueString = new string(Enumerable.Repeat(chars, size)
                   .Select(s => s[rnd.Next(s.Length)]).ToArray());
            return RandomValueString;
        }


        public static string ImageSave_Profile(HttpPostedFileBase Content_Two, int x, int y, string prefix)
        {

            string path = ReturnPathPhysicalMode("ProfileImageBasePath", "ImageSave_Profile()");
            string extension = Path.GetExtension(Content_Two.FileName);
            string curFile = "";
            //string RandomValueString;
            //Random rnd = new Random();
            //do
            //{
            //    const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            //    RandomValueString = new string(Enumerable.Repeat(chars, 12)
            //     .Select(s => s[rnd.Next(s.Length)]).ToArray());
            curFile = path + "/" + prefix + ".png";  //Your path
            //} while (File.Exists(curFile));
            //   string pic = System.IO.Path.GetFileName(RandomValueString + extension);

            WebImage img = new WebImage(Content_Two.InputStream);
            img.Resize(x, y);
            img.Save(curFile, "png", false);
            return prefix + ".png";
        }




        //public static ContactManagementModel XMLToPersonalSetting(string profile)
        //{

        //    ContactManagementModel OBj = new ContactManagementModel();
        //    string path = ReturnPathPhysicalMode("ContactPath", profile, "ContactManagementModel()");



        //        var serializer = new XmlSerializer(typeof(ContactManagementModel));

        //        using (var reader = XmlReader.Create(path + "/" + profile + "_PersonalSetting" + ".xml"))
        //        {
        //            OBj = (ContactManagementModel)serializer.Deserialize(reader);
        //            return OBj;
        //        }


        //}




        public static string ReturnPath(string ConfigPath, string Caller)
        {
            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");
            string Path = string.Format(section[ConfigPath]);
            MakeVaidPath(Path, Caller, false);
            return Path;
        }
        public static string ReturnPathPhysicalMode(string ConfigPath, string Caller)
        {
            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");
            string Path = HttpContext.Current.Server.MapPath("~" + string.Format(section[ConfigPath]));
            MakeVaidPath(Path, Caller, true);
            return Path;
        }

        public static void MakeVaidPath(string Path, string Caller, bool isPhysicalPath)
        {
            if (!isPhysicalPath)
            {
                Path = HttpContext.Current.Server.MapPath(Path);
            }
            if (!System.IO.Directory.Exists(Path))
            {
                System.IO.Directory.CreateDirectory(Path);
                PipoLog(string.Format("Log : Directory Not Exist <<{0}>> Called by {1} At Time {2} For User {3}", Path, Caller, DateTime.Now));
            }
        }
        public static void PipoLog(string Content)
        {
            System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~/App_Data/PipoLog/PipoLog.txt"), Content + Environment.NewLine);
        }

    }



    //public class SendingReciveingSMS
    //{
    //    string IP = "http://193.104.22.14:2055/CPSMSService/Access";
    //    string Number = "10006020";
    //    string UserName = "ATSIGNCO9";
    //    string Password = "m@hfye4@5";
    //    string Company = "ATSIGNCO";

    //    /// <summary>
    //    /// ersale taki
    //    /// </summary>
    //    /// <param name="msg">matne payam</param>
    //    /// <param name="dest">adrese magsad</param>
    //    /// <returns>vaziyate payamake ersal shode ke dars surate ersale movafag true mibashad</returns>
    //    public string[] Send_single(string msg, string dest)
    //    {
    //        string[] status = new string[2];
    //        if (string.IsNullOrEmpty(msg))
    //        {
    //            status[0] = "message is empty";
    //            return status;
    //        }

    //        if (string.IsNullOrEmpty(dest))
    //        {
    //            status[0] = "destination is empty";
    //            return status;
    //        }

    //        int msg_part = (int)Math.Ceiling((double)msg.Length / 70);
    //        int sms_amount;
    //        if (int.TryParse(SMS_Amount(), out sms_amount))
    //        {
    //            if (sms_amount < msg_part)
    //                status[0] = "SMS amount is insufficient";
    //        }

    //        Cls_SMS.ClsSend sms_Single = new Cls_SMS.ClsSend();
    //        status = sms_Single.SendSMS_Single(msg, dest, Number, UserName, Password, IP, Company, false);
    //        return status;
    //    }
    //    public string SMS_Amount()
    //    {
    //        Cls_SMS.ClsGetRemain sms = new Cls_SMS.ClsGetRemain();
    //        string amount = sms.GetRemainCredit(UserName, Password, Company, IP);
    //        //lblRemain.Text = rem + " عدد پيامك  ";
    //        return amount;
    //    }
    //}

    public class PageTitle
    {
        public PageTitle(string _Pagetitle, string _ActionName, string _ControllerName, string _HtmlAttribute, string _RouteValue)
        {
            Pagetitle = _Pagetitle;
            ActionName = _ActionName;
            ControllerName = _ControllerName;
            RouteValue = _RouteValue;
            HtmlAttribute = _HtmlAttribute;
        }
        public string Pagetitle { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string RouteValue { get; set; }
        public string HtmlAttribute { get; set; }
    }


    public class InitilizeUserRequirements
    {
        public string InitUserFolders(string profile)
        {

            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");
            string Result;
            try
            {
                foreach (string key in section)
                {
                    System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath(string.Format("~/" + section[key], profile)));
                }
                Result = "OK";
            }
            catch (Exception e)
            {
                Result = "NOK :  " + e.ToString();
            }
            return Result;

        }
        public string DestroyUserFolders(string profile)
        {

            NameValueCollection section = (NameValueCollection)ConfigurationManager.GetSection("UsersFoldersPath");
            string Result;
            try
            {
                foreach (string key in section)
                {
                    System.IO.Directory.Delete(HttpContext.Current.Server.MapPath(string.Format(section[key], profile)), true);
                }
                Result = "OK";
            }
            catch (Exception e)
            {
                Result = "NOK :  " + e.ToString();
            }
            return Result;

        }
        public void SavePasswordFor3mill(string pass, string profile, string Role)
        {
            string Path = HttpContext.Current.Server.MapPath("~/App_Data/UsersKey/" + profile + "/");
            if (!System.IO.Directory.Exists(Path))
            {
                System.IO.Directory.CreateDirectory(Path);
            }
            string curFile = Path + Role + "_Key.txt";
            string EncryptedPass = StringCipher.Encrypt(pass, "ParsaDorsa");
            System.IO.File.WriteAllText(curFile, EncryptedPass);
        }

        public string GetPasswordFor3mill(string profile, string Role)
        {
            string pass = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/App_Data/UsersKey/" + profile + "/" + Role + "_Key.txt"));
            return StringCipher.Decrypt(pass, "ParsaDorsa");
        }
    }

    public static class StringCipher
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}