using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.ComponentModel;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class MonAnData
    {
        [Key]
        public int maMonAn { get; set; }
        [Display(Name="Loại món ăn *")]
        [Required(ErrorMessage="Loại món ăn không được bỏ trống.")]
        public int? maLoaiMonAn { get; set; }
        [Display(Name="Tên món ăn *")]
        [Required(ErrorMessage="Tên món ăn không được bỏ trống.")]
        [MaxLength(100, ErrorMessage = "Tên món ăn không được quá 100 kí tự. ")]
        public string tenMonAn { get; set; }
        [Display(Name="Số lượng người *")]
        [Required(ErrorMessage="Số lượng người không được bỏ trống.")]
        [Range(0, int.MaxValue, ErrorMessage = "Nhập vào một số nguyên.")]
        public int? soLuongNguoi { get; set; }
        [Display(Name="Chuẩn bị")]
        public string chuanBi { get; set; }
        [Display(Name="Chế biến")]
        public string cheBien { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        [ReadOnly(true)]
        public double? tongProtidDV { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
         [ReadOnly(true)]
        public double? tongProtidTV { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
         [ReadOnly(true)]
        public double? tongLipidDV { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
         [ReadOnly(true)]
        public double? tongLipidTV { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
         [ReadOnly(true)]
        public double? tongGlucid { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
         [ReadOnly(true)]
        public double? tongCalo { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
         [ReadOnly(true)]
        public double? tongCanxi { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
         [ReadOnly(true)]
        public double? tongPhotpho { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
         [ReadOnly(true)]
        public double? tongSat { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
         [ReadOnly(true)]
        public double? tongVitaminA { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
         [ReadOnly(true)]
        public double? tongVitaminB1 { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
         [ReadOnly(true)]
        public double? tongVitaminB2 { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
         [ReadOnly(true)]
        public double? tongVitaminPP { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
         [ReadOnly(true)]
        public double? tongVitaminC { get; set; }
        [Display(Name="Ghi chú")]
        [MaxLength(200, ErrorMessage = "Chiều dài ghi chú cần nhỏ hơn 200 kí tự. ")]
        public string ghiChu { get; set; }

        public List<TPMonAn> thanhPhanMonAn { get; set; }

        public List<MonAnData> danhSachMonAn { get; set; }

    }
    public class TPMonAn
    {
        [Key]
        public int maMonAnCT { get; set; }
        [Display(Name="Món ăn")]
        public int maMonAn { get; set; }
        public string tenMonAn { get; set; }
        [Display(Name="Thực phẩm")]
        [Required(ErrorMessage = "Chọn thực phẩm.")]
        public int maThucPham { get; set; }
        [Display(Name = "Thực phẩm")]
        public string tenThucPham { get; set; }
        [Required(ErrorMessage = "Số lượng không được bỏ trống.")]
        [Display(Name="Số lượng(gam)")]
        [Range(0, double.MaxValue, ErrorMessage = "Nhập vào một số")]
        public double? soLuongGam { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        [ReadOnly(true)]
        public double? protidDV { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        [ReadOnly(true)]
        public double? protidTV { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        [ReadOnly(true)]
        public double? lipidDV { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        [ReadOnly(true)]
        public double? lipidTV { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        [ReadOnly(true)]
        public double? glucid { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        [ReadOnly(true)]
        public double? calo { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        [ReadOnly(true)]
        public double? canxi { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        [ReadOnly(true)]
        public double? photPho { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        [ReadOnly(true)]
        public double? sat { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        [ReadOnly(true)]
        public double? vitaminA { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        [ReadOnly(true)]
        public double? vitaminB1 { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        [ReadOnly(true)]
        public double? vitaminB2 { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        [ReadOnly(true)]
        public double? vitaminPP { get; set; }
        [DisplayFormat(DataFormatString = "{0:n4}", ApplyFormatInEditMode = true)]
        [ReadOnly(true)]
        public double? vitaminC { get; set; }
        [Display(Name = "Ghi chú")]
        [MaxLength(200, ErrorMessage = "Chiều dài ghi chú cần nhỏ hơn 200 kí tự. ")]
        public string ghiChu { get; set; }

        public List<TPMonAn> danhSachTPMonAn { get; set; }
    }

}