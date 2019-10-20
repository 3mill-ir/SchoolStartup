using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolService.Models.DataModel
{
    public class Moallem_Dars
    {

        public Moallem moallem { get; set; }
       public int?[] DarsId { get; set; }
       public SelectList Darsha { get; set; }

    }
}