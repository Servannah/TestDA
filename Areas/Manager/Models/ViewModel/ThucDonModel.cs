using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class ThucDonData
    {
        [Key]
        public int maTD { get; set; }
        [Required(ErrorMessage = "Tên thực đơn không bỏ trống.")]
        [Display(Name = "Tên thực đơn * ")]
        public string tenThucDon { get; set; }
        [Required(ErrorMessage = "Nội dung thực đơn không bỏ trống.")]
        [Display(Name = "Nội dung * ")]
        [AllowHtml]
        public string noiDung { get; set; }
        [Required(ErrorMessage = "Ngày lập thực đơn.")]
        [UIHint("DateTime")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
        [Display(Name = "Ngày lập * ")]
        public DateTime? ngayLap { get; set; }
        [Required(ErrorMessage = "Người tạo thực đơn.")]
        [Display(Name = "Người lập * ")]
        public string nguoiLap { get; set; }
        public string tenNguoiTao { get; set; }
        [Display(Name = "Ghi chú")]
        public string ghiChu { get; set; }

        public List<ThucDonData> danhSachTD { get; set; }
    }  
}