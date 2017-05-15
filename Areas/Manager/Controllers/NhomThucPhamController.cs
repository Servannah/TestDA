using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDA.DB;
using TestDA.Areas.Manager.Models.EntityManager;
using TestDA.Areas.Manager.Models.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using TestDA.Security;

namespace TestDA.Areas.Manager.Controllers
{
    public class NhomThucPhamController : BaseController
    {
        // GET: nhóm thực phẩm
        [AuthorizeRoles("Admin", "HieuTruong", "NhaBep")]
        public ActionResult Index(string tukhoa)//từ khóa tìm kiếm
        {
            NhomThucPhamManager nhomtin = new NhomThucPhamManager();
            NhomThucPhamData data = new NhomThucPhamData();
            if (!String.IsNullOrEmpty(tukhoa))
            {
                tukhoa = tukhoa.Trim();
            }
            data.danhSachNhomTP = nhomtin.TimKiemNhomTP(tukhoa);
            ViewBag.TuKhoaTK = tukhoa;
            return View(data);
        }
        //Thêm mới một danh mục
        [HttpPost]
        public ActionResult Index(NhomThucPhamData obj)
        {
            if (ModelState.IsValid)
            {
                NhomThucPhamManager nhomtp = new NhomThucPhamManager();
                nhomtp.ThemMoiNhomTP(obj);
                TempData["Success"] = "Thêm mới thành công";
                return RedirectToAction("Index", "NhomThucPham");
            }
            NhomThucPhamManager nhomTP = new NhomThucPhamManager();
            obj = new NhomThucPhamData();
            obj.danhSachNhomTP = nhomTP.LayDSNhomTP();
            return View(obj);
        }
        //Sửa thông tin một danh mục nhóm thực phẩm
        public ActionResult SuaNhomTP(int maNhomTP, string tenNhomTP, string ghiChu)
        {
            if (ModelState.IsValid)
            {
                NhomThucPhamManager nhomtp = new NhomThucPhamManager();
                nhomtp.SuaNhomTP(maNhomTP,tenNhomTP,ghiChu);
                return Json("Sửa thông tin bản ghi thành công .");
            }
            NhomThucPhamData obj = new NhomThucPhamData();
            obj.tenNhomThucPham = tenNhomTP;
            obj.ghiChu = ghiChu;
            return View(obj) ;
           
        } 
        //Xóa bỏ một danh mục
        public ActionResult XoaNhomTP(int maNhom)
        {
            NhomThucPhamManager nhomTP = new NhomThucPhamManager();
            int kq = (int)nhomTP.XoaNhomTP(maNhom);
            return Json(kq);
        }      
    }
}