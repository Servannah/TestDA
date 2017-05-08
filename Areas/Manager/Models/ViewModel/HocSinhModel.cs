using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class HocSinhData
    {
        [Key]
        [Required(ErrorMessage = "Nhập vào mã học sinh")]
        [MaxLength(10, ErrorMessage = "Mã học sinh không quá 10 kí tự.")]
        [RegularExpression(@"^\S*$", ErrorMessage = "Mã học sinh không chứa khoảng trắng.")]
        [Remote("IsMaHocSinhAvailble", "HocSinh", ErrorMessage = "Mã học sinh đã tồn tại.")]
        [Display(Name = "Mã học sinh *")]
        public string maHocSinh{get; set;}
        [Required(ErrorMessage = "Nhập vào họ tên học sinh")]
        [MaxLength(150, ErrorMessage = "Họ tên không quá 150 kí tự.")]
        [Display(Name="Họ tên *")]
        public string hoTen{get; set;}
        public string maTen { get; set; }
        [UIHint("DateTime")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
        [Display(Name="Ngày sinh *")]
        [Required(ErrorMessage = "Nhập vào ngày sinh.")]
        public DateTime ? ngaySinh{get; set;}
        [Display(Name="Giới tính *")]
        [Required(ErrorMessage = "Lựa chọn giới tính.")]
        public string gioiTinh{get; set;}
        [MaxLength(50, ErrorMessage = "Dân tộc không quá 50 kí tự.")]
        [Display(Name="Dân Tộc")]
        public string danToc{get; set;}
        [MaxLength(50, ErrorMessage = "Tôn giáo không quá 50 kí tự.")]
        [Display(Name="Tôn giáo")]
        public string tonGiao{get; set;}
        [MaxLength(150, ErrorMessage = "Quê quán không quá 150 kí tự.")]
        [Display(Name="Quê quán")]
        public string queQuan{get; set;}
        [Required(ErrorMessage = "Nhập vào địa chỉ chỗ ở.")]
        [Display(Name="Địa chỉ thường trú *")]
        [MaxLength(150, ErrorMessage = "Địa chỉ không quá 150 kí tự.")]
        public string diaChiTamTru{get; set;}
        [Display(Name="Đối tượng ưu tiên *")]
        [Required(ErrorMessage="Lựa chọn đối tượng ưu tiên.")]
        public int? maUuTien{get; set;}
        public double? dinhMucGiam { get; set; }
        public string tenDTUuTien { get; set; }
        [Display(Name="Ngày vào học")]
        [UIHint("DateTime")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
        public DateTime? ngayVaoHoc{get; set;}
         [Required(ErrorMessage = "Chọn thông tin")]
        [Display(Name="Đã nghỉ học *")]
        public string daNghiHoc{get; set;}
        [UIHint("DateTime")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
        [Display(Name="Ngày nghỉ học")]
        public DateTime? ngayNghiHoc{get; set;}
        [MaxLength(150, ErrorMessage = "Họ tên không quá 150 kí tự.")]
        [Display(Name="Họ tên cha")]
        public string hoTenCha{get; set;}
        [UIHint("DateTime")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
        [Display(Name="Ngày sinh cha")]
        public DateTime? ngaySinhCha{get; set;}
        [MaxLength(150, ErrorMessage = "Nghề nghiệp không quá 150 kí tự.")]
        [Display(Name="Nghề nghiệp cha")]
        public string ngheNghiepCha{get; set;}
        [MaxLength(150, ErrorMessage = "Họ tên không quá 150 kí tự.")]
        [Display(Name="Họ tên mẹ")]
        public string hoTenMe{get; set;}
        [Display(Name="Ngày sinh mẹ")]
        [UIHint("DateTime")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
        public DateTime? ngaySinhMe{get; set;}
        [MaxLength(150, ErrorMessage = "Nghề nghiệp không quá 150 kí tự.")]
        [Display(Name="Nghề nghiệp mẹ")]
        public string ngheNghiepMe{get; set;}
        [Display(Name="Điện thoại phụ huynh")]
        [RegularExpression(@"((0\d{10})|(0\d{9}))", ErrorMessage = "Số điện thoại bắt đầu từ 0, chiều dài 10 hoặc 11 số.")]
        public string dienThoaiLienHe{get; set;}
        [Display(Name="Email phụ huynh")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Địa chỉ email không đúng định dạng !")]
        public string emailLienHe{get;set;}
        [Display(Name="Hình ảnh")]
        public string hinhAnh{get; set;}
        [MaxLength(150, ErrorMessage = "Tên thường gọi không quá 50 kí tự.")]
        [Display(Name="Tên thường gọi")]
        public string tenThuongGoi{get; set;}
        [MaxLength(250, ErrorMessage = "Tính cách không quá 250 kí tự.")]
        [Display(Name="Tính cách")]
        public string tinhCach{get;  set;}
         [Required(ErrorMessage = "Chọn thông tin")]
        [Display(Name="Đã hoàn thành mẫu giáo *")]
        public string hoanThanhMauGiao{get; set;}
        [Display(Name = "Ghi chú")]
        public string ghiChu { get; set; }


        public List<HocSinhData> danhSachHocSinh { get; set; }
    }
}