using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DataModel
{
    public class BarnameHaftegi_ModelList
    {

        public BarnameHaftegi_ModelList()
        {
          
        }
        public BarnameHaftegi_ModelList(string ForDetail)
        {
            BarnamehaftegiList = new BarnameHaftegi_Model[6][];
        }

        public int KelasId { get; set; }
        public int MaxZang { get; set; }
       public BarnameHaftegi_Model[][] BarnamehaftegiList { get; set; }

    }
    public class BarnameHaftegi_Model
    {
        public int Barnamehaftegi_ID { get; set; }
        public int Barnamehaftegi_MoallemID { get; set; }
        public int Barnamehaftegi_DoroosID { get; set; }
        public int? Barnamehaftegi_ruz{ get; set; }
        public int? Barnamehaftegi_zang { get; set; }
        public string Barnamehaftegi_MoallemFullName { get; set; }
        public string Barnamehaftegi_DoroosName { get; set; }
    }
}