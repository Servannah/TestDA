//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using TestDA.Areas.Manager.Models.ViewModel;
//using TestDA.DB;
//using System.Data.Entity.Validation;
//using System.Text.RegularExpressions;
//using System.Web.Mvc;
//using TestDA.Common;

//namespace TestDA.Areas.Manager.Models.EntityManager
//{
//    public class NhaCungCapManager
//    {
//        //lấy tất cả danh sách nhà cung cấp trong csdl
//        public List<NhaCungCapData> LayDSNhaCungCap(string inputtext)
//        {
//            List<NhaCungCapData> list = new List<NhaCungCapData>();
//            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
//            {
//                if (!String.IsNullOrEmpty(inputtext))
//                {
//                    list = (from m in db.tbl_nhacungcap
//                            where (m.TenNhaCC.Contains(inputtext) || m.DiaChi.Contains(inputtext) || m.DienThoai.Contains(inputtext) || m.Email.Contains(inputtext) || m.Fax.Contains(inputtext) || m.MaSoThue.Contains(inputtext) || m.TaiKhoan.Contains(inputtext))
//                            select new NhaCungCapData
//                            {
//                                maNhaCC = m.MaNhaCC,
//                                tenNhaCC = m.TenNhaCC,
//                                diaChi = m.DiaChi,
//                                dienThoai = m.DienThoai,
//                                fax = m.Fax,
//                                maSoThue = m.MaSoThue,
//                                taiKhoan = m.TaiKhoan,
//                                email = m.Email,
//                                ghiChu = m.GhiChu
//                            }).OrderByDescending(m => m.maNhaCC).ToList();
//                }
//                else
//                {
//                    list = db.tbl_nhacungcap.Select(m => new NhaCungCapData
//                    {
//                        maNhaCC = m.MaNhaCC,
//                        tenNhaCC = m.TenNhaCC,
//                        diaChi = m.DiaChi,
//                        dienThoai = m.DienThoai,
//                        fax = m.Fax,
//                        maSoThue = m.MaSoThue,
//                        taiKhoan = m.TaiKhoan,
//                        email = m.Email,
//                        ghiChu = m.GhiChu
//                    }).OrderByDescending(m => m.maNhaCC).ToList();
//                }
//            }
//            return list;

//        }
//        //lấy danh sách nhà cung cấp trong cơ sở dữ liệu
//        public List<NhaCungCapData> ListNhaCC()
//        {
//            List<NhaCungCapData> list = new List<NhaCungCapData>();
//            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
//            {
//                list = db.tbl_nhacungcap.Select(m => new NhaCungCapData
//                {
//                    maNhaCC = m.MaNhaCC,
//                    tenNhaCC = m.TenNhaCC
//                }).OrderByDescending(m => m.maNhaCC).ToList();
//            }
//            return list;
//        }
//        //lấy dữ liệu một danh mục
//        public NhaCungCapData LayChiTietNhaCC(int id)
//        {
//            NhaCungCapData NCC = new NhaCungCapData();
//            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
//            {
//                var nhaCC = db.tbl_nhacungcap.Find(id);
//                if (nhaCC != null)
//                {

//                    NCC.maNhaCC = nhaCC.MaNhaCC;
//                    NCC.tenNhaCC = nhaCC.TenNhaCC;
//                    NCC.diaChi = nhaCC.DiaChi;
//                    NCC.dienThoai = nhaCC.DienThoai;
//                    NCC.fax = nhaCC.Fax;
//                    NCC.maSoThue = nhaCC.MaSoThue;
//                    NCC.taiKhoan = nhaCC.TaiKhoan;
//                    NCC.email = nhaCC.Email;
//                    NCC.ghiChu = nhaCC.GhiChu;
//                }
//            }
//            return NCC;
//        }
//        ////
//        //Thêm mới một danh mục
//        public void ThemMoiNhaCC(NhaCungCapData obj)
//        {
//            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
//            {
//                try
//                {
//                    tbl_nhacungcap nhaCC = new tbl_nhacungcap();
//                    nhaCC.MaNhaCC = obj.maNhaCC;
//                    nhaCC.TenNhaCC = obj.tenNhaCC;
//                    nhaCC.DiaChi = obj.diaChi;
//                    nhaCC.DienThoai = obj.dienThoai;
//                    nhaCC.Fax = obj.fax;
//                    nhaCC.MaSoThue = obj.maSoThue;
//                    nhaCC.TaiKhoan = obj.taiKhoan;
//                    nhaCC.Email = obj.email;
//                    nhaCC.GhiChu = obj.ghiChu;

//                    db.tbl_nhacungcap.Add(nhaCC);
//                    db.SaveChanges();

//                }
//                catch (DbEntityValidationException e)
//                {
//                    Console.Write(e);
//                }
//            }
//        }
//        // chỉnh sửa danh mục nhà cung cấp
//        public void SuaNhaCC(NhaCungCapData obj)
//        {
//            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
//            {
//                using (var dbContextTransaction = db.Database.BeginTransaction())
//                {
//                    try
//                    {
//                        var timkiem = db.tbl_nhacungcap.Where(o => o.MaNhaCC == obj.maNhaCC);
//                        if (timkiem.Any())
//                        {
//                            tbl_nhacungcap kq = timkiem.FirstOrDefault();
//                            kq.TenNhaCC = obj.tenNhaCC;
//                            kq.DiaChi = obj.diaChi;
//                            kq.DienThoai = obj.dienThoai;
//                            kq.Fax = obj.fax;
//                            kq.MaSoThue = obj.maSoThue;
//                            kq.TaiKhoan = obj.taiKhoan;
//                            kq.Email = obj.email;
//                            kq.GhiChu = obj.ghiChu;

//                            db.SaveChanges();
//                        }
//                        dbContextTransaction.Commit();
//                    }
//                    catch
//                    {
//                        dbContextTransaction.Rollback();
//                    }
//                }
//            }
//        }
//        //xóa  một danh mục nhà cung cấp
//        public int XoaNhaCC(int maNhaCC)
//        {
//            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
//            {
//                using (var dbContextTransaction = db.Database.BeginTransaction())
//                {
//                    try
//                    {
//                        //kiểm tra nhà cung cấp này có trong bảng tbl_phieunhapkho hay không.
//                        var nhaCC = db.tbl_hoadon.Where(o => o.MaNhaCC == maNhaCC);
//                        if (nhaCC.Any())
//                        {
//                            return 1;
//                        }
//                        else
//                        {
//                            var ID = db.tbl_nhacungcap.Where(o => o.MaNhaCC == maNhaCC);
//                            //Xóa id danh mục đã chọn trong bảng loại nhà cung cấp
//                            if (ID.Any())
//                            {
//                                db.tbl_nhacungcap.Remove(ID.FirstOrDefault());
//                                db.SaveChanges();
//                            }
//                        }

//                        dbContextTransaction.Commit();

//                    }
//                    catch
//                    {
//                        dbContextTransaction.Rollback();
//                    }
//                }
//                return 0;
//            }
//        }//kết thúc xóa một danh mục nhà cung cấp
//    }
//}