using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.AndroidJsonModel
{
    public class Chat_Model
    {
        public Chat_Model(int AType,string AText,string ATarikh,int AID)
        {
            Type = AType; Text = AText; Tarikh = ATarikh; ID = AID;
        }
        public int ID { get; set; }
        public int Type { get; set; }
        public string Text { get; set; }
        public string Tarikh { get; set; }
    }
}