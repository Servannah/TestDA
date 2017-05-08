using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestDA.Common;
using TestDA.Areas.Manager.Models.ViewModel;
using System.Web.Routing;

namespace TestDA.Areas.Manager.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sesstion = (DangNhap)Session[CommonConstants.USER_SESSION];
            if (sesstion == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "TaiKhoan", action = "DangNhap", Area = "Manager" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}