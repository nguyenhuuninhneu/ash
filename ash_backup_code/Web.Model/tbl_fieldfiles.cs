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
    
    public partial class tbl_fieldfiles
    {
        public int Id { get; set; }
        public Nullable<int> FieldID { get; set; }
        public string LinkFile { get; set; }
        public string NameFile { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> Status { get; set; }
        public string ReplaceName { get; set; }
        public string Size { get; set; }
        public string Code { get; set; }
        public string LangCode { get; set; }
    }
}