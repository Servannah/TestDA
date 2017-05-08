using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class LoaiChiPhiData
    {
        [Key]
        public int maLoaiChiPhi { get; set; }
        [Display(Name = "Tên loại chi phí")]
        [Required(ErrorMessage = "Tên không được bỏ trống!")]
        [MaxLength(200, ErrorMessage = "Chiều dài không được quá 200 kí tự. ")]
        public string tenLoai { get; set; }
        [Display(Name = "Mô tả")]
        public string ghiChu { get; set; }
        [Display(Name = "Học phí chính khóa *")]
        [Required(ErrorMessage = "Yêu cầu chọn")]
        public string laHPKhoaChinh { get; set; }

        public List<LoaiChiPhiData> danhSachLoaiChiPhi { get; set; }
    }
}