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
using TestDA.Common;

namespace TestDA.Areas.Manager.Controllers
{
    public class HocPhiController : Controller
    {
        // GET: Manager/HocPhi
        public ActionResult Index(string hocSinh, string namHoc, string loaiHP, int? thang, int page = 1, int pageSize = 20)
        {
            //lấy ra danh sách học phí
            HocPhiManager hocphi = new HocPhiManager();
            HocPhiData data = new HocPhiData();

            //thực hiện phân trang
            //custom phân trang
            int totalRecord = 0;

            if (!String.IsNullOrEmpty(hocSinh))
            {
                hocSinh = hocSinh.Trim();
            }
            data.danhSachHocPhi = hocphi.dsHocPhi(hocSinh, namHoc, loaiHP, thang, ref totalRecord, page, pageSize);
            ViewBag.TKTimKiem = hocSinh;
            setYears();
            setLoaiApDung();

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
        public ActionResult TKHocPhi()
        {
            setYears();
            setLoaiApDung();
            setThang();

            return View();
        }
        //thống kê danh sách những học sinh còn nợ tiền học phí theo năm, tháng
        public ActionResult TKHocPhiPartial(string namHoc, string loaiHP, int? thang)
        {
            HocPhiManager hocphi = new HocPhiManager();
            HocPhiData data = new HocPhiData();

            data.danhSachHocPhi = hocphi.dsHocPhiNoHP(namHoc, loaiHP, thang);

            setYears(namHoc);
            setLoaiApDung(loaiHP);
            setThang(thang);

            return PartialView(data);
        }
        //thống kê học phí thu
        public ActionResult HocPhiThu()
        {
            setYears();
            setThang();
            return View();
        }
        //thống kê học phí thu 
        public ActionResult HocPhiThuPartial(string namHoc, int? thang)
        {
            HocPhiManager hocphi = new HocPhiManager();
            HocPhiData data = new HocPhiData();

            data.danhSachHocPhi = hocphi.dsThuHocPhi(namHoc, thang);

            setYears(namHoc);
            setThang(thang);

            return PartialView(data);
        }
        //
        //thống kê danh sách tổng học phí thu được
        //public ActionResult TKTongHocPhiPartial(string namHoc, int? thang)
        //{
        //    List<HocPhiData> list = new List<HocPhiData>();
        //    using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
        //    {
        //        var hp = from h in db.tbl_hocphi where h.NamHoc == namHoc select h;
        //        if (thang.HasValue)
        //        {
        //            hp = from t in hp where t.Thang == thang select t;
        //        }
        //        //////thống kê học phí theo từng năm học
        //        var query = (from t in hp
        //                     group t by t.Thang
        //                         into gThang
        //                         select new HocPhiData
        //                         {
        //                             thang = gThang.Key,
        //                             loaiHP = (from m in gThang
        //                                      group m by m.LoaiHP
        //                                          into mLoaiHP
        //                                          select new HocPhiData
        //                                          {
        //                                              loaiHP = mLoaiHP.Key,
        //                                              tongHocPhi = mLoaiHP.Sum(t => t.TongHocPhi),
        //                                              conNo = mLoaiHP.Sum(t => t.ConNo)
        //                                          })
        //                         }).ToList();
        //        list = query;
        //    }
        //    return PartialView(list);
        //}
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
        //set học sinh
        public void setHocSinh(string selectedID = null)
        {
            HocSinhManager HSM = new HocSinhManager();
            ViewBag.maHocSinh = new SelectList(HSM.ListHS(), "maHocSinh", "maTen", selectedID);
        }
        public void setNhanVien(string selectedID = null)
        {
            NhanVienManager NVM = new NhanVienManager();
            ViewBag.nguoiThu = new SelectList(NVM.ListNhanVien(), "maNV", "maHoTen", selectedID);
        }
        //set tháng của năm
        public void setThang(int? selectedID = null)
        {
            List<SelectListItem> Month = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                Month.Add(new SelectListItem()
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = false
                });
            }
            ViewBag.thang = new SelectList(Month, "Value", "Text", selectedID);
        }
        //set loại áp dung
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
            ViewBag.loaiHP = new SelectList(SetValue, "Value", "Text", selectedID);
        }
        //lấy danh sách định mức học phí trong năm học cần đóng theo tháng  hoặc theo năm
        public ActionResult KhoanPhiPartial(string maHocSinh, string namHoc, string loaiHP)
        {
            DMHocPhiManager dm = new DMHocPhiManager();
            DMHocPhiData DPD = new DMHocPhiData();
            //lấy danh sách khoản phí theo tháng hoặc theo năm     s      
            DPD.danhSachDMHocPhi = dm.DanhSachDMHocPhi(namHoc, loaiHP);
            TinhTien(maHocSinh, namHoc, loaiHP);

            return PartialView(DPD);
        }
        public void TinhTien(string maHocSinh, string namHoc, string loaiHP)
        {
            //lấy thông tin học sinh có thuộc đối tượng miễn giảm
            HocSinhManager hs = new HocSinhManager();
            HocSinhData data = hs.LayChiTietHS(maHocSinh);
            int mg = int.Parse(data.dinhMucGiam.Value.ToString());
            string dtmg = data.tenDTUuTien + " Giảm:" + mg + "% học phí";
            ViewBag.doituong = dtmg;

            DMHocPhiManager dm = new DMHocPhiManager();
            DMHocPhiData DPD = new DMHocPhiData();
            //lấy học phí của năm học
            int hp = dm.LayHocPhiCK(namHoc).soTien.Value;
            //tính định mức giảm
            int miengiam = hp * mg / 100;
            ViewBag.tienGiam = miengiam;

            //lấy danh sách khoản phí theo tháng hoặc theo năm           
            DPD.danhSachDMHocPhi = dm.DanhSachDMHocPhi(namHoc, loaiHP);
            //tính tổng các khoản phí
            decimal tongPhi = 0;
            foreach (var item in DPD.danhSachDMHocPhi)
            {
                tongPhi += item.soTien.Value;
            }
            //tính số tiền cần đóng
            int tongdong = int.Parse(tongPhi.ToString()) - miengiam;
            ViewBag.tongPhi = tongPhi;
            ViewBag.tongTien = tongdong;
            ViewBag.docTien = ReadCurrency.NumberToTextVN(tongPhi);
            ViewBag.tongDong = ReadCurrency.NumberToTextVN(tongdong);
        }
        //Xem chi tiết thông tin học phí của học sinh
        public ActionResult XemChiTiet()
        {
            string maHocPhi = Request.QueryString["maHocPhi"].ToString();

            HocPhiManager LM = new HocPhiManager();
            HocPhiData LD = LM.LayChiTietHP(maHocPhi);
            ViewBag.maHocSinh = LD.maHocSinh;
            ViewBag.namHoc = LD.namHoc;

            TinhTien(LD.maHocSinh, LD.namHoc, LD.loaiHP);

            return View(LD);
        }
        //thực hiện chức năng thêm sửa bản ghi
        public ActionResult ChucNang()
        {

            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                ViewBag.ChucNang = "Thêm mới phiếu thu";

                setNhanVien();
                setThang();
                setLoaiApDung();
                setYears();

                ViewBag.type = 1;

                return View();
            }
            else
            {
                ViewBag.ChucNang = "Sửa thông tin phiếu thu";

                HocPhiManager LM = new HocPhiManager();

                string maHocPhi = Request.QueryString["maHocPhi"].ToString();
                HocPhiData LD = LM.LayChiTietHP(maHocPhi);

                setNhanVien(LD.nguoiThu);
                setThang(LD.thang);
                setLoaiApDung(LD.loaiHP);
                setYears(LD.namHoc);

                ViewBag.type = 2;

                return View(LD); ;
            }
        }
        [HttpPost]
        public ActionResult ChucNang(HocPhiData obj)
        {

            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                if (ModelState.IsValid)//trả về false
                {
                    HocPhiManager LM = new HocPhiManager();
                    LM.ThemHocPhi(obj);
                    TempData["Success"] = "Thêm mới thành công";
                    return RedirectToAction("Index", "HocPhi");
                }

                setNhanVien(obj.nguoiThu);
                setThang(obj.thang);
                setLoaiApDung(obj.loaiHP);
                setYears(obj.namHoc);


                return View(obj);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    HocPhiManager LM = new HocPhiManager();
                    LM.SuaHocPhi(obj);
                    TempData["Success"] = "Chỉnh sửa thành công";
                    return RedirectToAction("Index", "HocPhi");
                }

                setYears(obj.namHoc);
                setNhanVien(obj.nguoiThu);
                setThang(obj.thang);
                setLoaiApDung(obj.loaiHP);

                return View(obj);
            }
        }
        //Xóa bỏ một bản ghi
        public ActionResult XoaHocPhi(string maHocPhi)
        {
            HocPhiManager hocphi = new HocPhiManager();
            hocphi.XoaHocPhi(maHocPhi);
            return Json("0");
        }//kết thúc xóa bỏ bản ghi
        ///kiểm tra mã học sinh có trùng trong bảng dữ liệu không
        public ActionResult IsMaPhieuThuAvailble(string maHocPhi)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    var mahp = db.tbl_hocphi.Single(m => m.MaHocPhi == maHocPhi);
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }/////

    }
}