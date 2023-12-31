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
    
    public partial class Hazine
    {
        public Hazine()
        {
            this.Check = new HashSet<Check>();
            this.Pardakht = new HashSet<Pardakht>();
        }
    
        public int ID { get; set; }
        public Nullable<int> F_ServiceId { get; set; }
        public Nullable<System.DateTime> Tarikh { get; set; }
        public Nullable<int> F_OvliaId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<double> Bedehi { get; set; }
        public string F_ParrentID { get; set; }
    
        public virtual ICollection<Check> Check { get; set; }
        public virtual Ovlia Ovlia { get; set; }
        public virtual Service Service { get; set; }
        public virtual ICollection<Pardakht> Pardakht { get; set; }
    }
}
