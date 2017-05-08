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
    public class MonAnController : Controller
    {
        // GET: Manager/MonAn
        public ActionResult Index(string tukhoa, int page = 1, int pageSize = 20)
        {
            MonAnManager thucpham = new MonAnManager();
            MonAnData data = new MonAnData();
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
            data.danhSachMonAn = thucpham.LayDSMonAn(tukhoa, ref totalRecord, page, pageSize);

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
            //set loại món ăn
            setLoaiMonAn();

            return View(data);
        }///
        //load thành phần món ăn
        public ActionResult TPMonAnPartial(int maMonAn)
        {
            MonAnManager monan = new MonAnManager();
            MonAnData data = new MonAnData();
            //lấy 
            TPMonAn tp = new TPMonAn();
            tp.danhSachTPMonAn = monan.danhSachTPMonAn(maMonAn);
            //data.thanhPhanMonAn = tp.danhSachTPMonAn;
            return PartialView(tp);
        }///
        //set danh sách loại món ăn
        public void setLoaiMonAn(int? selectedId = null)
        {
            //lấy id, tên loại món ăn+
            LoaiMonAnManager NLM = new LoaiMonAnManager();
            ViewBag.maLoaiMonAn = new SelectList(NLM.ListLoaiMA(), "maLoaiMonAn", "tenLoaiMonAn", selectedId);
        }
        //set danh sách thực phẩm
        public void setThucPham(int? selectedId = null)
        {
            //lấy id, tên loại món ăn+
            ThucPhamManager NLM = new ThucPhamManager();
            ViewBag.maThucPham = new SelectList(NLM.ListThucPham(), "maThucPham", "tenThucPham", selectedId);
        }
        //thực hiện chức năng thêm bản ghi
        [HttpPost]
        public ActionResult ThemMonAn(MonAnData obj)
        {
            MonAnManager MA = new MonAnManager();
            MA.ThemMoiMonAn(obj);
            TempData["Success"] = "Thêm mới thành công";

            return RedirectToAction("Index", "MonAn");
        }
        //lấy danh sách 
        //lấy thông tin bản ghi cần sửa
        public ActionResult SuaMonAn()
        {
            ViewBag.ChucNang = "Chỉnh sửa thông tin món ăn";
            MonAnManager LM = new MonAnManager();
            string maMonAn = Request.QueryString["maMonAn"].ToString();
            int id = int.Parse(maMonAn);
            MonAnData LD = LM.LayChiTietMonAn(id);
            //load thành phần món ăn
            TPMonAn tp = new TPMonAn();
            LD.thanhPhanMonAn = LM.danhSachTPMonAn(id);

            setLoaiMonAn(LD.maLoaiMonAn);
            setThucPham();

            return View(LD);
        }
        [HttpPost]
        public ActionResult SuaMonAn(MonAnData obj)
        {
            MonAnManager MA = new MonAnManager();
            MA.SuaMonAn(obj);
            TempData["Success"] = "Chỉnh sửa thành công";
            return RedirectToAction("Index", "MonAn");
        }
        //Xóa bỏ một bản ghi
        public ActionResult XoaMonAn(int maMonAn)
        {
            MonAnManager LM = new MonAnManager();
            int kq = (int)LM.XoaMonAn(maMonAn);
            TempData["Success"] = "Xóa bản ghi thành công";
            return Json(kq);
        }//
        //thêm thành phần món ăn vào bảng dữ liệu
        public ActionResult ThemTPMonAn(int maMonAn, int maThucPham, double soLuongGam)
        {
            MonAnManager MA = new MonAnManager();
            MA.ThemTPMonAn(maMonAn, maThucPham, soLuongGam);
            return RedirectToAction("SuaTPMA", "MonAn", new {type="Edit",maMonAn=maMonAn});

        }
        //chỉnh sửa thông tin thành phần món ăn
        public ActionResult SuaTPMonAn(int maMonAnCT, int maThucPham, double soLuongGam)
        {
            MonAnManager MA = new MonAnManager();
            MA.SuaTPMonAn(maMonAnCT, maThucPham, soLuongGam);
            return Json("0");
        }
        //lấy thông tin vào SuaTPMA
        public ActionResult SuaTPMA()
        {
            MonAnManager monan = new MonAnManager();
            MonAnData data = new MonAnData();
            string maMonAn = Request.QueryString["maMonAn"].ToString();
            int id = int.Parse(maMonAn);
            TPMonAn tp = new TPMonAn();
            tp.danhSachTPMonAn = monan.danhSachTPMonAn(id);
            //lấy chi tiết món ăn
            ViewBag.tenMonAn = monan.LayChiTietMonAn(id).tenMonAn;
            ViewBag.maMonAn = id;
            setThucPham();
            return View(tp);
        }
        ///
        //xóa thành phần món ăn
        public ActionResult XoaTPMonAn(int maMonAnCT)
        {
            MonAnManager MA = new MonAnManager();
           int kq =  MA.XoaTPMonAn(maMonAnCT);
            ///
           return Json(kq);

        }
        //
    }

}