using SchoolService.Areas.Admin3mill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DataModel
{

    public class Nomre_Model
    {
        public Nomre_Model()
        {
            Nomarat = new List<KarnameNomreModel>();
        }
        public int ID { get; set; }
        public string NomreDars { get; set; }
        public string NaameDars { get; set; }
        public string Tarikh { get; set; }
        public int? F_kelasid { get; set; }
        public int? F_DaneshAmuzID { get; set; }
        public DateTime? TarikhDate { get; set; }

        public string Mah { get; set; }
        public string Madrese { get; set; }
        public string Maghta { get; set; }
        public string SaleTahsili { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string CodeMelli { get; set; }
        public string Paaye { get; set; }
        public string Kelas { get; set; }
        public string MoaddeleKol { get; set; }
        public string RotbeDarKelas { get; set; }
        public string RotbeDarMadrese { get; set; }
        public string ModirFirstName { get; set; }
        public string ModirLastName { get; set; }
        public string MoavenFirstName { get; set; }
        public string MoavenLastName { get; set; }
        public List<KarnameNomreModel> Nomarat { get; set; }
    }

    public class Ranking
    {
        public double darsID { get; set; }
        public double KelasID { get; set; }
        public double daneshamuzID { get; set; }
        public double? avg { get; set; }

    }

    public class RizNomre_Model
    {
        public RizNomre_Model()
        {
            Dates = new List<DateTime?>();
            DaneshAmoozan = new List<DaneshAmoozNomreHelper>();
        }
        public List<DateTime?> Dates { get; set; }
        public List<DaneshAmoozNomreHelper> DaneshAmoozan { get; set; }
    }


   public class NomreHelper
   {
       public int ID { get; set; }
       public string NomreDars { get; set; }
       public DateTime? Tarikh { get; set; }
   }
   public class DaneshAmoozNomreHelper
   {
       public DaneshAmoozNomreHelper()
       {
           Nomarat = new List<NomreHelper>();
       }
       public string DaneshAmoozFullName { get; set; }
       public List<NomreHelper> Nomarat { get; set; }
   }
}