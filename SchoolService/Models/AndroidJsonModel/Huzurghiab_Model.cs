using SchoolService.Areas.Admin3mill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.AndroidJsonModel
{
    public class Huzurghiab_Model
    {
        public Huzurghiab_Model(string ATarikh, string ATakhir, string ATovzihat)
        {
            Tarikh = ATarikh;
            Takhir = ATakhir;
            Tovzihat = ATovzihat;
        }
        public string Tarikh { get; set; }
        public string Takhir { get; set; }
        public string Tovzihat { get; set; }
    }
}