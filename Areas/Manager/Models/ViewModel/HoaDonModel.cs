using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class HoaDonData
    {
        [Key]
        public int maPhieuHD { get; set; }
        [Display(Name = "Thực đơn *")]
        [Required(ErrorMessage = "Chọn mã thực đơn")]
        public int? maThucDon { get; set; }
        [Display(Name = "Số hóa đơn *")]
        [Required(ErrorMessage = "Số phiếu không được bỏ trống!")]
        [MaxLength(50, ErrorMessage = "Chiều dài không được quá 50 kí tự. ")]
        public string soPhieuHD { get; set; }
        [Display(Name = "Ngày lập hóa đơn *")]
        [Required(ErrorMessage = "Ngày lập hóa đơn không được bỏ trống!")]
        [UIHint("DateTime")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ngayLap { get; set; }
        [Required(ErrorMessage = "Người lập không được bỏ trống!")]
        [Display(Name = "Người lập *")]
        public string nguoiLap { get; set; }
        public string tenNguoiLap { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Tổng tiền *")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Không được bỏ trống.")]
        public double? tongTien { get; set; }
        [Display(Name = "Ghi chú")]
        public string ghiChu { get; set; }

        public string noiDungTD { get; set; }

        public List<HoaDonData> danhSachPhieuHD { get; set; }

    }

}