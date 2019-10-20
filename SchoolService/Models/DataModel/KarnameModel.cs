using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DataModel
{
    public class KarnameModel
    {
        public KarnameModel()
        {
            Nomarat = new List<KarnameNomreModel>();
        }
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
    public class KarnameNomreModel
    {
        public string NaameDars { get; set; }
        public string NomreDars { get; set; }
        public string RotbeDarKelas { get; set; }
        public string RotbeDarMadrese { get; set; }
    }
}