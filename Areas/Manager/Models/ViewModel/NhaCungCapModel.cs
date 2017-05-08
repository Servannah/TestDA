using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class NhaCungCapData
    {
        [Key]
        public int maNhaCC { get; set; }
        [Display(Name = "Tên nhà cung cấp (*)")]
        [Required(ErrorMessage = "Tên nhà cung cấp không được bỏ trống!")]
        [MaxLength(200, ErrorMessage = "Chiều dài không được quá 200 kí tự. ")]
        public string tenNhaCC { get; set; }
        [Display(Name = "Địa chỉ")]
        [MaxLength(200, ErrorMessage = "Chiều dài không được quá 200 kí tự. ")]
        public string diaChi { get; set; }
        [Display(Name = "Điện thoại")]
        [RegularExpression(@"((0\d{10})|(0\d{9}))", ErrorMessage = "Số điện thoại bắt đầu từ 0, chiều dài 10 hoặc 11 số.")]
        public string dienThoai { get; set; }
        [Display(Name = "Fax")]
        [MaxLength(50, ErrorMessage = "Chiều dài không được quá 50 kí tự. ")]
        public string fax { get; set; }
        [Display(Name = "Mã số thuế")]
        [MaxLength(50, ErrorMessage = "Chiều dài không được quá 50 kí tự. ")]
        public string maSoThue { get; set; }
        [Display(Name = "Tài khoản")]
        [MaxLength(50, ErrorMessage = "Chiều dài không được quá 50 kí tự. ")]
        public string taiKhoan { get; set; }
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",ErrorMessage="Không đúng định dạng Email")]
        [Display(Name = "Email")]
        [MaxLength(200, ErrorMessage = "Chiều dài không được quá 200 kí tự. ")]
        public string email { get; set; }
        [Display(Name = "Ghi chú")]
        [MaxLength(200, ErrorMessage = "Ghi chú không quá 200 kí tự.")]
        public string ghiChu { get; set; }
        public List<NhaCungCapData> danhSachNhaCungCap { get; set; }
    }
}