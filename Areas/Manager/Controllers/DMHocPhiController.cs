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
using System.Web.UI.WebControls;

namespace TestDA.Areas.Manager.Controllers
{
    public class DMHocPhiController : Controller
    {
        // GET: Manager/DMHocPhi
        public ActionResult Index(string namHoc, string loaiApDung)
        {
            //lấy toàn bộ định mức học phí trong cơ sở dữ liệu
            DMHocPhiManager DMHP = new DMHocPhiManager();
            DMHocPhiData data = new DMHocPhiData();
            data.danhSachDMHocPhi = DMHP.DanhSachDMHocPhi(namHoc, loaiApDung);
            setYears();
            return View(data);
        }
        public void setYears(string selectedID = null)
        {
            List<SelectListItem> Years = new List<SelectListItem>();
            int now = DateTime.Now.Year;
            for (int i = now; i >2010;i-- )
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
        public void setLoaiChiPhi(int? selectedId = null)
        {
            //lấy danh sách loại chi phí
            LoaiChiPhiManager LCP = new LoaiChiPhiManager();
            ViewBag.maLoaiChiPhi = new SelectList(LCP.ListLoaiChiPhi(), "maLoaiChiPhi", "tenLoai", selectedId);
        }
        public void setNhomLop(int? selectedId = null)
        {
            //lấy danh sách nhóm lớp
            NhomLopManager NLM = new NhomLopManager();
            ViewBag.maNhomLop = new SelectList(NLM.ListNhomLop(), "maNhomLop", "tenNhomLop", selectedId);
        }
        public void setLoaiApDung(string selectedID = null)
        {
            List<SelectListItem> SetValue = new List<SelectListItem>();
            SetValue.Add(new SelectListItem()
            {
                Text = "Đóng theo tháng",
                Value = "1",
                Selected = false
            });
            SetValue.Add(new SelectListItem()
            {
                Text = "Đóng theo năm",
                Value = "2",
                Selected = false
            });
            ViewBag.loaiApDung = new SelectList(SetValue, "Value", "Text", selectedID);
        }
        //thực hiện chức năng thêm, sửa bản ghi
        public ActionResult ChucNang()
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                ViewBag.ChucNang = "Thêm mới định mức học phí";

                setLoaiChiPhi();
                setYears();
                setLoaiApDung();

                return View();
            }
            else
            {
                ViewBag.ChucNang = "Chỉnh sửa thông tin định mức phí";

                string maDMHP = Request.QueryString["maDMHP"].ToString();
                int id = int.Parse(maDMHP);

                DMHocPhiManager DPM = new DMHocPhiManager();

                DMHocPhiData data = DPM.LayChiTiet(id);

                setLoaiChiPhi(data.maLoaiChiPhi);
                setYears(data.namHoc);
                setLoaiApDung(data.loaiApDung);

                return View(data);

            }
        }
        //thực hiện chức năng thêm, sửa bản ghi
        [HttpPost]
        public ActionResult ChucNang(DMHocPhiData obj)
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn

            if (type == "Insert")
            {
                if (ModelState.IsValid)//trả về false
                {
                    DMHocPhiManager DPM = new DMHocPhiManager();
                    DPM.ThemMoiDMHP(obj);
                    TempData["Success"] = "Thêm mới thành công";
                    return RedirectToAction("Index", "DMHocPhi");
                }

                setLoaiChiPhi(obj.maLoaiChiPhi);
                setYears(obj.namHoc);
                setLoaiApDung(obj.loaiApDung);

                return View(obj);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    DMHocPhiManager DPM = new DMHocPhiManager();
                    DPM.SuaDMHocPhi(obj);
                    TempData["Success"] = "Chỉnh sửa thành công";
                    return RedirectToAction("Index", "DMHocPhi");
                }

                setLoaiChiPhi(obj.maLoaiChiPhi);
                setYears(obj.namHoc);
                setLoaiApDung(obj.loaiApDung);

                return View(obj);
            }
        }  //kết thúc chức năng thêm , sửa bản ghi 
        //sửa thông tin định mức học phí
        //public ActionResult SuaDMHP()
        //{
        //    DMHocPhiManager DPM = new DMHocPhiManager();
        //    string maDMHP = Request.QueryString["maDMHP"].ToString();
        //    int id = int.Parse(maDMHP);
        //    DMHocPhiData data = DPM.LayChiTietDMHP(id);
        //    setLoaiChiPhi(data.maLoaiChiPhi);
        //    setNhomLop(data.maNhomLop);
        //    return View(data);
        //}
        //[HttpPost]
        //public ActionResult SuaDMHP(DMHocPhiData obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        DMHocPhiManager DPM = new DMHocPhiManager();
        //        DPM.SuaDMHocPhi(obj);
        //        TempData["Success"] = "Chỉnh sửa thành công";
        //        return RedirectToAction("Index", "DMHocPhi");
        //    }

        //    setLoaiChiPhi(obj.maLoaiChiPhi);
        //    setNhomLop(obj.maNhomLop);

        //    return View(obj);
        //}
        //Xóa danh sách bản ghi đã chọn
        //public ActionResult XoaDMHP(int[] dmIDs)
        //{
        //    using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
        //    {
        //        foreach (int maDM in dmIDs)
        //        {
        //            tbl_dmhocphi obj = db.tbl_dmhocphi.Find(maDM);
        //            db.tbl_dmhocphi.Remove(obj);
        //        }
        //        db.SaveChanges();
        //        return Json("Xóa thành công những bản ghi được chọn!");
        //    }
        //}

        //Xóa bỏ một bản ghi
        public ActionResult XoaDMHocPhi(int maDMHP)
        {
            DMHocPhiManager LM = new DMHocPhiManager();
            int kq = (int)LM.XoaDMHP(maDMHP);
            return Json(kq);
        }//kết thúc xóa bỏ bản ghi
    }
}