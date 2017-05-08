using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class NhuCauDinhDuongData
    {
        [Key]
        public int maNhuCauDD { get; set; }
        [Display(Name="Tuổi")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Required(ErrorMessage="Tuổi không được bỏ trống.")]
        public int tuoi { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "KCalo")]
        public double? kcalo { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Protein")]
        public double? protein { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Canxi")]
        public double? calsi { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Sắt")]
        public double? sat { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Vitamin A")]
        public double? vitaminA { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Vitamin B1")]
        public double? vitaminB1 { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Vitamin B2")]
        public double? vitaminB2 { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Vitamin PP")]
        public double? vitaminPP { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Vitamin C")]
        public double? vitaminC { get; set; }

        [Display(Name = "Ghi chú")]
        [MaxLength(128, ErrorMessage = "Ghi chú không quá 128 kí tự.")]
        public string ghiChu { get; set; }
        public List<NhuCauDinhDuongData> danhSachNCDD { get; set; }
    }
}