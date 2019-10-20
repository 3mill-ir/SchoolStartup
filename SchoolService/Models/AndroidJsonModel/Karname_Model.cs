using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.AndroidJsonModel
{
    public class KarnameDars_Model
    {
        public string Nomre { get; set; }
        public string NaameDars { get; set; }
    }

    public class Karname_Model
    {
        public Karname_Model()
        {
            TermeAvval = new List<KarnameDars_Model>();
            TermeDovvom = new List<KarnameDars_Model>();
        }
        public List<KarnameDars_Model> TermeAvval { get; set; }
        public List<KarnameDars_Model> TermeDovvom { get; set; }
    }
}