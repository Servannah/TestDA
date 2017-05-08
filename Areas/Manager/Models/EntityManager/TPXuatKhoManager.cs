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
    public class TPXuatKhoManager
    {
        //lấy tất cả danh sách bản ghi trong cơ sở dữ liệu
        public List<TPXuatKhoData> LayDSTP(DateTime? date, string tenThucPham, ref int totalRecord, int pageIndex = 1, int pageSize = 10)
        {
            List<TPXuatKhoData> list = new List<TPXuatKhoData>();


            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //ref :: ở bên ngoài truyền vào
                //lấy tổng bản ghi
                totalRecord = db.tbl_tpxuatkho.Count();

                list = (from m in db.tbl_tpxuatkho
                        join t in db.tbl_thucpham on m.MaTP equals t.MaThucPham
                        select new TPXuatKhoData
                        {
                            maXuatKho = m.MaTPXK,
                            maThucPham = m.MaTP,
                            tenThucPham = t.TenThucPham,
                            soLuong = m.SoLuong,
                            ngayXuatKho = m.NgayXuat,
                            ghiChu = m.GhiChu
                        }).OrderByDescending(m => m.maXuatKho).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                if (date.HasValue)
                {
                    list = (from m in db.tbl_tpxuatkho
                            join t in db.tbl_thucpham on m.MaTP equals t.MaThucPham
                            where m.NgayXuat == date
                            select new TPXuatKhoData
                            {
                                maXuatKho = m.MaTPXK,
                                maThucPham = m.MaTP,
                                tenThucPham = t.TenThucPham,
                                soLuong = m.SoLuong,
                                ngayXuatKho = m.NgayXuat,
                                ghiChu = m.GhiChu
                            }).OrderByDescending(m => m.maXuatKho).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
                if (!string.IsNullOrEmpty(tenThucPham))
                {
                    list = (from m in db.tbl_tpxuatkho
                            join t in db.tbl_thucpham on m.MaTP equals t.MaThucPham
                            where t.TenThucPham.Contains(tenThucPham)
                            select new TPXuatKhoData
                            {
                                maXuatKho = m.MaTPXK,
                                maThucPham = m.MaTP,
                                tenThucPham = t.TenThucPham,
                                soLuong = m.SoLuong,
                                ngayXuatKho = m.NgayXuat,
                                ghiChu = m.GhiChu
                            }).OrderByDescending(m => m.maXuatKho).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
            }
            return list;

        }
        //lấy dữ liệu một danh mục
        public TPXuatKhoData LayChiTietTP(int id)
        {
            TPXuatKhoData LOP = new TPXuatKhoData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var tp = db.tbl_tpxuatkho.Find(id);
                if (tp != null)
                {
                    LOP.maXuatKho = tp.MaTPXK;
                    LOP.maThucPham = tp.MaTP;
                    //lấy tên thực phẩm
                    var tenTP = db.tbl_thucpham.Where(o => o.MaThucPham == tp.MaTP);
                    LOP.tenThucPham = tenTP.FirstOrDefault().TenThucPham;
                    LOP.soLuong = tp.SoLuong;
                    LOP.ngayXuatKho = tp.NgayXuat;
                    LOP.ghiChu = tp.GhiChu;
                }
            }
            return LOP;
        }
        //Thêm mới một danh mục
        public void ThemTPXuatKho(TPXuatKhoData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_tpxuatkho tpKho = new tbl_tpxuatkho();

                    tpKho.MaTPXK = obj.maXuatKho;
                    tpKho.MaTP = obj.maThucPham;
                    tpKho.SoLuong = obj.soLuong;
                    tpKho.NgayXuat = obj.ngayXuatKho;
                    tpKho.GhiChu = obj.ghiChu;

                    db.tbl_tpxuatkho.Add(tpKho);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        // chỉnh sửa danh mục  
        public void SuaTPXuatKho(TPXuatKhoData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_tpxuatkho.Where(o => o.MaTPXK == obj.maXuatKho);
                        if (timkiem.Any())
                        {
                            tbl_tpxuatkho kq = timkiem.FirstOrDefault();

                            kq.MaTPXK = obj.maXuatKho;
                            kq.MaTP = obj.maThucPham;
                            kq.SoLuong = obj.soLuong;
                            kq.NgayXuat = obj.ngayXuatKho;
                            kq.GhiChu = obj.ghiChu;

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
        //xóa  một danh mục bản ghi
        public int XoaTPXuatKho(int maTPXK)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var ID = db.tbl_tpxuatkho.Where(o => o.MaTPXK == maTPXK);
                        //Xóa id danh mục đã chọn trong bảng
                        if (ID.Any())
                        {
                            db.tbl_tpxuatkho.Remove(ID.FirstOrDefault());
                            db.SaveChanges();
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