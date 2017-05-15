using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using TestDA.DB;
using TestDA.Areas.Manager.Models.EntityManager;
using TestDA.Areas.Manager.Models.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace TestDA.Areas.Manager.Controllers
{
    public class ThucPhamController : BaseController
    {
        // GET: Manager/ThucPham
        public ActionResult Index(string tukhoa, int page = 1, int pageSize = 20)
        {
            ThucPhamManager thucpham = new ThucPhamManager();
            ThucPhamData data = new ThucPhamData();
            //thực hiện phân trang
            //custom phân trang
            int totalRecord = 0;
            //Tìm kiếm với từ khóa nhập vào
            if (!String.IsNullOrEmpty(tukhoa))
            {
                tukhoa = tukhoa.Trim();
            }
            ViewBag.TuKhoaTK = tukhoa;
            //lấy danh sách thực phẩm
            data.danhSachThucPham = thucpham.LayDSThucPham(tukhoa,ref totalRecord, page, pageSize);

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            int maxPage = 8;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize)) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            ///
            return View(data);
        }
        //Thêm mới một thực phẩm
        public ActionResult Insert()
        {
            ThucPhamData obj = new ThucPhamData();
            //lấy danh sách nhóm thực phẩm
            NhomThucPhamManager nhomtp = new NhomThucPhamManager();
            ViewBag.maNhomTP = new SelectList(nhomtp.LayDSNhomTP(), "maNhomTP", "tenNhomThucPham");
            return View();
        }
        [HttpPost]
        public ActionResult Insert(ThucPhamData obj)
        {
            if (ModelState.IsValid)//trả về false
            {
                ThucPhamManager TPM = new ThucPhamManager();
                TPM.ThemMoiThucPham(obj);
                TempData["Success"] = "Thêm mới thành công";
                return RedirectToAction("Index", "ThucPham");
            }
            //lấy danh sách nhóm thực phẩm
            NhomThucPhamManager nhomtp = new NhomThucPhamManager();
            ViewBag.maNhomTP = new SelectList(nhomtp.LayDSNhomTP(), "maNhomTP", "tenNhomThucPham");

            return View(obj);
        }
        //lấy thông tin thực phẩm
        public ActionResult TTThucPham( int maThucPham)
        {
            ThucPhamManager thucpham = new ThucPhamManager();
            ThucPhamData TPD = thucpham.LayChiTietTP(maThucPham);
            //lấy danh sách nhóm thực phẩm
            NhomThucPhamManager nhomtp = new NhomThucPhamManager();
            ViewBag.maNhomTP = new SelectList(nhomtp.LayDSNhomTP(), "maNhomTP", "tenNhomThucPham", TPD.maNhomTP);

            return Json(TPD);
        }
        //chỉnh sửa thực phẩm
        public ActionResult Edit()
        {
            ThucPhamManager thucpham = new ThucPhamManager();
            string maThucPham = Request.QueryString["mathucpham"].ToString();

            int id = int.Parse(maThucPham);
            ThucPhamData TPD = thucpham.LayChiTietTP(id);
            //lấy danh sách nhóm thực phẩm
            NhomThucPhamManager nhomtp = new NhomThucPhamManager();
            ViewBag.maNhomTP = new SelectList(nhomtp.LayDSNhomTP(), "maNhomTP", "tenNhomThucPham", TPD.maNhomTP);

            return View(TPD);
        }
        [HttpPost]
        public ActionResult Edit(ThucPhamData obj)
        {
           
            if (ModelState.IsValid)
            {
                ThucPhamManager thucpham = new ThucPhamManager();
                thucpham.SuaThucPham(obj);
                TempData["Success"] = "Chỉnh sửa thành công";
                return RedirectToAction("Index", "ThucPham");
            }

            //lấy danh sách nhóm thực phẩm
            NhomThucPhamManager nhomtp = new NhomThucPhamManager();
            ViewBag.maNhomTP = new SelectList(nhomtp.LayDSNhomTP(), "maNhomTP", "tenNhomThucPham", obj.maNhomTP);
            return View(obj);
        }
        //Xóa bỏ một danh mục
        public ActionResult XoaThucPham(int maThucPham)
        {
            ThucPhamManager thucpham = new ThucPhamManager();
            int kq = (int)thucpham.XoaThucPham(maThucPham);
            TempData["Success"] = "Xóa bản ghi thành công";
            return Json(kq);
        }
    }
}