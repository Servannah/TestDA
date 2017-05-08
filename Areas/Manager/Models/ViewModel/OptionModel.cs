using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class OptionData
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "Tiêu đề *")]
        [Required(ErrorMessage = "Tiêu đề không được bỏ trống")]
        [MaxLength(200, ErrorMessage="Tiêu đề không quá 200 kí tự")]
        public string name { get; set; }
        [Display(Name = "Đường dẫn")]
        [MaxLength(500, ErrorMessage = "Tiêu đề không quá 200 kí tự")]
        public string link { get; set; }
        [Display(Name = "Mô tả")]
        [MaxLength(200, ErrorMessage = "Mô tả không được quá 200 kí tự. ")]
        public string description { get; set; }
        public string type { get; set; }
        [Display(Name = "Thứ tự")]
        public int? order { get; set; }
        [Display(Name="Nội dung")]
        [AllowHtml]
        public string content { get; set; }

        public List<OptionData> danhSachDuLieu { get; set; }
    }
}