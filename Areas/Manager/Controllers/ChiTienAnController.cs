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
    public class ChiTienAnController : BaseController
    {
        // GET: Manager/ChiTienAn
        [AuthorizeRoles("Admin", "HieuTruong", "VanThu","NhaBep")]
        public ActionResult Index(string tukhoa,DateTime? ngayLap, int page = 1, int pageSize = 20)
        {
            PhieuHoaDonManager phieuHD = new PhieuHoaDonManager();
            HoaDonData data = new HoaDonData();
            //thực hiện phân trang
            //custom phân trang
            int totalRecord = 0;
            //Tìm kiếm với từ khóa nhập vào
            if (!String.IsNullOrEmpty(tukhoa))
            {
                tukhoa = tukhoa.Trim();
            }
            ViewBag.TuKhoaTK = tukhoa;
            //lấy danh sách phiếu nhập kho
            data.danhSachPhieuHD = phieuHD.LayDSPhieuHD(tukhoa,ngayLap, ref totalRecord, page, pageSize);

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

            return View(data);
        }
        ///
        public void setThucDon(int? selectedID = null)
        {
            //lấy danh sách thực đơn
            ThucDonManager td = new ThucDonManager();
            ViewBag.maThucDon = new SelectList(td.ListTD(), "maTD", "tenThucDon", selectedID);
        }

        public void setNhanVien(string selectedID = null)
        {
            //lấy danh sách nhân viên trong cơ sở dữ liệu
            NhanVienManager nhanVien = new NhanVienManager();
            ViewBag.nguoiLap = new SelectList(nhanVien.ListNhanVien(), "maNV", "maHoTen", selectedID);
        }
        //lấy chi tiết thực đơn 
        public ActionResult ChiTietTD(int maThucDon)
        {
            PhieuHoaDonManager PHM = new PhieuHoaDonManager();
            ThucDonData data = PHM.ChiTietThucDon(maThucDon);
            return PartialView(data);
        }
        public ActionResult ChucNang()
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                ViewBag.ChucNang = "Thêm mới thông tin";
                setThucDon();
                setNhanVien();

                ViewBag.Type = 1;

                return View();
            }
            else
            {
                ViewBag.ChucNang = "Chỉnh sửa thông tin ";
                PhieuHoaDonManager PHD = new PhieuHoaDonManager();
                string maHoaDon = Request.QueryString["maPhieuHD"].ToString();
                int id = int.Parse(maHoaDon);
                HoaDonData PNK = new HoaDonData();

                PNK = PHD.LayChiTietPhieuHD(id);
                //lấy danh sách nhân viên 
                setNhanVien(PNK.nguoiLap);
                setThucDon(PNK.maThucDon);

                ViewBag.Type = 2;

                return View(PNK);
            }
        }
        //thực hiện chức năng thêm, sửa bản ghi
        [HttpPost]
        public ActionResult ChucNang(HoaDonData obj)
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                if (ModelState.IsValid)//trả về false
                {
                    PhieuHoaDonManager HDD = new PhieuHoaDonManager();
                    HDD.ThemMoiPhieuHD(obj);
                    TempData["Success"] = "Thêm mới thành công";
                    return RedirectToAction("Index", "ChiTienAn");
                }
                setNhanVien();
                setThucDon();

                return View(obj);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    PhieuHoaDonManager phieuHD = new PhieuHoaDonManager();
                    phieuHD.SuaPhieuHD(obj);
                    TempData["Success"] = "Chỉnh sửa thành công";
                    return RedirectToAction("Index", "ChiTienAn");
                }
                setThucDon(obj.maThucDon);
                setNhanVien(obj.nguoiLap);

                return View(obj);
            }
        }
        //Xóa bỏ một danh mục
        public ActionResult XoaPhieuHD(int maPhieuHD)
        {
            PhieuHoaDonManager phieuND = new PhieuHoaDonManager();
            int kq = (int)phieuND.XoaPhieuHD(maPhieuHD);
            return Json(kq);
        }//kết thúc xóa bỏ
        ///
    }
}