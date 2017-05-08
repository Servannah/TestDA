using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDA.Areas.Manager.Models.ViewModel;
using TestDA.DB;
using System.Data.Entity.Validation;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TestDA.Common;

namespace TestDA.Areas.Manager.Models.EntityManager
{
    public class LopManager
    {
        //lấy tất cả danh sách bản ghi trong cơ sở dữ liệu
        public List<LopData> LayDSLop(string inputtext, int? nhomLop)
        {
            List<LopData> list = new List<LopData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_lop
                        join n in db.tbl_nhomlop on m.NhomLopID equals n.NhomLopID
                        select new LopData
                        {
                            maLop = m.MaLop,
                            tenLop = m.TenLop,
                            tenNhomLop = n.TenNhomLop,
                            moTa = m.MoTa
                        }).OrderByDescending(m => m.maLop).ToList();

                if (!String.IsNullOrEmpty(inputtext) && nhomLop.HasValue)
                {
                    list = (from m in db.tbl_lop
                            join n in db.tbl_nhomlop on m.NhomLopID equals n.NhomLopID
                            where (m.TenLop.Contains(inputtext) && m.NhomLopID == nhomLop)
                            select new LopData
                            {
                                maLop = m.MaLop,
                                tenLop = m.TenLop,
                                tenNhomLop = n.TenNhomLop,
                                moTa = m.MoTa
                            }).OrderByDescending(m => m.maLop).ToList();
                }
                else if (!String.IsNullOrEmpty(inputtext))
                {
                    list = (from m in db.tbl_lop
                            join n in db.tbl_nhomlop on m.NhomLopID equals n.NhomLopID
                            where m.TenLop.Contains(inputtext)
                            select new LopData
                            {
                                maLop = m.MaLop,
                                tenLop = m.TenLop,
                                tenNhomLop = n.TenNhomLop,
                                moTa = m.MoTa
                            }).OrderByDescending(m => m.maLop).ToList();
                }
                else if (nhomLop.HasValue)
                {
                    list = (from m in db.tbl_lop
                            join n in db.tbl_nhomlop on m.NhomLopID equals n.NhomLopID
                            where (n.NhomLopID == nhomLop)
                            select new LopData
                            {
                                maLop = m.MaLop,
                                tenLop = m.TenLop,
                                tenNhomLop = n.TenNhomLop,
                                moTa = m.MoTa
                            }).OrderByDescending(m => m.maLop).ToList();

                }
            }
            return list;

        }
        //lấy danh sách lớp theo nhóm lớp
        public List<LopData> ListLopTheoNhomLop(int nhomLop)
        {
            List<LopData> list = new List<LopData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_lop
                        join n in db.tbl_nhomlop on m.NhomLopID equals n.NhomLopID
                        where n.NhomLopID==nhomLop
                        select new LopData
                        {
                            maLop = m.MaLop,
                            tenLop =m.TenLop,

                        }).ToList();
            }
            return list;
        }
        //lấy id, tên lớp
        public List<LopData> ListLop()
        {
             List<LopData> list = new List<LopData>();
             using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
             {
                 list = (from m in db.tbl_lop
                         join n in db.tbl_nhomlop on m.NhomLopID equals n.NhomLopID
                         select new LopData
                         {
                             maLop = m.MaLop,
                             tenLop =m.TenLop,

                         }).OrderByDescending(m => m.maLop).ToList();
             }
             return list;
        }
        //lấy dữ liệu một danh mục
        public LopData LayChiTietLop(int id)
        {
            LopData LOP = new LopData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var lop = db.tbl_lop.Find(id);
                if (lop != null)
                {
                    LOP.maLop = lop.MaLop;
                    LOP.tenLop = lop.TenLop;
                    LOP.nhomLop = lop.NhomLopID;
                    LOP.moTa = lop.MoTa;
                }
            }
            return LOP;
        }
        //thêm mới học sinh cho lớp
        //public void ThemHSLop()
        //{

        //}
        //Thêm mới một danh mục
        public void ThemMoiLop(LopData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_lop lop = new tbl_lop();
                    lop.MaLop = obj.maLop;
                    lop.NhomLopID = obj.nhomLop;
                    lop.TenLop = obj.tenLop;
                    lop.MoTa = obj.moTa;

                    db.tbl_lop.Add(lop);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        // chỉnh sửa danh mục  nhóm thực phẩm
        public void SuaLop(LopData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_lop.Where(o => o.MaLop == obj.maLop);
                        if (timkiem.Any())
                        {
                            tbl_lop kq = timkiem.FirstOrDefault();
                            kq.TenLop = obj.tenLop;
                            kq.NhomLopID = obj.nhomLop;
                            kq.MoTa = obj.moTa;
                            db.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
        //xóa  một danh mục nhóm tin
        public int XoaLop(int maLop)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //kiểm tra lớp này có trong bảng tbl_hocsinhlop hay không
                        var lop = db.tbl_hocsinhlop.Where(o => o.MaLop == maLop);
                        if (lop.Any())
                        {
                            return 1;
                        }
                        else
                        {
                            var ID = db.tbl_lop.Where(o => o.MaLop == maLop);
                            //Xóa id danh mục đã chọn trong bảng
                            if (ID.Any())
                            {
                                db.tbl_lop.Remove(ID.FirstOrDefault());
                                db.SaveChanges();
                            }
                        }

                        dbContextTransaction.Commit();

                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
                return 0;
            }
        }//kết thúc xóa một danh mục 
    }
}