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
    
    public partial class tbl_HomeMenu_OLD
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Nullable<int> Ordering { get; set; }
        public Nullable<bool> isNewsTab { get; set; }
        public bool Status { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public int LinkType { get; set; }
        public int PageElementId { get; set; }
        public short isHome { get; set; }
        public bool isMenu { get; set; }
        public bool IsPermAdd { get; set; }
        public string FeautureImage { get; set; }
        public bool IsHome2 { get; set; }
        public bool IsHome3 { get; set; }
        public string LangCode { get; set; }
    }
}
