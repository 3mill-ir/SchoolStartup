//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SchoolService.Models.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Karmandaan
    {
        public Karmandaan()
        {
            this.Farakhanha = new HashSet<Farakhanha>();
        }
    
        public int ID { get; set; }
        public string F_UserInfromation { get; set; }
        public string Semat { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string F_ParrentID { get; set; }
    
        public virtual ICollection<Farakhanha> Farakhanha { get; set; }
        public virtual UserInformation UserInformation { get; set; }
    }
}
