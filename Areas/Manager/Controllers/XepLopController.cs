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
    public class XepLopController : Controller
    {
        // GET: Manager/XepLop
        public ActionResult Index(string namHoc, int page = 1, int pageSize = 20)
        {
            //lấy danh sách học sinh chưa xếp lớp
            XepLopManager xepLop = new XepLopManager();
            XepLopData hs = new XepLopData();
            //thực hiện phân trang
            //custom phân trang
            int totalRecord = 0;

            hs.danhSachHSChuaLop = xepLop.danhSachHSChuaLop(namHoc, ref totalRecord, page, pageSize);
            setLop();
            setNamHoc();
            setNhomLop();
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
            return View(hs);
        }

        //hiển thị danh sách học sinh của một lớp
        public ActionResult HocSinhLop(int maLop, string namHoc)
        {
            XepLopManager xepLop = new XepLopManager();
            XepLopData hs = new XepLopData();
            hs.danhSachHSLop = xepLop.ListHSTheoLop(maLop, namHoc);

            setLop(maLop);
            setNamHoc(namHoc);

            return View(hs);
        }
        public void setLop(int? selectedID = null)
        {
            //lấy danh sách lớp
            LopManager Lop = new LopManager();
            ViewBag.maLop = new SelectList(Lop.ListLop(), "maLop", "tenLop", selectedID);
        }
        //lấy danh sách nhóm lớp
        public void setNhomLop(int? selectedID = null)
        {
            NhomLopManager nhomLop = new NhomLopManager();
            ViewBag.maNhomLop = new SelectList(nhomLop.ListNhomLop(), "maNhomLop", "tenNhomLop", selectedID);
        }
        //set năm học
        public void setNamHoc(string selectedID = null)
        {
            List<SelectListItem> Years = new List<SelectListItem>();
            int now = DateTime.Now.Year;
            for (int i =now; i >2010; i--)
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
        public void setSelect(int? selectedId = null)
        {
            //lấy id, tên nhóm lớp
            NhomLopManager NLM = new NhomLopManager();
            ViewBag.nhomLop = new SelectList(NLM.ListNhomLop(), "maNhomLop", "tenNhomLop", selectedId);
        }///
        public ActionResult ThemHocSinhLop(string[] hsIDs, int maLop, string namHoc)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                foreach (string maHS in hsIDs)
                {
                    tbl_hocsinhlop obj = new tbl_hocsinhlop();
                    obj.MaLop = maLop;
                    obj.NamHoc = namHoc;
                    obj.MaHocSinh = maHS;

                    db.tbl_hocsinhlop.Add(obj);

                    db.SaveChanges();
                }
                return Json("Thêm học sinh vào lớp thành công");
            }
        }/////
        // sửa thông tin học sinh trong lơp
        //public ActionResult SuaHocSinhLop(int[] mhSL, int maLop, string namHoc)
        //{
        //    using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
        //    {
        //        foreach (int maHS in mhSL)
        //        {
        //            tbl_hocsinhlop obj = new tbl_hocsinhlop();

        //            obj.MaLop = maLop;
        //            obj.NamHoc = namHoc;
        //            obj.MaHocSinh = maHS;

        //            db.tbl_hocsinhlop.Add(obj);

        //            db.SaveChanges();
        //        }
        //        return Json("Chuyển lớp thành công");
        //    }
        //}
        //xóa danh sách học sinh lớp đã chọn
        public ActionResult XoaHocSinhLop(int[] mhSL)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                foreach (int mahsL in mhSL)
                {
                    tbl_hocsinhlop obj = db.tbl_hocsinhlop.Find(mahsL);

                    db.tbl_hocsinhlop.Remove(obj);

                    db.SaveChanges();
                }
                return Json("Xóa bản ghi thành công");
            }
        }
        //public ActionResult KiemTraMaHS(string maHocSinh)
        //{
        //    HocSinhManager HSM = new HocSinhManager();
        //    bool resutl = HSM.KiemTraMaSoHocSinh(maHocSinh);
        //    return Json(resutl);
        //}
        public ActionResult TimKiemHocSinhLopPartial(string maHocSinh, string namHoc)
        {
            XepLopManager XLM = new XepLopManager();
            XepLopData data = new XepLopData();
            data.danhSachHSLop = XLM.HocSinhThuocLop(maHocSinh, namHoc);

            return PartialView(data);
        }//
        //xóa học sinh ra khỏi lớp
        public ActionResult XoaHSLop(int maHSLop)
        {
            XepLopManager XLM = new XepLopManager();
            XLM.XoaHSLop(maHSLop);

            return Json("");
        }//
    }
}