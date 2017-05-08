using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDA.DB;
using System.Data.Entity.Validation;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TestDA.Common;
using TestDA.Areas.Manager.Models.ViewModel;

namespace TestDA.Areas.Manager.Models.EntityManager
{
    public class CTHoaDonManager
    {
        //lấy tất cả danh sách kho thực phẩm trong csdl
        public List<CTHoaDonData> LayDSCTHD(string tukhoa, ref int totalRecord, int pageIndex = 1, int pageSize = 10)
        {
            List<CTHoaDonData> list = new List<CTHoaDonData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //ref :: ở bên ngoài truyền vào
                //lấy tổng bản ghi
                totalRecord = db.tbl_hdchitiet.Count();
                //lấy danh sách 
                    list = (from m in db.tbl_hdchitiet
                            join p in db.tbl_hoadon on m.MaPhieuHoaDon equals p.MaPhieuHD
                            join t in db.tbl_thucpham on m.MaThucPham equals t.MaThucPham
                            select new CTHoaDonData
                            {
                                maHDCT = m.MaHDCT,
                                soPhieuHD = m.MaPhieuHoaDon + " - " + p.SoPhieu,
                                tenThucPham = t.TenThucPham,
                                ngayTao = m.NgayLap,
                                donViTinh = m.DonViTinh,
                                
                                giaCa = t.GiaCa,
                                soLuong = m.SoLuong,
                                thanhTien = m.ThanhTien,
                                ghiChu = m.GhiChu
                            }).OrderByDescending(m => m.maHDCT).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
               
            }
            return list;
        }
        //lấy dữ liệu một thực phẩm trong kho
        //lấy dữ liệu theo id trong kho
        public CTHoaDonData LayChiTietHD(int id)
        {
            CTHoaDonData KTP = new CTHoaDonData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var hdCT = db.tbl_hdchitiet.Find(id);
                if (hdCT != null)
                {
                    //lấy số phiếu hóa đơn                                                  
                    var phieuHD = db.tbl_hoadon.Where(o => o.MaPhieuHD == hdCT.MaPhieuHoaDon).FirstOrDefault();
                    KTP.soPhieuHD = phieuHD.MaPhieuHD + " - " + phieuHD.SoPhieu;
                    KTP.maHDCT = hdCT.MaHDCT;
                    //lấy tên của thực phẩm
                    var thucPham = db.tbl_thucpham.Where(o => o.MaThucPham == hdCT.MaThucPham).FirstOrDefault();
                    KTP.tenThucPham = thucPham.TenThucPham;
                    KTP.maThucPham = thucPham.MaThucPham;
                    KTP.ngayTao = hdCT.NgayLap;
                    //đơn vị tính chuyển hết về gam
                    KTP.soLuong = hdCT.SoLuong;
                    KTP.thanhTien = hdCT.ThanhTien;
                    KTP.ghiChu = hdCT.GhiChu;
                }
            }
            return KTP;
        }
        ////Lấy danh sách thực phẩm theo mã hóa đơn chi tiết
        public List<CTHoaDonData> DSThucPhamTheoPhieuHD(int maHDCT)
        {
            List<CTHoaDonData> list = new List<CTHoaDonData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_hdchitiet
                        join p in db.tbl_hoadon on m.MaPhieuHoaDon equals p.MaPhieuHD
                        join t in db.tbl_thucpham on m.MaThucPham equals t.MaThucPham
                        where p.MaPhieuHD == maHDCT
                        select new CTHoaDonData
                        {
                            maHDCT = m.MaHDCT,
                            soPhieuHD = m.MaPhieuHoaDon + " - " + p.SoPhieu,
                            tenThucPham = t.TenThucPham,
                            ngayTao = m.NgayLap,
                            maThucPham = m.MaThucPham,
                            donViTinh = m.DonViTinh,
                            soLuong = m.SoLuong,
                            giaCa = t.GiaCa,
                            thanhTien = m.ThanhTien,
                            ghiChu = m.GhiChu
                        }).OrderByDescending(m => m.maHDCT).ToList();
            }
            return list;
        }
        //tính lại tổng tiền trong phiếu nhập kho
        public HoaDonData TinhTongTien(int maHoaDon)
        {
            HoaDonData data = new HoaDonData();
            data.tongTien = 0;
            //lấy danh sách thực phẩm của hóa đơn này
            List<CTHoaDonData> list = DSThucPhamTheoPhieuHD(maHoaDon);
            foreach (var item in list)
            {
                data.tongTien += item.thanhTien;
            }
            return data;
        }

        //Thêm mới một thực phẩm vào hóa đơn chi tiết
        public void ThemMoiTP(CTHoaDonData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_hdchitiet hdct = new tbl_hdchitiet();

                    hdct.MaHDCT = obj.maHDCT;
                    hdct.MaPhieuHoaDon = obj.maHoaDon;
                    hdct.MaThucPham = obj.maThucPham;
                    //lấy ngày tạo hóa đơn trong bảng hóa đơn
                    var hD = db.tbl_hoadon.Where(o => o.MaPhieuHD == hdct.MaPhieuHoaDon).FirstOrDefault() ;
                    hdct.NgayLap = hD.NgayTao;
                    hdct.DonViTinh = "kg";
                    hdct.SoLuong = obj.soLuong;
                    //lấy thông tin giá cả của thực phẩm
                    var id = db.tbl_thucpham.Where(o=>o.MaThucPham==obj.maThucPham).FirstOrDefault();
                    hdct.ThanhTien = obj.soLuong*id.GiaCa;
                    hdct.GhiChu = obj.ghiChu;

                    db.tbl_hdchitiet.Add(hdct);
                    db.SaveChanges();

                    //tính lại tổng tiền trong hóa đơn
                    var hd = db.tbl_hoadon.Where(o => o.MaPhieuHD == obj.maHoaDon);
                    if (hd.Any())
                    {
                        tbl_hoadon HoaDon = hd.FirstOrDefault();
                        HoaDonData da = TinhTongTien(hd.FirstOrDefault().MaPhieuHD);

                        HoaDon.MaPhieuHD = hd.FirstOrDefault().MaPhieuHD;
                        HoaDon.TongTien = da.tongTien;

                        db.SaveChanges();
                    }

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        // chỉnh sửa thông tin thực phẩm trong hóa đơn chi tiết
        public void SuaHDCT(CTHoaDonData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_hdchitiet.Where(o => o.MaHDCT == obj.maHDCT);
                        if (timkiem.Any())
                        {
                            tbl_hdchitiet kq = timkiem.FirstOrDefault();

                            kq.MaHDCT = obj.maHDCT;
                            kq.MaThucPham = obj.maThucPham;
                            kq.NgayLap = obj.ngayTao;
                            kq.DonViTinh = "kg";//mặc định cho các đơn vị tính của thực phẩm là gam
                            kq.SoLuong = obj.soLuong;
                            //lấy thông tin giá cả của thực phẩm
                            var id = db.tbl_thucpham.Where(o => o.MaThucPham == obj.maThucPham).FirstOrDefault();
                            kq.ThanhTien = obj.soLuong * id.GiaCa;
                            kq.ThanhTien = obj.thanhTien;
                            kq.GhiChu = obj.ghiChu;

                            db.SaveChanges();

                            //cần update lại tổng tiền trong phiếu nhập kho
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
        //xóa  một thực phẩm trong hóa đơn chi tiết
        public int XoaCTTP(int maHDCT)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var ID = db.tbl_hdchitiet.Where(o => o.MaHDCT == maHDCT);
                        var maHD = ID.FirstOrDefault().MaPhieuHoaDon;
                        //Xóa id danh mục đã chọn trong bảng kho thực phẩm
                        if (ID.Any())
                        {
                            db.tbl_hdchitiet.Remove(ID.FirstOrDefault());
                            db.SaveChanges();
                            //cần update lại tổng tiền trong phiếu nhập kho
                        }

                        dbContextTransaction.Commit();

                        //tính lại tổng tiền trong hóa đơn
                        var hd = db.tbl_hoadon.Where(o => o.MaPhieuHD == maHD);
                        if (hd.Any())
                        {
                            tbl_hoadon HoaDon = hd.FirstOrDefault();
                            HoaDonData da = TinhTongTien(hd.FirstOrDefault().MaPhieuHD);

                            HoaDon.MaPhieuHD = hd.FirstOrDefault().MaPhieuHD;
                            HoaDon.TongTien = da.tongTien;

                            db.SaveChanges();
                        }//kết thúc tính tổng tiền

                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
                return 0;
            }
        }//kết thúc xóa thực phẩm trong phiếu chi tiết
    }
}