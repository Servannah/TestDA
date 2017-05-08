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
    public class DMHocPhiManager
    {
        //lấy danh sách dinh mức học phí theo từng nhóm lớp
        public List<DMHocPhiData> DanhSachDMHocPhi(string namHoc, string loaiApDung)
        {
            List<DMHocPhiData> list = new List<DMHocPhiData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //list = (from m in db.tbl_dmhocphi
                //        join n in db.tbl_loaichiphi on m.MaLoaiChiPhi equals n.LoaiChiPhi
                //        select new DMHocPhiData
                //        {
                //            maDMHP = m.MaDMHocPhi,
                //            maLoaiChiPhi = m.MaLoaiChiPhi,
                //            tenLoaiChiPhi = n.TenLoaiChiPhi,
                //            soTien = m.SoTien,
                //            ngayApDung = m.NgayApDung,
                //            namHoc = m.NamHoc,
                //            loaiApDung = m.LoaiApDung,
                //            ghiChu = m.GhiChu
                //        }).OrderByDescending(m => m.maDMHP).ToList();
               var data = from m in db.tbl_dmhocphi select m;

                if (!String.IsNullOrEmpty(namHoc))
                {
                    data = data.Where(m=>m.NamHoc==namHoc);
                    //list = (from m in db.tbl_dmhocphi
                    //        join n in db.tbl_loaichiphi on m.MaLoaiChiPhi equals n.LoaiChiPhi
                    //        where m.NamHoc == namHoc
                    //        select new DMHocPhiData
                    //        {
                    //            maDMHP = m.MaDMHocPhi,
                    //            maLoaiChiPhi = m.MaLoaiChiPhi,
                    //            tenLoaiChiPhi = n.TenLoaiChiPhi,
                    //            soTien = m.SoTien,
                    //            ngayApDung = m.NgayApDung,
                    //            namHoc = m.NamHoc,
                    //            loaiApDung = m.LoaiApDung,
                    //            ghiChu = m.GhiChu
                    //        }).OrderByDescending(m => m.maDMHP).ToList();
                }
                if (!String.IsNullOrEmpty(loaiApDung))
                {
                    data = data.Where(m=>m.LoaiApDung==loaiApDung);
                    //list = (from m in db.tbl_dmhocphi
                    //        join n in db.tbl_loaichiphi on m.MaLoaiChiPhi equals n.LoaiChiPhi
                    //        where m.LoaiApDung == loaiApDung
                    //        select new DMHocPhiData
                    //        {
                    //            maDMHP = m.MaDMHocPhi,
                    //            maLoaiChiPhi = m.MaLoaiChiPhi,
                    //            tenLoaiChiPhi = n.TenLoaiChiPhi,
                    //            soTien = m.SoTien,
                    //            ngayApDung = m.NgayApDung,
                    //            namHoc = m.NamHoc,
                    //            loaiApDung = m.LoaiApDung,
                    //            ghiChu = m.GhiChu
                    //        }).OrderByDescending(m => m.maDMHP).ToList();
                }
                var hp = db.tbl_loaichiphi;

                var result = (from m in data
                            join n in hp on m.MaLoaiChiPhi equals n.LoaiChiPhi
                       select new DMHocPhiData{
                            maDMHP = m.MaDMHocPhi,
                            maLoaiChiPhi = m.MaLoaiChiPhi,
                            tenLoaiChiPhi = n.TenLoaiChiPhi,
                            soTien = m.SoTien,
                            ngayApDung = m.NgayApDung,
                            namHoc = m.NamHoc,
                            loaiApDung = m.LoaiApDung,
                            ghiChu = m.GhiChu

                       }).OrderByDescending(m => m.maDMHP).ToList();

                list = result;
            }
            return list;
        }
        //lấy số tiền học phí chính khóa của năm học đó
        public DMHocPhiData LayHocPhiCK(string namHoc)
        {
            DMHocPhiData data = new DMHocPhiData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var dm = from d in db.tbl_dmhocphi
                         join l in db.tbl_loaichiphi on d.MaLoaiChiPhi equals l.LoaiChiPhi
                         where l.LaHPKhoaChinh == "1"
                         select d;
                if (dm.Any())
                {
                    var ID = dm.FirstOrDefault();
                    data.maDMHP = ID.MaDMHocPhi;
                    data.maLoaiChiPhi = ID.MaLoaiChiPhi;
                    data.soTien = ID.SoTien;
                    data.ngayApDung = ID.NgayApDung;
                    data.namHoc = ID.NamHoc;
                    data.loaiApDung = ID.LoaiApDung;
                    data.ghiChu = ID.GhiChu;
                }
            }
            return data;
        }
        //láy chi tiết thông tin định mức phí cần đóng
        public DMHocPhiData LayChiTiet(int maDMHocPhi)
        {
            DMHocPhiData data = new DMHocPhiData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var dm = db.tbl_dmhocphi.Find(maDMHocPhi);
                if (dm != null)
                {
                    data.maDMHP = dm.MaDMHocPhi;
                    data.maLoaiChiPhi = dm.MaLoaiChiPhi;
                    data.soTien = dm.SoTien;
                    data.ngayApDung = dm.NgayApDung;
                    data.namHoc = dm.NamHoc;
                    data.loaiApDung = dm.LoaiApDung;
                    data.ghiChu = dm.GhiChu;
                }
            }
            return data;
        }
        //thêm mới một định mức học phí
        public void ThemMoiDMHP(DMHocPhiData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_dmhocphi dmhp = new tbl_dmhocphi();

                    dmhp.MaDMHocPhi = obj.maDMHP;
                    dmhp.MaLoaiChiPhi = obj.maLoaiChiPhi;
                    dmhp.SoTien = obj.soTien;
                    dmhp.NgayApDung = obj.ngayApDung;
                    dmhp.LoaiApDung = obj.loaiApDung;
                    dmhp.NamHoc = obj.namHoc;
                    dmhp.GhiChu = obj.ghiChu;

                    db.tbl_dmhocphi.Add(dmhp);

                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        // chỉnh sửa danh mục
        public void SuaDMHocPhi(DMHocPhiData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_dmhocphi.Where(o => o.MaDMHocPhi == obj.maDMHP);
                        if (timkiem.Any())
                        {
                            tbl_dmhocphi kq = timkiem.FirstOrDefault();

                            kq.MaLoaiChiPhi = obj.maLoaiChiPhi;
                            kq.LoaiApDung = obj.loaiApDung;
                            kq.SoTien = obj.soTien;
                            kq.NgayApDung = obj.ngayApDung;
                            kq.NamHoc = obj.namHoc;
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
        //xóa  một danh mục
        public int XoaDMHP(int maDMHP)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var ID = db.tbl_dmhocphi.Where(o => o.MaDMHocPhi == maDMHP);
                        //Xóa id danh mục đã chọn trong bảng 
                        if (ID.Any())
                        {
                            db.tbl_dmhocphi.Remove(ID.FirstOrDefault());
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
        }//kết thúc xóa bản ghi
    }
}