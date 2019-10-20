using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DataModel
{

     public class BarnameEmtehani_ModelList
    {
         public BarnameEmtehani_ModelList(){
             BarnameEMtehaniList=new List<BarnameEmtehani_Model>();
         }
        public List<BarnameEmtehani_Model> BarnameEMtehaniList { get; set; }

    }
    public class BarnameEmtehani_Model
    {
        public int MoallemDoroosID { get; set; }
        public string NameDars { get; set; }
        public DateTime Tarikh { get; set; }
    }
}