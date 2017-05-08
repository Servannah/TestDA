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
    public class TPXuatKhoController : Controller
    {
        // GET: Manager/TPXuatKho
        public ActionResult Index(DateTime? ngayXK, string tukhoa, int page = 1, int pageSize = 20)
        {
            TPXuatKhoManager LM = new TPXuatKhoManager();
            TPXuatKhoData data = new TPXuatKhoData();
            //thực hiện phân trang
            //custom phân trang
            int totalRecord = 0;

            if (!String.IsNullOrEmpty(tukhoa))
            {
                tukhoa = tukhoa.Trim();
            }

            data.danhSachTPXuatKho = LM.LayDSTP(ngayXK, tukhoa, ref totalRecord, page, pageSize);
            ViewBag.TuKhoaTK = tukhoa;
            ViewBag.ngayXuatKho = ngayXK;

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
        //lấy từ danh sách thực phẩm trong kho
        public void setThucPham(int? selectedID = null)
        {
            TPNhapKhoManager TPM = new TPNhapKhoManager();
            ViewBag.maThucPham = new SelectList(TPM.ListTP(), "maThucPham", "tenThucPham", selectedID);
        }
        //thực hiện chức năng thêm, sửa bản ghi
        public ActionResult ChucNang()
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                ViewBag.ChucNang = "Thêm mới thực phẩm";
                setThucPham();

                return View();
            }
            else
            {
                ViewBag.ChucNang = "Chỉnh sửa thông tin thực phẩm";

                TPXuatKhoManager TPK = new TPXuatKhoManager();
                string maXuatKho = Request.QueryString["maXuatKho"].ToString();
                int id = int.Parse(maXuatKho);
                TPXuatKhoData TKD = TPK.LayChiTietTP(id);

                setThucPham(TKD.maThucPham);

                return View(TKD);
            }
        }
        //thực hiện chức năng thêm, sửa bản ghi
        [HttpPost]
        public ActionResult ChucNang(TPXuatKhoData obj)
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                if (ModelState.IsValid)//trả về false
                {
                    TPXuatKhoManager TNK = new TPXuatKhoManager();
                    TNK.ThemTPXuatKho(obj);
                    TempData["Success"] = "Thêm mới thành công";
                    return RedirectToAction("Index", "TPXuatKho");
                }

                setThucPham(obj.maThucPham);
                return View(obj);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    TPXuatKhoManager TNM = new TPXuatKhoManager();
                    TNM.SuaTPXuatKho(obj);
                    TempData["Success"] = "Chỉnh sửa thành công";
                    return RedirectToAction("Index", "TPXuatKho");
                }

                setThucPham(obj.maThucPham);
                return View(obj);
            }
        }  //kết thúc chức năng thêm , sửa bản ghi      
        //Xóa bỏ một bản ghi
        public ActionResult XoaTPXuatKho(int maXuatKho)
        {
            TPNhapKhoManager LM = new TPNhapKhoManager();
            int kq = (int)LM.XoaTPKho(maXuatKho);
            return Json(kq);
        }//kết thúc xóa bỏ bản ghi
    }
}