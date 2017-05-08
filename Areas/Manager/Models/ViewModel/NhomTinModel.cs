using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class NhomTinData
    {
        [Key]
        public int maNhom { get; set; }
        [Display(Name="Tên danh mục (*)")]
        [Required(ErrorMessage="Tên danh mục không được bỏ trống!")]
        [MaxLength(150, ErrorMessage="Chiều dài tên danh mục không được quá 150 kí tự. ")]
        public string tenNhom { get; set; }
        [Display(Name="Slug")]
        public string slug { get; set; }
        [Display(Name="Danh mục cha")]
        public int ? danhMucCha { get; set; }
        [Display(Name="Mô tả")]
        public string moTa { get; set; }
        [Display(Name="Tình trạng")]
        public int ? tinhTrang{get; set;}
        public int? thuTu { get; set; }
        public SelectList DanhSachCha { get; set; }
        public List<NhomTinData> DanhSachCon { get; set; }
        public List<NhomTinData> danhSachNhomTin { get; set; }
    }
}