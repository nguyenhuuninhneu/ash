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
    
    public partial class tbl_Magazine
    {
        public int ID { get; set; }
        public Nullable<int> Year { get; set; }
        public string Title { get; set; }
        public string Photo { get; set; }
        public string Attachment { get; set; }
        public int Ordering { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public bool Status { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentSize { get; set; }
        public string LangCode { get; set; }
    }
}
