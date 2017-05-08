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

namespace TestDA.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public ActionResult Index()
        {          
            return View();
        }
        public ActionResult MenuPartial()
        {
            OptionManager OM = new OptionManager();
            OptionData data = new OptionData();
            data.danhSachDuLieu = OM.ListOptionData();
            return PartialView(data);        
        }
    }
}