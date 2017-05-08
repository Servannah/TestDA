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
    public class NhomLopController : Controller
    {
        // GET: Manager/NhomLop
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult NhomLopPartial(string tukhoa)
        {
            NhomLopManager nhomlop = new NhomLopManager();
            NhomLopData data = new NhomLopData();
            if (!String.IsNullOrEmpty(tukhoa))
            {
                tukhoa = tukhoa.Trim();
            }
            data.danhSachNhomLop = nhomlop.LayDSNhomLop(tukhoa);
            ViewBag.TuKhoaTK = tukhoa;

            return PartialView(data);
        }
        //tạo partial view hiển thị danh sách lớp thep nhóm lớp
        public ActionResult DSNhomLopPartial()
        {
            NhomLopManager nhomlop = new NhomLopManager();
            NhomLopData data = new NhomLopData();
            data.danhSachNhomLop = nhomlop.ListNhomLop();

            return PartialView(data);
        }
        //Thêm mới một danh mục 
        [HttpPost]
        public ActionResult ThemNhomLop(NhomLopData obj)
        {
            if (ModelState.IsValid)
            {
                NhomLopManager nhomlop = new NhomLopManager();
                nhomlop.ThemMoiNhomLop(obj);
                return Json("1");
            }
            var errorList = ModelState.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
        );
            return Json(errorList);

        }
        //Sửa thông tin một danh mục bản ghi
        public ActionResult SuaNhomLop(int maNhomLop, string tenNhomLop, string ghiChu)
        {
            if (ModelState.IsValid)
            {
                NhomLopManager nhomlop = new NhomLopManager();
                nhomlop.SuaNhomLop(maNhomLop, tenNhomLop, ghiChu);
                return Json("Sửa thông tin bản ghi thành công");
            }
            NhomLopData obj = new NhomLopData();
            obj.tenNhomLop = tenNhomLop;
            obj.ghiChu = ghiChu;
            return View(obj);

        }
        //Xóa bỏ một danh mục
        public ActionResult XoaNhomLop(int maNhomLop)
        {
            NhomLopManager nhomlop = new NhomLopManager();
            int kq = (int)nhomlop.XoaNhomLop(maNhomLop);
            return Json(kq);
        }//kết thúc xóa bỏ một danh mục loại món ăn
    }
}