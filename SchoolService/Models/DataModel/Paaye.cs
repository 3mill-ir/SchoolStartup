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
    
    public partial class Paaye
    {
        public Paaye()
        {
            this.Doroos = new HashSet<Doroos>();
            this.Kelas = new HashSet<Kelas>();
        }
    
        public int ID { get; set; }
        public Nullable<int> F_MaghaateID { get; set; }
        public Nullable<bool> isDeleted { get; set; }
        public string NaamePaye { get; set; }
    
        public virtual ICollection<Doroos> Doroos { get; set; }
        public virtual ICollection<Kelas> Kelas { get; set; }
        public virtual Maghaate Maghaate { get; set; }
    }
}