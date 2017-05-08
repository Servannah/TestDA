using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class LopData
    {
        [Key]
        public int maLop { get; set; }
        [Display(Name = "Nhóm lớp")]
        [Required(ErrorMessage = "Nhóm lớp không được bỏ trống!")]
        public int? nhomLop { get; set; }
        public string tenNhomLop { get; set; }
        [Display(Name = "Tên lớp")]
        [Required(ErrorMessage = "Tên lớp không được bỏ trống!")]
        [MaxLength(50, ErrorMessage = "Chiều dài tên lớp không được quá 50 kí tự. ")]
        public string tenLop { get; set; }
        [Display(Name = "Mô tả")]
        [MaxLength(200, ErrorMessage = "Mô tả không quá 200 kí tự.")]
        public string moTa { get; set; }

        public List<LopData> danhSachLop { get; set; }
    }
}