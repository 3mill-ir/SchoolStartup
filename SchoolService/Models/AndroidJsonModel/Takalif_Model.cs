using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.AndroidJsonModel
{
    public class Takalif_Model
    {
        public Takalif_Model(string ATarikh,string AOnvan,string ATovzih,string AFile)
        {
            Tarikh = ATarikh;
            Onvan = AOnvan;
            Tovzih = ATovzih;
            File = AFile;
        }
        public string Tarikh { get; set; }
        public string Onvan { get; set; }
        public string Tovzih { get; set; }
        public string File { get; set; }
    }
}