using SchoolService.Areas.Admin3mill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.AndroidJsonModel
{
    public class Chart_Model
    {
        public Chart_Model(double avg, string mon)
        {
            Avrage = avg;
            Month = mon;
        }
        public double Avrage { get; set; }
        public string Month { get; set; }

    }
}