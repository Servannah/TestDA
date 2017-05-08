using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class LoaiMonAnData
    {
        [Key]
        public int maLoaiMonAn { get; set; }
        [Display(Name = "Tên loại món ăn (*)")]
        [Required(ErrorMessage = "Tên loại món ăn không được bỏ trống!")]
        [MaxLength(50, ErrorMessage = "Chiều dài tên loại món ăn không được quá 50 kí tự. ")]
        public string tenLoaiMonAn { get; set; }
        [AllowHtml]
        [Display(Name = "Ghi chú")]
        [MaxLength(250, ErrorMessage = "Ghi chú không quá 250 kí tự.")]
        public string ghiChu { get; set; }       
        public List<LoaiMonAnData> danhSachLoaiMonAn { get; set; }
    }
}