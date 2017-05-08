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
    public class PhieuHoaDonManager
    {
        //lấy tất cả danh sách phiếu hóa đơn trong csdl
        public List<HoaDonData> LayDSPhieuHD(string inputtext, ref int totalRecord, int pageIndex = 1, int pageSize = 10)
        {
            List<HoaDonData> list = new List<HoaDonData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //ref :: ở bên ngoài truyền vào
                //lấy tổng bản ghi
                totalRecord = db.tbl_hoadon.Count();
                //lấy danh sách 
                if (!String.IsNullOrEmpty(inputtext))
                {
                    list = (from m in db.tbl_hoadon
                            join n in db.tbl_nhanvien on m.NguoiLap equals n.MaNV
                            where (m.SoPhieu.Contains(inputtext))
                            select new HoaDonData
                            {
                                maPhieuHD = m.MaPhieuHD,
                                soPhieuHD = m.SoPhieu,
                                tenNguoiLap = n.Ho + " " + n.Ten,
                                tongTien = m.TongTien,
                                ngayLap = m.NgayTao,
                                ghiChu = m.GhiChu
                            }).OrderByDescending(m => m.maPhieuHD).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    list = (from m in db.tbl_hoadon
                            join n in db.tbl_nhanvien on m.NguoiLap equals n.MaNV
                            select new HoaDonData
                            {
                                maPhieuHD = m.MaPhieuHD,
                                soPhieuHD = m.SoPhieu,
                                tenNguoiLap = n.Ho + " " + n.Ten,
                                ngayLap = m.NgayTao,
                                tongTien = m.TongTien,
                                ghiChu = m.GhiChu
                            }).OrderByDescending(m => m.maPhieuHD).ToList();
                }
            }
            return list;

        }
        //lấy thông tin thực đơn của hóa đơn này
        public ThucDonData ChiTietThucDon(int maThucDon)
        {

            ThucDonData TDD = new ThucDonData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var thucdon = db.tbl_thucdon.Find(maThucDon);
                if (thucdon != null)
                {
                    TDD.tenThucDon = thucdon.TenThucDon;
                    TDD.noiDung = thucdon.NoiDung;
                }
            }
            return TDD;
        }
        //lấy id và số phiếu nhâp kho
        public List<HoaDonData> ListPHD()
        {
            List<HoaDonData> list = new List<HoaDonData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_hoadon
                        select new HoaDonData
                        {
                            maPhieuHD = m.MaPhieuHD,
                            soPhieuHD = m.SoPhieu
                        }).OrderByDescending(m => m.maPhieuHD).ToList();
            }
            return list;
        }
        //lấy dữ liệu một phiếu nhập kho
        public HoaDonData LayChiTietPhieuHD(int id)
        {
            HoaDonData PNK = new HoaDonData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var phieuHD = db.tbl_hoadon.Find(id);
                if (phieuHD != null)
                {
                    PNK.maPhieuHD = phieuHD.MaPhieuHD;
                    PNK.soPhieuHD = phieuHD.SoPhieu;
                    PNK.maThucDon = phieuHD.MaThucDon;
                    //lấy tên nhân viên nhận hàng
                    var nhanvien = db.tbl_nhanvien.Where(o => o.MaNV == phieuHD.NguoiLap).FirstOrDefault();

                    PNK.tenNguoiLap = nhanvien.Ho + nhanvien.Ten;
                    PNK.nguoiLap = phieuHD.NguoiLap;

                    PNK.ngayLap = phieuHD.NgayTao;
                    PNK.tongTien = phieuHD.TongTien;
                    //lấy thông tin nội dung của hóa đơn này
                    PNK.maThucDon = phieuHD.MaThucDon;
                    var thucdon = db.tbl_thucdon.Find(PNK.maThucDon);
                    PNK.noiDungTD = thucdon.NoiDung;

                    PNK.ghiChu = phieuHD.GhiChu;
                }
            }
            return PNK;
        }
        //Thêm mới một phiếu hóa đơn vào cơ sở dữ liệu
        public void ThemMoiPhieuHD(HoaDonData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    //thêm mới một phiếu nhập kho
                    tbl_hoadon phieuHD = new tbl_hoadon();

                    phieuHD.MaPhieuHD = obj.maPhieuHD;
                    phieuHD.SoPhieu = obj.soPhieuHD;
                    phieuHD.MaThucDon = obj.maThucDon;
                    phieuHD.NguoiLap = obj.nguoiLap;
                    phieuHD.NgayTao = obj.ngayLap;
                    phieuHD.TongTien = obj.tongTien;
                    phieuHD.GhiChu = obj.ghiChu;


                    db.tbl_hoadon.Add(phieuHD);
                    db.SaveChanges();
                    //thêm mới danh sách thực phẩm vào bảng chi tiết hóa đơn
                    //foreach (var i in ktp)
                    //{
                    //    tbl_hdchitiet ct = new tbl_hdchitiet();

                    //    ct.MaHDCT = i.maHDCT;
                    //    ct.MaPhieuHoaDon = phieuHD.MaPhieuHD;
                    //    ct.MaThucPham = i.maThucPham;
                    //    ct.NgayLap = obj.ngayLap;
                    //    ct.DonViTinh = i.donViTinh;
                    //    ct.SoLuong = i.soLuong;
                    //    ct.ThanhTien = i.soLuong * i.giaCa;
                    //    ct.GhiChu = i.ghiChu;

                    //    db.tbl_hdchitiet.Add(ct);
                    //    db.SaveChanges();
                    //}
                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        // chỉnh sửa thông tin phiếu nhập kho
        public void SuaPhieuHD(HoaDonData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_hoadon.Where(o => o.MaPhieuHD == obj.maPhieuHD);
                        if (timkiem.Any())
                        {
                            tbl_hoadon kq = timkiem.FirstOrDefault();
                            kq.MaThucDon = obj.maThucDon;
                            kq.SoPhieu = obj.soPhieuHD;
                            kq.NgayTao = obj.ngayLap;
                            kq.NguoiLap = obj.nguoiLap;
                            kq.TongTien = obj.tongTien;
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
        //xóa  một phiếu nhập kho
        public int XoaPhieuHD(int maPhieuHD)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //xóa thông tin trong bảng chi tiết hóa đơn
                        var ct = db.tbl_hdchitiet.Where(o => o.MaPhieuHoaDon == maPhieuHD);
                        if (ct.Any())
                        {
                            db.tbl_hdchitiet.RemoveRange(ct);
                            db.SaveChanges();
                        }
                        var ID = db.tbl_hoadon.Where(o => o.MaPhieuHD == maPhieuHD);
                        //Xóa id danh mục đã chọn trong bảng phiếu hóa đơn
                        if (ID.Any())
                        {
                            db.tbl_hoadon.Remove(ID.FirstOrDefault());
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
        }//kết thúc xóa một phiếu hóa đơn
    }
}