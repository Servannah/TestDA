﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DoAnTotNghiepEntities : DbContext
    {
        public DoAnTotNghiepEntities()
            : base("name=DoAnTotNghiepEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<tbl_config> tbl_config { get; set; }
        public virtual DbSet<tbl_dmhocphi> tbl_dmhocphi { get; set; }
        public virtual DbSet<tbl_doituonguutien> tbl_doituonguutien { get; set; }
        public virtual DbSet<tbl_hdchitiet> tbl_hdchitiet { get; set; }
        public virtual DbSet<tbl_hoadon> tbl_hoadon { get; set; }
        public virtual DbSet<tbl_hoantrahocphi> tbl_hoantrahocphi { get; set; }
        public virtual DbSet<tbl_hocphi> tbl_hocphi { get; set; }
        public virtual DbSet<tbl_hocsinh> tbl_hocsinh { get; set; }
        public virtual DbSet<tbl_hocsinhlop> tbl_hocsinhlop { get; set; }
        public virtual DbSet<tbl_loaichiphi> tbl_loaichiphi { get; set; }
        public virtual DbSet<tbl_loaimonan> tbl_loaimonan { get; set; }
        public virtual DbSet<tbl_lop> tbl_lop { get; set; }
        public virtual DbSet<tbl_monan> tbl_monan { get; set; }
        public virtual DbSet<tbl_nhacungcap> tbl_nhacungcap { get; set; }
        public virtual DbSet<tbl_nhanvien> tbl_nhanvien { get; set; }
        public virtual DbSet<tbl_nhomlop> tbl_nhomlop { get; set; }
        public virtual DbSet<tbl_nhomthucpham> tbl_nhomthucpham { get; set; }
        public virtual DbSet<tbl_nhomtin> tbl_nhomtin { get; set; }
        public virtual DbSet<tbl_nhucaudinhduong> tbl_nhucaudinhduong { get; set; }
        public virtual DbSet<tbl_nhucaunangluong> tbl_nhucaunangluong { get; set; }
        public virtual DbSet<tbl_option> tbl_option { get; set; }
        public virtual DbSet<tbl_quyennguoidung> tbl_quyennguoidung { get; set; }
        public virtual DbSet<tbl_taikhoan> tbl_taikhoan { get; set; }
        public virtual DbSet<tbl_thanhphanmonan> tbl_thanhphanmonan { get; set; }
        public virtual DbSet<tbl_thucdon> tbl_thucdon { get; set; }
        public virtual DbSet<tbl_thucpham> tbl_thucpham { get; set; }
        public virtual DbSet<tbl_tintuc> tbl_tintuc { get; set; }
        public virtual DbSet<tbl_tpkho> tbl_tpkho { get; set; }
        public virtual DbSet<tbl_tpxuatkho> tbl_tpxuatkho { get; set; }
    }
}
