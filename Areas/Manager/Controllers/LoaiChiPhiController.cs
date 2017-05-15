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
using TestDA.Security;

namespace TestDA.Areas.Manager.Controllers
{
    public class LoaiChiPhiController : BaseController
    {
        // GET: Manager/LoaiChiPhi
        [AuthorizeRoles("Admin", "HieuTruong", "VanThu")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LoaiChiPhiPartial(string tukhoa)
        {
            LoaiChiPhiManager LM = new LoaiChiPhiManager();
            LoaiChiPhiData data = new LoaiChiPhiData();
            if (!String.IsNullOrEmpty(tukhoa))
            {
                tukhoa = tukhoa.Trim();
            }
            data.danhSachLoaiChiPhi = LM.LayDSLoaiChiPhi(tukhoa);
            ViewBag.TuKhoaTK = tukhoa;
            setLaHocPhiKhoaChinh();

            return PartialView(data);
        }
        //Thêm mới một danh mục 
        [HttpPost]
        public ActionResult ThemLoaiChiPhi(LoaiChiPhiData obj)
        {
            if (ModelState.IsValid)
            {
                LoaiChiPhiManager LM = new LoaiChiPhiManager();
                LM.ThemMoiLoaiChiPhi(obj);
                TempData["Success"] = "Thêm bản ghi thành công";
                return Json("1");
            }
            var errorList = ModelState.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );
            return Json(errorList);

        }//kết thúc thêm bản ghi
        ////
        ///
        //chỉnh sửa thông tin bản ghi
        //public ActionResult SuaLoaiChiPhi(int maLoaiCP)
        //{
        //    LoaiChiPhiManager obj = new LoaiChiPhiManager();
        //    LoaiChiPhiData LCP = obj.LayChiTietLoaiChiPhi(maLoaiCP);
        //    return PartialView(LCP);
        //}
        public void setLaHocPhiKhoaChinh(string selectedID = null)
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
                Value = "2",
                Selected = false
            });
            ViewBag.laHPKhoaChinh = new SelectList(SetValue, "Value", "Text", selectedID);
        }
        public ActionResult SuaLoaiChiPhi()
        {
            //lấy id maLoaiChiPhi trên đường dẫn

            ViewBag.ChucNang = "Sửa loại học phí";

            string maLoaiChiPhi = Request.QueryString["maLoaiChiPhi"].ToString();
            int id = int.Parse(maLoaiChiPhi);
            LoaiChiPhiManager LCM = new LoaiChiPhiManager();
            LoaiChiPhiData data = LCM.LayChiTietLoaiChiPhi(id);

            setLaHocPhiKhoaChinh(data.laHPKhoaChinh);

            return View(data);

        }
        [HttpPost]
        public ActionResult SuaLoaiChiPhi(LoaiChiPhiData obj)
        {
            if (ModelState.IsValid)
            {
                LoaiChiPhiManager LCP = new LoaiChiPhiManager();
                LCP.SuaLoaiChiPhi(obj);
                TempData["Success"] = "Sửa bản ghi thành công";
                return RedirectToAction("Index", "LoaiChiPhi");
            }

            setLaHocPhiKhoaChinh(obj.laHPKhoaChinh);

            return View(obj);
        }
        //kết thúc chỉnh sửa thông tin bản ghi
        //Xóa bỏ một bản ghi
        public ActionResult XoaLoaiChiPhi(int maLoaiChiPhi)
        {
            LoaiChiPhiManager LM = new LoaiChiPhiManager();
            int kq = (int)LM.XoaLoaiChiPhi(maLoaiChiPhi);
            // ModelState.Clear();
            TempData["Success"] = "Xóa bản ghi thành công";
            return Json(kq);
        }//kết thúc xóa bỏ bản ghi
    }
}