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

namespace TestDA.Areas.Manager.Controllers
{
    public class HocSinhController : Controller
    {
        // GET: Manager/HocSinh
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult HocSinhPartial(string tukhoa, int page = 1, int pageSize = 20)
        {
            HocSinhManager HSM = new HocSinhManager();
            HocSinhData data = new HocSinhData();
            //thực hiện phân trang
            //custom phân trang
            int totalRecord = 0;

            if (!String.IsNullOrEmpty(tukhoa))
            {
                tukhoa = tukhoa.Trim();
            }
            data.danhSachHocSinh = HSM.LayDSHocSinh(tukhoa,ref totalRecord, page, pageSize);
            ViewBag.TuKhoaTK = tukhoa;

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

            return PartialView(data);
        }
        //xem thông tin chi tiết học sinh
        public ActionResult ChiTietHocSinh()
        {
            HocSinhManager HSM = new HocSinhManager();
            string maHocSinh = Request.QueryString["maHocSinh"].ToString();

            HocSinhData HSD = HSM.LayChiTietHS(maHocSinh);
            setDoiTuong(HSD.maUuTien);
            if (HSD.hinhAnh != null)
            {
                ViewBag.hinhAnh = @Url.Content(HSD.hinhAnh);
            }
            else
            { ViewBag.hinhAnh = null; }
            return View(HSD);
        }
        public void setDoiTuong(int? selectedId = null)
        {
            DoiTuongUuTienManager dt = new DoiTuongUuTienManager();
            ViewBag.maUuTien = new SelectList(dt.LayDSLoaiUT(), "maUuTien", "loaiDT", selectedId);
        }
        //thực hiện chức năng thêm, sửa bản ghi
        public ActionResult ChucNang()
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                ViewBag.ChucNang = "Thêm mới học sinh";
                ViewBag.hinhAnh = null;
                setDoiTuong();

                ViewBag.type = 1;

                return View();
            }
            else
            {
                ViewBag.ChucNang = "Chỉnh sửa thông tin học sinh";
                HocSinhManager HSM = new HocSinhManager();
                string maHocSinh = Request.QueryString["maHocSinh"].ToString();

                HocSinhData HSD = HSM.LayChiTietHS(maHocSinh);
                setDoiTuong(HSD.maUuTien);

                if (HSD.hinhAnh != null)
                {
                    ViewBag.hinhAnh = @Url.Content(HSD.hinhAnh);
                }else
                { ViewBag.hinhAnh = null; }

                ViewBag.type = 2;
                

                return View(HSD);
            }
        }
        //thực hiện chức năng thêm, sửa bản ghi
        [HttpPost]
        public ActionResult ChucNang(HocSinhData obj)
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                if (ModelState.IsValid)//trả về false
                {
                    HocSinhManager HSM = new HocSinhManager();
                    HSM.ThemMoiHS(obj);

                    TempData["Success"] = "Thêm mới thành công";

                    return RedirectToAction("Index", "HocSinh");
                }
                setDoiTuong();
                return View(obj);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    HocSinhManager HSM = new HocSinhManager();
                    HSM.SuaHocSinh(obj);
                    TempData["Success"] = "Chỉnh sửa thành công";
                    return RedirectToAction("Index", "HocSinh");
                }
                setDoiTuong(obj.maUuTien);
                return View(obj);
            }
        }  //kết thúc chức năng thêm , sửa bản ghi      
        //Xóa bỏ một bản ghi
        public ActionResult XoaHocSinh(string maHocSinh)
        {
            HocSinhManager HSM = new HocSinhManager();
            int kq = (int)HSM.XoaHocSinh(maHocSinh);
            //TempData["Success"] = "Xóa bản ghi thành công";
            return Json(kq);
        }//kết thúc xóa bỏ bản ghi
        ///kiểm tra mã học sinh có trùng trong bảng dữ liệu không
        public ActionResult IsMaHocSinhAvailble(string maHocSinh)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    var maHS = db.tbl_hocsinh.Single(m => m.MaHS == maHocSinh);
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }
        //        
        public ActionResult IsMaHocSinhExit(string maHocSinh)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    var maHS = db.tbl_hocsinh.Single(m => m.MaHS == maHocSinh);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
        }////\
    }
}