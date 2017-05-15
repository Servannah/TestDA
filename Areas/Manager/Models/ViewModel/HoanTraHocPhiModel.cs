using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class HoanTraHocPhiData
    {
        [Key]
        public int maHTHP { get; set; }
        [Display(Name = "Mã học sinh *")]
        [Required(ErrorMessage = "Mã học sinh không được bỏ trống!")]
        [Remote("IsMaHocSinhExit", "HocSinh", ErrorMessage = "Mã học sinh không tồn tại.")]
        public string maHocSinh { get; set; }
        public string tenHocSinh { get; set; }
         [Display(Name = "Lớp học *")]
         [Required(ErrorMessage = "Chọn lớp học")]
        public int? maLop { get; set; }
        public string tenLop { get; set; }
        [Display(Name = "Năm học *")]
        [Required(ErrorMessage = "Chọn năm học.")]
        public string namHoc { get; set; }
         [Display(Name = "Tiền hoàn trả *")]
         [Required(ErrorMessage = "Không được bỏ trống")]
        [RegularExpression(@"[0-9]*\.?[0-9]+", ErrorMessage = "Nhập vào một số.")]
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int? tienHoanTra { get; set; }
        [Display(Name = "Lý do chi trả")]
        [MaxLength(200, ErrorMessage = "Mô tả không quá 200 kí tự.")]
         public string lyDoTra { get; set; }
        [Display(Name = "Ngày lập *")]
        [Required(ErrorMessage = "Không được bỏ trống")]
        [UIHint("DateTime")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")] 
        public DateTime? ngayLap { get; set; }
        [Display(Name = "Ghi chú")]
        public string ghiChu { get; set; }

        public List<HoanTraHocPhiData> danhSachHoanTra { get; set; }
    }
}