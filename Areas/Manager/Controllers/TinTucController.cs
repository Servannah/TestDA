using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDA.Areas.Manager.Models.EntityManager;
using System.ComponentModel.DataAnnotations;
using TestDA.Areas.Manager.Models.ViewModel;
using TestDA.DB;
using TestDA.Common;

namespace TestDA.Areas.Manager.Controllers
{
    public class TinTucController : BaseController
    {
        // GET: Post
        public ActionResult Index(string stinhTrang, int? sdanhMuc, string SearchTerm, int page = 1, int pageSize = 10)
        {
            TinTucManager NM = new TinTucManager();
            TinTucData tintuc = new TinTucData();
            //custom phân trang
            int totalRecord = 0;
            tintuc.danhSachTin = NM.LayDSTinTuc(stinhTrang, sdanhMuc, SearchTerm,ref totalRecord, page, pageSize);
            @ViewBag.SearchTerm = SearchTerm;
            
            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize)) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            //lấy tất cả category
            NhomTinManager nhomTin = new NhomTinManager();
            ViewBag.danhSachNhomTin = new SelectList(nhomTin.LayDSNhomTin(), "maNhom", "tenNhom");
            return View(tintuc);
        }
        public void setTinNoiBat(int? selectedID = null)
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
                Value = "",
                Selected = false
            });
            ViewBag.tinNoiBat = new SelectList(SetValue, "Value", "Text", selectedID);
        }
        public ActionResult ChucNang()
        {
            //lấy thuộc tính trên đường dẫn
            string type = Request.QueryString["type"].ToString();
            if (type == "Insert")
            {
                ViewBag.ChucNang = "Thêm mới tin tức";
                setNhomTin();
                setTinNoiBat();

                ViewBag.anhDaiDien = null;
                ViewBag.type = 1;

                return View();
            }
            else
            {
                ViewBag.ChucNang = "Chỉnh sửa tin tức";
                TinTucManager tintuc = new TinTucManager();

                string maTin = Request.QueryString["maTin"].ToString();
                int id = int.Parse(maTin);
                TinTucData noidung = tintuc.LayChiTietTin(id);

                setNhomTin(noidung.nhomTin);
                setTinNoiBat(noidung.tinNoiBat);

                if (noidung.anhDaiDien != null)
                {
                    ViewBag.anhDaiDien = @Url.Content(noidung.anhDaiDien);
                }
                else
                { ViewBag.anhDaiDien = null; }

                ViewBag.type = 2;

                return View(noidung);
            }
        }
        [HttpPost]
        public ActionResult ChucNang(TinTucData obj)
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                if (ModelState.IsValid)//trả về false
                {
                    TinTucManager NM = new TinTucManager();
                    //lấy id người đăng nhập
                    string tenDangNhap = User.Identity.Name;
                    NM.ThemMoiTinTuc(obj, tenDangNhap);
                    TempData["Success"] = "Thêm mới thành công";
                    return RedirectToAction("Index", "TinTuc");
                }
                setNhomTin(obj.nhomTin);
                setTinNoiBat(obj.tinNoiBat);
                return View(obj);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    TinTucManager NM = new TinTucManager();
                    //lấy id người đăng nhập
                    string tenDangNhap = User.Identity.Name;
                    NM.SuaTinTuc(obj, tenDangNhap);
                    TempData["Success"] = "Chỉnh sửa thành công";
                    return RedirectToAction("Index", "TinTuc");
                }
                setNhomTin(obj.nhomTin);
                setTinNoiBat(obj.tinNoiBat);
                return View(obj);
            }
        }
        public void setNhomTin(int? selectedId = null)
        {
            NhomTinManager nhomtin = new NhomTinManager();
            ViewBag.nhomTin = new SelectList(nhomtin.LayDSNhomTin(), "maNhom", "tenNhom", selectedId);
        }
        //Xóa bản ghi đã chọn
        public ActionResult XoaTinTuc(int maTin)
        {
            TinTucManager tintuc = new TinTucManager();
            tintuc.XoaTinTuc(maTin);
            return Json("Xóa bản ghi thành công!");
        }
        //Xóa danh sách bản ghi đã chọn
        public ActionResult XoaTinDaChon(int[] newsIDs)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                foreach (int maTin in newsIDs)
                {
                    tbl_tintuc obj = db.tbl_tintuc.Find(maTin);
                    db.tbl_tintuc.Remove(obj);
                }
                db.SaveChanges();
                return Json("Xóa thành công những bản ghi được chọn!");
            }
        }
        //hiển thị slug khi gõ tên
        public ActionResult Slug(string tieuDeTin)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                string slug = CreateSlug.GenerateSlug(tieuDeTin);
                return Json(slug);
            }

        }
    }
}