//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using TestDA.DB;
//using TestDA.Areas.Manager.Models.EntityManager;
//using TestDA.Areas.Manager.Models.ViewModel;
//using System.ComponentModel.DataAnnotations;
//using System.Data;

//namespace TestDA.Areas.Manager.Controllers
//{
//    public class NhaCungCapController : Controller
//    {
//        // GET: nhà cung cấp
//        public ActionResult Index(string tukhoa)//từ khóa tìm kiếm
//        {
//            NhaCungCapManager nhaCC = new NhaCungCapManager();
//            NhaCungCapData data = new NhaCungCapData();
//            if (!String.IsNullOrEmpty(tukhoa))
//            {
//                tukhoa = tukhoa.Trim();
//            }
//            data.danhSachNhaCungCap = nhaCC.LayDSNhaCungCap(tukhoa);
//            ViewBag.TKTimKiem = tukhoa;

//            return View(data);
//        }
//        //public ActionResult NhaCCPartial(string tukhoa)
//        //{
           
//        //}
//        //thực hiện chức năng thêm, sửa bản ghi
//        public ActionResult ChucNang()
//        {
//            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
//            if (type == "Insert")
//            {
//                ViewBag.ChucNang = "Thêm mới nhà cung cấp";
//                return View();
//            }
//            else
//            {
//                ViewBag.ChucNang = "Chỉnh sửa thông tin nhà cung cấp";
//                NhaCungCapManager nhaCC = new NhaCungCapManager();
//                string maNCC = Request.QueryString["manhacc"].ToString();
//                int id = int.Parse(maNCC);

//                NhaCungCapData NCC = nhaCC.LayChiTietNhaCC(id);
//                return View(NCC);
//            }
//        }
//        //thực hiện chức năng thêm, sửa bản ghi
//        [HttpPost]
//        public ActionResult ChucNang(NhaCungCapData obj)
//        {
//            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
//            if (type == "Insert")
//            {
//                if (ModelState.IsValid)
//                {
//                    NhaCungCapManager nhaCC = new NhaCungCapManager();
//                    nhaCC.ThemMoiNhaCC(obj);
//                    TempData["Success"] = "Thêm mới thành công";
//                    return RedirectToAction("Index", "NhaCungCap");
//                }
//                return View(obj);
//            }
//            else
//            {
//                if (ModelState.IsValid)
//                {
//                    NhaCungCapManager nhaCC = new NhaCungCapManager();
//                    nhaCC.SuaNhaCC(obj);
//                    TempData["Success"] = "Chỉnh sửa thành công";
//                    return RedirectToAction("Index", "NhaCungcap");
//                }
//                return View(obj);
//            }
//        }        
//        //Xóa bỏ một danh mục
//        public ActionResult XoaNhaCC(int maNhaCC)
//        {
//            NhaCungCapManager nhaCC = new NhaCungCapManager();
//            int kq = (int)nhaCC.XoaNhaCC(maNhaCC);
//            return Json(kq);
//        }//kết thúc xóa bỏ nhà cung cấp
//    }
//}