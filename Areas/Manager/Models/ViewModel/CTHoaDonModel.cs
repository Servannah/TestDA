using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class CTHoaDonData
    {
        [Key]
        public int maHDCT { get; set; }
        [Display(Name = "Hóa đơn")]
        public int? maHoaDon { get; set; }
        public string soPhieuHD { get; set; }
        [Display(Name = "Thực phẩm *")]
        [Required(ErrorMessage = "Thực phẩm không được bỏ trống!")]
        public int? maThucPham { get; set; }
        public string tenThucPham { get; set; }
        [Display(Name = "Ngày tạo")]
        [UIHint("DateTime")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")] 
        public DateTime? ngayTao { get; set; }
        [Display(Name = "Đơn vị tính")]
        public string donViTinh { get; set; }
        [Display(Name = "Số lượng *")]
        [Required(ErrorMessage = "Số lượng không được bỏ trống!")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        public double? soLuong { get; set; }
       [Display(Name = "Gía cả")]
       [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public double? giaCa { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [ReadOnly(true)]
        [Display(Name = "Thành tiền")]
        public double? thanhTien { get; set; }      
        [Display(Name = "Ghi chú")]
        public string ghiChu { get; set; }

        public List<CTHoaDonData> danhSachCTHD { get; set; }
    }
}