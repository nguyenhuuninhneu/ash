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
    
    public partial class CustomerSupport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public Nullable<int> PriorityId { get; set; }
        public Nullable<System.DateTime> SupportDate { get; set; }
        public string LinkTicket { get; set; }
        public string LinkDevops { get; set; }
        public string SolutionNote { get; set; }
        public Nullable<System.DateTime> ClosedDate { get; set; }
        public Nullable<bool> IsFinish { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<int> Ordering { get; set; }
    }
}
