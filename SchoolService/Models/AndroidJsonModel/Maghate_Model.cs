using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolService.Models.AndroidJsonModel
{
    public class Maghate_Model
    {
        public Maghate_Model()
        {
            KelasHa = new List<MaghateClass_Model>();
        }
        public string MaghtaName { get; set; }
        public List<MaghateClass_Model> KelasHa { get; set; }
    }
    public class MaghateClass_Model
    {
        public MaghateClass_Model(string ANaameKelas, int AID, int AHuzurghiabStatus)
        {
            NaameKelas = ANaameKelas;
            ID = AID;
            HuzurGhiabStatus = AHuzurghiabStatus;
        }
        public string NaameKelas { get; set; }
        public int ID { get; set; }
        public int HuzurGhiabStatus { get; set; }
    }
}