using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class QuyenNDData
    {
        [Key]
        public int maPQ { get; set; }
        [Required(ErrorMessage = "Tên không bỏ trống.")]
        [Display(Name = "Tên quyền người dùng * ")]
        [MaxLength(100, ErrorMessage = "Chiều dài không được quá 100 kí tự. ")]
        public string tenPQ { get; set; }
        [Display(Name = "Chức năng * ")]
        [MaxLength(500, ErrorMessage = "Chiều dài không được quá 500 kí tự. ")]
        [Required(ErrorMessage="Nhập thông tin mô tả chức năng")]
        public string chucNang { get; set; }
        public DateTime? ngayTao { get; set; }
        public DateTime? ngaySua { get; set; }

        public List<QuyenNDData> danhSachQuyen{get; set;}
    }
}