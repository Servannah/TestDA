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
    public class NhomTinController : BaseController
    {
        public void SetViewBag(int? selectedId = null)
        {
            NhomTinManager nhomtin = new NhomTinManager();
            ViewBag.danhMucCha = new SelectList(nhomtin.LayDSNhomTin(), "maNhom", "tenNhom", selectedId);
        }
        //tao partial hiển thị nhóm tin
        public ActionResult GetItemPartial()
        {
            NhomTinManager NTM = new NhomTinManager();
            NhomTinData data = new NhomTinData();
            data.danhSachNhomTin = NTM.LayDSNhomTin();
            return View(data);
        }
        // GET: Category
        public ActionResult Index(string tukhoa)//từ khóa tìm kiếm
        {
            NhomTinManager nhomtin = new NhomTinManager();
            NhomTinData data = new NhomTinData();
            if (!String.IsNullOrEmpty(tukhoa))
            {
                tukhoa = tukhoa.Trim();
            }
            data.danhSachNhomTin = nhomtin.TimKiemNhomTin(tukhoa);
            SetViewBag();
            ViewBag.TuKhoaTK = tukhoa;
            return View(data);
        }
        //Thêm mới một danh mục
        [HttpPost]
        public ActionResult Index(NhomTinData obj)
        {
            if (ModelState.IsValid)
            {
                NhomTinManager nhomtin = new NhomTinManager();
                nhomtin.ThemMoiNhomTin(obj);
                return RedirectToAction("Index", "NhomTin");
            }
            NhomTinManager group = new NhomTinManager();
            obj = new NhomTinData();
            obj.danhSachNhomTin = group.LayDSNhomTin();
            SetViewBag();
            return View(obj);
        }
        //chỉnh sửa danh mục 
        public ActionResult Edit()
        {
            NhomTinManager nhomtin = new NhomTinManager();
            string maNhomTin = Request.QueryString["manhomtin"].ToString();
            int id = int.Parse(maNhomTin);
            NhomTinData GND = nhomtin.LayChiTietNhomTin(id);
            SetViewBag(id);
            return View(GND);
        }
        [HttpPost]
        public ActionResult Edit(NhomTinData obj)
        {
            if (ModelState.IsValid)
            {
                NhomTinManager nhomtin = new NhomTinManager();
                nhomtin.SuaNhomTin(obj);
                return RedirectToAction("Index", "NhomTin");
            }
            NhomTinManager group = new NhomTinManager();
            obj = group.LayChiTietNhomTin(obj.maNhom);
            SetViewBag(obj.maNhom);
            return View(obj);
        }
        //Xóa bỏ một danh mục
        public ActionResult XoaNhomTin(int maNhom)
        {
            NhomTinManager nhomTin = new NhomTinManager();
            int kq = (int)nhomTin.XoaNhomTin(maNhom);
            return Json(kq);
        }
        //tạo slug khi gõ tên group news
        public ActionResult Slug(string tenNhomTin)
        {
            NhomTinManager nhomtin = new NhomTinManager();
            string slug = nhomtin.Slug(tenNhomTin);
            return Json(slug);
        }
        //xóa bản ghi chọn bằng checkbox
        public ActionResult XoaNhomTinDaChon(int[] customerIDs)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                foreach (int maNhom in customerIDs)
                {
                    tbl_nhomtin obj = db.tbl_nhomtin.Find(maNhom);
                    db.tbl_nhomtin.Remove(obj);
                }
                db.SaveChanges();
                return Json("Xóa thành công những bản ghi được chọn!");
            }
        }
    }
}