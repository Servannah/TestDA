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
    
    public partial class tbl_lop
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_lop()
        {
            this.tbl_hocsinhlop = new HashSet<tbl_hocsinhlop>();
        }
    
        public int MaLop { get; set; }
        public Nullable<int> NhomLopID { get; set; }
        public string TenLop { get; set; }
        public string MoTa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_hocsinhlop> tbl_hocsinhlop { get; set; }
    }
}
