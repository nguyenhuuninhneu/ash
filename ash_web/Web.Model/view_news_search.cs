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
    
    public partial class view_news_search
    {
        public string Title { get; set; }
        public int ID { get; set; }
        public int CategoryId { get; set; }
        public string KeyWords { get; set; }
        public string Tags { get; set; }
        public Nullable<int> PageElementId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string FeautureImage { get; set; }
        public string Description { get; set; }
        public Nullable<byte> NoiBo { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<byte> IsSlide { get; set; }
        public Nullable<byte> IsHot { get; set; }
        public Nullable<byte> IsNew { get; set; }
        public string CropImage { get; set; }
    }
}
