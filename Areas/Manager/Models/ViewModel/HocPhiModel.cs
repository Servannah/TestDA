using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class HocPhiData
    {
        [Key]
        [Required(ErrorMessage = "Nhập vào số phiếu thu")]
        [MaxLength(10, ErrorMessage = "Số phiếu không quá 10 kí tự.")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Số phiêu không chứa khoảng trắng.")]
        [Remote("IsMaPhieuThuAvailble", "HocPhi", ErrorMessage = "Số phiếu đã tồn tại.")]
        [Display(Name = "Số phiếu thu *")]
        public string maHocPhi { get; set; }
        [Display(Name = "Nội dung*")]
        [Required(ErrorMessage = "Nhập thông tin.")]
        [MaxLength(50, ErrorMessage = "Chiều dài tên không được quá 50 kí tự. ")]
        public string tenHDHocPhi { get; set; }
        [Remote("IsMaHocSinhExit", "HocSinh", ErrorMessage = "Mã học sinh không tồn tại.")]
        [Display(Name = "Mã học sinh *")]
        [Required(ErrorMessage = "Nhập mã học sinh")]
        public string maHocSinh { get; set; }
        public string tenHocSinh { get; set; }
        public string tenLop { get; set; }
        [Display(Name = "Năm học *")]
        [Required(ErrorMessage = "Chọn năm học")]
        public string namHoc { get; set; }
        [Display(Name = "Tháng *")]
        [Required(ErrorMessage = "Chọn tháng")]
        public int? thang { get; set; }
        [Display(Name="Đối tượng miễn giảm")]
        public string tenDTMienGiam { get; set; }
       
        public double? dinhMucGiam { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
         [Display(Name = "Số tiền giảm")]
        public int? tienMienGiam { get; set; }
        [Display(Name = "Ngày thu *")]
        [Required(ErrorMessage = "Ngày thu không bỏ trống.")]
        [UIHint("DateTime")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
        public DateTime? ngayThu { get; set; }
        [Display(Name = "Người thu *")]
        [Required(ErrorMessage = "Người thu không bỏ trống.")]
        public string nguoiThu { get; set; }

        public string tenNguoiThu { get; set; }
        [Display(Name = "Tổng tiền")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int? tongHocPhi { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Display(Name = "Đã thu")]
        [Required(ErrorMessage="Số tiền đã thu")]
        public int? daThu { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        [Display(Name = "Còn nợ")]
        public int? conNo { get; set; }
        [Display(Name = "Hình thức *")]
        [Required(ErrorMessage = "Chọn hình thức thu.")]
        public string loaiHP { get; set; }

        [Display(Name = "Ghi chú")]
        public string ghiChu { get; set; }

        public List<HocPhiData> danhSachHocPhi { get; set; }
    }

}