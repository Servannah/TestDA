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
    
    public partial class tbl_tpxuatkho
    {
        public int MaTPXK { get; set; }
        public Nullable<int> MaTP { get; set; }
        public Nullable<double> SoLuong { get; set; }
        public Nullable<System.DateTime> NgayXuat { get; set; }
        public string GhiChu { get; set; }
    
        public virtual tbl_thucpham tbl_thucpham { get; set; }
    }
}
