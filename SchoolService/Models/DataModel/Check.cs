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
    
    public partial class Check
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> TarikheCheck { get; set; }
        public Nullable<double> MablagheCheck { get; set; }
        public string Bank { get; set; }
        public string VaziateVosul { get; set; }
        public Nullable<int> F_HazineId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string F_ParrentID { get; set; }
    
        public virtual Hazine Hazine { get; set; }
    }
}
