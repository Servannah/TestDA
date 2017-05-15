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
    public class CTHoaDonController : Controller
    {
        // GET: Manager/Hóa đơn chi tiết
        public ActionResult Index(string tukhoa, int? maCTHD, DateTime? ngayLap, int page = 1, int pageSize = 20)
        {
            //lấy danh sách ctiết phiếu nhập kho
            PhieuHoaDonManager PHD = new PhieuHoaDonManager();

            ViewBag.maHoaDon = new SelectList(PHD.ListPHD(), "maPhieuHD", "soPhieuHD");

            CTHoaDonManager ctHD = new CTHoaDonManager();
            CTHoaDonData data = new CTHoaDonData();
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
            data.danhSachCTHD = ctHD.LayDSCTHD(tukhoa, ref totalRecord, page, pageSize);

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
        //
        //thực hiện chức năng thêm, sửa bản ghi
        //public ActionResult ChucNang()
        //{
        //    string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
        //    if (type == "Insert")
        //    {

        //        ViewBag.ChucNang = "Thêm mới thực phẩm cho hóa đơn";

        //        string maHoaDon = Request.QueryString["maHoaDon"].ToString();
        //        int id = int.Parse(maHoaDon);

        //        //lấy danh sách mã phiếu nhập kho trong cơ sở dữ liệu
        //        PhieuHoaDonManager PHD = new PhieuHoaDonManager();
        //        ViewBag.maHoaDon = new SelectList(PHD.ListPHD(), "maPhieuHD", "soPhieuHD",id);
        //        //lấy danh sách thực phẩm trong cơ sở dữ liệu
        //        ThucPhamManager thucPham = new ThucPhamManager();
        //        ViewBag.maThucPham = new SelectList(thucPham.ListThucPham(), "maThucPham", "tenThucPham");
        //        //lấy ra thông tin hóa đơn để thay đổi thực phẩm
        //        return View();
        //    }
        //    else
        //    {
        //        ViewBag.ChucNang = "Chỉnh sửa thông tin thực phẩm ";
        //        CTHoaDonManager CTHD = new CTHoaDonManager();
        //        string maHoaDon = Request.QueryString["maHoaDon"].ToString();
        //        int id = int.Parse(maHoaDon);
        //        //lấy thông tin chi tiết thực phẩm trong kho
        //        CTHoaDonData KTP = CTHD.LayChiTietHD(id);
        //        //lấy danh sách mã phiếu nhập kho trong cơ sở dữ liệu
        //        PhieuHoaDonManager PHD = new PhieuHoaDonManager();

        //        ViewBag.maHoaDon = new SelectList(PHD.ListPHD(), "maPhieuHD", "soPhieuHD", KTP.maHoaDon);
        //        //lấy danh sách thực phẩm trong cơ sở dữ liệu
        //        ThucPhamManager thucPham = new ThucPhamManager();
        //        ViewBag.maThucPhamNK = new SelectList(thucPham.ListThucPham(), "maThucPham", "tenThucPham", KTP.maThucPham);
        //        return View(KTP);
        //    }
        //}
        public ActionResult ChucNang()
        {
            ViewBag.ChucNang = "Chỉnh sửa thông tin thực phẩm";

            string maHoaDon = Request.QueryString["maPhieuHD"].ToString();
            int id = int.Parse(maHoaDon);
            ViewBag.maHoaDon = maHoaDon;
            setThucPham();

            return View();
        }
        [HttpPost]
        public ActionResult ChucNang(CTHoaDonData obj)
        {
            if (ModelState.IsValid)
            {
                CTHoaDonManager ctHD = new CTHoaDonManager();
                ctHD.ThemMoiTP(obj);
                //TempData["Success"] = "Thêm mới thành công";
                return RedirectToAction("ChucNang", "CTHoaDon", new { type = "Insert", maPhieuHD = obj.maHoaDon });
            }
            //lấy danh sách thực phẩm trong cơ sở dữ liệu
            setThucPham(obj.maThucPham);
            ViewBag.maHoaDon = obj.maHoaDon;

            return View(obj);
        }
        public void setHoaDon(int? selectedID = null)
        {
            //lấy danh sách mã phiếu nhập kho trong cơ sở dữ liệu
            PhieuHoaDonManager PHD = new PhieuHoaDonManager();
            ViewBag.maHoaDon = new SelectList(PHD.ListPHD(), "maPhieuHD", "soPhieuHD", selectedID);
        }

        public void setThucPham(int? selectedID = null)
        {
            //lấy danh sách thực phẩm trong cơ sở dữ liệu
            ThucPhamManager thucPham = new ThucPhamManager();
            ViewBag.maThucPham = new SelectList(thucPham.ListThucPham(), "maThucPham", "tenThucPham", selectedID);
        }
        //thực hiện chức năng thêm, sửa bản ghi
        //[HttpPost]
        //public ActionResult ChucNang(CTHoaDonData obj)
        //{
        //    string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
        //    if (type == "Insert")
        //    {
        //        if (ModelState.IsValid)//trả về false
        //        {
        //            CTHoaDonManager CTHD = new CTHoaDonManager();
        //            CTHD.ThemMoiTP(obj);
        //            TempData["Success"] = "Thêm mới thành công";
        //            return RedirectToAction("Index", "CThoaDon");
        //        }
        //        //lấy danh sách mã phiếu nhập kho trong cơ sở dữ liệu
        //        PhieuHoaDonManager PHD = new PhieuHoaDonManager();
        //        ViewBag.maHoaDon = new SelectList(PHD.ListPHD(), "maPhieuHD", "soPhieuHD", obj.maHoaDon);
        //        setThucPham(obj.maThucPham);
        //        return View(obj);
        //    }
        //    else
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            CTHoaDonManager ctHD = new CTHoaDonManager();
        //            ctHD.SuaHDCT(obj);
        //            TempData["Success"] = "Chỉnh sửa thành công";
        //            return RedirectToAction("Index", "CTHoaDon");
        //        }
        //        //lấy danh sách mã phiếu nhập kho trong cơ sở dữ liệu
        //        PhieuHoaDonManager PHD = new PhieuHoaDonManager();

        //        ViewBag.maHoaDon = new SelectList(PHD.ListPHD(), "maPhieuHD", "soPhieuHD", obj.maHoaDon);
        //        //lấy danh sách thực phẩm trong cơ sở dữ liệu
        //        ThucPhamManager thucPham = new ThucPhamManager();
        //        ViewBag.maThucPhamNK = new SelectList(thucPham.ListThucPham(), "maThucPham", "tenThucPham", obj.maThucPham);
        //        return View(obj);
        //    }
        //}
        //Xóa bỏ một thực phẩm có trong kho
        public ActionResult XoaCTTP(int maHDCT)
        {
            CTHoaDonManager khoTP = new CTHoaDonManager();
            int kq = (int)khoTP.XoaCTTP(maHDCT);
            return Json(kq);
        }//kết thúc xóa bỏ phiếu nhập kho
    }
}