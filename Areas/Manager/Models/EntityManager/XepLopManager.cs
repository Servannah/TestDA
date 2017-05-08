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
    public class XepLopManager
    {
        //lấy danh sách học sinh chưa xếp lớp
        public List<HocSinhData> danhSachHSChuaLop(string namHoc, ref int totalRecord, int pageIndex = 1, int pageSize = 20)
        {
            List<HocSinhData> list = new List<HocSinhData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {

                list = (from m in db.tbl_hocsinh
                        join n in db.tbl_hocsinhlop on m.MaHS equals n.MaHocSinh into hsLop
                        from hs in hsLop.DefaultIfEmpty()
                        where hs.MaLop == null
                        select new HocSinhData
                        {
                            maHocSinh = m.MaHS,
                            hoTen = m.HoTen,
                            ngaySinh = m.NgaySinh,
                            ngayVaoHoc = m.NgayVaoHoc
                        }).OrderByDescending(m => m.maHocSinh).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                //lấy danh sách học sinh chưa xếp lớp năm học namHoc
                if (!String.IsNullOrEmpty(namHoc))
                {
                    list = (from m in db.tbl_hocsinh
                            join n in db.tbl_hocsinhlop on m.MaHS equals n.MaHocSinh into hsLop
                            from hs in hsLop.DefaultIfEmpty()
                            where hs.MaHocSinh == null && hs.NamHoc == namHoc
                            select new HocSinhData
                            {
                                maHocSinh = m.MaHS,
                                hoTen = m.HoTen,
                                ngaySinh = m.NgaySinh,
                                ngayVaoHoc = m.NgayVaoHoc

                            }).OrderByDescending(m => m.maHocSinh).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
                //lấy tổng số bản ghi
                totalRecord = list.Count();

            }
            return list;
        }//
        //tìm kiếm học sinh trong năm học thuộc lớp nào
        public List<XepLopData> HocSinhThuocLop(string maHocSinh, string namHoc)
        {
            List<XepLopData> list = new List<XepLopData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                if ((!String.IsNullOrEmpty(maHocSinh)) && (!String.IsNullOrEmpty(namHoc)))
                {
                    list = (from m in db.tbl_hocsinh
                            join n in db.tbl_hocsinhlop on m.MaHS equals n.MaHocSinh
                            join lp in db.tbl_lop on n.MaLop equals lp.MaLop
                            where m.MaHS == maHocSinh && n.NamHoc == namHoc
                            select new XepLopData
                            {
                                maHocSinh = m.MaHS,
                                tenHocSinh = m.HoTen,
                                tenLop = lp.TenLop,
                                namHoc = n.NamHoc
                            }).ToList();
                }
                else
                {
                    list = (from m in db.tbl_hocsinh
                            join n in db.tbl_hocsinhlop on m.MaHS equals n.MaHocSinh
                            join lp in db.tbl_lop on n.MaLop equals lp.MaLop
                            where m.MaHS == maHocSinh
                            select new XepLopData
                            {
                                maHocSinh = m.MaHS,
                                tenHocSinh = m.HoTen,
                                tenLop = lp.TenLop,
                                namHoc = n.NamHoc
                            }).ToList();
                }

            }
            return list;
        }//
        ///hiển thị danh sách học sinh của một lớp trong năm học nào đó
        public List<XepLopData> ListHSTheoLop(int maLop, string namHoc)
        {
            List<XepLopData> list = new List<XepLopData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {

                list = (from m in db.tbl_hocsinh
                        join n in db.tbl_hocsinhlop on m.MaHS equals n.MaHocSinh
                        where n.MaLop == maLop && n.NamHoc == namHoc
                        select new XepLopData
                        {
                            maHocSinh = m.MaHS,
                            maHSLop = n.MaHSLop,
                            tenHocSinh = m.HoTen,
                            ngaySinh = m.NgaySinh,
                            gioiTinh = m.GioiTinh,
                            queQuan = m.QueQuan

                        }).OrderByDescending(m => m.maHocSinh).ToList();
            }
            return list;
        }//
        ///
        //xóa  một danh mục nhóm tin
        public void XoaHSLop(int maHSLop)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var ID = db.tbl_hocsinhlop.Where(o => o.MaHSLop == maHSLop);
                        //Xóa id danh mục đã chọn trong bảng
                        if (ID.Any())
                        {
                            db.tbl_hocsinhlop.Remove(ID.FirstOrDefault());
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
        }//kết thúc xóa một danh mục 
    }
}