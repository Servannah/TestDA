using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDA.Security;
using TestDA.DB;
using TestDA.Areas.Manager.Models.EntityManager;
using TestDA.Areas.Manager.Models.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace TestDA.Areas.Manager.Controllers
{
    public class AdminController : BaseController
    {
        // GET: Admin
        //[Authorize]
        public ActionResult Index()
        {
            return View();
        }  
        //lấy thông tin người dùng đăng nhập
        public ActionResult HeaderPartial()
        {
            var id = HttpContext.User.Identity.Name;
            NhanVienManager NVM = new NhanVienManager();
            NhanVienData data = NVM.LayChiTietNV(id);

            ViewBag.hinhAnh = @Url.Content(data.hinhAnh);

            return PartialView(data);
        }
         public ActionResult Welcome()
         {
             return View();
         }
         [AuthorizeRoles("Admin")]
         //[AuthorizeRoles("Admin","Manager")]
         public ActionResult AdminOnly()
         {
             return View();
         }
        public ActionResult UnAuthorized()
        {
            return View();
        }
        public ActionResult HoanThanhDangKi()
        {
            return View();
        }
    }
}