using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.AndroidJsonModel
{
    public class TashvighTanbih_Model
    {
        public TashvighTanbih_Model(string ATarikh, string ATovzihat, string AEmtiaz)
        {
            Emtiaz = AEmtiaz;
            Tovzihat = ATovzihat;
            Tarikh = ATarikh;
        }
        public string Emtiaz { get; set; }
        public string Tovzihat { get; set; }
        public string Tarikh { get; set; }
    }
}