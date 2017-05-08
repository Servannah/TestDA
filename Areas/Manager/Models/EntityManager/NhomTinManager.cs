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
    public class NhomTinManager
    {
        //lấy tất cả danh sách danh mục trong csdl
        public List<NhomTinData> LayDSNhomTin()
        {
            List<NhomTinData> list = new List<NhomTinData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = db.tbl_nhomtin.Select(o => new NhomTinData
                {
                    maNhom = o.MaNhom,
                    tenNhom = o.TenNhom,
                    slug = o.Slug,
                    moTa = o.MoTa,
                    danhMucCha = o.DanhMucCha,
                    tinhTrang = o.TinhTrang
                }).OrderByDescending(o => o.maNhom).ToList();
            }
            return list;

        }
        //lấy dữ liệu một danh mục
        public NhomTinData LayChiTietNhomTin(int id)
        {
            NhomTinData NT = new NhomTinData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var nhomtin = db.tbl_nhomtin.Find(id);
                if (nhomtin != null)
                {
                    NT.maNhom = nhomtin.MaNhom;
                    NT.tenNhom = nhomtin.TenNhom;
                    NT.slug = nhomtin.Slug;
                    NT.moTa = nhomtin.MoTa;
                    NT.danhMucCha = nhomtin.DanhMucCha;
                    NT.tinhTrang = nhomtin.TinhTrang;
                }
            }
            return NT;
        }
        ////
        //Tìm kiếm thông tin một danh mục nhóm tin
        public List<NhomTinData> TimKiemNhomTin(string inputtext)
        {
            List<NhomTinData> kq = new List<NhomTinData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //lấy danh sách nhóm tin
                kq = LayDSNhomTin();
                if (!String.IsNullOrEmpty(inputtext))
                {
                    kq = (from m in db.tbl_nhomtin
                          where (m.TenNhom.Contains(inputtext))
                          select new NhomTinData
                         {
                             maNhom = m.MaNhom,
                             tenNhom = m.TenNhom,
                             slug = m.Slug,
                             moTa = m.MoTa,
                             tinhTrang = m.TinhTrang
                         }).OrderByDescending(m => m.maNhom).ToList();
                }
            }
            return kq;
        }
        ////
        //
        //tạo và hiển thị slug ngay sau khi nhập tên
        public string Slug(string tenNhom)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                string slug = CreateSlug.GenerateSlug(tenNhom);
                return slug;
            }
        }
        /////////
        //Thêm mới một danh mục
        public void ThemMoiNhomTin(NhomTinData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_nhomtin nhomtin = new tbl_nhomtin();
                    nhomtin.MaNhom = obj.maNhom;
                    nhomtin.TenNhom = obj.tenNhom;
                    //xét trường hợp nếu người dùng nhập tên rồi click luôn vào thêm mới
                    nhomtin.Slug = (obj.slug != null) ? CreateSlug.GenerateSlug(obj.slug) : CreateSlug.GenerateSlug(obj.tenNhom);
                    nhomtin.MoTa = obj.moTa;
                    nhomtin.DanhMucCha = (obj.danhMucCha == null) ? 0 : obj.danhMucCha;
                    nhomtin.TinhTrang = obj.tinhTrang;

                    db.tbl_nhomtin.Add(nhomtin);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        // chỉnh sửa danh mục 
        public void SuaNhomTin(NhomTinData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_nhomtin.Where(o => o.MaNhom == obj.maNhom);
                        if (timkiem.Any())
                        {
                            tbl_nhomtin kq = timkiem.FirstOrDefault();
                            kq.TenNhom = obj.tenNhom;
                            //Xét cho trường hợp người dùng xóa tên và nhấn vào chỉnh sửa
                            kq.Slug = (obj.slug != null) ? CreateSlug.GenerateSlug(obj.slug) : CreateSlug.GenerateSlug(obj.tenNhom);
                            kq.MoTa = obj.moTa;
                            kq.DanhMucCha = (obj.danhMucCha == null) ? 0 : obj.danhMucCha;
                            kq.TinhTrang = obj.tinhTrang;

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
        public int XoaNhomTin(int maNhomTin)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //kiểm tra danh mục này có danh mục con hay không.
                        var nhomtin = db.tbl_nhomtin.Where(o => o.DanhMucCha == maNhomTin);
                        if (nhomtin.Any())
                        {
                            return 1;
                        }
                        //kiểm tra danh mục này có bài viết hay không
                        var baiviet = db.tbl_tintuc.Where(o => o.NhomTin == maNhomTin);
                        if (baiviet.Any())
                        {
                            return 2;
                        }
                        else
                        {
                            var ID = db.tbl_nhomtin.Where(o => o.MaNhom == maNhomTin);
                            //Xóa id danh mục đã chọn trong bảng mn_term
                            if (ID.Any())
                            {
                                db.tbl_nhomtin.Remove(ID.FirstOrDefault());
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