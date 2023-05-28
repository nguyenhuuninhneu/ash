﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Model
{
   public partial class tbl_News_Custom
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string CateComma { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
        public Nullable<int> Status { get; set; }
        public bool IsPublish { get; set; }
        public bool IsTraLai { get; set; }
        public bool IsXBDuyet { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string CategoryIdStr { get; set; }
        public string SourceNews { get; set; }
        public string Author { get; set; }
        public string LangCode { get; set; }
        public string Tags { get; set; }
        public Nullable<int> Type { get; set; }
        public string Position { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifyBy { get; set; }
        public Nullable<long> ViewCount { get; set; }
        public Nullable<byte> AllowComment { get; set; }
        public Nullable<byte> IsGetNews { get; set; }
        public bool IsComment { get; set; }
        public Nullable<int> NewMoney { get; set; }
        public string RelatedNews { get; set; }
        public string Attachment { get; set; }
        public int UserActId { get; set; }
        public int ProcedureId { get; set; }
        public long NhuanBut { get; set; }
        public Nullable<int> IdUserChoose { get; set; }
        public string OldId { get; set; }
        public string AuthorGuidID { get; set; }
        public short IsDelete { get; set; }
        public string ZoneID { get; set; }
        public Nullable<int> CTVID { get; set; }
    }
}
