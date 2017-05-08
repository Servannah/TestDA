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
    public class NhomLopManager
    {
        //tìm kiếm thông tin nhóm lớp
        public List<NhomLopData> LayDSNhomLop(string inputtext)
        {
            List<NhomLopData> list = new List<NhomLopData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                if (!String.IsNullOrEmpty(inputtext))
                {
                    list = (from m in db.tbl_nhomlop
                            where (m.TenNhomLop.Contains(inputtext))
                            select new NhomLopData
                            {
                                maNhomLop = m.NhomLopID,
                                tenNhomLop = m.TenNhomLop,
                                ghiChu = m.GhiChu
                            }).OrderByDescending(m => m.maNhomLop).ToList();
                }
                else
                {
                     list = (from m in db.tbl_nhomlop
                            select new NhomLopData
                            {
                                maNhomLop = m.NhomLopID,
                                tenNhomLop = m.TenNhomLop,
                                ghiChu = m.GhiChu
                            }).OrderByDescending(m => m.maNhomLop).ToList();
                }
            }
            return list;

        }
        //lấy danh sách nhóm lớp
        public List<NhomLopData> ListNhomLop()
        {
            List<NhomLopData> list = new List<NhomLopData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_nhomlop
                        select new NhomLopData
                        {
                            maNhomLop = m.NhomLopID,
                            tenNhomLop = m.TenNhomLop,
                        }).ToList();
            } 
            return list;
        }

        //lấy dữ liệu một danh mục
        public NhomLopData LayChiTietNhomLop(int id)
        {
            NhomLopData NLD = new NhomLopData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var data = db.tbl_nhomlop.Find(id);
                if (data != null)
                {
                    NLD.maNhomLop  = data.NhomLopID;
                    NLD.tenNhomLop = data.TenNhomLop;
                    NLD.ghiChu = data.GhiChu;
                }
            }
            return NLD;
        }
        //Thêm mới một danh mục
        public void ThemMoiNhomLop(NhomLopData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_nhomlop nhomLop = new tbl_nhomlop();
                    nhomLop.NhomLopID = obj.maNhomLop;
                    nhomLop.TenNhomLop = obj.tenNhomLop;
                    nhomLop.GhiChu = obj.ghiChu;

                    db.tbl_nhomlop.Add(nhomLop);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        // chỉnh sửa danh mục 
        public void SuaNhomLop(int maNhomLop, string tenNhomLop, string ghiChu)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_nhomlop.Where(o => o.NhomLopID == maNhomLop);
                        if (timkiem.Any())
                        {
                            tbl_nhomlop kq = timkiem.FirstOrDefault();
                            kq.TenNhomLop =tenNhomLop;
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
        //xóa  một danh mục 
        public int XoaNhomLop(int maNhomLop)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //kiểm tra nhóm lớp này có trong bảng tbl_lop hay không
                        var lop = db.tbl_lop.Where(o => o.NhomLopID == maNhomLop);
                        if (lop.Any())
                        {
                            return 1;
                        }
                        else
                        {
                            var ID = db.tbl_nhomlop.Where(o => o.NhomLopID == maNhomLop);
                            //Xóa id danh mục đã chọn trong bảng
                            if (ID.Any())
                            {
                                db.tbl_nhomlop.Remove(ID.FirstOrDefault());
                                db.SaveChanges();
                            }

                            dbContextTransaction.Commit();

                        }
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