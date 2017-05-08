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
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index()
        {
            return View();
        }
        //xem chi tiết 1 trang option
        public ActionResult ViewPage()
        {
            string id = Request.QueryString["id"].ToString();
            OptionManager OM = new OptionManager();
            int ma = int.Parse(id);
            OptionData data = OM.GetData(ma);
            return View(data);
        }
        //lấy nội dung của một trang tin tức
        public ActionResult ViewNews()
        {
            string id = Request.QueryString["id"].ToString();
            TinTucManager OM = new TinTucManager();
            int ma = int.Parse(id);
            TinTucData data = OM.LayChiTietTin(ma);
            return View(data);
        }
        //lấy danh sách tin tức nổi bật
        public ActionResult GetNewsHotPartial()
        {
            TinTucManager news = new TinTucManager();
            TinTucData data = new TinTucData();
            data.danhSachTin = news.LayDSTinTucNoiBat();

            return PartialView(data);
        }
        //lấy ra 5 bài viết mới nhất
        public ActionResult NewArticlePartial()
        {
            TinTucManager TM = new TinTucManager();
            TinTucData data = new TinTucData();
            data.danhSachTin = TM.NewArticle();
            return PartialView(data);
        }
        public ActionResult SidebarPartial()
        {
            List<NhomTinData> list = new List<NhomTinData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = db.tbl_nhomtin.Select(o => new NhomTinData
                {
                    maNhom = o.MaNhom,
                    tenNhom = o.TenNhom,
                    danhMucCha = o.DanhMucCha
                }).OrderBy(o => o.danhMucCha).ToList();
            }
            return PartialView(list);
        }
        //hiển thị danh sách tin tức

        //hiển thị danh sách tin tức 
        public ActionResult GroupNews(string tinhTrang, int? danhMuc, string SearchTerm, int page = 1, int pageSize = 10)
        {
            TinTucManager news = new TinTucManager();
            TinTucData data = new TinTucData();
            //custom phân trang
            int totalRecord = 0;
            tinhTrang = "published";
            data.danhSachTin = news.LayDSTinTuc(tinhTrang, danhMuc, SearchTerm, ref totalRecord, page, pageSize);
            @ViewBag.SearchTerm = SearchTerm;

            ViewBag.Total = totalRecord;
            ViewBag.Page = page;

            int maxPage = 5;
            int totalPage = 0;

            totalPage = (int)Math.Ceiling((double)(totalRecord / pageSize)) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            ViewBag.TotalPage = totalPage;
            ViewBag.MaxPage = maxPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(data);
        }

        //
    }
}