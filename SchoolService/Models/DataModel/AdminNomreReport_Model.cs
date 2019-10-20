using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DataModel
{
    public class AdminNomreReport_Model
    {
        public AdminNomreReport_Model() 
        {
            NomarateKelasi = new List<AdminNomre_Model>();
            NomarateEmtehani = new List<AdminNomre_Model>();
        }
        public List<AdminNomre_Model> NomarateKelasi { get; set; }
        public List<AdminNomre_Model> NomarateEmtehani { get; set; }
    }
    public class AdminNomre_Model
    {
        public string NomreDars { get; set; }
        public DateTime Tarikh { get; set; }
    }
}