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
    
    public partial class tbl_Video
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Images { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsHot { get; set; }
        public bool Status { get; set; }
        public string VideoCategoryId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string LangCode { get; set; }
        public string CreatedBy { get; set; }
        public int Type { get; set; }
        public Nullable<int> Ordering { get; set; }
        public long ViewNumber { get; set; }
        public string Duration { get; set; }
        public bool IsHome { get; set; }
        public int UserActId { get; set; }
        public int ProcedureId { get; set; }
        public Nullable<int> NewMoney { get; set; }
        public Nullable<int> CreateById { get; set; }
    }
}