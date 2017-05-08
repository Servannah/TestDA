using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class XepLopData
    {
        [Key]
        public int maHSLop { get; set; }
        [Display(Name = "Học sinh *")]
        [Required(ErrorMessage = "Học sinh không được bỏ trống!")]
        public string maHocSinh { get; set; }
        public string tenHocSinh { get; set; }
        public DateTime? ngaySinh { get; set; }
        [Display(Name = "Tên lớp *")]
        [Required(ErrorMessage = "Chọn lớp học")]
        public int? maLop { get; set; }
        public string tenLop { get; set; }
        [Display(Name = "Năm học *")]
        [Required(ErrorMessage = "Chọn năm học")]
        public string namHoc { get; set; }
        public string gioiTinh { get; set; }
        public string queQuan { get; set; }

        public List<XepLopData> danhSachHSLop { get; set; }

        public List<HocSinhData> danhSachHSChuaLop { get; set; }
    }  
}