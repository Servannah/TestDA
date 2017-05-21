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
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.IO;
using TestDA.Security;
using System.Web.Security;

namespace TestDA.Areas.Manager.Controllers
{
    public class HocPhiController : BaseController
    {
        // GET: Manager/HocPhi
        [AuthorizeRoles("Admin", "HieuTruong", "VanThu")]
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
            setLop();

            return View();
        }
        //thống kê danh sách những học sinh còn nợ tiền học phí theo năm, tháng
        public ActionResult TKHocPhiPartial(string tukhoa, string namHoc, string loaiHP, int? thang, int? maLop)
        {
            HocPhiManager hocphi = new HocPhiManager();
            HocPhiData data = new HocPhiData();

            if (!String.IsNullOrEmpty(tukhoa))
            {
                tukhoa = tukhoa.Trim();
            }

            data.danhSachHocPhi = hocphi.dsHocPhiNoHP(tukhoa, namHoc, loaiHP, thang, maLop);

            setYears(namHoc);
            setLoaiApDung(loaiHP);
            setThang(thang);
            setLop(maLop);

            return PartialView(data);
        }
        //export to excel
        public ActionResult ExportExcel(string tukhoa, string namHoc, string loaiHP, int? thang, int? maLop)
        {
            try
            {
                Excel.Application application = new Excel.Application();
                Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                Excel.Worksheet worksheet = workbook.ActiveSheet;
                //lấy dữ liệu
                HocPhiManager HPM = new HocPhiManager();
                var data = HPM.dsHocPhiNoHP(tukhoa, namHoc, loaiHP, thang, maLop);

                worksheet.Cells[1, 1] = "Phiếu thu";
                worksheet.Cells[1, 2] = "Mã học sinh";
                worksheet.Cells[1, 3] = "Tên học sinh";
                worksheet.Cells[1, 4] = "Lớp";
                worksheet.Cells[1, 5] = "Năm học";
                worksheet.Cells[1, 6] = "Người thu";
                worksheet.Cells[1, 7] = "Ngày thu";
                worksheet.Cells[1, 8] = "Tổng học phí";
                worksheet.Cells[1, 9] = "Hình thức thu";
                worksheet.Cells[1, 10] = "Còn nợ";
                worksheet.Cells[1, 11] = "Ghi chú";

                int row = 2;
                foreach (var e in data)
                {
                    if (e.conNo != 0)
                    {
                        worksheet.Cells[row, 1] = e.maHocPhi;
                        worksheet.Cells[row, 2] = e.maHocSinh;
                        worksheet.Cells[row, 3] = e.tenHocSinh;
                        worksheet.Cells[row, 4] = e.tenLop;
                        worksheet.Cells[row, 5] = e.namHoc;
                        worksheet.Cells[row, 6] = e.tenNguoiThu;
                        worksheet.Cells[row, 7] = e.ngayThu;
                        worksheet.Cells[row, 8] = e.tongHocPhi;
                        if (e.loaiHP == "1")
                        {
                            worksheet.Cells[row, 9] = "Theo tháng";
                        }
                        else
                        {
                            worksheet.Cells[row, 9] = "Theo năm";
                        }
                        worksheet.Cells[row, 10] = e.conNo;
                        worksheet.Cells[row, 11] = e.ghiChu;

                        row++;
                    }
                }
                worksheet.get_Range("A1", "K1").EntireColumn.AutoFit();
                //format heading
                var range_heading = worksheet.get_Range("A1", "K1");
                range_heading.Font.Bold = true;
                //format date
                var range_date = (Excel.Range)worksheet.Cells[1, 7];
                range_date.EntireColumn.NumberFormat = "DD/MM/YYYY";

                //format currency
                //tổng học phí
                var range_currency = (Excel.Range)worksheet.Cells[1, 8];
                range_currency.NumberFormat = "#,###,###";
                //còn nợ
                var range_currency1 = (Excel.Range)worksheet.Cells[1, 10];
                range_currency1.NumberFormat = "#,###,###";

                //save file
                workbook.SaveAs(@"E:\hocphino.xls");
                workbook.Close();
                Marshal.ReleaseComObject(workbook);

                application.Quit();
                Marshal.FinalReleaseComObject(application);
                //// Save the Excel spreadsheet to a MemoryStream and return it to the client
            }
            catch (Exception ex)
            {
                ViewBag.Result = ex.Message;
            }
            return Json("Hoàn thành tải xuống tại E:\\hocphino.xls");
        }
        //In ra file pdf
        public ActionResult InHocPhi(string maHocPhi)
        {
            Dictionary<string, string> cookieCollection = new Dictionary<string, string>();
            foreach (var key in Request.Cookies.AllKeys)
            {
                cookieCollection.Add(key, Request.Cookies.Get(key).Value);
            }
            return new Rotativa.ActionAsPdf("InChiTiet", new { maHocPhi = maHocPhi })
            {
                Cookies = cookieCollection,
                CustomSwitches = "--load-error-handling ignore"
            };
            // return new Rotativa.ActionAsPdf("InChiTiet", new { maHocPhi = maHocPhi }); ;
        }
        //in ra chi tiết
        public ActionResult InChiTiet(string maHocPhi)
        {
            HocPhiManager LM = new HocPhiManager();
            HocPhiData LD = LM.LayChiTietHP(maHocPhi);
            ViewBag.maHocSinh = LD.maHocSinh;
            ViewBag.namHoc = LD.namHoc;

            TinhTien(LD.maHocSinh, LD.namHoc, LD.loaiHP);

            return View(LD);
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

            data.danhSachHocPhi = hocphi.dsThuHocPhiThang(namHoc, thang);
            setYears(namHoc);
            setThang(thang);

            return PartialView(data);
        }
        public ActionResult HocPhiThuThangPartial(string namHoc, int? thang)
        {
            HocPhiManager hocphi = new HocPhiManager();
            HocPhiData data = new HocPhiData();

            data.danhSachHocPhi = hocphi.dsThuHocPhi(namHoc, thang);


            return PartialView(data);
        }
        //liệt kê chi tiết
        public ActionResult TKChiTietPartial(string namHoc, int? thang, string loaiHP, int tongHS, int tienMienGiam)
        {
            //lấy danh sách khoản đóng
            DMHocPhiManager dm = new DMHocPhiManager();
            DMHocPhiData data = new DMHocPhiData();
            data.danhSachDMHocPhi = dm.DanhSachDMHocPhi(namHoc, loaiHP);
            ViewBag.tongHS = tongHS;
            ViewBag.tienMienGiam = tienMienGiam;
            return PartialView(data);
        }
        //
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
        //set lớp học
        public void setLop(int? selectedID = null)
        {
            LopManager LM = new LopManager();
            ViewBag.maLop = new SelectList(LM.ListLop(), "maLop", "tenLop", selectedID);
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