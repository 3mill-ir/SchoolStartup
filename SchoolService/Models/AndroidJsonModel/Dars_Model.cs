using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.AndroidJsonModel
{
    public class Dars_Model
    {
        public int BarnameHaftegiId { get; set; }
        public string NameDars { get; set; }
        public string NameMoallem { get; set; }
    }

    public class DarsCombo_Model
    {
        public int ID { get; set; }
        public string NameDars { get; set; }
    }
}