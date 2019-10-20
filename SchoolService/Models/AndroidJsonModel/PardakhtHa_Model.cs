using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.AndroidJsonModel
{
    public class PardakhtHa_Model
    {
        public PardakhtHa_Model()
        {
            PardakhthayeNaghdi = new List<PardakhteNaghdi_Model>();
            PardakhthayeChecki = new List<PardakhteChecki_Model>();
        }
        public List<PardakhteNaghdi_Model> PardakhthayeNaghdi { get; set; }
        public List<PardakhteChecki_Model> PardakhthayeChecki { get; set; }
    }
    public class PardakhteNaghdi_Model
    {
        public string Tarikh { get; set; }
        public string MablaghePardakhti { get; set; }
        public string AzBabate { get; set; }
    }
    public class PardakhteChecki_Model
    {
        public string TarikheCheck { get; set; }
        public string MablagheCheck { get; set; }
        public string AzBabate { get; set; }
        public string Banke { get; set; }
        public string VaziateVosul { get; set; }
    }
}