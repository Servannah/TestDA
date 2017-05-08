using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TestDA.Areas.Manager.Models.ViewModel
{ 
    public class ParentData
    {
         [Key]
        public int id { get; set; }
        [Display(Name ="Tên đăng nhập *")]
        [Required(ErrorMessage = "Tên đăng nhập không bỏ trống!")]
        public string userName { get; set; }
        [Display(Name = "Mật khẩu *")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Mật khẩu không được bỏ trống!")]
        [MaxLength(50, ErrorMessage = "Chiều dài không được quá 50 kí tự. ")]
        public string pass { get; set; }

        public List<ParentData> listAccount { get; set; }
    }
    public class UserData
    {
        [Key]
        public string UserName { get; set; }
        [Required(ErrorMessage="Nhập mật khẩu đang sử dụng")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ *")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage="Nhập mật khẩu mới")]
        [StringLength(50, ErrorMessage = "Mật khẩu phải ít nhất 6 kí tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới *")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu mới và mật khẩu xác nhận không trùng nhau.")]
        public string ConfirmPassword { get; set; }
    }
}