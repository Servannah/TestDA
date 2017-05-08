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

namespace TestDA.Areas.Manager.Controllers
{
    public class OptionController : Controller
    {
        // GET: Manager/Option
        public ActionResult Index()
        {
            //tạo menu hiển thị lên trang chủ
            return View();
        }
        //lấy danh sách các trang
        public ActionResult PageIndex()
        {
            OptionManager LM = new OptionManager();
            OptionData data = new OptionData();
            //
            string type = "page";
            data.danhSachDuLieu = LM.ListData(type);
            return View(data);
        }
        //set order
        public void setOrd(int? selectedID = null)
        {
            List<SelectListItem> SetValue = new List<SelectListItem>();
            SetValue.Add(new SelectListItem()
            {
                Text = "Có",
                Value = "1",
                Selected = false
            });
            SetValue.Add(new SelectListItem()
            {
                Text = "Không",
                Value = "",
                Selected = false
            });
            ViewBag.order = new SelectList(SetValue, "Value", "Text", selectedID);
        }
        //Xem trang
        public ActionResult DetailPage()
        {
            string id = Request.QueryString["pageId"].ToString();
            OptionManager OM = new OptionManager();
            int ma = int.Parse(id);
            OptionData data = OM.GetData(ma);
            return View(data);
        }
        //thêm mới một trang
        public ActionResult EditPage()
        {
            string type = Request.QueryString["t"].ToString();//lấy thuộc tính type trong đường dẫn

            if (type == "Insert")
            {
                ViewBag.ChucNang = "Thêm trang mới";
                setOrd();
                //get type
                return View();
            }
            else
            {
                ViewBag.ChucNang = "Chỉnh sửa trang";
                OptionManager LM = new OptionManager();
                string id = Request.QueryString["id"].ToString();
                int ma = int.Parse(id);
                setOrd();
                OptionData LD = LM.GetData(ma);

                return View(LD);
            }
        }
        [HttpPost]
        public ActionResult EditPage(OptionData obj)
        {
            string type = Request.QueryString["t"].ToString();//lấy thuộc tính type trong đường dẫn
            string loai = "page";
            if (type == "Insert")
            {
                if (ModelState.IsValid)//trả về false
                {
                    OptionManager LM = new OptionManager();
                    LM.AddData(obj, loai);
                    TempData["Success"] = "Thêm mới thành công";
                    return RedirectToAction("PageIndex", "Option");
                }
                setOrd();
                return View(obj);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    OptionManager LM = new OptionManager();
                    LM.UpdateData(obj);
                    TempData["Success"] = "Chỉnh sửa thành công";
                    return RedirectToAction("PageIndex", "Option");
                }
                setOrd();
                return View(obj);
            }
        }  //kết thúc chức năng thêm , sửa bản ghi 
        //add info contact, email, phone, social
        public ActionResult InfoIndex()
        {
            //get info not page
            OptionManager OM = new OptionManager();
            List<OptionData> data = new List<OptionData>();
            data = OM.ListSocial();
            return View(data);
        }
        [HttpPost]
        public ActionResult InfoIndex(List<OptionData> obj)
        {
            OptionManager OM = new OptionManager();
            foreach (var data in obj)
            {
                //OptionData data = new OptionData();
                OM.UpdateData(data);
            }
            TempData["Success"] = "Sửa thông tin thành công";
            return RedirectToAction("InfoIndex", "Option");
        }

        //xóa một bản ghi trong cơ sở dữ liệu
        public ActionResult DeleteData(int id)
        {
            OptionManager LM = new OptionManager();
            LM.DeleteData(id);
            return Json("0");
        }
        //
    }
}