using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class ThucPhamData
    {
        [Key]
        public int maThucPham { get; set; }
        [Display(Name="Tên thực phẩm")]
        [StringLength(100, ErrorMessage = "Tên thực phẩm không quá 100 kí tự.")]
        [Required(ErrorMessage= "Tên thực phẩm không được bỏ trống")]
        public string tenThucPham { get; set; }
        [Display(Name="Nguồn từ động vật")]
        [Required(ErrorMessage = "Không được bỏ trống")]
        public bool? nguonTuDV { get; set; }
        [TestDA.Common.ValidateNotValidForProperty.ValidDecimal(ErrorMessage = "Nhập vào một số !")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Tỷ lệ hấp thụ")]
        public double? tyLeHapThu { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Tỷ lệ thải")]
        public double? tyLeThai { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Năng lượng calo")]
        public double? nangLuongCalo { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Nước")]
        public double? tphhNuoc { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Protid")]
        public double? tphhProtid { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Lipid")]
        public double? tphhLipid { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Glucid")]
        public double? tphhGlucid { get; set; }
        [Display(Name = "Cellulose")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        public double? tphhCellulose { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Cholesterol")]
        public double? tphhCholesterol { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Canxi")]
        public double? mkCalci { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Phot pho")]
        public double? mkPhotpho { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Sắt")]
        public double? mkSat { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Vitamin Caroten")]
        public double? vitaminCaroten { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Vitamin A")]
        public double? vitaminA { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Vitamin B2")]
        public double? vitaminB1 { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Vitamin B2")]
        public double? vitaminB2 { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Vitamin C")]
        public double? vitaminC { get; set; }
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [Display(Name = "Vitamin PP")]
        public double? vitaminPP { get; set; }

        [Display(Name = "Ghi chú")]
        public string ghiChu { get; set; }

        [Display(Name="Tên nhóm thực phẩm")]
        public string tenNhomTP { get; set; }
        [Required(ErrorMessage="Chọn nhóm thực phẩm")]
        [Display(Name = "Nhóm thực phẩm")]
        public int maNhomTP { get; set; }

        public List<ThucPhamData> danhSachThucPham { get; set; }
    }
}