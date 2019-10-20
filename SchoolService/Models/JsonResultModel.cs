using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models
{
    public class JsonResultModel
    {
        public string Text { get; set; }
        public string Key { get; set; }
        public string Option { get; set; }
    }

    public class OvliaJsonResultModel
    {
        public OvliaJsonResultModel()
        {
            DaneshAmuzan = new List<DaneshAmuz_Model>();
        }
        public string Text { get; set; }
        public string Key { get; set; }
        public string OvliaChatID { get; set; }
        public List<DaneshAmuz_Model> DaneshAmuzan { get; set; }
    }

    public class DaneshAmuz_Model
    {
        public string FullName { get; set; }
        public int ID { get; set; }
    }
}