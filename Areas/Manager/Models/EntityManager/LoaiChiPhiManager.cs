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
    public class LoaiChiPhiManager
    {
        public List<LoaiChiPhiData> LayDSLoaiChiPhi(string inputtext)
        {
            List<LoaiChiPhiData> list = new List<LoaiChiPhiData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                if (!String.IsNullOrEmpty(inputtext))
                {
                    list = (from m in db.tbl_loaichiphi
                            where (m.TenLoaiChiPhi.Contains(inputtext))
                            select new LoaiChiPhiData
                            {
                                maLoaiChiPhi = m.LoaiChiPhi,
                                tenLoai = m.TenLoaiChiPhi,
                                laHPKhoaChinh = m.LaHPKhoaChinh,
                                ghiChu = m.GhiChu
                            }).OrderByDescending(m => m.maLoaiChiPhi).ToList();
                }
                else
                {
                    list = (from m in db.tbl_loaichiphi
                            select new LoaiChiPhiData
                            {
                                maLoaiChiPhi = m.LoaiChiPhi,
                                tenLoai = m.TenLoaiChiPhi,
                                laHPKhoaChinh = m.LaHPKhoaChinh,
                                ghiChu = m.GhiChu
                            }).OrderByDescending(m => m.maLoaiChiPhi).ToList();
                }
            }
            return list;

        }
        //lấy id, tên loại chi phí
        public List<LoaiChiPhiData> ListLoaiChiPhi()
        {
            List<LoaiChiPhiData> list = new List<LoaiChiPhiData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_loaichiphi
                        select new LoaiChiPhiData
                        {
                            maLoaiChiPhi = m.LoaiChiPhi,
                            tenLoai = m.TenLoaiChiPhi
                        }).OrderByDescending(m => m.maLoaiChiPhi).ToList();

            }
            return list;
        }
        //lấy dữ liệu một danh mục
        public LoaiChiPhiData LayChiTietLoaiChiPhi(int id)
        {
            LoaiChiPhiData LCP = new LoaiChiPhiData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var data = db.tbl_loaichiphi.Find(id);
                if (data != null)
                {
                    LCP.maLoaiChiPhi = data.LoaiChiPhi;
                    LCP.tenLoai = data.TenLoaiChiPhi;
                    LCP.laHPKhoaChinh = data.LaHPKhoaChinh;
                    LCP.ghiChu = data.GhiChu;
                }
            }
            return LCP;
        }
        //Thêm mới một danh mục
        public void ThemMoiLoaiChiPhi(LoaiChiPhiData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_loaichiphi loaiChiPhi = new tbl_loaichiphi();
                    loaiChiPhi.LoaiChiPhi = obj.maLoaiChiPhi;
                    loaiChiPhi.TenLoaiChiPhi = obj.tenLoai;
                    loaiChiPhi.LaHPKhoaChinh = obj.laHPKhoaChinh;
                    loaiChiPhi.GhiChu = obj.ghiChu;

                    db.tbl_loaichiphi.Add(loaiChiPhi);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        // chỉnh sửa danh mục 
        public void SuaLoaiChiPhi(LoaiChiPhiData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_loaichiphi.Where(o => o.LoaiChiPhi == obj.maLoaiChiPhi);
                        if (timkiem.Any())
                        {
                            tbl_loaichiphi kq = timkiem.FirstOrDefault();

                            kq.TenLoaiChiPhi = obj.tenLoai;
                            kq.LaHPKhoaChinh = obj.laHPKhoaChinh;
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
        public int XoaLoaiChiPhi(int maLoaiChiPhi)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //kiểm tra lớp này có trong bảng tbl_hocPhi hay không
                        var dm = db.tbl_dmhocphi.Where(o => o.MaLoaiChiPhi == maLoaiChiPhi);
                        if (dm.Any())
                        {
                            return 1;
                        }
                        else
                        {
                            var ID = db.tbl_loaichiphi.Where(o => o.LoaiChiPhi == maLoaiChiPhi);
                            //Xóa id danh mục đã chọn trong bảng
                            if (ID.Any())
                            {
                                db.tbl_loaichiphi.Remove(ID.FirstOrDefault());
                                db.SaveChanges();
                            }

                            dbContextTransaction.Commit();
                            return 0;
                        }
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
                return 2; 
            }
        }//kết thúc xóa một danh mục 
    }
}