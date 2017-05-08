using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class NhomThucPhamData
    {
        [Key]
        public int maNhomTP { get; set; }
        [Display(Name = "Tên nhóm thực phẩm (*)")]
        [Required(ErrorMessage = "Tên nhóm thực phẩm không được bỏ trống!")]
        [MaxLength(150, ErrorMessage = "Chiều dài tên danh mục không được quá 150 kí tự. ")]
        public string tenNhomThucPham { get; set; }
        [AllowHtml]
         [Display(Name = "Ghi chú")]
        public string ghiChu { get; set; }
         public List<NhomThucPhamData> danhSachNhomTP { get; set; }
    }
}