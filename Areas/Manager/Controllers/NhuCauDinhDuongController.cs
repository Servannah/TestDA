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
using System.Web.UI.WebControls;

namespace TestDA.Areas.Manager.Controllers
{
    public class NhuCauDinhDuongController : Controller
    {
        // GET: Manager/NhuCauDinhDuong
        public ActionResult Index()
        {
            NhuCauDinhDuongManager NCDD = new NhuCauDinhDuongManager();
            NhuCauDinhDuongData data = new NhuCauDinhDuongData();
            data.danhSachNCDD = NCDD.LayDSNCDD();

            return View(data);
        }
        //
        public void SetAge(int? selectedId = null)
        {
            List<ListItem> listItems = new List<ListItem>();
            listItems.Add(new ListItem
            {
                Text = "2",
                Value = "2"
            });
            listItems.Add(new ListItem
            {
                Text = "3",
                Value = "3",
                Selected = true
            });
            listItems.Add(new ListItem
            {
                Text = "4",
                Value = "4"
            });
            listItems.Add(new ListItem
            {
                Text = "5",
                Value = "5"
            });
            listItems.Add(new ListItem
            {
                Text = "6",
                Value = "6"
            });
            ViewBag.tuoi = new SelectList(listItems, "Value", "Text",selectedId);
        }
        //thực hiện chức năng thêm, sửa thông tin bản ghi
        public ActionResult ChucNang()
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                ViewBag.ChucNang = "Thêm mới thông tin dinh dưỡng";
                SetAge();
                return View();
            }
            else
            {
                ViewBag.ChucNang = "Chỉnh sửa thông tin dinh dưỡng ";
                NhuCauDinhDuongManager NCDD = new NhuCauDinhDuongManager();
                string maNCDD = Request.QueryString["mancdd"].ToString();
                int id = int.Parse(maNCDD);
                NhuCauDinhDuongData NCC = NCDD.LayChiTietNCDD(id);
                SetAge(NCC.tuoi);
                return View(NCC);
            }
        }
        //thực hiện chức năng thêm, sửa bản ghi
        [HttpPost]
        public ActionResult ChucNang(NhuCauDinhDuongData obj)
        {
            string type = Request.QueryString["type"].ToString();//lấy thuộc tính type trong đường dẫn
            if (type == "Insert")
            {
                if (ModelState.IsValid)//trả về false
                {
                    NhuCauDinhDuongManager nhuCDD = new NhuCauDinhDuongManager();
                    nhuCDD.ThemMoiNhuCauDD(obj);
                    TempData["Success"] = "Thêm mới thành công";
                    return RedirectToAction("Index", "NhuCauDinhDuong");
                }
                SetAge();
                return View(obj);
            }
            else
            {
                if (ModelState.IsValid)
                {
                    NhuCauDinhDuongManager nhuCDD = new NhuCauDinhDuongManager();
                    nhuCDD.SuaNhuCauDD(obj);
                    TempData["Success"] = "Chỉnh sửa thành công";
                    return RedirectToAction("Index", "NhuCauDinhDuong");
                }
                SetAge(obj.tuoi);
                return View(obj);
            }
        }
        //Xóa bỏ một danh mục
        public ActionResult XoaNhuCauDD(int maNhuCauDD)
        {
            NhuCauDinhDuongManager nhuCauDD = new NhuCauDinhDuongManager();
            int kq = (int)nhuCauDD.XoaNhuCauDinhDuong(maNhuCauDD);
            TempData["Success"] = "Xóa bản ghi thành công";
            return Json(kq);
        }//kết thúc xóa bỏ bản ghi
    }
}