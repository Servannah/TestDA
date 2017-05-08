using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDA.DB;
using TestDA.Common;
using TestDA.Areas.Manager.Models.ViewModel;
using System.Data.Entity.Validation;
using System.Web.Mvc;

namespace TestDA.Areas.Manager.Models.EntityManager
{
    public class ThucPhamManager
    {
        //lấy tất cả danh sách danh mục trong csdl
        public List<ThucPhamData> LayDSThucPham(string inputtext, ref int totalRecord, int pageIndex = 1, int pageSize = 10)
        {

            List<ThucPhamData> list = new List<ThucPhamData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //ref :: ở bên ngoài truyền vào
                //lấy tổng bản ghi
                totalRecord = db.tbl_thucpham.Count();
                //lấy danh sách thực phẩm
                if (!String.IsNullOrEmpty(inputtext))
                {
                    list = (from m in db.tbl_thucpham
                            where (m.TenThucPham.Contains(inputtext))
                            select new ThucPhamData
                            {
                                maThucPham = m.MaThucPham,
                                tenThucPham = m.TenThucPham,
                                nguonTuDV = m.NguonTuDV,
                                tyLeHapThu = m.TyLeHapThu,
                                tyLeThai = m.TyLeThai,
                                nangLuongCalo = m.NangLuongCalo,
                                tphhNuoc = m.TphhNuoc,
                                tphhProtid = m.TphhProtid,
                                tphhLipid = m.TphhLipid,
                                tphhGlucid = m.TphhGlucid,
                                tphhCellulose = m.TphhCellulose,
                                tphhCholesterol = m.TphhCholesterol,
                                mkCalci = m.MkCalci,
                                mkPhotpho = m.MkPhotpho,
                                mkSat = m.MkSat,
                                vitaminCaroten = m.VitaminCaroten,
                                vitaminA = m.VitaminA,
                                vitaminB1 = m.VitaminB1,
                                vitaminB2 = m.VitaminB2,
                                vitaminC = m.VitaminC,
                                vitaminPP = m.VitaminPP,
                                ghiChu = m.GhiChu
                            }).OrderByDescending(m => m.maThucPham).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    list = db.tbl_thucpham.Select(o => new ThucPhamData
                    {
                        maThucPham = o.MaThucPham,
                        tenThucPham = o.TenThucPham,
                        nguonTuDV = o.NguonTuDV,
                        tyLeHapThu = o.TyLeHapThu,
                        tyLeThai = o.TyLeThai,
                        nangLuongCalo = o.NangLuongCalo,
                        tphhNuoc = o.TphhNuoc,
                        tphhProtid = o.TphhProtid,
                        tphhLipid = o.TphhLipid,
                        tphhGlucid = o.TphhGlucid,
                        tphhCellulose = o.TphhCellulose,
                        tphhCholesterol = o.TphhCholesterol,
                        mkCalci = o.MkCalci,
                        mkPhotpho = o.MkPhotpho,
                        mkSat = o.MkSat,
                        vitaminCaroten = o.VitaminCaroten,
                        vitaminA = o.VitaminA,
                        vitaminB1 = o.VitaminB1,
                        vitaminB2 = o.VitaminB2,
                        vitaminC = o.VitaminC,
                        vitaminPP = o.VitaminPP,
                        ghiChu = o.GhiChu
                    }).OrderByDescending(o => o.maThucPham).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
            }
            return list;
        }//
        //lấy danh sách thực phẩm
        public List<ThucPhamData> ListThucPham()
        {
            List<ThucPhamData> list = new List<ThucPhamData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = db.tbl_thucpham.Select(o => new ThucPhamData
                    {
                        maThucPham = o.MaThucPham,
                        tenThucPham = o.TenThucPham
                    }).OrderByDescending(o => o.maThucPham).ToList();
            }
            return list;

        }
        /////lấy dữ liệu một thực phẩm
        public ThucPhamData LayChiTietTP(int maTP)
        {
            ThucPhamData TP = new ThucPhamData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var thucpham = db.tbl_thucpham.Find(maTP);
                if (thucpham != null)
                {
                    //lấy tên nhóm thực phẩm
                    var nhomTP = (from n in db.tbl_nhomthucpham
                                  join t in db.tbl_thucpham on n.MaNhomTP equals t.MaNhomTP
                                  where t.MaThucPham == maTP
                                  select new { tenNhomTP = n.TenNhomTP, maNhomTP = n.MaNhomTP }).FirstOrDefault();

                    TP.tenNhomTP = nhomTP.tenNhomTP;
                    TP.maNhomTP = nhomTP.maNhomTP;

                    TP.maThucPham = thucpham.MaThucPham;
                    TP.tenThucPham = thucpham.TenThucPham;
                    TP.nguonTuDV = thucpham.NguonTuDV;
                    TP.tyLeHapThu = thucpham.TyLeHapThu;
                    TP.tyLeThai = thucpham.TyLeThai;
                    TP.nangLuongCalo = thucpham.NangLuongCalo;
                    TP.tphhNuoc = thucpham.TphhNuoc;
                    TP.tphhProtid = thucpham.TphhProtid;
                    TP.tphhLipid = thucpham.TphhLipid;
                    TP.tphhGlucid = thucpham.TphhGlucid;
                    TP.tphhCellulose = thucpham.TphhCellulose;
                    TP.tphhCholesterol = thucpham.TphhCholesterol;
                    TP.mkCalci = thucpham.MkCalci;
                    TP.mkPhotpho = thucpham.MkPhotpho;
                    TP.mkSat = thucpham.MkSat;
                    TP.vitaminCaroten = thucpham.VitaminCaroten;
                    TP.vitaminA = thucpham.VitaminA;
                    TP.vitaminB1 = thucpham.VitaminB1;
                    TP.vitaminB2 = thucpham.VitaminB2;
                    TP.vitaminC = thucpham.VitaminC;
                    TP.vitaminPP = thucpham.VitaminPP;
                    TP.ghiChu = thucpham.GhiChu;
                }
            }
            return TP;
        }
        /////
        //Tìm kiếm thông tin thực phẩm
        ////tìm kiếm theo từ khóa nhập vào
        //public List<ThucPhamData> TimKiemThucPham(string inputtext)
        //{
        //    List<ThucPhamData> kq = new List<ThucPhamData>();
        //    using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
        //    {
        //        //lấy tổng số bản ghi
        //        int totalRecord = db.tbl_thucpham.Count();
        //        int pageIndex = 1;
        //        int pageSize = 10;
        //        //lấy danh sách nhóm tin
        //        kq = LayDSThucPham();
        //        if (!String.IsNullOrEmpty(inputtext))
        //        {
        //            kq = (from m in db.tbl_thucpham
        //                  where (m.TenThucPham.Contains(inputtext))
        //                  select new ThucPhamData
        //                  {
        //                      maThucPham = m.MaThucPham,
        //                      tenThucPham = m.TenThucPham,
        //                      tyLeHapThu = m.TyLeHapThu,
        //                      tyLeThai = m.TyLeThai,
        //                      nangLuongCalo = m.NangLuongCalo,
        //                      tphhNuoc = m.TphhNuoc,
        //                      tphhProtid = m.TphhProtid,
        //                      tphhLipid = m.TphhLipid,
        //                      tphhGlucid = m.TphhGlucid,
        //                      tphhCellulose = m.TphhCellulose,
        //                      tphhCholesterol = m.TphhCholesterol,
        //                      mkCalci = m.MkCalci,
        //                      mkPhotpho = m.MkPhotpho,
        //                      mkSat = m.MkSat,
        //                      vitaminCaroten = m.VitaminCaroten,
        //                      vitaminA = m.VitaminA,
        //                      vitaminB1 = m.VitaminB1,
        //                      vitaminB2 = m.VitaminB2,
        //                      vitaminC = m.VitaminC,
        //                      vitaminPP = m.VitaminPP,
        //                      ghiChu = m.GhiChu
        //                  }).OrderByDescending(m => m.maThucPham).ToList();
        //        }
        //    }
        //    return kq;
        //}
        /////////
        //Thêm mới một thực phẩm
        public void ThemMoiThucPham(ThucPhamData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_thucpham thucpham = new tbl_thucpham();
                    thucpham.MaThucPham = obj.maThucPham;
                    thucpham.MaNhomTP = obj.maNhomTP;
                    thucpham.TenThucPham = obj.tenThucPham;
                    thucpham.NguonTuDV = obj.nguonTuDV;
                    thucpham.TyLeHapThu = obj.tyLeHapThu;
                    thucpham.TyLeThai = obj.tyLeThai;
                    thucpham.NangLuongCalo = obj.nangLuongCalo;
                    thucpham.TphhNuoc = obj.tphhNuoc;
                    thucpham.TphhProtid = obj.tphhProtid;
                    thucpham.TphhLipid = obj.tphhLipid;
                    thucpham.TphhGlucid = obj.tphhGlucid;
                    thucpham.TphhCellulose = obj.tphhCellulose;
                    thucpham.TphhCholesterol = obj.tphhCholesterol;
                    thucpham.MkCalci = obj.mkCalci;
                    thucpham.MkPhotpho = obj.mkPhotpho;
                    thucpham.MkSat = obj.mkSat;
                    thucpham.VitaminCaroten = obj.vitaminCaroten;
                    thucpham.VitaminA = obj.vitaminA;
                    thucpham.VitaminB1 = obj.vitaminB1;
                    thucpham.VitaminB2 = obj.vitaminB2;
                    thucpham.VitaminC = obj.vitaminC;
                    thucpham.VitaminPP = obj.vitaminPP;
                    thucpham.GhiChu = obj.ghiChu;
                    //lưu thông tin thực phẩm vào csdl
                    db.tbl_thucpham.Add(thucpham);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        // chỉnh sửa thông tin thực phẩm 
        public void SuaThucPham(ThucPhamData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_thucpham.Where(o => o.MaThucPham == obj.maThucPham);
                        if (timkiem.Any())
                        {
                            tbl_thucpham kq = timkiem.FirstOrDefault();
                            kq.TenThucPham = obj.tenThucPham;
                            kq.NguonTuDV = obj.nguonTuDV;
                            kq.TyLeHapThu = obj.tyLeHapThu;
                            kq.TyLeThai = obj.tyLeThai;
                            kq.NangLuongCalo = obj.nangLuongCalo;
                            kq.TphhNuoc = obj.tphhNuoc;
                            kq.TphhProtid = obj.tphhProtid;
                            kq.TphhLipid = obj.tphhLipid;
                            kq.TphhGlucid = obj.tphhGlucid;
                            kq.TphhCellulose = obj.tphhCellulose;
                            kq.TphhCholesterol = obj.tphhCholesterol;
                            kq.MkCalci = obj.mkCalci;
                            kq.MkPhotpho = obj.mkPhotpho;
                            kq.MkSat = obj.mkSat;
                            kq.VitaminCaroten = obj.vitaminCaroten;
                            kq.VitaminA = obj.vitaminA;
                            kq.VitaminB1 = obj.vitaminB1;
                            kq.VitaminB2 = obj.vitaminB2;
                            kq.VitaminC = obj.vitaminC;
                            kq.VitaminPP = obj.vitaminPP;
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
        ///
        //Xóa thông tin thực phẩm
        public int XoaThucPham(int maThucPham)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //kiểm tra thực phẩm này có trong bảng hóa đơn chi tiết hay không
                        var thucpham = db.tbl_hdchitiet.Where(o => o.MaThucPham == maThucPham);
                        if (thucpham.Any())
                        {
                            return 1;
                        }
                        //kiểm tra thực phẩm này có trong bảng thành phần món ăn hay không
                        var tpma = db.tbl_thanhphanmonan.Where(o => o.MaThucPham == maThucPham);
                        if (tpma.Any())
                        {
                            return 4;
                        }
                        else
                        {
                            var ID = db.tbl_thucpham.Where(o => o.MaThucPham == maThucPham);
                            //Xóa id danh mục đã chọn trong bảng mn_term
                            if (ID.Any())
                            {
                                db.tbl_thucpham.Remove(ID.FirstOrDefault());
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

        //////
    }
}