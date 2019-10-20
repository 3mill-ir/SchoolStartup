using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.AndroidJsonModel
{
    public class OvliaMainJson_Model
    {
        public OvliaMainJson_Model()
        {
            Nomarat = new List<Nomre_Model>();
            Farakhanha = new List<string>();
            NafarateBartar = new List<NafarateBartar_Model>();
            Barname = new List<AndroidBarnameHaftegi_Model>();
        }
        public List<Nomre_Model> Nomarat { get; set; }
        public List<string> Farakhanha { get; set; }
        public List<NafarateBartar_Model> NafarateBartar { get; set; }
        public string NameAndPaye { get; set; }
        public List<AndroidBarnameHaftegi_Model> Barname { get; set; }
    }
}