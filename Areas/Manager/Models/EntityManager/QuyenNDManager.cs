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
    public class QuyenNDManager
    {
        //lấy tất cả danh sách bản ghi trong cơ sở dữ liệu
        public List<QuyenNDData> LayDSQuyen()
        {
            List<QuyenNDData> list = new List<QuyenNDData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_quyennguoidung
                        select new QuyenNDData
                        {
                            maPQ = m.MaPQ,
                            tenPQ = m.TenPQ,
                            chucNang = m.ChucNang
                        }).ToList();

            }
            return list;

        }
        //lấy dữ liệu một danh mục
        public QuyenNDData LayChiTietQuyen(int id)
        {
            QuyenNDData LOP = new QuyenNDData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var kq = db.tbl_quyennguoidung.Find(id);
                if (kq != null)
                {
                    LOP.maPQ = kq.MaPQ;
                    LOP.tenPQ = kq.TenPQ;
                    LOP.chucNang = kq.ChucNang;
                }
            }
            return LOP;
        }
        //Thêm mới một danh mục
        public void ThemMoiQuyen(QuyenNDData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_quyennguoidung data = new tbl_quyennguoidung();
                    data.TenPQ = obj.tenPQ;
                    data.ChucNang = obj.chucNang;
                    data.NgayTao = DateTime.Now;
                    data.NgaySua = DateTime.Now;

                    db.tbl_quyennguoidung.Add(data);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        // chỉnh sửa danh mục 
        public void SuaQuyen(QuyenNDData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_quyennguoidung.Where(o => o.MaPQ == obj.maPQ);
                        if (timkiem.Any())
                        {
                            tbl_quyennguoidung kq = timkiem.FirstOrDefault();
                            kq.TenPQ = obj.tenPQ;
                            kq.ChucNang = obj.chucNang;
                            kq.NgayTao = obj.ngayTao;
                            kq.NgaySua = DateTime.Now;
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
        public int XoaQuyen(int maPQ)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //kiểm tra quyền này đã được sử dụng trong bảng tài koanr
                        var lop = db.tbl_taikhoan.Where(o => o.MaQuyen == maPQ);
                        if (lop.Any())
                        {
                            return 1;
                        }
                        else
                        {
                            var ID = db.tbl_quyennguoidung.Where(o => o.MaPQ == maPQ);
                            //Xóa id danh mục đã chọn trong bảng
                            if (ID.Any())
                            {
                                db.tbl_quyennguoidung.Remove(ID.FirstOrDefault());
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
        }//kết thúc xóa một danh m
    }
}