using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class DMHocPhiData
    {
        [Key]
        public int maDMHP { get; set; }
        [Display(Name = "Loại chi phí *")]
        [Required(ErrorMessage = "Loại chi phí không được bỏ trống!")]
        public int? maLoaiChiPhi { get; set; }
        public string tenLoaiChiPhi { get; set; }
        [Required(ErrorMessage = "Số tiền không được bỏ trống!")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Số tiền cần đóng >0.")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Currency)]
        [Display(Name = "Số tiền *")]
        public int? soTien { get; set; }
        [UIHint("DateTime")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Ngày áp dụng không được bỏ trống!")]
        [Display(Name = "Ngày áp dụng *")]
        public DateTime? ngayApDung { get; set; }
        [Required(ErrorMessage = "Năm học không được bỏ trống!")]
        [Display(Name = "Năm học *")]
        public string namHoc { get; set; }
        [Required(ErrorMessage = "Yêu cầu chọn một.")]
        [Display(Name = "Hình thức *")]
        public string loaiApDung { get; set; }
        [Display(Name = "Ghi chú")]
        public string ghiChu { get; set; }
        public string laHPCK { get; set; }

        public List<DMHocPhiData> danhSachDMHocPhi { get; set; }
    }
}