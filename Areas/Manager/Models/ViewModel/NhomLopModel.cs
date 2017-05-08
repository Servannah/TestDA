using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class NhomLopData
    {
        [Key]
        public int maNhomLop { get; set; }
        [Display(Name = "Tên nhóm lớp")]
        [Required(ErrorMessage = "Tên không được bỏ trống!")]
        [MaxLength(50, ErrorMessage = "Chiều dài không được quá 50 kí tự. ")]
        public string tenNhomLop { get; set; }
        [Display(Name = "Mô tả")]
        public string ghiChu { get; set; }

        public List<NhomLopData> danhSachNhomLop { get; set; }
    }
}