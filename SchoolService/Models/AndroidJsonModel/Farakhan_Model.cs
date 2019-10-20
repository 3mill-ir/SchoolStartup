using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.AndroidJsonModel
{
    public class Farakhan_Model
    {
        public Farakhan_Model(string AMovzoo, string AMatn, string ATarikheFarakhan)
        {
            Movzoo = AMovzoo;
            Matn = AMatn;
            TarikheFarakhan = ATarikheFarakhan;
        }
        public string Movzoo { get; set; }
        public string Matn { get; set; }
        public string TarikheFarakhan { get; set; }
    }
}