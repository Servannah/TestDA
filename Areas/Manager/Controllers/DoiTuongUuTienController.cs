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
    public class DoiTuongUuTienController : BaseController
    {
        // GET: Manager/DoiTuongUuTien
         [AuthorizeRoles("Admin", "HieuTruong", "VanThu")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DoiTuongUuTienPartial(string tukhoa)
        {
            DoiTuongUuTienManager DTUT = new DoiTuongUuTienManager();
            DoiTuongUuTienData data = new DoiTuongUuTienData();
            if (!String.IsNullOrEmpty(tukhoa))
            {
                tukhoa = tukhoa.Trim();
            }
            data.danhSachUT = DTUT.LayDSLoaiUT(tukhoa);
            ViewBag.TuKhoaTK = tukhoa;

            return PartialView(data);
        }
        //thực hiện chức năng thêm, sửa bản ghi
        public ActionResult ChucNang()
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                ViewBag.ChucNang = "Thêm mới loại đối tượng ưu tiên";
                return View();
            }
            else
            {
                ViewBag.ChucNang = "Chỉnh sửa thông tin đối tượng ưu tiên";
                DoiTuongUuTienManager LM = new DoiTuongUuTienManager();
                string maUuTien = Request.QueryString["maUuTien"].ToString();
                int id = int.Parse(maUuTien);

                DoiTuongUuTienData DTUT = LM.LayChiTietDT(id);

                return View(DTUT);
            }
        }
        //thực hiện chức năng thêm, sửa bản ghi
        [HttpPost]
        public ActionResult ChucNang(DoiTuongUuTienData obj)
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                if (ModelState.IsValid)//trả về false
                {
                    DoiTuongUuTienManager DUM = new DoiTuongUuTienManager();
                    DUM.ThemMoiLoaiDT(obj);

                    TempData["Success"] = "Thêm mới thành công";

                    return RedirectToAction("Index", "DoiTuongUuTien");
                }
                return View(obj);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    DoiTuongUuTienManager DUM = new DoiTuongUuTienManager();
                    DUM.SuaLoaiDT(obj);
                    TempData["Success"] = "Chỉnh sửa thành công";
                    return RedirectToAction("Index", "DoiTuongUuTien");
                }
                return View(obj);
            }
        }  //kết thúc chức năng thêm , sửa bản ghi      
        //Xóa bỏ một bản ghi
        public ActionResult XoaDTUuTien(int maUuTien)
        {
            DoiTuongUuTienManager DUM = new DoiTuongUuTienManager();
            int kq = (int)DUM.XoaLoaiDT(maUuTien);

            return Json(kq);
        }//kết thúc xóa bỏ bản ghi
    }
}