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
    public class TPNhapKhoManager
    {
        //lấy tất cả danh sách bản ghi trong cơ sở dữ liệu
        public List<TPNhapKhoData> LayDSTP(DateTime? date,string tenThucPham, ref int totalRecord, int pageIndex = 1, int pageSize = 10)
        {
            List<TPNhapKhoData> list = new List<TPNhapKhoData>();
        

            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //ref :: ở bên ngoài truyền vào
                //lấy tổng bản ghi
                totalRecord = db.tbl_tpkho.Count();

                list = (from m in db.tbl_tpkho
                        join t in db.tbl_thucpham on m.MaTP equals t.MaThucPham
                        select new TPNhapKhoData
                        {
                            maTPKho = m.MaTPKho,
                            maThucPham = m.MaTP,
                            tenThucPham = t.TenThucPham,
                            soLuong = m.SoLuong,
                            ngayNhapKho = m.NgayNhapKho,
                            ghiChu = m.GhiChu
                        }).OrderByDescending(m => m.maTPKho).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                if (date.HasValue)
                {
                    list = (from m in db.tbl_tpkho
                            join t in db.tbl_thucpham on m.MaTP equals t.MaThucPham
                            where m.NgayNhapKho == date
                            select new TPNhapKhoData
                            {
                                maTPKho = m.MaTPKho,
                                maThucPham = m.MaTP,
                                tenThucPham = t.TenThucPham,
                                soLuong = m.SoLuong,
                                ngayNhapKho = m.NgayNhapKho,
                                ghiChu = m.GhiChu
                            }).OrderByDescending(m => m.maTPKho).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
                if (!string.IsNullOrEmpty(tenThucPham))
                {
                    list = (from m in db.tbl_tpkho
                            join t in db.tbl_thucpham on m.MaTP equals t.MaThucPham
                            where t.TenThucPham.Contains(tenThucPham)
                            select new TPNhapKhoData
                            {
                                maTPKho = m.MaTPKho,
                                maThucPham = m.MaTP,
                                tenThucPham = t.TenThucPham,
                                soLuong = m.SoLuong,
                                ngayNhapKho = m.NgayNhapKho,
                                ghiChu = m.GhiChu
                            }).OrderByDescending(m => m.maTPKho).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
            }
            return list;

        }
        //lấy danh sách thực phẩm trong kho
        public List<TPNhapKhoData> ListTP()
        {
            List<TPNhapKhoData> list = new List<TPNhapKhoData>();

            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {

                list = (from m in db.tbl_tpkho
                        join t in db.tbl_thucpham on m.MaTP equals t.MaThucPham
                        select new TPNhapKhoData
                        {
                            maTPKho = m.MaTPKho,
                            maThucPham = m.MaTP,
                            tenThucPham = t.TenThucPham
                        }).OrderByDescending(m => m.maTPKho).ToList();
            }
            return list;
        }
        //lấy dữ liệu một danh mục
        public TPNhapKhoData LayChiTietTP(int id)
        {
            TPNhapKhoData LOP = new TPNhapKhoData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var tp = db.tbl_tpkho.Find(id);
                if (tp != null)
                {
                    LOP.maTPKho = tp.MaTPKho;
                    LOP.maThucPham = tp.MaTP;
                    //lấy tên thực phẩm
                    var tenTP = db.tbl_thucpham.Where(o => o.MaThucPham == tp.MaTP);
                    LOP.tenThucPham = tenTP.FirstOrDefault().TenThucPham;
                    LOP.soLuong = tp.SoLuong;
                    LOP.ngayNhapKho = tp.NgayNhapKho;
                    LOP.ghiChu = tp.GhiChu;
                }
            }
            return LOP;
        }
        //Thêm mới một danh mục
        public void ThemTPNhapKho(TPNhapKhoData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_tpkho tpKho = new tbl_tpkho();

                    tpKho.MaTPKho = obj.maTPKho;
                    tpKho.MaTP = obj.maThucPham;
                    tpKho.SoLuong = obj.soLuong;
                    tpKho.NgayNhapKho = obj.ngayNhapKho;
                    tpKho.GhiChu = obj.ghiChu;

                    db.tbl_tpkho.Add(tpKho);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        // chỉnh sửa danh mục  
        public void SuaTPKho(TPNhapKhoData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_tpkho.Where(o => o.MaTPKho == obj.maTPKho);
                        if (timkiem.Any())
                        {
                            tbl_tpkho kq = timkiem.FirstOrDefault();

                            kq.MaTPKho = obj.maTPKho;
                            kq.MaTP = obj.maThucPham;
                            kq.SoLuong = obj.soLuong;
                            kq.NgayNhapKho = obj.ngayNhapKho;
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
        public int XoaTPKho(int maTPKho)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var ID = db.tbl_tpkho.Where(o => o.MaTPKho == maTPKho);
                        //Xóa id danh mục đã chọn trong bảng
                        if (ID.Any())
                        {
                            db.tbl_tpkho.Remove(ID.FirstOrDefault());
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