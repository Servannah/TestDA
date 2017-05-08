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
    public class ConfigController : Controller
    {
        // GET: Manager/Config
        public ActionResult Index()
        {
            return View();
        }
        public void setShow(int? selectedID = null)
        {
            List<SelectListItem> SetValue = new List<SelectListItem>();
            SetValue.Add(new SelectListItem()
            {
                Text = "Có",
                Value = "1",
                Selected = false
            });
            SetValue.Add(new SelectListItem()
            {
                Text = "Không",
                Value = "2",
                Selected = false
            });
            ViewBag.ord = new SelectList(SetValue, "Value", "Text", selectedID);
        }
        public ActionResult ThemeIndex()
        {
            ConfigManager LM = new ConfigManager();
            ConfigData data = new ConfigData();

            string type = Request.QueryString["type"].ToString();
            if (type == "slide")
            {
                ViewBag.loai = 1;
                ViewBag.header = "Slide";
                ViewBag.add = "Thêm mới hình ảnh";
            }
            if (type == "info")
            {
                ViewBag.loai = 2;
                ViewBag.header = "Thông tin";
                ViewBag.add = "Thêm mới thông tin";
            }
            if (type == "class")
            {
                ViewBag.loai = 3;
                ViewBag.header = "Lớp học";
                ViewBag.add = "Thêm mới thông tin";
            }
            data.listData = LM.ListDataType(type);

            return View(data);
        }
        //lấy danh sách slide hiển thị
        public ActionResult SlideShow()
        {
            ConfigManager LM = new ConfigManager();
            ConfigData data = new ConfigData();

            string type = "slide";
            data.listData = LM.ListDataShow(type);

            return PartialView(data);
        }
        //thêm mới một slide
        public ActionResult EditTheme()
        {
            string t = Request.QueryString["t"].ToString();//lấy thuộc tính type trong đường dẫn
            string type = Request.QueryString["type"].ToString();
            if (t == "Insert")
            {
                ViewBag.ChucNang = "Thêm mới thông tin";
                ViewBag.image = null;
                if (type == "slide")
                {
                    ViewBag.type = 1;
                    ViewBag.header = "Slide";
                }
                if (type == "info")
                {
                    ViewBag.type = 2;
                    ViewBag.header = "Thông tin";
                }
                if (type == "class")
                {
                    ViewBag.type = 3;
                    ViewBag.header = "Lớp học";
                }
                setShow();
                return View();
            }
            else
            {
                ViewBag.ChucNang = "Chỉnh sửa thông tin";
                ConfigManager LM = new ConfigManager();
                string id = Request.QueryString["id"].ToString();
                int ma = int.Parse(id);
                ConfigData LD = LM.GetData(ma);
                setShow(LD.ord);
                if (LD.image != null)
                {
                    ViewBag.image = @Url.Content(LD.image);
                }
                else
                { ViewBag.image = null; }

                if (type == "slide")
                {
                    ViewBag.type = 1;
                    ViewBag.header = "Slide";
                }
                if (type == "info")
                {
                    ViewBag.type = 2;
                    ViewBag.header = "Thông tin";
                }
                if (type == "class")
                {
                    ViewBag.type = 3;
                    ViewBag.header = "Lớp học";
                }
                return View(LD);
            }
        }
        [HttpPost]
        public ActionResult EditTheme(ConfigData obj)
        {
            string t = Request.QueryString["t"].ToString();//lấy thuộc tính type trong đường dẫn
            string type = Request.QueryString["type"].ToString();
            if (t == "Insert")
            {
                if (ModelState.IsValid)//trả về false
                {
                    ConfigManager HSM = new ConfigManager();
                    HSM.AddData(obj);

                    TempData["Success"] = "Thêm mới thành công";
                    if (type == "slide")
                    {
                        return RedirectToAction("ThemeIndex", "Config", new { type = "slide" });
                    }
                    if(type=="info")
                    {
                        return RedirectToAction("ThemeIndex", "Config", new { type = "info" });
                    }
                    if (type == "class")
                    {
                        return RedirectToAction("ThemeIndex", "Config", new { type = "class" });
                    }

                }
                setShow(obj.ord);
                return View(obj);

            }
            else
            {
                if (ModelState.IsValid)
                {
                    ConfigManager HSM = new ConfigManager();
                    HSM.UpdateData(obj);
                    TempData["Success"] = "Chỉnh sửa thành công";

                    if (type == "slide")
                    {
                        return RedirectToAction("ThemeIndex", "Config", new { type = "slide" });
                    }
                    if(type=="info")
                    {
                        return RedirectToAction("ThemeIndex", "Config", new { type = "info" });
                    }
                    if(type=="class")
                    {
                        return RedirectToAction("ThemeIndex", "Config", new { type = "class" });
                    }
                }
                setShow(obj.ord);
                return View(obj);
            }
        }  //kết thúc chức năng thêm , sửa bản ghi 
        //lấy thông tin giáo viên đang làm việc
        public ActionResult StaffIndex()
        {
            ConfigManager LM = new ConfigManager();
            ConfigData data = new ConfigData();
            string type = "staff";
            data.listData = LM.ListDataType(type);
            return View(data);
        }
        //lấy danh sách nhân viên 
        public void setNhanVien(string selectedID = null)
        {
            NhanVienManager NVM = new NhanVienManager();
            ViewBag.maNV = new SelectList(NVM.DSNhanVienLamViec(), "maNV", "hoTen", selectedID);
        }
        //lấy thông tin nhân viên
        public ActionResult GetDataStaff(string maNV)
        {
            NhanVienManager LM = new NhanVienManager();
            NhanVienData data = LM.LayChiTietNV(maNV);
            return Json(data);
        }
        public ActionResult EditStaff()
        {
            string t = Request.QueryString["t"].ToString();//lấy thuộc tính type trong đường dẫn
            if (t == "Insert")
            {
                ViewBag.ChucNang = "Thêm mới thông tin";
                ViewBag.image = null;
                setShow();
                setNhanVien();
                return View();
            }
            else
            {
                ViewBag.ChucNang = "Chỉnh sửa thông tin";
                ConfigManager LM = new ConfigManager();
                string id = Request.QueryString["id"].ToString();
                int ma = int.Parse(id);
                ConfigData LD = LM.GetData(ma);
                setShow(LD.ord);
                setNhanVien();
                if (LD.image != null)
                {
                    ViewBag.image = @Url.Content(LD.image);
                }
                else
                { ViewBag.image = null; }

                return View(LD);
            }
        }
        [HttpPost]
        public ActionResult EditStaff(ConfigData obj)
        {
            string t = Request.QueryString["t"].ToString();//lấy thuộc tính type trong đường dẫn
            if (t == "Insert")
            {
                if (ModelState.IsValid)//trả về false
                {
                    ConfigManager HSM = new ConfigManager();
                    HSM.AddData(obj);
                    TempData["Success"] = "Thêm mới thành công";
                    return RedirectToAction("StaffIndex", "Config");
                }
                setNhanVien();
                setShow(obj.ord);
                return View(obj);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    ConfigManager HSM = new ConfigManager();
                    HSM.UpdateData(obj);
                    TempData["Success"] = "Chỉnh sửa thành công";
                    return RedirectToAction("StaffIndex", "Config");
                }
                setNhanVien();
                setShow(obj.ord);
                return View(obj);
            }
        }  //kết thúc chức năng thêm , sửa bản ghi
        //lựa chọn tin hiển thị chính 
        public ActionResult NewsIndex()
        {            
            ConfigManager CM = new ConfigManager();
            ConfigData data = CM.GetNewsHot();
            if (data.image != null)
            {
                ViewBag.image = @Url.Content(data.image);
            }
            else
            { ViewBag.image = null; }

            return View(data);
        }
        [HttpPost]
        public ActionResult NewsIndex(ConfigData obj)
        {
            if (ModelState.IsValid)
            {
                ConfigManager HSM = new ConfigManager();
                HSM.UpdateData(obj);
                TempData["Success"] = "Chỉnh sửa thành công";
                return RedirectToAction("NewsIndex", "Config");
            }

            return View(obj);
        }  
        //tạo partal view lấy danh sách tin tức nổi bật
        public ActionResult NewsHotPartial()
        {
            //lấy danh sách tin nổi bật
            TinTucManager news = new TinTucManager();
            TinTucData data = new TinTucData();
            data.danhSachTin = news.LayDSTinTucNoiBat();

            return PartialView(data);
        }
        //lấy thông tin tin tức được chọn
        public ActionResult GetDataNews(int maTin)
        {
            TinTucManager LM = new TinTucManager();
            TinTucData data = LM.LayChiTietTin(maTin);
            return Json(data);
        }
        //cài đặt menu
        //lấy các page trong bảng option
        public ActionResult GetPagePartial()
        {
            OptionManager OM = new OptionManager();
            string type = "page";
            OptionData data = new OptionData();
            data.danhSachDuLieu= OM.ListData(type);
            return PartialView(data);
        }
        //lấy danh sách danh mục 
        public ActionResult GetItemPartial()
        {
            NhomTinManager NTM = new NhomTinManager();
            NhomTinData data = new NhomTinData();
            data.danhSachNhomTin = NTM.LayDSNhomTin();
            return View(data);
        }
        public ActionResult MenuIndex()
        {
            return View();
        }
        //lấy danh sách danh mục trong cơ sở dữ liệu
        public void setClass(int? selectedID = null)
        {
            NhomTinManager NTM = new NhomTinManager();
            ViewBag.nhomTin = new SelectList(NTM.LayDSNhomTin(), "maNhom", "tenNhom", selectedID);
        }
        //xóa một dữ liệu
        public ActionResult DeleteData(int id)
        {
            ConfigManager LM = new ConfigManager();
            LM.DeleteData(id);
            TempData["Success"] = "Xóa thông tin thành công";
            return Json("0");
        }
        ///
    }
}