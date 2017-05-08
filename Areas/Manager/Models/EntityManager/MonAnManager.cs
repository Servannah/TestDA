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
    public class MonAnManager
    {
        //lấy tất cả danh sách danh mục trong csdl
        public List<MonAnData> LayDSMonAn(string inputtext, ref int totalRecord, int pageIndex = 1, int pageSize = 10)
        {

            List<MonAnData> list = new List<MonAnData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //ref :: ở bên ngoài truyền vào
                //lấy tổng bản ghi
                totalRecord = db.tbl_monan.Count();
                //lấy danh sách thực phẩm
                if (!String.IsNullOrEmpty(inputtext))
                {
                    list = (from m in db.tbl_monan
                            where (m.TenMonAn.Contains(inputtext))
                            select new MonAnData
                            {
                                maMonAn = m.MaMonAn,
                                tenMonAn = m.TenMonAn,
                                soLuongNguoi = m.SoLuongNguoi,
                                chuanBi = m.ChuanBi,
                                cheBien = m.CheBien,
                                tongProtidDV = m.TongProtidDV,
                                tongProtidTV = m.TongProtidTV,
                                tongLipidDV = m.TongLipidDV,
                                tongLipidTV = m.TongLipidTV,
                                tongGlucid = m.TongGlucid,
                                tongCalo = m.TongCalo,
                                tongCanxi = m.TongCanxi,
                                tongPhotpho = m.TongPhotpho,
                                tongSat = m.TongSat,
                                tongVitaminA = m.TongVitaminA,
                                tongVitaminB1 = m.TongVitaminB1,
                                tongVitaminB2 = m.TongVitaminB2,
                                tongVitaminPP = m.TongVitaminPP,
                                tongVitaminC = m.TongVitaminC,
                                ghiChu = m.GhiChu
                            }).OrderByDescending(m => m.maMonAn).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
                else
                {
                    list = db.tbl_monan.Select(m => new MonAnData
                    {
                        maMonAn = m.MaMonAn,
                        tenMonAn = m.TenMonAn,
                        soLuongNguoi = m.SoLuongNguoi,
                        chuanBi = m.ChuanBi,
                        cheBien = m.CheBien,
                        tongProtidDV = m.TongProtidDV,
                        tongProtidTV = m.TongProtidTV,
                        tongLipidDV = m.TongLipidDV,
                        tongLipidTV = m.TongLipidTV,
                        tongGlucid = m.TongGlucid,
                        tongCalo = m.TongCalo,
                        tongCanxi = m.TongCanxi,
                        tongPhotpho = m.TongPhotpho,
                        tongSat = m.TongSat,
                        tongVitaminA = m.TongVitaminA,
                        tongVitaminB1 = m.TongVitaminB1,
                        tongVitaminB2 = m.TongVitaminB2,
                        tongVitaminPP = m.TongVitaminPP,
                        tongVitaminC = m.TongVitaminC,
                        ghiChu = m.GhiChu
                    }).OrderByDescending(m => m.maMonAn).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
            }
            return list;
        }//kết thúc lấy danh sách món ăn
        //lấy danh sách mónaăn theo id, ten
        public List<MonAnData> ListMonAn()
        {

            List<MonAnData> list = new List<MonAnData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = db.tbl_monan.Select(m => new MonAnData
                {
                    maMonAn = m.MaMonAn,
                    tenMonAn = m.TenMonAn
                }).OrderByDescending(m => m.maMonAn).ToList();
            }
            return list;
        }
        //lấy danh sách thành phần  món ăn
        public List<TPMonAn> danhSachTPMonAn(int maMonAn)
        {
            List<TPMonAn> list = new List<TPMonAn>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_monan
                        join tp in db.tbl_thanhphanmonan on m.MaMonAn equals tp.MaMonAn
                        join f in db.tbl_thucpham on tp.MaThucPham equals f.MaThucPham
                        where m.MaMonAn == maMonAn
                        select new TPMonAn
                        {
                            maMonAnCT = tp.MaMonAnCT,
                            maMonAn = m.MaMonAn,
                            tenMonAn = m.MaMonAn + "-" + m.TenMonAn,
                            maThucPham = f.MaThucPham,
                            tenThucPham = f.TenThucPham,
                            soLuongGam = tp.SoLuongGam,
                            protidDV = tp.ProtidDV,
                            protidTV = tp.ProtidTV,
                            lipidDV = tp.LipidDV,
                            lipidTV = tp.LipidTV,
                            glucid = tp.Glucid,
                            calo = tp.Calo,
                            canxi = tp.Canxi,
                            photPho = tp.PhotPho,
                            sat = tp.Sat,
                            vitaminA = tp.VitaminA,
                            vitaminB1 = tp.VitaminB1,
                            vitaminB2 = tp.VitaminB2,
                            vitaminPP = tp.VitaminPP,
                            vitaminC = tp.VitaminC,
                            ghiChu = tp.GhiChu

                        }).OrderByDescending(m => m.maThucPham).ToList();
            }
            return list;
        }//kết thúc lấy danh sách thành phần món ăn
        //tính lại tổng dinh dưỡng cho món ăn
        public MonAnData TinhDinhDuongMonAn(int maMonAn)
        {
            MonAnData data = new MonAnData();

            data.tongProtidDV = 0;
            data.tongProtidTV = 0;
            data.tongLipidDV = 0;
            data.tongLipidTV = 0;
            data.tongGlucid = 0;
            data.tongCalo = 0;
            data.tongCanxi = 0;
            data.tongPhotpho = 0;
            data.tongSat = 0;
            data.tongVitaminA = 0;
            data.tongVitaminB1 = 0;
            data.tongVitaminB2 = 0;
            data.tongVitaminPP = 0;
            data.tongVitaminC = 0;

            List<TPMonAn> list = danhSachTPMonAn(maMonAn);

            foreach (var item in list)
            {
                data.tongProtidDV += item.protidDV;
                data.tongProtidTV += item.protidTV;
                data.tongLipidDV += item.lipidDV;
                data.tongLipidTV += item.lipidTV;
                data.tongGlucid += item.glucid;
                data.tongCalo += item.calo;
                data.tongCanxi += item.canxi;
                data.tongPhotpho += item.photPho;
                data.tongSat += item.sat;
                data.tongVitaminA += item.vitaminA;
                data.tongVitaminB1 += item.vitaminB1;
                data.tongVitaminB2 += item.vitaminB2;
                data.tongVitaminPP += item.vitaminPP;
                data.tongVitaminC += item.vitaminC;
            }
            return data;
        }

        //lấy chi tiết thông tin món ăn
        public MonAnData LayChiTietMonAn(int id)
        {
            MonAnData LMA = new MonAnData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var monAn = db.tbl_monan.Find(id);
                if (monAn != null)
                {
                    LMA.maMonAn = monAn.MaMonAn;
                    LMA.maLoaiMonAn = monAn.MaLoaiMonAn;
                    LMA.tenMonAn = monAn.TenMonAn;
                    LMA.soLuongNguoi = monAn.SoLuongNguoi;
                    LMA.chuanBi = monAn.ChuanBi;
                    LMA.cheBien = monAn.CheBien;
                    //tính lại thông tin của thành phần dinh dưỡng món ăn
                    MonAnData da = TinhDinhDuongMonAn(id);

                    LMA.tongProtidDV = da.tongProtidDV;
                    LMA.tongProtidTV = da.tongProtidTV;
                    LMA.tongLipidDV = da.tongLipidDV;
                    LMA.tongLipidTV = da.tongLipidTV;
                    LMA.tongGlucid = da.tongGlucid;
                    LMA.tongCalo = da.tongCalo;
                    LMA.tongCanxi = da.tongCanxi;
                    LMA.tongPhotpho = da.tongPhotpho;
                    LMA.tongSat = da.tongSat;
                    LMA.tongVitaminA = da.tongVitaminA;
                    LMA.tongVitaminB1 = da.tongVitaminB1;
                    LMA.tongVitaminB2 = da.tongVitaminB2;
                    LMA.tongVitaminPP = da.tongVitaminPP;
                    LMA.tongVitaminC = da.tongVitaminC;

                    LMA.ghiChu = monAn.GhiChu;

                }
            }
            return LMA;
        }//kết thúc lấy chi tiết bản ghi
        //thêm mới một bản ghi
        public void ThemMoiMonAn(MonAnData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_monan monAn = new tbl_monan();

                    monAn.MaMonAn = obj.maMonAn;
                    monAn.MaLoaiMonAn = obj.maLoaiMonAn;
                    monAn.TenMonAn = obj.tenMonAn;
                    monAn.SoLuongNguoi = obj.soLuongNguoi;
                    monAn.ChuanBi = obj.chuanBi;
                    monAn.CheBien = obj.cheBien;
                    monAn.TongProtidDV = obj.tongProtidDV;
                    monAn.TongProtidTV = obj.tongProtidTV;
                    monAn.TongLipidDV = obj.tongLipidDV;
                    monAn.TongLipidTV = obj.tongLipidTV;
                    monAn.TongGlucid = obj.tongGlucid;
                    monAn.TongCalo = obj.tongCalo;
                    monAn.TongCanxi = obj.tongCanxi;
                    monAn.TongPhotpho = obj.tongPhotpho;
                    monAn.TongSat = obj.tongSat;
                    monAn.TongVitaminA = obj.tongVitaminA;
                    monAn.TongVitaminB1 = obj.tongVitaminB1;
                    monAn.TongVitaminB2 = obj.tongVitaminB2;
                    monAn.TongVitaminPP = obj.tongVitaminPP;
                    monAn.TongVitaminPP = obj.tongVitaminPP;
                    monAn.GhiChu = obj.ghiChu;

                    db.tbl_monan.Add(monAn);

                    db.SaveChanges();

                    //thêm dữ liệu thành phần món ăn vào bảng

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }///
        //sửa thông tin bản ghi
        public void SuaMonAn(MonAnData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_monan.Where(o => o.MaMonAn == obj.maMonAn);
                        if (timkiem.Any())
                        {
                            tbl_monan kq = timkiem.FirstOrDefault();

                            kq.MaMonAn = obj.maMonAn;
                            kq.MaLoaiMonAn = obj.maLoaiMonAn;
                            kq.TenMonAn = obj.tenMonAn;
                            kq.SoLuongNguoi = obj.soLuongNguoi;
                            kq.ChuanBi = obj.chuanBi;
                            kq.CheBien = obj.cheBien;
                            kq.TongProtidDV = obj.tongProtidDV;
                            kq.TongProtidTV = obj.tongProtidTV;
                            kq.TongLipidDV = obj.tongLipidDV;
                            kq.TongLipidTV = obj.tongLipidTV;
                            kq.TongGlucid = obj.tongGlucid;
                            kq.TongCalo = obj.tongCalo;
                            kq.TongCanxi = obj.tongCanxi;
                            kq.TongPhotpho = obj.tongPhotpho;
                            kq.TongSat = obj.tongSat;
                            kq.TongVitaminA = obj.tongVitaminA;
                            kq.TongVitaminB1 = obj.tongVitaminB1;
                            kq.TongVitaminB2 = obj.tongVitaminB2;
                            kq.TongVitaminPP = obj.tongVitaminPP;
                            kq.TongVitaminPP = obj.tongVitaminPP;
                            kq.GhiChu = obj.ghiChu;

                            db.SaveChanges();

                            //sửa thông tin thành phần món ăn
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

        //kết thúc sửa thông tin bản ghi
        //xóa thông tin bản ghi
        public int XoaMonAn(int maMonAn)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //kiểm tra món ăn có chứa trong bảng tbl_thanhphanmonan hay không.
                        var tpmonan = db.tbl_thanhphanmonan.Where(o => o.MaMonAn == maMonAn);
                        if (tpmonan.Any())
                        {
                            //thực hiện xóa tất cả thành phần món ăn của món đó
                            db.tbl_thanhphanmonan.RemoveRange(tpmonan);
                            db.SaveChanges();
                        }
                        //cần kiểm tra xem món ăn này có chứa trong bảng thực đơn hay không
                        else
                        {
                            var ID = db.tbl_monan.Where(o => o.MaMonAn == maMonAn);
                            //Xóa id danh mục đã chọn 
                            if (ID.Any())
                            {
                                db.tbl_monan.Remove(ID.FirstOrDefault());
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
        //kết thúc xóa thông tin bản ghi
        //thêm thành phần món ăn vào bảng dữ liêu
        public void ThemTPMonAn(int maMonAn, int maThucPham, double soLuong)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_thanhphanmonan tp = new tbl_thanhphanmonan();

                    //tp.MaMonAnCT = obj.maMonAnCT;
                    tp.MaMonAn = maMonAn;
                    tp.MaThucPham = maThucPham;
                    tp.SoLuongGam = soLuong;
                    //kiểm tra đó là thực phẩm từ động vật hay thực phẩm từ thực vật
                    var TP = db.tbl_thucpham.Find(maThucPham);
                    var kt = TP.NguonTuDV;
                    if (kt == true)
                    {
                        tp.ProtidDV = TP.TphhProtid * soLuong / 100;
                        tp.LipidDV = TP.TphhLipid * soLuong / 100;
                        tp.ProtidTV = 0;
                        tp.LipidTV = 0;

                    }
                    else
                    {
                        tp.ProtidDV = 0;
                        tp.LipidDV = 0;
                        tp.ProtidTV = TP.TphhProtid * soLuong / 100;
                        tp.LipidTV = TP.TphhLipid * soLuong / 100;
                    }
                    //
                    tp.Glucid = TP.TphhGlucid * soLuong / 100;
                    tp.Calo = TP.NangLuongCalo * soLuong / 100;
                    tp.Canxi = TP.MkCalci * soLuong / 100;
                    tp.PhotPho = TP.MkPhotpho * soLuong / 100;
                    tp.Sat = TP.MkSat * soLuong / 100;
                    tp.VitaminA = TP.VitaminA * soLuong / 100;
                    tp.VitaminB1 = TP.VitaminB1 * soLuong / 100;
                    tp.VitaminB2 = TP.VitaminB2 * soLuong / 100;
                    tp.VitaminPP = TP.VitaminPP * soLuong / 100;
                    tp.VitaminC = TP.VitaminC * soLuong / 100;

                    db.tbl_thanhphanmonan.Add(tp);

                    db.SaveChanges();

                    //tính lại thông tin của thành phần dinh dưỡng món ăn
                    var ma = db.tbl_monan.Where(o => o.MaMonAn == maMonAn);
                    if (ma.Any())
                    {
                        tbl_monan MA = ma.FirstOrDefault();
                        MonAnData da = TinhDinhDuongMonAn(maMonAn);
                        MA.MaMonAn = maMonAn;
                        MA.TongProtidDV = da.tongProtidDV;
                        MA.TongProtidTV = da.tongProtidTV;
                        MA.TongLipidDV = da.tongLipidDV;
                        MA.TongLipidTV = da.tongLipidTV;
                        MA.TongGlucid = da.tongGlucid;
                        MA.TongCalo = da.tongCalo;
                        MA.TongCanxi = da.tongCanxi;
                        MA.TongPhotpho = da.tongPhotpho;
                        MA.TongSat = da.tongSat;
                        MA.TongVitaminA = da.tongVitaminA;
                        MA.TongVitaminB1 = da.tongVitaminB1;
                        MA.TongVitaminB2 = da.tongVitaminB2;
                        MA.TongVitaminPP = da.tongVitaminPP;
                        MA.TongVitaminPP = da.tongVitaminPP;

                        db.SaveChanges();
                    }


                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }///
        //sửa thành phần món ăn vào bảng dữ liêu
        public void SuaTPMonAn(int maMonAnCT, int maThucPham, double soLuongGam)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    var timkiem = db.tbl_thanhphanmonan.Where(o => o.MaMonAnCT == maMonAnCT && o.MaThucPham == maThucPham);
                    if (timkiem.Any())
                    {
                        tbl_thanhphanmonan tp = timkiem.FirstOrDefault();

                        tp.MaMonAnCT = maMonAnCT;
                        tp.MaThucPham = maThucPham;
                        tp.SoLuongGam = soLuongGam;
                        tp.ProtidDV = tp.ProtidDV * soLuongGam / 100;
                        tp.LipidDV = tp.LipidDV * soLuongGam / 100;
                        tp.ProtidTV = tp.ProtidTV * soLuongGam / 100;
                        tp.LipidTV = tp.LipidTV * soLuongGam / 100;
                        tp.Glucid = tp.Glucid * soLuongGam / 100;
                        tp.Calo = tp.Calo * soLuongGam / 100;
                        tp.Canxi = tp.Canxi * soLuongGam / 100;
                        tp.PhotPho = tp.PhotPho * soLuongGam / 100;
                        tp.Sat = tp.Sat * soLuongGam / 100;
                        tp.VitaminA = tp.VitaminA * soLuongGam / 100;
                        tp.VitaminB1 = tp.VitaminB1 * soLuongGam / 100;
                        tp.VitaminB2 = tp.VitaminB2 * soLuongGam / 100;
                        tp.VitaminPP = tp.VitaminPP * soLuongGam / 100;
                        tp.VitaminC = tp.VitaminC * soLuongGam / 100;

                        db.SaveChanges();


                        //tính lại thông tin của thành phần dinh dưỡng món ăn
                        var ma = db.tbl_monan.Where(o => o.MaMonAn == tp.MaMonAn);
                        if (ma.Any())
                        {
                            tbl_monan MA = ma.FirstOrDefault();

                            MonAnData da = TinhDinhDuongMonAn(MA.MaMonAn);

                            MA.MaMonAn = MA.MaMonAn;
                            MA.TongProtidDV = da.tongProtidDV;
                            MA.TongProtidTV = da.tongProtidTV;
                            MA.TongLipidDV = da.tongLipidDV;
                            MA.TongLipidTV = da.tongLipidTV;
                            MA.TongGlucid = da.tongGlucid;
                            MA.TongCalo = da.tongCalo;
                            MA.TongCanxi = da.tongCanxi;
                            MA.TongPhotpho = da.tongPhotpho;
                            MA.TongSat = da.tongSat;
                            MA.TongVitaminA = da.tongVitaminA;
                            MA.TongVitaminB1 = da.tongVitaminB1;
                            MA.TongVitaminB2 = da.tongVitaminB2;
                            MA.TongVitaminPP = da.tongVitaminPP;
                            MA.TongVitaminPP = da.tongVitaminPP;

                            db.SaveChanges();
                        }
                    }
                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }///
        //xóa bỏ thông tin thành phần món ăn
        public int XoaTPMonAn(int maMonAnCT)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var ID = db.tbl_thanhphanmonan.Where(o => o.MaMonAnCT == maMonAnCT);
                        var maMonAn = ID.FirstOrDefault().MaMonAn;
                        //Xóa id danh mục đã chọn 
                        if (ID.Any())
                        {
                            db.tbl_thanhphanmonan.Remove(ID.FirstOrDefault());
                            db.SaveChanges();
                        }

                        dbContextTransaction.Commit();

                        //tính lại thông tin của thành phần dinh dưỡng món ăn
                        var ma = db.tbl_monan.Where(o => o.MaMonAn == maMonAn);
                        if (ma.Any())
                        {
                            tbl_monan MA = ma.FirstOrDefault();

                            MonAnData da = TinhDinhDuongMonAn(MA.MaMonAn);
                            MA.MaMonAn = MA.MaMonAn;
                           MA.TongProtidDV = da.tongProtidDV;
                            MA.TongProtidTV = da.tongProtidTV;
                            MA.TongLipidDV = da.tongLipidDV;
                            MA.TongLipidTV = da.tongLipidTV;
                            MA.TongGlucid = da.tongGlucid;
                            MA.TongCalo = da.tongCalo;
                            MA.TongCanxi = da.tongCanxi;
                            MA.TongPhotpho = da.tongPhotpho;
                            MA.TongSat = da.tongSat;
                            MA.TongVitaminA = da.tongVitaminA;
                            MA.TongVitaminB1 = da.tongVitaminB1;
                            MA.TongVitaminB2 = da.tongVitaminB2;
                            MA.TongVitaminPP = da.tongVitaminPP;
                            MA.TongVitaminPP = da.tongVitaminPP;

                            db.SaveChanges();
                        }

                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
                return 0;
            }
        }
        //kết thúc xóa bỏ thông tjn thành phần món ăn

        // kết thúc
    }
}