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
    
    public partial class tbl_TaiLieu
    {
        public int ID { get; set; }
        public int CatID { get; set; }
        public string NameFile { get; set; }
        public string LinkFile { get; set; }
        public string ReplaceName { get; set; }
        public int Ordering { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool Status { get; set; }
        public string LangCode { get; set; }
    }
}
