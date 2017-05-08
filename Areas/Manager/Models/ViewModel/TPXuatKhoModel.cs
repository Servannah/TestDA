using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class TPXuatKhoData
    {
        [Key]
        public int maXuatKho { get; set; }
        [Display(Name = "Thực phẩm *")]
        [Required(ErrorMessage = "Mã lớp không được bỏ trống!")]
        public int? maThucPham { get; set; }
        public string tenThucPham { get; set; }
        [Display(Name = "Ngày xuất kho")]
        [Required(ErrorMessage = "Chọn ngày nhập kho.")]
        [UIHint("DateTime")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ngayXuatKho { get; set; }
        [Display(Name = "Số lượng *")]
        [Required(ErrorMessage = "Số lượng không được bỏ trống!")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        public double? soLuong { get; set; }
        [Display(Name = "Ghi chú")]
        public string ghiChu { get; set; }

        public List<TPXuatKhoData> danhSachTPXuatKho { get; set; }
    }
}