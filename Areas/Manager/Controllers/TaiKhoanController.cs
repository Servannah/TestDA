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

namespace TestDA.Areas.Manager.Controllers
{
    public class TaiKhoanController : Controller
    {
        public ActionResult DangKi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKi(DangKi DK)
        {
            if (ModelState.IsValid)
            {
                NhanVienManager NV = new NhanVienManager();
                if (!NV.KiemTraTenDangKi(DK.tenDangNhap)) ///kiểm tra tên đăng kí có tồn tại
                {
                    NV.ThemTaiKhoan(DK);
                    FormsAuthentication.SetAuthCookie(DK.ho, false);
                    return RedirectToAction("HoanThanhDangKi", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại !");
                }
            }
            return View();
        }
        //đăng nhập
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(DangNhap DN, string returnUrl)
        {
            //validate dữ liệu nhập vào
            if (ModelState.IsValid)
            {
                NhanVienManager NV = new NhanVienManager();
                //lấy mật khẩu của tên người dùng nhập vào
                string matkhau = NV.LayMatKhau(DN.tenDangNhap);
                if (string.IsNullOrEmpty(matkhau))
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu cung cấp không đúng!");
                }
                else
                {
                    if (Encryptor.MD5Hash(DN.matKhau).Equals(matkhau))
                    {
                        //lưu sesstion đăng nhập
                        Session.Add(CommonConstants.USER_SESSION, DN);
                        //không giữ FA cookie, bởi nó được lưu ở máy client và có thể bị đánh cắp
                        FormsAuthentication.SetAuthCookie(DN.tenDangNhap, false);
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mật khẩu cung cấp là không đúng !");
                    }
                }
            }
            return View(DN);
        }
        [Authorize]
        public ActionResult DangXuat()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("DangNhap", "TaiKhoan");
        }
    }
}