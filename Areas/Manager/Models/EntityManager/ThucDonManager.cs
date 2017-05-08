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
    public class ThucDonManager
    {
        //lấy tất cả danh sách bản ghi trong cơ sở dữ liệu
        public List<ThucDonData> LayDSTD(DateTime? ngayLap, string maNhanVien, ref int totalRecord, int pageIndex = 1, int pageSize = 10)
        {
            List<ThucDonData> list = new List<ThucDonData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //ref :: ở bên ngoài truyền vào
                //lấy tổng bản ghi
                totalRecord = db.tbl_thucpham.Count();

                var query = (from m in db.tbl_thucdon
                             join n in db.tbl_nhanvien on m.NguoiLap equals n.MaNV
                             select new
                             {
                                 maTD = m.MaThucDon,
                                 tenThucDon = m.TenThucDon,
                                 noiDung = m.NoiDung,
                                 ngayLap = m.NgayLap,
                                 tenNguoiTao = n.Ho + " " + n.Ten,
                                 nguoiLap = m.NguoiLap,
                                 ghiChu = m.GhiChu
                             });//.OrderByDescending(m => m.ngayLap).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                if (ngayLap.HasValue)
                {
                    query = from q in query where q.ngayLap == ngayLap select q;
                }
                if (!String.IsNullOrEmpty(maNhanVien))
                {
                    query = from q in query where q.nguoiLap == maNhanVien || q.tenNguoiTao.Contains(maNhanVien) select q;
                }
                var result = (from m in query
                              select new ThucDonData
                              {
                                  maTD = m.maTD,
                                  tenThucDon = m.tenThucDon,
                                  noiDung = m.noiDung,
                                  ngayLap = m.ngayLap,
                                  tenNguoiTao = m.tenNguoiTao,
                                  ghiChu = m.ghiChu
                              }).OrderByDescending(m => m.ngayLap).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                list = result;
            }
            return list;
        }      ///
        //lấy id, tên của thực đơn

        public List<ThucDonData> ListTD()
        {
            List<ThucDonData> list = new List<ThucDonData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_thucdon
                        join n in db.tbl_nhanvien on m.NguoiLap equals n.MaNV
                        select new ThucDonData
                        {
                            maTD = m.MaThucDon,
                            tenThucDon = m.TenThucDon
                        }).OrderByDescending(m => m.maTD).ToList();
            }
            return list;
        }
        //lấy thông tin chi tiết thực đơn
        public ThucDonData LayChiTietThucDon(int id)
        {
            ThucDonData TD = new ThucDonData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var obj = db.tbl_thucdon.Find(id);
                if (obj != null)
                {
                    TD.maTD = obj.MaThucDon;
                    TD.tenThucDon = obj.TenThucDon;
                    TD.noiDung = obj.NoiDung;
                    TD.nguoiLap = obj.NguoiLap;
                    TD.ngayLap = obj.NgayLap;
                    TD.ghiChu = obj.GhiChu;
                }
            }
            return TD;
        }
        //thêm mới một thực đơn
        public void ThemThucDon(ThucDonData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_thucdon thucdon = new tbl_thucdon();

                    thucdon.MaThucDon = obj.maTD;
                    thucdon.TenThucDon = obj.tenThucDon;
                    thucdon.NoiDung = obj.noiDung;
                    thucdon.NguoiLap = obj.nguoiLap;
                    thucdon.NgayLap = obj.ngayLap;
                    thucdon.GhiChu = obj.ghiChu;

                    db.tbl_thucdon.Add(thucdon);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        //chỉnh sửa thông tin thực đơn
        public void SuaThucDon(ThucDonData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_thucdon.Where(o => o.MaThucDon == obj.maTD);
                        if (timkiem.Any())
                        {
                            tbl_thucdon kq = timkiem.FirstOrDefault();
                            kq.MaThucDon = obj.maTD;
                            kq.TenThucDon = obj.tenThucDon;
                            kq.NoiDung = obj.noiDung;
                            kq.NgayLap = obj.ngayLap;
                            kq.NguoiLap = obj.nguoiLap;
                            kq.GhiChu = obj.ghiChu;

                            db.SaveChanges();
                        }
                        dbContextTransaction.Commit();

                    }
                    catch (DbEntityValidationException e)
                    {
                        Console.Write(e);
                    }
                }
            }
        }
        //xóa thông tin thực đơn
        public int XoaThucDon(int maThucDon)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //kiểm tra lớp này có trong bảng tbl_hoadon hay không
                        var lop = db.tbl_hoadon.Where(o => o.MaThucDon == maThucDon);
                        if (lop.Any())
                        {
                            return 1;
                        }
                        else
                        {
                            var ID = db.tbl_thucdon.Where(o => o.MaThucDon == maThucDon);
                            //Xóa id danh mục đã chọn trong bảng
                            if (ID.Any())
                            {
                                db.tbl_thucdon.Remove(ID.FirstOrDefault());
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