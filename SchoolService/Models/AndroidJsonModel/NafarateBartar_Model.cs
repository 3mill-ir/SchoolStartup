using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.AndroidJsonModel
{
    public class NafarateBartar_Model
    {
        public NafarateBartar_Model(int rate, string fullname)
        {
            Rate = rate;
            FullName = fullname;
        }
        public int Rate { get; set; }
        public string FullName { get; set; }
    }
}