using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class ConfigData
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Tiêu đề *")]
        [Required(ErrorMessage = "Tiêu đề không được bỏ trống!")]
        [MaxLength(50,ErrorMessage="Tiêu để không quá 50 kí tự")]
        public string name { get; set; }
        [Display(Name = "Loại")]
        [Required(ErrorMessage = "Loại không được bỏ trống")]
        public string type { get; set; }
        [Display(Name = "Hình ảnh *")]
        [Required(ErrorMessage = "Hình ảnh không được bỏ trống!")]
        public string image { get; set; }
        [Display(Name = "Nội dung *")]
        [Required(ErrorMessage = "Nội dung không được bỏ trống!")]
        public string content { get; set; }
        [Required(ErrorMessage = "Lựa chọn hiển thị")]
        public int? ord { get; set; }
        public int? temp1 { get; set; }

        public List<ConfigData> listData { get; set; }
    }
}