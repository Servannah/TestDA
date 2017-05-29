using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDA.DB;
using TestDA.Areas.Manager.Models.ViewModel;
using TestDA.Areas.Manager.Models.EntityManager;
using System.Web.Security;
using TestDA.Common;
using System.Threading.Tasks;

namespace TestDA.Controllers
{
    public class ParentController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        //lây thông tin chi tiết của học sinh
        public ActionResult DetailStudent(string id)
        {
            HocSinhManager HSM = new HocSinhManager();

            HocSinhData HSD = HSM.LayChiTietHS(id);
            if (HSD.hinhAnh != null)
            {
                ViewBag.hinhAnh = @Url.Content(HSD.hinhAnh);
            }
            else
            { ViewBag.hinhAnh = null; }

            return View(HSD);
        }
        [HttpPost]
        public ActionResult DetailStudent(HocSinhData obj)
        {
            if (ModelState.IsValid)
            {
                HocSinhManager HSM = new HocSinhManager();
                HSM.SuaHocSinh(obj);
                TempData["Success"] = "Chỉnh sửa thành công";

                return RedirectToAction("DetailStudent", "Parent", new { id = obj.maHocSinh });
            }

            return View(obj);
        }
        //xem học phí của học sinh theo tháng, năm
        public ActionResult CourseStudent(string id, string namHoc, string loaiHP, int? thang, int page = 1, int pageSize = 20)
        {
            //lấy ra danh sách học phí
            HocPhiManager hocphi = new HocPhiManager();
            HocPhiData data = new HocPhiData();

            //thực hiện phân trang
            //custom phân trang
            int totalRecord = 0;
            data.danhSachHocPhi = hocphi.dsHocPhi(id, namHoc, loaiHP, thang, ref totalRecord, page, pageSize);
            ViewBag.maHocSinh = id;
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
        }//danh sách khoản phí đóng
        public ActionResult KhoanPhiPartial(string maHocSinh, string namHoc, string loaiHP)
        {
            DMHocPhiManager dm = new DMHocPhiManager();
            DMHocPhiData DPD = new DMHocPhiData();
            //lấy danh sách khoản phí theo tháng hoặc theo năm     s      
            DPD.danhSachDMHocPhi = dm.DanhSachDMHocPhi(namHoc, loaiHP);

            return PartialView(DPD);
        }
        //lấy danh sách thực đơn hàng ngày
        public ActionResult MenuStudent(DateTime? ngayLap, string maNhanVien, int page = 1, int pageSize = 20)
        {
            //custom phân trang
            int totalRecord = 0;
            //lấy danh sách thực đơn
            ThucDonManager LM = new ThucDonManager();
            ThucDonData data = new ThucDonData();
            data.danhSachTD = LM.LayDSTD(ngayLap, maNhanVien, ref totalRecord, page, pageSize);
            ViewBag.ngayLap = ngayLap;
            ViewBag.nguoiLap = maNhanVien;

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
        //thay đổi mật khẩu
        public ActionResult ChangePass(string id)
        {
            ViewBag.UserName = id;
            return View();
        }
        [HttpPost]
        public ActionResult ChangePass(UserData obj)
        {
            if (ModelState.IsValid)//trả về false
            {
                //kiểm tra  mật khẩu người dùng nhập vào so sánh với mật khẩu đúng
                ParentManager PM = new ParentManager();
                var pass = PM.GetPassWord(obj.UserName);
                if (Encryptor.MD5Hash(obj.OldPassword).Equals(pass))
                {
                    PM.UpdatePass(obj);
                    TempData["Success"]="Đổi mật khẩu thành công";

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("OldPassword", "Mật khẩu cũ cung cấp là không đúng !");
                }
                
            }
            return View(obj);
        }
        //Đăng nhập phụ huynh
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(DangNhap DN, string returnUrl)
        {
            //validate dữ liệu nhập vào
            if (ModelState.IsValid)
            {
                ParentManager NV = new ParentManager();
                //lấy mật khẩu của tên người dùng nhập vào
                string pass = NV.GetPassWord(DN.tenDangNhap);
                if (string.IsNullOrEmpty(pass))
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu cung cấp không đúng!");
                }
                else
                {
                    if (Encryptor.MD5Hash(DN.matKhau).Equals(pass))
                    {
                        //lưu sesstion đăng nhập
                        var userSession = new DangNhap();
                        userSession.tenDangNhap = DN.tenDangNhap;
                        userSession.matKhau = DN.matKhau;
                        Session.Add(CommonConstants.USER_SESSION, userSession);
                        //không giữ FA cookie, bởi nó được lưu ở máy client và có thể bị đánh cắp
                        FormsAuthentication.SetAuthCookie(DN.tenDangNhap, false);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("pass", "Mật khẩu cung cấp là không đúng !");
                    }
                }
            }
            return View(DN);
        }
        [Authorize]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index", "Home");
        }
    }
}