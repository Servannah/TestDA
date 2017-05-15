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
    public class HoanTraHocPhiController : BaseController
    {
        // GET: Manager/HoanTraHocPhi
        [AuthorizeRoles("Admin", "HieuTruong", "VanThu")]
        public ActionResult Index(string maHocSinh, string namHoc, int page = 1, int pageSize = 20)
        {
            //lấy ra danh sách thông tin
            HoanTraHocPhiManager HPM = new HoanTraHocPhiManager();
            HoanTraHocPhiData data = new HoanTraHocPhiData();

            //thực hiện phân trang
            //custom phân trang
            int totalRecord = 0;
            if (!String.IsNullOrEmpty(maHocSinh))
            {
                maHocSinh = maHocSinh.Trim();
            }
            data.danhSachHoanTra = HPM.DanhSachThongTin(maHocSinh, namHoc, ref totalRecord, page, pageSize);
            ViewBag.TuKhoaTK = maHocSinh;
            setYears();

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
        //set năm học
        public void setYears(string selectedID = null)
        {
            List<SelectListItem> Years = new List<SelectListItem>();
            int now = DateTime.Now.Year;
            for (int i = now; i > 2010; i--)
            {
                Years.Add(new SelectListItem()
                {
                    Text = ((i - 1) + "-" + i).ToString(),
                    Value = ((i - 1) + "-" + i).ToString(),
                    Selected = false
                });
            }
            ViewBag.namHoc = new SelectList(Years, "Value", "Text", selectedID);
        }
        //set lớp học
        public void setLop(int? selectedID = null)
        {
            //lấy danh sách lớp
            LopManager Lop = new LopManager();
            ViewBag.maLop = new SelectList(Lop.ListLop(), "maLop", "tenLop", selectedID);
        }
        //thực hiện chức năng thêm, sửa bản ghi
        public ActionResult ChucNang()
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                ViewBag.ChucNang = "Thêm mới thông tin";
                setLop();
                setYears();
                return View();
            }
            else
            {
                ViewBag.ChucNang = "Chỉnh sửa thông tin";
                HoanTraHocPhiManager LM = new HoanTraHocPhiManager();
                string maHTHP = Request.QueryString["maHTHP"].ToString();
                int id = int.Parse(maHTHP);

                HoanTraHocPhiData DTUT = LM.LayChiTiet(id);
                setLop(DTUT.maLop);
                setYears(DTUT.namHoc);
                return View(DTUT);
            }
        }
        //thực hiện chức năng thêm, sửa bản ghi
        [HttpPost]
        public ActionResult ChucNang(HoanTraHocPhiData obj)
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                if (ModelState.IsValid)//trả về false
                {
                    HoanTraHocPhiManager DUM = new HoanTraHocPhiManager();
                    DUM.ThemThongTin(obj);

                    TempData["Success"] = "Thêm mới thành công";

                    return RedirectToAction("Index", "HoanTraHocPhi");
                }
                setLop(obj.maLop);
                setYears(obj.namHoc);
                return View(obj);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    HoanTraHocPhiManager DUM = new HoanTraHocPhiManager();
                    DUM.SuaThongTin(obj);
                    TempData["Success"] = "Chỉnh sửa thành công";
                    return RedirectToAction("Index", "HoanTraHocPhi");
                }
                setLop(obj.maLop);
                setYears(obj.namHoc);
                return View(obj);
            }
        }  //kết thúc chức năng thêm , sửa bản ghi      
        //Xóa bỏ một bản ghi
        public ActionResult XoaThongTin(int maHTHP)
        {
            HoanTraHocPhiManager DUM = new HoanTraHocPhiManager();
            DUM.XoaThongTin(maHTHP);
            return Json("0");
        }//kết thuc
        ///
    }
}