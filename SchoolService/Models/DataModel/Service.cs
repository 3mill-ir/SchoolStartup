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
    
    public partial class Service
    {
        public Service()
        {
            this.Hazine = new HashSet<Hazine>();
        }
    
        public int ID { get; set; }
        public string ServiceName { get; set; }
        public Nullable<int> F_MadraseId { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string F_ParrentID { get; set; }
    
        public virtual ICollection<Hazine> Hazine { get; set; }
        public virtual Madaares Madaares { get; set; }
    }
}
