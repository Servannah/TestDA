using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class DangNhap
    {
        [Key]
        public string MaTK { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [Display(Name = "Tên đăng nhập")]
        public string tenDangNhap { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string matKhau { get; set; }
    }
    public class DangKi
    {
        [Key]
        public string maNV { get; set; }
        public int maQuyen { get; set; }
        public string tenQuyen { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [Display(Name = "Tên đăng nhập")]
        public string tenDangNhap { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string matKhau { get; set; }
        [Required(ErrorMessage = "Nhập vào email liên hệ.")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Địa chỉ email không đúng định dạng !")]
        [Display(Name = "Email")]
        public string email { get; set; }
        [Required(ErrorMessage = "Nhập vào họ.")]
        [Display(Name = "Họ")]
        public string ho { get; set; }
        [Required(ErrorMessage = "Nhập vào tên.")]
        [Display(Name = "Tên")]
        public string ten { get; set; }

        [Display(Name = "Giới tính")]
        public string gioiTinh { get; set; }
    }
    public class NhanVienData{
        [Key]
        [Required(ErrorMessage = "Nhập vào mã nhân viên")]
        [MaxLength(10, ErrorMessage = "Mã nhân viên không quá 20 kí tự.")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Mã nhân viên không chứa khoảng trắng.")]
        [Remote("IsMaNhanVienAvailble", "NhanVien", ErrorMessage = "Mã nhân viên đã tồn tại.")]
        [Display(Name = "Mã nhân viên *")]
        public string maNV { get; set; }
        [Required(ErrorMessage = "Nhập vào họ.")]
        [MaxLength(50, ErrorMessage="Chiều dài không quá 50 kí tự.")]
        [Display(Name = "Họ *")]
        public string ho { get; set; }
        [Required(ErrorMessage = "Nhập vào tên.")]
        [MaxLength(50, ErrorMessage = "Chiều dài không quá 50 kí tự.")]
        [Display(Name = "Tên *")]
        public string ten { get; set; }
        public string hoTen { get; set; }
        public string maHoTen { get; set; }
        [Display(Name="Ngày sinh *")]
        [UIHint("DateTime")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
        public DateTime? ngaySinh { get; set; }
         [Display(Name = "Giới tính *")]
         [Required(ErrorMessage = "Chọn giới tính")]
        public string gioiTinh { get; set; }
         [Display(Name = "Dân Tộc")]
         [MaxLength(50, ErrorMessage = "Chiều dài không quá 50 kí tự.")]
         public string danToc { get; set; }
         [Display(Name = "Tôn giáo")]
         [MaxLength(50, ErrorMessage = "Chiều dài không quá 50 kí tự.")]
         public string tonGiao { get; set; }
        [Display(Name = "Số CMND *")]
        [Required(ErrorMessage = "Nhập số CMND")]
         [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
         [MaxLength(11, ErrorMessage = "Chiều dài không quá 11 kí tự.")]
         public string soCMND { get; set; }
         [UIHint("DateTime")]
         [DataType(DataType.Date)]
         [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
         [Display(Name = "Ngày cấp ")]
         public DateTime? ngayCapCMND { get; set; }
        [Display(Name = "Nơi cấp ")]
         public string noiCapCMND { get; set; }
         [Display(Name = "Địa chỉ *")]
        [Required(ErrorMessage="Nhập địa chỉ ")]
         public string diaChi { get; set; }
         [Display(Name = "Điện thoại")]
         [RegularExpression(@"((0\d{10})|(0\d{9}))", ErrorMessage = "Số điện thoại bắt đầu từ 0, chiều dài 10 hoặc 11 số.")]
         public string dienThoai { get; set; }
         [Display(Name = "Email ")]
         [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Địa chỉ email không đúng định dạng !")]
         public string email { get; set; }
        [Display(Name = "Chức vụ *")]
        [Required(ErrorMessage = "Chọn thông tin")]
         public string chucVu { get; set; }
        [Display(Name = "Quyền truy cập *")]
        [Required(ErrorMessage = "Chọn quyền truy cập.")]
        public int? quyenTruyCap { get; set; }
        public string tenQuyenTruyCap { get; set; }
        [Display(Name = "Trình độ")]
         public string trinhDo { get; set; }
        [UIHint("DateTime")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
        [Display(Name = "Ngày vào Đảng")]
         public DateTime? ngayVaoDang { get; set; }
        [Display(Name = "Số thẻ đảng")]
         public string soTheDang { get; set; }
         [Display(Name = "Hình ảnh")]
        [Required(ErrorMessage="Tải ảnh đại diện")]
         public string hinhAnh { get; set; }
         [Display(Name = "Hợp đồng *")]
        [Required(ErrorMessage="Nhập vào thông tin")]
         public string loaiHopDong { get; set; }
         [UIHint("DateTime")]
         [DataType(DataType.Date)]
         [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
         [Display(Name = "Ngày vào làm việc *")]
         [Required(ErrorMessage = "Chọn thông tin")]
         public DateTime? ngayVaoLamViec { get; set; }
         [Display(Name = "Đã nghỉ việc*")]
         [Required(ErrorMessage = "Chọn thông tin")]
         public string daNghiViec { get; set; }
         [UIHint("DateTime")]
         [DataType(DataType.Date)]
         [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
        [Display(Name = "Ngày nghỉ việc")]
         public DateTime? ngayNghiViec { get; set; }
        [Display(Name = "Ngày tạo")]
         public DateTime? ngayTao { get; set; }
        [Display(Name = "Ngày sửa")]
         public DateTime? ngaySua { get; set; }
        [Display(Name = "Ghi chú")]
         public string ghiChu { get; set; }

        public List<NhanVienData> dsNhanVien { get; set; }
    }
}