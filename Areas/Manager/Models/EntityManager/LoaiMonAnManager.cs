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
    public class LoaiMonAnManager
    {
        //lấy tất cả danh sách loại món ăn trong csdl
        public List<LoaiMonAnData> LayDSLoaiMonAn(string inputtext)
        {
            List<LoaiMonAnData> list = new List<LoaiMonAnData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                if (!String.IsNullOrEmpty(inputtext))
                {
                    list = (from m in db.tbl_loaimonan
                            where (m.TenLoaiMonAn.Contains(inputtext))
                            select new LoaiMonAnData
                            {
                                maLoaiMonAn = m.MaLoaiMonAn,
                                tenLoaiMonAn = m.TenLoaiMonAn,
                                ghiChu = m.GhiChu
                            }).OrderByDescending(m => m.maLoaiMonAn).ToList();
                }
                else
                {
                    list = db.tbl_loaimonan.Select(o => new LoaiMonAnData
                    {
                        maLoaiMonAn = o.MaLoaiMonAn,
                        tenLoaiMonAn = o.TenLoaiMonAn,
                        ghiChu = o.GhiChu
                    }).OrderByDescending(o => o.maLoaiMonAn).ToList();
                }
            }
            return list;

        }
        //lấy id, tên loại món ăn
        public List<LoaiMonAnData> ListLoaiMA()
        {
            List<LoaiMonAnData> list = new List<LoaiMonAnData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                    list = db.tbl_loaimonan.Select(o => new LoaiMonAnData
                    {
                        maLoaiMonAn = o.MaLoaiMonAn,
                        tenLoaiMonAn = o.TenLoaiMonAn
                    }).OrderByDescending(o => o.maLoaiMonAn).ToList();
            }
            return list;
        }
        //lấy dữ liệu một danh mục
        public LoaiMonAnData LayChiTietLoaiMonAn(int id)
        {
            LoaiMonAnData LMA = new LoaiMonAnData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var loaimonan = db.tbl_loaimonan.Find(id);
                if (loaimonan != null)
                {
                    LMA.maLoaiMonAn = loaimonan.MaLoaiMonAn;
                    LMA.tenLoaiMonAn = loaimonan.TenLoaiMonAn;
                    LMA.ghiChu = loaimonan.GhiChu;
                }
            }
            return LMA;
        }
        //Thêm mới một danh mục
        public void ThemMoiMaLoaiMonAn(LoaiMonAnData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_loaimonan loaimonan = new tbl_loaimonan();
                    loaimonan.MaLoaiMonAn = obj.maLoaiMonAn;
                    loaimonan.TenLoaiMonAn = obj.tenLoaiMonAn;
                    loaimonan.GhiChu = obj.ghiChu;

                    db.tbl_loaimonan.Add(loaimonan);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        // chỉnh sửa danh mục  nhóm thực phẩm
        public void SuaLoaiMonAn(int maLoaiMonAn, string tenLoaiMonAn, string ghiChu)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_loaimonan.Where(o => o.MaLoaiMonAn == maLoaiMonAn);
                        if (timkiem.Any())
                        {
                            tbl_loaimonan kq = timkiem.FirstOrDefault();
                            kq.TenLoaiMonAn = tenLoaiMonAn;
                            kq.GhiChu = ghiChu;
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
        public int XoaLoaiMonAn(int maLoaiMonAn)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //kiểm tra loại món ăn  này có chứa món ăn hay không.
                        var loaimonan = db.tbl_monan.Where(o => o.MaLoaiMonAn == maLoaiMonAn);
                        if (loaimonan.Any())
                        {
                            return 1;
                        }
                        else
                        {
                            var ID = db.tbl_loaimonan.Where(o => o.MaLoaiMonAn == maLoaiMonAn);
                            //Xóa id danh mục đã chọn trong bảng loại món ăn
                            if (ID.Any())
                            {
                                db.tbl_loaimonan.Remove(ID.FirstOrDefault());
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
        }//kết thúc xóa một danh mục loại món ăn
    }
}