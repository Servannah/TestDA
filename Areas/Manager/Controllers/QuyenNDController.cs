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
    public class QuyenNDController : BaseController
    {
        // GET: Manager/QuyenND
        [AuthorizeRoles("Admin")]
        public ActionResult Index()
        {
            QuyenNDManager DTUT = new QuyenNDManager();
            QuyenNDData data = new QuyenNDData();
            data.danhSachQuyen = DTUT.LayDSQuyen();
            return View(data);
        }
        [AuthorizeRoles("Admin")]
        //thực hiện chức năng thêm, sửa bản ghi
        public ActionResult ChucNang()
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                ViewBag.ChucNang = "Thêm mới quyền người dùng";
                return View();
            }
            else
            {
                ViewBag.ChucNang = "Chỉnh sửa thông tin";
                QuyenNDManager LM = new QuyenNDManager();
                string maPQ = Request.QueryString["maPQ"].ToString();
                int id = int.Parse(maPQ);

                QuyenNDData DTUT = LM.LayChiTietQuyen(id);

                return View(DTUT);
            }
        }
        //thực hiện chức năng thêm, sửa bản ghi
        [HttpPost]
        public ActionResult ChucNang(QuyenNDData obj)
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                if (ModelState.IsValid)//trả về false
                {
                    QuyenNDManager DUM = new QuyenNDManager();
                    DUM.ThemMoiQuyen(obj);

                    TempData["Success"] = "Thêm mới thành công";

                    return RedirectToAction("Index", "QuyenND");
                }
                return View(obj);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    QuyenNDManager DUM = new QuyenNDManager();
                    DUM.SuaQuyen(obj);
                    TempData["Success"] = "Chỉnh sửa thành công";
                    return RedirectToAction("Index", "QuyenND");
                }
                return View(obj);
            }
        }  //kết thúc chức năng thêm , sửa bản ghi      
        //Xóa bỏ một bản ghi
        [AuthorizeRoles("Admin")]
        public ActionResult XoaQuyen(int maPQ)
        {
            QuyenNDManager DUM = new QuyenNDManager();
            int kq = (int)DUM.XoaQuyen(maPQ);

            return Json(kq);
        }//kết t
        ///
    }
}