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
    
    public partial class tbl_thucpham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_thucpham()
        {
            this.tbl_hdchitiet = new HashSet<tbl_hdchitiet>();
            this.tbl_thanhphanmonan = new HashSet<tbl_thanhphanmonan>();
            this.tbl_tpkho = new HashSet<tbl_tpkho>();
            this.tbl_tpxuatkho = new HashSet<tbl_tpxuatkho>();
        }
    
        public int MaThucPham { get; set; }
        public Nullable<int> MaNhomTP { get; set; }
        public string TenThucPham { get; set; }
        public Nullable<double> GiaCa { get; set; }
        public Nullable<bool> NguonTuDV { get; set; }
        public Nullable<double> TyLeHapThu { get; set; }
        public Nullable<double> TyLeThai { get; set; }
        public Nullable<double> NangLuongCalo { get; set; }
        public Nullable<double> TphhNuoc { get; set; }
        public Nullable<double> TphhProtid { get; set; }
        public Nullable<double> TphhLipid { get; set; }
        public Nullable<double> TphhGlucid { get; set; }
        public Nullable<double> TphhCellulose { get; set; }
        public Nullable<double> TphhCholesterol { get; set; }
        public Nullable<double> MkCalci { get; set; }
        public Nullable<double> MkPhotpho { get; set; }
        public Nullable<double> MkSat { get; set; }
        public Nullable<double> VitaminCaroten { get; set; }
        public Nullable<double> VitaminA { get; set; }
        public Nullable<double> VitaminB1 { get; set; }
        public Nullable<double> VitaminB2 { get; set; }
        public Nullable<double> VitaminC { get; set; }
        public Nullable<double> VitaminPP { get; set; }
        public string GhiChu { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_hdchitiet> tbl_hdchitiet { get; set; }
        public virtual tbl_nhomthucpham tbl_nhomthucpham { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_thanhphanmonan> tbl_thanhphanmonan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_tpkho> tbl_tpkho { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_tpxuatkho> tbl_tpxuatkho { get; set; }
    }
}