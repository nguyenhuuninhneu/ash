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
    
    public partial class tbl_Config
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public Nullable<int> Price { get; set; }
        public bool Status { get; set; }
        public Nullable<int> Ordering { get; set; }
        public string LangCode { get; set; }
    }
}
