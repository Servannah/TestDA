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
        //đổi mật khẩu
        //thay đổi mật khẩu
        public ActionResult DoiMatKhau(string id)
        {
            ViewBag.UserName = id;
            return View();
        }
        [HttpPost]
        public ActionResult DoiMatKhau(UserData obj)
        {
            if (ModelState.IsValid)//trả về false
            {
                //kiểm tra  mật khẩu người dùng nhập vào so sánh với mật khẩu đúng
                NhanVienManager PM = new NhanVienManager();
                var pass = PM.LayMatKhau(obj.UserName);
                if (Encryptor.MD5Hash(obj.OldPassword).Equals(pass))
                {
                    PM.UpdatePass(obj);
                    TempData["ChangePassSuccess"] = "Đổi mật khẩu thành công";

                    return RedirectToAction("DoiMatKhau", "TaiKhoan");
                }
                else
                {
                    ModelState.AddModelError("OldPassword", "Mật khẩu cũ cung cấp là không đúng !");
                }

            }
            return View(obj);
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
                        var userSession = new DangNhap();
                        userSession.tenDangNhap = DN.tenDangNhap;
                        userSession.matKhau = DN.matKhau;
                        Session.Add(CommonConstants.USER_SESSION, userSession);
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
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("DangNhap", "TaiKhoan");
        }
    }
}