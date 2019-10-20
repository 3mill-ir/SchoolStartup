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
    
    public partial class Madaares
    {
        public Madaares()
        {
            this.Doroos = new HashSet<Doroos>();
            this.Kelas = new HashSet<Kelas>();
            this.Moallem = new HashSet<Moallem>();
            this.Service = new HashSet<Service>();
            this.UserInformation = new HashSet<UserInformation>();
        }
    
        public int ID { get; set; }
        public Nullable<int> F_NemayandegiID { get; set; }
        public Nullable<int> F_MaghaateID { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<bool> isDeleted { get; set; }
        public string NaameMadrese { get; set; }
        public string Address { get; set; }
        public string Moassesin { get; set; }
        public string Telephon { get; set; }
        public string KodeMadrese { get; set; }
        public Nullable<int> F_CityId { get; set; }
        public Nullable<int> F_SaleTahsiliId { get; set; }
        public Nullable<int> F_MadreseId { get; set; }
        public Nullable<int> ModirID { get; set; }
        public string F_ParrentID { get; set; }
        public Nullable<bool> NomreType { get; set; }
    
        public virtual AddressCity AddressCity { get; set; }
        public virtual ICollection<Doroos> Doroos { get; set; }
        public virtual ICollection<Kelas> Kelas { get; set; }
        public virtual Nemayandegi Nemayandegi { get; set; }
        public virtual Maghaate Maghaate { get; set; }
        public virtual SaleTahsili SaleTahsili { get; set; }
        public virtual ICollection<Moallem> Moallem { get; set; }
        public virtual ICollection<Service> Service { get; set; }
        public virtual ICollection<UserInformation> UserInformation { get; set; }
    }
}
