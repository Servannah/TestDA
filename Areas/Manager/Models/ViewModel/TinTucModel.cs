using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class TinTucData
    {
        [Key]
        public int maTin { get; set; }
        [Display(Name = "Tiêu đề *")]
        [Required(ErrorMessage="Tiêu đề không được bỏ trống .")]
        [MaxLength(70,ErrorMessage="Tiêu đề không quá 70 kí tự")]
        public string tieuDe { get; set; }
        [Display(Name="Slug")]
        [MaxLength(150,ErrorMessage="Slug không quá 150 kí tự")]
        public string slug { get; set; }
        [Display(Name = "Ảnh đại diện")]
        [Required(ErrorMessage="Thêm vào ảnh đại diện")]
        public string anhDaiDien { get; set; }       
        [AllowHtml]
        [Display(Name = "Nội dung ")]
        public string noiDung { get; set; }
        [Display(Name = "Người tạo")]
        public string nguoiTao { get; set; }
        [Display(Name="Ngày tạo")]
        [UIHint("DateTime")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
        public DateTime ? ngayTao{get;set;}
        [Display(Name="Người chỉnh sửa")]
        public string nguoiSua{get; set;}
        [Display(Name = "Chỉnh sửa lần cuối")]
        [UIHint("DateTime")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
        public DateTime ? ngaySua{get; set;}
        [Required(ErrorMessage = "Danh mục chứa bài viết")]
        [Display(Name = "Danh mục * ")]
        public int nhomTin { get; set; }  
        [Display(Name="Danh mục tin")]

        public string tenDanhMuc{get; set;}
        [Required(ErrorMessage = "Tình trạng bài viết")] 
        [Display(Name = "Tình trạng")]
        public string tinhTrang { get; set; }
        [Display(Name="Tin nổi bật")]
        public int? tinNoiBat { get; set; }
  
        public List<TinTucData> danhSachTin { get; set; }
    }
}