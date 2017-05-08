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
    public class LoaiMonAnController : Controller
    {
        // GET: nhóm thực phẩm
        public ActionResult Index()//từ khóa tìm kiếm
        {
            return View();
        }
        public ActionResult LoaiMonAnPartial(string tukhoa)
        {
            LoaiMonAnManager loaimonan = new LoaiMonAnManager();
            LoaiMonAnData data = new LoaiMonAnData();
            if (!String.IsNullOrEmpty(tukhoa))
            {
                tukhoa = tukhoa.Trim();
            }
            data.danhSachLoaiMonAn = loaimonan.LayDSLoaiMonAn(tukhoa);
            ViewBag.TuKhoaTK = tukhoa;

            return PartialView(data);
        }
        //Thêm mới một danh mục loại món ăn
        [HttpPost]
        public ActionResult ThemLoaiMonAn(LoaiMonAnData obj, string inputtext)
        {
            if (ModelState.IsValid)
            {
                LoaiMonAnManager loaimonan = new LoaiMonAnManager();
                loaimonan.ThemMoiMaLoaiMonAn(obj);
                return Json("1");
            }
            var errorList = ModelState.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
        );
            return Json(errorList);
           
        }
        //Sửa thông tin một danh mục loại món ăn
        public ActionResult SuaLoaiMonAn(int maLoaiMonAn, string tenLoaiMonAn, string ghiChu)
        {
            if (ModelState.IsValid)
            {
                LoaiMonAnManager loaimonan = new LoaiMonAnManager();
                loaimonan.SuaLoaiMonAn(maLoaiMonAn, tenLoaiMonAn, ghiChu);
                return Json("Sửa thông tin bản ghi thành công");
            }
            LoaiMonAnData obj = new LoaiMonAnData();
            obj.tenLoaiMonAn = tenLoaiMonAn;
            obj.ghiChu = ghiChu;
            return View(obj);

        }
        //Xóa bỏ một danh mục
        public ActionResult XoaLoaiMonAn(int maLoaiMonAn)
        {
            LoaiMonAnManager loaimonan = new LoaiMonAnManager();
            int kq = (int)loaimonan.XoaLoaiMonAn(maLoaiMonAn);
            return Json(kq);
        }//kết thúc xóa bỏ một danh mục loại món ăn
    }
}