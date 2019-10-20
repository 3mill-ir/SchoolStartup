using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.AndroidJsonModel
{
    public class BarnameMoallem_Model
    {
        public BarnameMoallem_Model()
        {
            KelasHa = new List<Class_Model>();
        }
        public List<Class_Model> KelasHa { get; set; }
    }
    public class Class_Model
    {
        public Class_Model()
        {
            Barname = new List<AndroidBarnameHaftegi_Model>();
        }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public List<AndroidBarnameHaftegi_Model> Barname { get; set; }
    }
    public class AndroidBarnameHaftegi_Model
    {
        public int RuzeHafte { get; set; }
        public int Zang { get; set; }
        public int BarnameHaftegiId { get; set; }
        public string NameDars { get; set; }
    }
}