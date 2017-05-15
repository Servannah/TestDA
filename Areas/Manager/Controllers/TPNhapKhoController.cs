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
    public class TPNhapKhoController : BaseController
    {
        // GET: Manager/TPNhapKho
         [AuthorizeRoles("Admin", "HieuTruong", "NhaBep")]
        public ActionResult Index(DateTime? ngayNK, string tukhoa, int page = 1, int pageSize = 20)
        {
            TPNhapKhoManager LM = new TPNhapKhoManager();
            TPNhapKhoData data = new TPNhapKhoData();
            //thực hiện phân trang
            //custom phân trang
            int totalRecord = 0;

            if (!String.IsNullOrEmpty(tukhoa))
            {
                tukhoa = tukhoa.Trim();
            }

            data.danhSachTPNhapKho = LM.LayDSTP(ngayNK, tukhoa, ref totalRecord, page, pageSize);
            ViewBag.TuKhoaTK = tukhoa;
            ViewBag.ngayNhapKho = ngayNK;

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
        public void setThucPham(int? selectedID = null)
        {
            //lấy danh sách thực phảm
            ThucPhamManager TPM = new ThucPhamManager();
            ViewBag.maThucPham = new SelectList(TPM.ListThucPham(), "maThucPham", "tenThucPham", selectedID);
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
                TPNhapKhoManager TPK = new TPNhapKhoManager();
                string maTPKho = Request.QueryString["maTPKho"].ToString();
                int id = int.Parse(maTPKho);
                TPNhapKhoData TKD = TPK.LayChiTietTP(id);

                setThucPham(TKD.maThucPham);

                return View(TKD);
            }
        }
        //thực hiện chức năng thêm, sửa bản ghi
        [HttpPost]
        public ActionResult ChucNang(TPNhapKhoData obj)
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                if (ModelState.IsValid)//trả về false
                {
                    TPNhapKhoManager TNK = new TPNhapKhoManager();
                    TNK.ThemTPNhapKho(obj);
                    TempData["Success"] = "Thêm mới thành công";
                    return RedirectToAction("Index", "TPNhapKho");
                }

                setThucPham(obj.maThucPham);
                return View(obj);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    TPNhapKhoManager TNM = new TPNhapKhoManager();
                    TNM.SuaTPKho(obj);
                    TempData["Success"] = "Chỉnh sửa thành công";
                    return RedirectToAction("Index", "TPNhapKho");
                }

                setThucPham(obj.maThucPham);
                return View(obj);
            }
        }  //kết thúc chức năng thêm , sửa bản ghi      
        //Xóa bỏ một bản ghi
        public ActionResult XoaTPNhapKho(int maTPKho)
        {
            TPNhapKhoManager LM = new TPNhapKhoManager();
            int kq = (int)LM.XoaTPKho(maTPKho);
            return Json(kq);
        }//kết thúc xóa bỏ bản ghi
    }
}