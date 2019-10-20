using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.DataModel
{
    public class SabteNomreModel
    {
        public SabteNomreModel()
        {
            Nomarat = new List<SabteNomre_NomreModel>();
        }
        public int DaneshAmoozId { get; set; }
        public int BarnameHaftegiId { get; set; }
        public string Tarikh { get; set; }
        public int Hafte { get; set; }
        public List<SabteNomre_NomreModel> Nomarat { get; set; }
    }
    public class SabteNomre_NomreModel
    {
        public int BarnameHaftegiId { get; set; }
        public int DaneshAmoozId { get; set; }
        public string Nomre { get; set; }
        public string Tozih { get; set; }
    }
}