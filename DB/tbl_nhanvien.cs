//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestDA.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_nhanvien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_nhanvien()
        {
            this.tbl_hocphi = new HashSet<tbl_hocphi>();
            this.tbl_thucdon = new HashSet<tbl_thucdon>();
        }
    
        public string MaNV { get; set; }
        public string Ho { get; set; }
        public string Ten { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string DanToc { get; set; }
        public string TonGiao { get; set; }
        public string SoCMND { get; set; }
        public Nullable<System.DateTime> NgayCapCMND { get; set; }
        public string NoiCapCMND { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string Email { get; set; }
        public string ChucVu { get; set; }
        public string TrinhDo { get; set; }
        public Nullable<System.DateTime> NgayVaoDang { get; set; }
        public string SoTheDang { get; set; }
        public string HinhAnh { get; set; }
        public string LoaiHopDong { get; set; }
        public Nullable<System.DateTime> NgayVaoLam { get; set; }
        public string DaNghiViec { get; set; }
        public Nullable<System.DateTime> NgayNghiViec { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public Nullable<System.DateTime> NgaySua { get; set; }
        public string GhiChu { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_hocphi> tbl_hocphi { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_thucdon> tbl_thucdon { get; set; }
    }
}