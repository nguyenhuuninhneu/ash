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
    
    public partial class tbl_FeedBack
    {
        public int ID { get; set; }
        public string FullNameOfQuestion { get; set; }
        public string Email { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> AnswerBy { get; set; }
        public Nullable<System.DateTime> AnswerDate { get; set; }
        public string UserNameOfAnswer { get; set; }
        public bool Status { get; set; }
        public string LangCode { get; set; }
    }
}
