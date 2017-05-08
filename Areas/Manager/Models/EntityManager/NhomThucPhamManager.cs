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
    public class NhomThucPhamManager
    {
        //lấy tất cả danh sách danh mục trong csdl
        public List<NhomThucPhamData> LayDSNhomTP()
        {
            List<NhomThucPhamData> list = new List<NhomThucPhamData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = db.tbl_nhomthucpham.Select(o => new NhomThucPhamData
                {
                    maNhomTP = o.MaNhomTP,
                    tenNhomThucPham = o.TenNhomTP,
                    ghiChu  = o.GhiChu
                }).OrderByDescending(o => o.maNhomTP).ToList();
            }
            return list;

        }
        //lấy dữ liệu một danh mục
        public NhomThucPhamData LayChiTietNhomTP(int id)
        {
            NhomThucPhamData NTP = new NhomThucPhamData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var nhomtp = db.tbl_nhomthucpham.Find(id);
                if (nhomtp != null)
                {
                    NTP.maNhomTP = nhomtp.MaNhomTP;
                    NTP.tenNhomThucPham = nhomtp.TenNhomTP;
                    NTP.ghiChu = nhomtp.GhiChu;
                }
            }
            return NTP;
        }
        ////
        //Tìm kiếm thông tin một danh mục nhóm tin
        public List<NhomThucPhamData> TimKiemNhomTP(string inputtext)
        {
            List<NhomThucPhamData> kq = new List<NhomThucPhamData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //lấy danh sách nhóm tin
                kq = LayDSNhomTP();
                if (!String.IsNullOrEmpty(inputtext))
                {
                    kq = (from m in db.tbl_nhomthucpham
                          where (m.TenNhomTP.Contains(inputtext))
                          select new NhomThucPhamData
                          {
                              maNhomTP = m.MaNhomTP,
                              tenNhomThucPham = m.TenNhomTP,
                              ghiChu = m.GhiChu
                          }).OrderByDescending(m => m.maNhomTP).ToList();
                }
            }
            return kq;
        }
        /////////
        //Thêm mới một danh mục
        public void ThemMoiNhomTP(NhomThucPhamData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_nhomthucpham nhomtp = new tbl_nhomthucpham();
                    nhomtp.MaNhomTP = obj.maNhomTP;
                    nhomtp.TenNhomTP = obj.tenNhomThucPham;
                    nhomtp.GhiChu = obj.ghiChu;

                    db.tbl_nhomthucpham.Add(nhomtp);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        // chỉnh sửa danh mục  nhóm thực phẩm
        public void SuaNhomTP(int maNhomTP, string tenNhomTP, string ghiChu)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_nhomthucpham.Where(o => o.MaNhomTP == maNhomTP);
                        if (timkiem.Any())
                        {
                            tbl_nhomthucpham kq = timkiem.FirstOrDefault();
                            kq.TenNhomTP = tenNhomTP;
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
        public int XoaNhomTP(int maNhomTP)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //kiểm tra nhóm thực phẩm  này có chứa thực phẩm hay không.
                        var nhomTP = db.tbl_thucpham.Where(o => o.MaNhomTP == maNhomTP);
                        if (nhomTP.Any())
                        {
                            return 1;
                        }
                        else
                        {
                            var ID = db.tbl_nhomthucpham.Where(o => o.MaNhomTP == maNhomTP);
                            //Xóa id danh mục đã chọn trong bảng mn_term
                            if (ID.Any())
                            {
                                db.tbl_nhomthucpham.Remove(ID.FirstOrDefault());
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
        }
    }
}