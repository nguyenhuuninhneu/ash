//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Release
    {
        public int Id { get; set; }
        public Nullable<int> TeamId { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> ReleaseDate { get; set; }
        public string ReleaseNote { get; set; }
        public string WhatsNew { get; set; }
        public string LinkSharePoint { get; set; }
        public string LinkQueryDevOps { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<int> Ordering { get; set; }
    }
}
