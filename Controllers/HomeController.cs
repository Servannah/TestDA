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

namespace DoAn.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        //create banner
        public ActionResult BannerPartial()
        {
            ConfigManager LM = new ConfigManager();
            ConfigData data = new ConfigData();

            string type = "slide";
            data.listData = LM.ListDataShow(type);

            return PartialView(data);
        }
        //create Info
        public ActionResult MainPartial()
        {
            ConfigManager LM = new ConfigManager();
            ConfigData data = new ConfigData();

            data.listData = LM.ListConfig();
            return PartialView(data);
        }
    }
}