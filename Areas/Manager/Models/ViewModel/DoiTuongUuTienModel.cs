using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class DoiTuongUuTienData
    {
        [Key]
        public int maUuTien { get; set; }
        [Display(Name = "Đối tượng ưu tiên (*)")]
        [Required(ErrorMessage = "Tên không được bỏ trống!")]
        [MaxLength(50, ErrorMessage = "Chiều dài không được quá 50 kí tự. ")]
        public string loaiDT { get; set; }
        [Display(Name = "Định mức giảm (*)")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Required(ErrorMessage = "Định mức giảm không được bỏ trống!")]
        public double? dinhMucGiam { get; set; }
        [Display(Name = "Mô tả")]
        public string moTa { get; set; }

        public List<DoiTuongUuTienData> danhSachUT { get; set; }

    }
}