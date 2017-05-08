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
    public class ThucDonController : Controller
    {
        // GET: Manager/ThucDon
        public ActionResult Index(DateTime? ngayLap, string maNhanVien, int page = 1, int pageSize = 20)
        {
            //custom phân trang
            int totalRecord = 0;
            //lấy danh sách thực đơn
            ThucDonManager LM = new ThucDonManager();
            ThucDonData data = new ThucDonData();
            data.danhSachTD = LM.LayDSTD(ngayLap, maNhanVien, ref totalRecord, page, pageSize);
            ViewBag.ngayLap = ngayLap;
            ViewBag.nguoiLap = maNhanVien;

            setNhanVien();
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
        //lấy danh sách nhân viên
        public void setNhanVien(string selectedID = null)
        {
            NhanVienManager nhanvien = new NhanVienManager();
            ViewBag.nguoiLap = new SelectList(nhanvien.ListNhanVien(), "maNV", "maHoTen", selectedID);
        }

        //thêm mới thực đơn
        public ActionResult ChucNang()
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                ViewBag.ChucNang = "Thêm mới thực đơn";
                setNhanVien();
                return View();
            }
            else
            {
                ViewBag.ChucNang = "Chỉnh sửa thông tin thực đơn";
                ThucDonManager TDM = new ThucDonManager();
                string maThucDon = Request.QueryString["maTD"].ToString();
                int id = int.Parse(maThucDon);
                ThucDonData LD = TDM.LayChiTietThucDon(id);
                setNhanVien(LD.nguoiLap);

                return View(LD);
            }
           
        }
        [HttpPost]
        public ActionResult ChucNang(ThucDonData obj)
        {
             string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
             if (type == "Insert")
             {
                 if (ModelState.IsValid)//trả về false
                 {
                     ThucDonManager LM = new ThucDonManager();
                     LM.ThemThucDon(obj);
                     TempData["Success"] = "Thêm mới thành công";
                     return RedirectToAction("Index", "ThucDon");
                 }
                 setNhanVien();
                 return View(obj);
             }
             else
             {
                 if (ModelState.IsValid)//trả về false
                 {
                     ThucDonManager LM = new ThucDonManager();
                     LM.SuaThucDon(obj);
                     TempData["Success"] = "Sửa bản ghi thành công";
                     return RedirectToAction("Index", "ThucDon");
                 }
                 setNhanVien(obj.nguoiLap);
                 return View(obj);
             }
        } ////
        //xóa bỏ một thực đơn
        public ActionResult XoaThucDon(int maThucDon)
        {
            ThucDonManager LM = new ThucDonManager();
            int kq = (int)LM.XoaThucDon(maThucDon);
            return Json(kq);
        }//kết thúc xóa bỏ bản ghi
        ////
    }
}