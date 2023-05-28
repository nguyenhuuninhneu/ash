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
    
    public partial class tbl_UserAdmin
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_UserAdmin()
        {
            this.tbl_LichCongTac = new HashSet<tbl_LichCongTac>();
        }
    
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public Nullable<byte> Gender { get; set; }
        public string Photo { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Country { get; set; }
        public Nullable<int> zip { get; set; }
        public bool Active { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string Password { get; set; }
        public string PasswordQuestion { get; set; }
        public string PasswordAnswer { get; set; }
        public string Phone { get; set; }
        public Nullable<System.DateTime> TimeLogin { get; set; }
        public string IPLogin { get; set; }
        public string GroupUserID { get; set; }
        public string PageElementID { get; set; }
        public string QuyTrinhXuatBanID { get; set; }
        public bool isAdmin { get; set; }
        public string SoTaiKhoan { get; set; }
        public bool IsDuyetComment { get; set; }
        public Nullable<System.DateTime> LastOnline { get; set; }
        public Nullable<bool> isOnline { get; set; }
        public string NganHang { get; set; }
        public string LangCode { get; set; }
        public Nullable<bool> IsCTV { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_LichCongTac> tbl_LichCongTac { get; set; }
    }
}