using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDA.DB;
using TestDA.Areas.Manager.Models.EntityManager;
using System.Web.Mvc;

namespace TestDA.Security
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        private readonly string[] userAssignedRoles;
        public AuthorizeRolesAttribute(params string[] roles)
        {
            this.userAssignedRoles = roles;
        }
        //ghi đè 2 phương thức
        //cho phép người dùng có truy cập vào một trang hay không
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                NhanVienManager UM = new NhanVienManager();
                foreach (var roles in userAssignedRoles)
                {
                    authorize = UM.LayQuyenND(httpContext.User.Identity.Name, roles);
                    if (authorize)
                        return authorize;
                }
            }
            return authorize;
        }
        //chuyển hướng người dùng đến trang UnAuthorized
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Manager/Admin/UnAuthorized");
        }
    }
}