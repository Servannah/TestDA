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
    public class LopController : BaseController
    {
        // GET: Manager/Lop
        [AuthorizeRoles("Admin", "HieuTruong", "VanThu")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LopPartial(string tukhoa,int? nhomLop)
        {
            LopManager LM = new LopManager();
            LopData data = new LopData();
            if (!String.IsNullOrEmpty(tukhoa))
            {
                tukhoa = tukhoa.Trim();
            }
            data.danhSachLop = LM.LayDSLop(tukhoa,nhomLop);
            ViewBag.TuKhoaTK = tukhoa;
            setSelect();

            return PartialView(data);
        }
        //tạo partialView hiển thị lớp theo  nhóm lớp
        public ActionResult LopThemNhomLopPartial(int maNhomLop)
        {
            LopManager lop = new LopManager();
            LopData data = new LopData();
            data.danhSachLop = lop.ListLopTheoNhomLop(maNhomLop);
            return PartialView(data);
        }
        public void setSelect(int? selectedId = null)
        {
           //lấy id, tên nhóm lớp
            NhomLopManager NLM = new NhomLopManager();
            ViewBag.nhomLop = new SelectList(NLM.ListNhomLop(), "maNhomLop", "tenNhomLop", selectedId);
        }
        //thực hiện chức năng thêm, sửa bản ghi
        public ActionResult ChucNang()
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                ViewBag.ChucNang = "Thêm mới lớp học";
                setSelect();
                return View();
            }
            else
            {
                ViewBag.ChucNang = "Chỉnh sửa thông tin lớp học";
                LopManager LM = new LopManager();
                string maLop = Request.QueryString["maLop"].ToString();
                int id = int.Parse(maLop);
                LopData LD = LM.LayChiTietLop(id);

                setSelect(LD.nhomLop);

                return View(LD);
            }
        }
        //thực hiện chức năng thêm, sửa bản ghi
        [HttpPost]
        public ActionResult ChucNang(LopData obj)
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                if (ModelState.IsValid)//trả về false
                {
                    LopManager LM = new LopManager();
                    LM.ThemMoiLop(obj);
                    TempData["Success"] = "Thêm mới thành công";
                    return RedirectToAction("Index", "Lop");
                }
                setSelect();
                return View(obj);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    LopManager LM = new LopManager();
                    LM.SuaLop(obj);
                    TempData["Success"] = "Chỉnh sửa thành công";
                    return RedirectToAction("Index", "Lop");
                }
                setSelect(obj.nhomLop);
                return View(obj);
            }
        }  //kết thúc chức năng thêm , sửa bản ghi      
        //Xóa bỏ một bản ghi
        public ActionResult XoaLop(int maLop)
        {
            LopManager LM = new LopManager();
            int kq = (int)LM.XoaLop(maLop);
            return Json(kq);
        }//kết thúc xóa bỏ bản ghi
    }
}