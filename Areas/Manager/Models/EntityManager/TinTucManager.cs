using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDA.DB;
using TestDA.Areas.Manager.Models.ViewModel;
using System.Data.Entity.Validation;
using TestDA.Common;
using PagedList.Mvc;

namespace TestDA.Areas.Manager.Models.EntityManager
{
    public class TinTucManager
    {
        //lấy tất cả bài viết trong cơ sở dữ liệu
        public List<TinTucData> LayDSTinTuc(string tinhTrang, int? danhMuc, string SearchTerm, ref int totalRecord, int pageIndex = 1, int pageSize = 10)
        {
            List<TinTucData> ketqua = new List<TinTucData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //ref :: ở bên ngoài truyền vào
                //lấy tổng bản ghi
                totalRecord = db.tbl_tintuc.Count();
                //lấy danh sách thuộc tính 
                var list = (from n in db.tbl_tintuc
                            join g in db.tbl_nhomtin on n.NhomTin equals g.MaNhom
                            orderby n.MaTin descending
                            select new
                             {
                                 maTin = n.MaTin,
                                 tieuDe = n.TieuDe,
                                 noiDung = n.NoiDung,
                                 slug = n.Slug,
                                 anhDaiDien = n.AnhDaiDien,
                                 nhomTin = n.NhomTin,
                                 nguoiTao = n.NguoiTao,
                                 ngayTao = n.NgayTao,
                                 nguoiSua = n.NguoiSua,
                                 ngaySua = n.NgaySua,
                                 tenDanhMuc = g.TenNhom,
                                 tinNoiBat = n.temp1,
                                 tinhTrang = n.TinhTrang

                             });//.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                if (!String.IsNullOrEmpty(tinhTrang))
                {
                    list = from l in list where l.tinhTrang == tinhTrang select l;
                }
                if (danhMuc.HasValue)
                {
                    list = from l in list where l.nhomTin == danhMuc select l;
                }
                if (!String.IsNullOrEmpty(SearchTerm))
                {
                    list = from l in list where l.tieuDe.Contains(SearchTerm) select l;
                }
                var result = (from n in list
                              select new TinTucData
                              {
                                  maTin = n.maTin,
                                  tieuDe = n.tieuDe,
                                  noiDung = n.noiDung,
                                  slug = n.slug,
                                  anhDaiDien = n.anhDaiDien,
                                  nguoiTao = n.nguoiTao,
                                  ngayTao = n.ngayTao,
                                  nguoiSua = n.nguoiSua,
                                  ngaySua = n.ngaySua,
                                  tenDanhMuc = n.tenDanhMuc,
                                  tinNoiBat = n.tinNoiBat,
                                  tinhTrang = n.tinhTrang

                              }).OrderByDescending(m => m.maTin).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                ketqua = result;

            }
            return ketqua;

        }
        //lấy ra 5 bài viết mới nhất
        public List<TinTucData> NewArticle()
        {
            List<TinTucData> list = new List<TinTucData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //lấy danh sách thuộc tính 
                list = (from n in db.tbl_tintuc
                            where n.TinhTrang == "published"
                        select new TinTucData
                            {
                                maTin = n.MaTin,
                                tieuDe = n.TieuDe,
                                noiDung = n.NoiDung,
                                anhDaiDien = n.AnhDaiDien,
                                ngaySua = n.NgaySua,
                                tinhTrang = n.TinhTrang

                            }).OrderByDescending(n => n.maTin).Take(5).ToList();                
            }
            return list;
        }
        ///lấy danh sách những tin tức nổi bật và trong tình trạng public
        public List<TinTucData> LayDSTinTucNoiBat()
        {
            List<TinTucData> list = new List<TinTucData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {

                list = (from n in db.tbl_tintuc
                        where n.temp1 == 1 && n.TinhTrang == "published"
                        select new TinTucData
                         {
                             maTin = n.MaTin,
                             tieuDe = n.TieuDe,
                             noiDung = n.NoiDung,
                             slug = n.Slug,
                             anhDaiDien = n.AnhDaiDien,
                             ngaySua = n.NgaySua,
                             tinNoiBat = n.temp1,
                             tinhTrang = n.TinhTrang

                         }).OrderByDescending(m => m.maTin).Take(3).ToList();
            }
            return list;
        }
        ///Lấy thông tin một bài viêts
        public TinTucData LayChiTietTin(int maTin)
        {
            TinTucData obj = new TinTucData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var tintuc = db.tbl_tintuc.Find(maTin);
                if (tintuc != null)
                {
                    obj.maTin = tintuc.MaTin;
                    obj.tieuDe = tintuc.TieuDe;
                    obj.noiDung = tintuc.NoiDung;
                    obj.slug = tintuc.Slug;
                    obj.anhDaiDien = tintuc.AnhDaiDien;
                    obj.nguoiTao = tintuc.NguoiTao;
                    obj.ngayTao = tintuc.NgayTao;
                    obj.nguoiSua = tintuc.NguoiSua;
                    obj.ngaySua = tintuc.NgaySua;
                    obj.nhomTin = tintuc.NhomTin.Value;
                    obj.tinNoiBat = tintuc.temp1;
                    obj.tinhTrang = tintuc.TinhTrang;
                }
            }
            return obj;
        }
        ///Thêm mới một bài viết
        public void ThemMoiTinTuc(TinTucData obj, string tenDangNhap)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_tintuc tintuc = new tbl_tintuc();

                    tintuc.MaTin = obj.maTin;
                    tintuc.TieuDe = obj.tieuDe;
                    tintuc.Slug = (obj.slug != null) ? CreateSlug.GenerateSlug(obj.slug) : CreateSlug.GenerateSlug(obj.tieuDe);
                    tintuc.NoiDung = obj.noiDung;
                    tintuc.AnhDaiDien = obj.anhDaiDien;
                    tintuc.NguoiTao = tenDangNhap;
                    tintuc.NgayTao = DateTime.Now;
                    tintuc.NguoiSua = tenDangNhap;
                    tintuc.NgaySua = DateTime.Now;
                    tintuc.NhomTin = obj.nhomTin;
                    tintuc.temp1 = obj.tinNoiBat;
                    tintuc.TinhTrang = obj.tinhTrang;

                    //Lưu thông tin vào cơ sở dữ liệu
                    db.tbl_tintuc.Add(tintuc);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        ///Chỉnh sửa thông tin bài viết
        public void SuaTinTuc(TinTucData obj, string tenDangNhap)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_tintuc.Where(o => o.MaTin == obj.maTin);
                        if (timkiem.Any())
                        {
                            tbl_tintuc tintuc = timkiem.FirstOrDefault();
                            tintuc.TieuDe = obj.tieuDe;
                            tintuc.Slug = (obj.slug != null) ? CreateSlug.GenerateSlug(obj.slug) : CreateSlug.GenerateSlug(obj.tieuDe);
                            tintuc.NoiDung = obj.noiDung;
                            tintuc.AnhDaiDien = obj.anhDaiDien;
                            tintuc.NguoiTao = obj.nguoiTao;
                            tintuc.NgayTao = obj.ngayTao;
                            tintuc.NguoiSua = tenDangNhap;
                            tintuc.NgaySua = DateTime.Now;
                            tintuc.NhomTin = obj.nhomTin;
                            tintuc.temp1 = obj.tinNoiBat;
                            tintuc.TinhTrang = obj.tinhTrang;

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
        ///xóa một bài viêts
        ///
        public void XoaTinTuc(int maTin)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var tintuc = db.tbl_tintuc.Where(o => o.MaTin == maTin);
                        //Xóa id danh mục đã chọn trong bảng tin tức
                        if (tintuc.Any())
                        {
                            db.tbl_tintuc.Remove(tintuc.FirstOrDefault());
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
        //////
    }
}