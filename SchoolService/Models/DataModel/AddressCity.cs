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
    
    public partial class AddressCity
    {
        public AddressCity()
        {
            this.Madaares = new HashSet<Madaares>();
            this.Nemayandegi = new HashSet<Nemayandegi>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> F_StateId { get; set; }
        public Nullable<bool> isDelete { get; set; }
    
        public virtual AddressState AddressState { get; set; }
        public virtual ICollection<Madaares> Madaares { get; set; }
        public virtual ICollection<Nemayandegi> Nemayandegi { get; set; }
    }
}
