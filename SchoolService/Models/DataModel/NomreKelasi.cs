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
    
    public partial class NomreKelasi
    {
        public int ID { get; set; }
        public Nullable<int> F_DaneshAmuzID { get; set; }
        public Nullable<int> F_BarnameHaftegiID { get; set; }
        public string Nomre { get; set; }
        public string Tovzihat { get; set; }
        public Nullable<int> Hafte { get; set; }
        public Nullable<System.DateTime> Tarikh { get; set; }
        public Nullable<bool> isDeleted { get; set; }
        public string F_ParrentID { get; set; }
    
        public virtual BarnameHaftegi BarnameHaftegi { get; set; }
        public virtual DaneshAmuz DaneshAmuz { get; set; }
    }
}
