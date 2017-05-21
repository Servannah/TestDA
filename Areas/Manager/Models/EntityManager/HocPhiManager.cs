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
    public class HocPhiManager
    {
        //
        //lấy danh sách học phí 
        public List<HocPhiData> dsHocPhi(string maHocSinh, string namHoc, string loaiHP, int? thang, ref int totalRecord, int pageIndex = 1, int pageSize = 10)
        {
            List<HocPhiData> list = new List<HocPhiData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //ref :: ở bên ngoài truyền vào
                //lấy tổng bản ghi
                totalRecord = db.tbl_hocphi.Count();
                //lấy danh sách học sinh
                var query = (from m in db.tbl_hocphi
                             join n in db.tbl_hocsinh on m.MaHocSinh equals n.MaHS
                             join hsl in db.tbl_hocsinhlop on n.MaHS equals hsl.MaHocSinh
                             join nv in db.tbl_nhanvien on m.NguoiThu equals nv.MaNV
                             join l in db.tbl_lop on hsl.MaLop equals l.MaLop
                             select new
                             {
                                 maHocPhi = m.MaHocPhi,
                                 tenHDHocPhi = m.TenHDHocPhi,
                                 maHocSinh = m.MaHocSinh,
                                 tenHocSinh = n.HoTen,
                                 tenLop = l.TenLop,
                                 namHoc = m.NamHoc,
                                 thang = m.Thang,
                                 tongHocPhi = m.TongHocPhi,
                                 nguoiThu = m.NguoiThu,
                                 tenNguoiThu = nv.Ho + " " + nv.Ten,
                                 ngayThu = m.NgayThu,
                                 conNo = m.ConNo,
                                 loaiHP = m.LoaiHP,
                                 ghiChu = m.GhiChu

                             });

                if (!String.IsNullOrEmpty(maHocSinh))
                {
                    query = from q in query where q.maHocSinh.Contains(maHocSinh) select q;
                }
                if (!String.IsNullOrEmpty(namHoc))
                {
                    query = from q in query where q.namHoc == namHoc select q;

                }
                if (!String.IsNullOrEmpty(loaiHP))
                {
                    query = from q in query where q.loaiHP == loaiHP select q;
                }
                if (thang.HasValue)
                {
                    query = from q in query where q.thang == thang select q;
                }
                var result = (from m in query
                              select new HocPhiData
                              {
                                  maHocPhi = m.maHocPhi,
                                  tenHDHocPhi = m.tenHDHocPhi,
                                  maHocSinh = m.maHocSinh,
                                  tenHocSinh = m.tenHocSinh,
                                  tenLop = m.tenLop,
                                  namHoc = m.namHoc,
                                  thang = m.thang,
                                  tongHocPhi = m.tongHocPhi,
                                  nguoiThu = m.nguoiThu,
                                  tenNguoiThu = m.tenNguoiThu,
                                  ngayThu = m.ngayThu,
                                  conNo = m.conNo,
                                  loaiHP = m.loaiHP,
                                  ghiChu = m.ghiChu
                              }).OrderByDescending(m => m.ngayThu).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                list = result;

            }
            return list;
        }
        //danh sách học sinh còn nợ học phí
        public List<HocPhiData> dsHocPhiNoHP(string inputtext, string namHoc, string loaiHP, int? thang, int? maLop)
        {
            List<HocPhiData> list = new List<HocPhiData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //var rt = from n in db.tbl_hocsinh
                //         join hsl in db.tbl_hocsinhlop on n.MaHS equals hsl.MaHocSinh
                //         join l in db.tbl_lop on hsl.MaLop equals l.MaLop
                //         select new
                //         {
                //             maHocSinh = n.MaHS,
                //             tenHocSinh = n.HoTen,
                //             maLop = l.MaLop,
                //             tenLop = l.TenLop,
                //             namHoc = hsl.NamHoc
                //         };
                //var shp = (from m in db.tbl_hocphi
                //           join nv in db.tbl_nhanvien on m.NguoiThu equals nv.MaNV
                //           select new
                //           {
                //               maHocPhi = m.MaHocPhi,
                //               maHocSinh = m.MaHocSinh,
                //               tenHDHocPhi = m.TenHDHocPhi,
                //               thang = m.Thang,
                //               tongHocPhi = m.TongHocPhi,
                //               nguoiThu = m.NguoiThu,
                //               tenNguoiThu = nv.Ho + " " + nv.Ten,
                //               ngayThu = m.NgayThu,
                //               conNo = m.ConNo,
                //               loaiHP = m.LoaiHP,
                //               ghiChu = m.GhiChu
                //           });

                //var query = (from x in rt
                //             join t in shp on x.maHocSinh equals t.maHocSinh into grouphp
                //             from m in grouphp.DefaultIfEmpty()
                //             select new
                //             {
                //                 maHocPhi = m.maHocPhi,
                //                 tenHDHocPhi = m.tenHDHocPhi,
                //                 maHocSinh = m.maHocSinh,
                //                 tenHocSinh = x.tenHocSinh,
                //                 maLop = x.maLop,
                //                 tenLop = x.tenLop,
                //                 namHoc = x.namHoc,
                //                 thang = m.thang,
                //                 tongHocPhi = m.tongHocPhi,
                //                 nguoiThu = m.nguoiThu,
                //                 tenNguoiThu = m.tenNguoiThu,
                //                 ngayThu = m.ngayThu,
                //                 conNo = m.conNo,
                //                 loaiHP = m.loaiHP,
                //                 ghiChu = m.ghiChu
                //             });
                //lấy danh sách học phí
                var query = (from m in db.tbl_hocphi
                             join n in db.tbl_hocsinh on m.MaHocSinh equals n.MaHS
                             join hsl in db.tbl_hocsinhlop on n.MaHS equals hsl.MaHocSinh
                             join nv in db.tbl_nhanvien on m.NguoiThu equals nv.MaNV
                             join l in db.tbl_lop on hsl.MaLop equals l.MaLop
                             select new
                             {
                                 maHocPhi = m.MaHocPhi,
                                 tenHDHocPhi = m.TenHDHocPhi,
                                 maHocSinh = m.MaHocSinh,
                                 tenHocSinh = n.HoTen,
                                 maLop = l.MaLop,
                                 tenLop = l.TenLop,
                                 namHoc = m.NamHoc,
                                 thang = m.Thang,
                                 tongHocPhi = m.TongHocPhi,
                                 nguoiThu = m.NguoiThu,
                                 tenNguoiThu = nv.Ho + " " + nv.Ten,
                                 ngayThu = m.NgayThu,
                                 conNo = m.ConNo,
                                 loaiHP = m.LoaiHP,
                                 ghiChu = m.GhiChu

                             });
                if (!String.IsNullOrEmpty(inputtext))
                {
                    query = (from q in query where q.maHocSinh.Contains(inputtext) || q.tenHocSinh.Contains(inputtext) || q.tenNguoiThu.Contains(inputtext) select q);
                }
                if (!String.IsNullOrEmpty(namHoc))
                {
                    query = (from q in query where q.namHoc == namHoc select q);

                }
                if (!String.IsNullOrEmpty(loaiHP))
                {
                    query = from q in query where q.loaiHP == loaiHP select q;
                }
                if (thang.HasValue)
                {
                    query = from q in query where q.thang == thang select q;
                }
                if (maLop.HasValue)
                {
                    query = from q in query where q.maLop == maLop select q;
                }
                var result = (from m in query
                              select new HocPhiData
                              {
                                  maHocPhi = m.maHocPhi,
                                  tenHDHocPhi = m.tenHDHocPhi,
                                  maHocSinh = m.maHocSinh,
                                  tenHocSinh = m.tenHocSinh,
                                  tenLop = m.tenLop,
                                  namHoc = m.namHoc,
                                  thang = m.thang,
                                  tongHocPhi = m.tongHocPhi,
                                  nguoiThu = m.nguoiThu,
                                  tenNguoiThu = m.tenNguoiThu,
                                  ngayThu = m.ngayThu,
                                  conNo = m.conNo,
                                  loaiHP = m.loaiHP,
                                  ghiChu = m.ghiChu
                              }).OrderByDescending(m => m.maHocPhi).ToList();

                list = result;

            }
            return list;
        }
        //lấy chi tiết theo từng loại học phí danh sách thu học phí
        public List<HocPhiData> dsThuHocPhiThang(string namHoc, int? thang)
        {
            List<HocPhiData> list = new List<HocPhiData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //lấy danh sách học phí
                var query = (from m in db.tbl_hocphi
                             where m.NamHoc == namHoc
                             group m by new
                             {
                                 thang = m.Thang,
                                 namHoc = m.NamHoc
                             } into d
                             select new
                             {
                                 thang = d.Key.thang,
                                 namHoc = d.Key.namHoc,
                                 tong = d.Sum(i => i.TongHocPhi),
                                 tongNo = d.Sum(i => i.ConNo)

                             });
                if (thang.HasValue)
                {
                    query = from q in query where q.thang == thang select q;
                }
                var result = (from m in query
                              select new HocPhiData
                              {
                                  thang = m.thang,
                                  namHoc = m.namHoc,
                                  tongHocPhi = m.tong,
                                  conNo = m.tongNo
                              }).ToList();

                list = result;

            }
            return list;
        }
        //lấy chi tiết theo từng loại học phí danh sách thu học phí
        public List<HocPhiData> dsThuHocPhi(string namHoc, int? thang)
        {
            List<HocPhiData> list = new List<HocPhiData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //lấy danh sách học phí
                var query = (from m in db.tbl_hocphi
                             where m.NamHoc == namHoc
                             group m by new
                             {
                                 thang = m.Thang,
                                 loaiHP = m.LoaiHP,
                                 namHoc = m.NamHoc
                             } into d
                             select new
                             {
                                 thang = d.Key.thang,
                                 loaiHP = d.Key.loaiHP,
                                 namHoc = d.Key.namHoc,
                                 tongHS = d.Count(),
                                 tienMienGiam = d.Sum(i => i.TienMienGiam),
                                 tong = d.Sum(i => i.TongHocPhi),
                                 tongNo = d.Sum(i => i.ConNo)

                             });
                if (thang.HasValue)
                {
                    query = from q in query where q.thang == thang select q;
                }
                var result = (from m in query
                              select new HocPhiData
                              {
                                  thang = m.thang,
                                  namHoc = m.namHoc,
                                  loaiHP = m.loaiHP,
                                  tongHS = m.tongHS,
                                  tienMienGiam = m.tienMienGiam,
                                  tongHocPhi = m.tong,
                                  conNo = m.tongNo
                              }).ToList();

                list = result;

            }
            return list;
        }
        //lấy nhóm lớp của học sinh
        public int NhomLopHS(string maHS, string namHoc)
        {
            int idNL = 0;
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    var nhomLop = from hsl in db.tbl_hocsinhlop
                                  join l in db.tbl_lop on hsl.MaLop equals l.MaLop
                                  where hsl.MaHocSinh == maHS && hsl.NamHoc == namHoc
                                  select new
                                  {
                                      maHS = hsl.MaHocSinh,
                                      nLop = l.NhomLopID
                                  };
                    if (nhomLop.Any())
                    {
                        var nl = nhomLop.FirstOrDefault();
                        idNL = nl.nLop.Value;
                    }
                    else
                    {
                        idNL = 0;
                    }
                }
                //
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
            return idNL;
        }
        //lấy chi tiết thông tin bản ghi
        public HocPhiData LayChiTietHP(string id)
        {
            HocPhiData HPD = new HocPhiData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var hp = db.tbl_hocphi.Find(id);
                if (hp != null)
                {
                    HPD.maHocPhi = hp.MaHocPhi;
                    HPD.tenHDHocPhi = hp.TenHDHocPhi;
                    HPD.maHocSinh = hp.MaHocSinh;
                    //lấy tên học sinh
                    var hocsinh = db.tbl_hocsinh.Find(HPD.maHocSinh);
                    HPD.maHocSinh = hocsinh.MaHS;
                    HPD.tenHocSinh = hocsinh.HoTen;
                    HPD.namHoc = hp.NamHoc;
                    HPD.thang = hp.Thang;
                    HPD.tienMienGiam = hp.TienMienGiam;
                    HPD.ngayThu = hp.NgayThu;
                    HPD.nguoiThu = hp.NguoiThu;
                    //lấy tên người thu
                    var nhanvien = db.tbl_nhanvien.Find(HPD.nguoiThu);
                    HPD.tenNguoiThu = nhanvien.Ho + " " + nhanvien.Ten;
                    HPD.tongHocPhi = hp.TongHocPhi;
                    HPD.daThu = hp.DaThu;
                    HPD.conNo = hp.ConNo;
                    HPD.loaiHP = hp.LoaiHP;
                    HPD.ghiChu = hp.GhiChu;
                }
            }
            return HPD;
        }///
        //
        //thêm mới một bản ghi
        public void ThemHocPhi(HocPhiData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_hocphi data = new tbl_hocphi();

                    data.MaHocPhi = obj.maHocPhi;
                    data.TenHDHocPhi = obj.tenHDHocPhi;
                    data.MaHocSinh = obj.maHocSinh;
                    data.NamHoc = obj.namHoc;
                    data.Thang = obj.thang;
                    data.TienMienGiam = obj.tienMienGiam;
                    data.NgayThu = obj.ngayThu;
                    data.NguoiThu = obj.nguoiThu;
                    data.TongHocPhi = obj.tongHocPhi;
                    data.DaThu = obj.daThu;
                    data.ConNo = obj.conNo;
                    data.LoaiHP = obj.loaiHP;
                    data.GhiChu = obj.ghiChu;

                    db.tbl_hocphi.Add(data);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        //sửa thông tin học phí của học ssinh
        public void SuaHocPhi(HocPhiData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_hocphi.Where(o => o.MaHocPhi == obj.maHocPhi);
                        if (timkiem.Any())
                        {
                            tbl_hocphi data = timkiem.FirstOrDefault();

                            data.MaHocPhi = obj.maHocPhi;
                            data.TenHDHocPhi = obj.tenHDHocPhi;
                            data.MaHocSinh = obj.maHocSinh;
                            data.NamHoc = obj.namHoc;
                            data.Thang = obj.thang;
                            data.TienMienGiam = obj.tienMienGiam;
                            data.NguoiThu = obj.nguoiThu;
                            data.NgayThu = obj.ngayThu;
                            data.TongHocPhi = obj.tongHocPhi;
                            data.DaThu = obj.daThu;
                            data.ConNo = obj.conNo;
                            data.LoaiHP = obj.loaiHP;
                            data.GhiChu = obj.ghiChu;

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
        //xóa học phí đã chọn
        //xóa  một danh mục nhóm tin
        public void XoaHocPhi(string maHocPhi)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var ID = db.tbl_hocphi.Where(o => o.MaHocPhi == maHocPhi);
                        //Xóa id danh mục đã chọn trong bảng
                        if (ID.Any())
                        {
                            db.tbl_hocphi.Remove(ID.FirstOrDefault());
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
        }//kết thúc xóa một danh mục 
        //
    }
}