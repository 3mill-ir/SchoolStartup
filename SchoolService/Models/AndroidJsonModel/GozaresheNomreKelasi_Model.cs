using SchoolService.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.AndroidJsonModel
{
    public class GozaresheNomreKelasi_Model
    {
        public GozaresheNomreKelasi_Model()
        {
            Nomarat = new List<Nomre_Model>();

            Chart = new List<Chart_Model>();
        }
        public List<Nomre_Model> Nomarat { get; set; }

        public List<Chart_Model> Chart { get; set; }
    }
}