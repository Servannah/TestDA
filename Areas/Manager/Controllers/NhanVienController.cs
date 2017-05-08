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
    public class NhanVienController : BaseController
    {
        // GET: Manager/NhanVien
        [AuthorizeRoles("Admin","HieuTruong")]
        public ActionResult Index(string inputtext, string daNghiViec, int page = 1, int pageSize = 20)
        {
            //đưa ra danh sách nhân viên trong cơ sở dữ liệu
            NhanVienManager HSM = new NhanVienManager();
            NhanVienData data = new NhanVienData();
            //thực hiện phân trang
            //custom phân trang
            int totalRecord = 0;

            if (!String.IsNullOrEmpty(inputtext))
            {
                inputtext = inputtext.Trim();
            }
            data.dsNhanVien = HSM.LayDSNhanVien(inputtext, daNghiViec, ref totalRecord, page, pageSize);
            ViewBag.TuKhoaTK = inputtext;
            setNghiViec();

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
        public void setNghiViec(string selectedID = null)
        {
            List<SelectListItem> SetValue = new List<SelectListItem>();
            SetValue.Add(new SelectListItem()
            {
                Text = "Đang làm việc",
                Value = "1",
                Selected = false
            });
            SetValue.Add(new SelectListItem()
            {
                Text = "Đã nghỉ việc",
                Value = "2",
                Selected = false
            });
            ViewBag.daNghiViec = new SelectList(SetValue, "Value", "Text", selectedID);
        }
        //set giới tính
        public void setGioiTinh(string selectedID = null)
        {
            List<SelectListItem> SetValue = new List<SelectListItem>();
            SetValue.Add(new SelectListItem()
            {
                Text = "Nữ",
                Value = "F",
                Selected = false
            });
            SetValue.Add(new SelectListItem()
            {
                Text = "Nam",
                Value = "M",
                Selected = false
            });
            ViewBag.gioiTinh = new SelectList(SetValue, "Value", "Text", selectedID);
        }
        //lấy danh sách quyền người dùng trong bản phân quyền
        public void setPhanQuyen(int? selectedID = null)
        {
            QuyenNDManager QNM = new QuyenNDManager();
            ViewBag.quyenTruyCap = new SelectList(QNM.LayDSQuyen(), "maPQ", "tenPQ", selectedID);
        }
        //thực hiện chức năng thêm, sửa bản ghi
        public ActionResult ChucNang()
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                ViewBag.ChucNang = "Thêm mới nhân viên";
                setNghiViec();
                setPhanQuyen();
                setGioiTinh();

                ViewBag.type = 1;

                return View();
            }
            else
            {
                ViewBag.ChucNang = "Chỉnh sửa thông tin nhân viên";
                NhanVienManager LM = new NhanVienManager();
                string maNV = Request.QueryString["maNV"].ToString();
                NhanVienData LD = LM.LayChiTietNV(maNV);

                if (LD.hinhAnh != null)
                {
                    ViewBag.hinhAnh = @Url.Content(LD.hinhAnh);
                }
                else
                { ViewBag.hinhAnh = null; }

                ViewBag.type = 2;

                setPhanQuyen(LD.quyenTruyCap);
                setNghiViec(LD.daNghiViec);
                setGioiTinh(LD.gioiTinh);

                return View(LD);
            }
        }
        //thực hiện chức năng thêm, sửa bản ghi
        [HttpPost]
        public ActionResult ChucNang(NhanVienData obj)
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                if (ModelState.IsValid)//trả về false
                {
                    NhanVienManager LM = new NhanVienManager();
                    LM.ThemNhanVien(obj);
                    TempData["Success"] = "Thêm mới thành công";
                    return RedirectToAction("Index", "NhanVien");
                }
                setNghiViec(obj.daNghiViec);
                setPhanQuyen(obj.quyenTruyCap);
                setGioiTinh(obj.gioiTinh);

                return View(obj);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    NhanVienManager LM = new NhanVienManager();
                    LM.SuaNhanVien(obj);
                    TempData["Success"] = "Chỉnh sửa thành công";
                    return RedirectToAction("Index", "NhanVien");
                }
                setNghiViec(obj.daNghiViec);
                setPhanQuyen(obj.quyenTruyCap);
                setGioiTinh(obj.gioiTinh);

                return View(obj);
            }
        }  //kết thúc chức năng thêm, sửa bản ghi
        //xem chi tiết nhân viên
        public ActionResult ChiTietNhanVien()
        {
            NhanVienManager NVM = new NhanVienManager();
            string maNV = Request.QueryString["maNV"].ToString();

            NhanVienData HSD = NVM.LayChiTietNV(maNV);

            if (HSD.hinhAnh != null)
            {
                ViewBag.hinhAnh = @Url.Content(HSD.hinhAnh);
            }
            else
            { ViewBag.hinhAnh = null; }

            return View(HSD);
        }
        //xóa nhân viên trong cơ sở dữ liệu
        public ActionResult XoaNhanVien(string maNV)
        {
            NhanVienManager NVM = new NhanVienManager();
            NVM.XoaNhanVien(maNV);
            return Json("0");
        }
        //kiểm tra mã nhân viên này đã tồn tại chưa
        public ActionResult IsMaNhanVienAvailble(string maNV)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    var maNhanVien = db.tbl_nhanvien.Single(m => m.MaNV == maNV);
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }////

    }
}