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
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace TestDA.Areas.Manager.Controllers
{
    public class XepLopController : BaseController
    {
        // GET: Manager/XepLop
         [AuthorizeRoles("Admin", "HieuTruong", "VanThu")]
        public ActionResult Index(int page = 1, int pageSize = 20)
        {
            //lấy danh sách học sinh chưa xếp lớp
            XepLopManager xepLop = new XepLopManager();
            XepLopData hs = new XepLopData();
            //thực hiện phân trang
            //custom phân trang
            int totalRecord = 0;

            hs.danhSachHSChuaLop = xepLop.danhSachHSChuaLop(ref totalRecord, page, pageSize);
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
         public ActionResult XemHocSinhLop()
         {
             setLop();
             setNamHoc();

             return View();
         }
         public ActionResult XemHocSinhLopPartial(int maLop, string namHoc)
         {
             XepLopManager xepLop = new XepLopManager();
             XepLopData hs = new XepLopData();
             hs.danhSachHSLop = xepLop.ListHSTheoLop(maLop, namHoc);

             setLop(maLop);
             setNamHoc(namHoc);

             return PartialView(hs);
         }
         ////export to excel
         public ActionResult ExportExcel(int maLop, string namHoc)
         {
             try
             {
                 Excel.Application application = new Excel.Application();
                 Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
                 Excel.Worksheet worksheet = workbook.ActiveSheet;
                 //lấy dữ liệu
                 XepLopManager xepLop = new XepLopManager();
                 var data = xepLop.ListHSTheoLop(maLop, namHoc);

                 //lấy thông tin lớp:
                 LopManager LM = new LopManager();
                 LopData lop = LM.LayChiTietLop(maLop);
                 var tenLop = lop.tenLop;
                 worksheet.Cells[1, 1] = "Tên lớp: "+tenLop;
                 worksheet.Cells[2, 1] = "Năm học: " + namHoc;

                 worksheet.Cells[3, 2] = "STT";
                 worksheet.Cells[3, 3] = "Mã học sinh";
                 worksheet.Cells[3, 4] = "Tên học sinh";
                 worksheet.Cells[3, 5] = "Ngày sinh";
                 worksheet.Cells[3, 6] = "Giới tính";
                 worksheet.Cells[3, 7] = "Quê quán";

                 int row = 4;
                 int i=1;
                 foreach (var e in data)
                 {
                         worksheet.Cells[row, 2] = i;
                         worksheet.Cells[row, 3] = e.maHocSinh;
                         worksheet.Cells[row, 4] = e.tenHocSinh;
                         worksheet.Cells[row, 5] = e.ngaySinh;
                         if (e.gioiTinh == "1")
                         {
                             worksheet.Cells[row, 6] = "Nam";
                         }
                         else
                         {
                             worksheet.Cells[row, 6] = "Nữ";
                         }
                         worksheet.Cells[row, 7] = e.queQuan;

                         row++;
                         i++;
                 }
                 worksheet.get_Range("B3", "G3").EntireColumn.AutoFit();
                 //format heading
                 var range_heading = worksheet.get_Range("B3", "G3");
                 range_heading.Font.Bold = true;
                 //format date
                 var range_date = (Excel.Range)worksheet.Cells[4, 4];
                 range_date.EntireColumn.NumberFormat = "DD/MM/YYYY";

                 //save file
                 workbook.SaveAs(@"E:\dsHocSinh.xls");
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
             return Json("Hoàn thành tải xuống tại E:\\dsHocSinh.xls");
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
        public ActionResult SuaHocSinhLop(string[] mhSL, int maLop, string namHoc)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                foreach (string maHS in mhSL)
                {
                    //kiểm tra xem đã tồn tại mã học sinh thuộc lớp trong năm học đó
                    XepLopManager XLM = new XepLopManager();

                    if (XLM.HocSinhLop(maHS, namHoc) == true)
                    {
                        //nếu tồn tại thực hiện update thông tin
                        using (var dbContextTransaction = db.Database.BeginTransaction())
                        {
                            try
                            {
                                tbl_hocsinhlop kq = db.tbl_hocsinhlop.Single(o => o.MaHocSinh == maHS && o.NamHoc == namHoc);
                                //kq.MaHSLop = kq.MaHSLop;
                                kq.MaLop = maLop;

                                db.SaveChanges();

                                dbContextTransaction.Commit();
                            }
                            catch
                            {
                                dbContextTransaction.Rollback();
                            }
                        }///
                    }
                    else
                    {
                        //nếu chưa có lớp thực hiện thêm vào lớp

                        tbl_hocsinhlop obj = new tbl_hocsinhlop();

                        obj.MaLop = maLop;
                        obj.NamHoc = namHoc;
                        obj.MaHocSinh = maHS;

                        db.tbl_hocsinhlop.Add(obj);

                        db.SaveChanges();
                    }

                }
            }
            return Json("Chuyển lớp thành công");
        }
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