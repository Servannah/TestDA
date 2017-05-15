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
    public class PhieuHoaDonController : BaseController
    {
        // GET: Manager/PhieuNhapKho
        [AuthorizeRoles("Admin", "HieuTruong", "VanThu")]
        public ActionResult Index(string tukhoa, int page = 1, int pageSize = 20)
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
            data.danhSachPhieuHD = phieuHD.LayDSPhieuHD(tukhoa, ref totalRecord, page, pageSize);

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
        //tạo partial hiển thị danh sách thực phẩm của hóa đơn này
        public ActionResult ThucPhamHDPartial(int maHoaDon)
        {
            CTHoaDonManager KTP = new CTHoaDonManager();
            CTHoaDonData data = new CTHoaDonData();
            data.danhSachCTHD = KTP.DSThucPhamTheoPhieuHD(maHoaDon);
            return PartialView(data);
        }
        //lấy chi tiết thực đơn 
        public ActionResult ChiTietTD(int maThucDon)
        {
            PhieuHoaDonManager PHM = new PhieuHoaDonManager();
            ThucDonData data = PHM.ChiTietThucDon(maThucDon);
            return PartialView(data);
        }
        ///tạo partialView thêm mới thực phẩm vào chi tiết hóa đơn
        public ActionResult ThemTPPartial()
        {
            //lấy danh sách thực phẩm
            ThucPhamManager TPM = new ThucPhamManager();
            ViewBag.ListTP = new SelectList(TPM.ListThucPham(), "maThucPham", "tenThucPham");
            List<CTHoaDonData> ktp = new List<CTHoaDonData>{
                new CTHoaDonData { 
                    maHDCT = 0, 
                    maHoaDon = 0,
                    maThucPham = null,
                    ngayTao = DateTime.UtcNow.Date,
                    soLuong =0,
                    donViTinh ="",
                    thanhTien =0,
                    ghiChu =""
                }
            };
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                return View(ktp);
            }
            else
            {
                //lấy danh sách thực phẩm  theo ID
                CTHoaDonManager KTP = new CTHoaDonManager();
                List<CTHoaDonData> data = new List<CTHoaDonData>();
                string maHoaDon = Request.QueryString["maHoaDon"].ToString();//lấy mã phiếu hóa đơn
                int id = int.Parse(maHoaDon);
                //lấy danh sách thực phẩm 

                data = KTP.DSThucPhamTheoPhieuHD(id);
                if (data.Count() == 0)
                {
                    return View(ktp);
                }
                //for (int i = 0; i < data.Count; i++)
                //{
                //ThucPhamManager thpM = new ThucPhamManager();        
                //ViewBag.ListTP = new SelectList(thpM.ListThucPham(), "maThucPham", "tenThucPham", data[i].maThucPhamNK);
                //}

                return View(data);
            }

        }
        //Hiển thị thông tin chi tiết hóa đơn
        public ActionResult Detail()
        {
            PhieuHoaDonManager PHD = new PhieuHoaDonManager();
            string maHoaDon = Request.QueryString["maHoaDon"].ToString();//lấy mã phiếu nhập kho
            int id = int.Parse(maHoaDon);
            HoaDonData hd = PHD.LayChiTietPhieuHD(id);
            return View(hd);
        }
        //thêm partialView hiển thị thông tin hóa đơn
        public ActionResult ViewPartial()
        {
            PhieuHoaDonManager PHD = new PhieuHoaDonManager();
            string idm = Request.QueryString["maPhieuHD"].ToString();
            int id = int.Parse(idm);
            HoaDonData hd = PHD.LayChiTietPhieuHD(id);
            return PartialView(hd);
        }
        public ActionResult TPHoaDonPartial()
        {
            CTHoaDonManager KTP = new CTHoaDonManager();
            CTHoaDonData data = new CTHoaDonData();
            string idm = Request.QueryString["maPhieuHD"].ToString();
            int id = int.Parse(idm);
            data.danhSachCTHD = KTP.DSThucPhamTheoPhieuHD(id);
            return PartialView(data);
        }
        public void setThucPham(int? selectedID = null)
        {
            //lấy danh sách thực phẩm trong cơ sở dữ liệu
            ThucPhamManager thucPham = new ThucPhamManager();
            ViewBag.maThucPham = new SelectList(thucPham.ListThucPham(), "maThucPham", "tenThucPham", selectedID);
        }
        //thực hiện chức năng thêm, sửa bản ghi
        public ActionResult ChucNang()
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            List<CTHoaDonData> ktp = new List<CTHoaDonData>{
                new CTHoaDonData { 
                    maHDCT = 0, 
                    maHoaDon = 0,
                    maThucPham = null,
                    ngayTao = DateTime.UtcNow.Date,
                    soLuong =0,
                    donViTinh ="",
                    thanhTien =0,
                    ghiChu =""
                }
            };
            if (type == "Insert")
            {
                ViewBag.ChucNang = "Thêm mới hóa đơn";
                setThucDon();
                setNhanVien();
                setThucPham();
                ViewBag.Type = 1;
                HoaDonData PNK = new HoaDonData();
                PNK.thucPham = ktp;

                return View(PNK);
            }
            else
            {
                ViewBag.ChucNang = "Chỉnh sửa thông tin hóa đơn";
                PhieuHoaDonManager PHD = new PhieuHoaDonManager();
                string maHoaDon = Request.QueryString["maPhieuHD"].ToString();
                int id = int.Parse(maHoaDon);
                HoaDonData PNK = new HoaDonData();

                PNK = PHD.LayChiTietPhieuHD(id);
                //lấy danh sách nhân viên 
                setNhanVien(PNK.nguoiLap);
                setThucDon(PNK.maThucDon);
                setThucPham();
                ViewBag.Type = 2;
                //lấy danh sách thực phẩm               
                CTHoaDonManager KTP = new CTHoaDonManager();
                PNK.thucPham = KTP.DSThucPhamTheoPhieuHD(id);
                if (PNK.thucPham.Count == 0)
                {
                    PNK.thucPham = ktp;
                }

                return View(PNK);
            }
        }
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
                    return RedirectToAction("Index", "PhieuHoaDon");
                }
                setNhanVien();
                setThucDon();
                setThucPham();
                return View(obj);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    PhieuHoaDonManager phieuHD = new PhieuHoaDonManager();
                    phieuHD.SuaPhieuHD(obj);
                    TempData["Success"] = "Chỉnh sửa thành công";
                    return RedirectToAction("Index", "PhieuHoaDon");
                }
                setThucDon(obj.maThucDon);
                setNhanVien(obj.nguoiLap);
                setThucPham();
                return View(obj);
            }
        }
        //Xóa bỏ một danh mục
        public ActionResult XoaPhieuHD(int maPhieuHD)
        {
            PhieuHoaDonManager phieuND = new PhieuHoaDonManager();
            int kq = (int)phieuND.XoaPhieuHD(maPhieuHD);
            return Json(kq);
        }//kết thúc xóa bỏ hóa đơn
    }
}