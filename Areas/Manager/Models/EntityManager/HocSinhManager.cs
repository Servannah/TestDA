using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDA.DB;
using TestDA.Areas.Manager.Models.ViewModel;
using TestDA.Common;
using System.Data.Entity.Validation;

namespace TestDA.Areas.Manager.Models.EntityManager
{
    public class HocSinhManager
    {
        //lấy tất cả danh sách học sinh
        public List<HocSinhData> LayDSHocSinh(string tukhoa,ref int totalRecord, int pageIndex = 1, int pageSize = 10)
        {
            List<HocSinhData> list = new List<HocSinhData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //ref :: ở bên ngoài truyền vào
                //lấy tổng bản ghi
                totalRecord = db.tbl_monan.Count();
                //lấy danh sách học sinh
                list = (from m in db.tbl_hocsinh
                        join u in db.tbl_doituonguutien on m.MaUuTien equals u.MaUuTien
                        select new HocSinhData
                {
                    maHocSinh = m.MaHS,
                    hoTen = m.HoTen,
                    ngaySinh = m.NgaySinh,
                    gioiTinh = m.GioiTinh,
                    danToc = m.DanToc,
                    tonGiao = m.TonGiao,
                    queQuan = m.QueQuan,
                    diaChiTamTru = m.DiaChiTamTru,
                    maUuTien = m.MaUuTien,
                    tenDTUuTien = u.LoaiDoiTuongUuTien,
                    ngayVaoHoc = m.NgayVaoHoc,
                    daNghiHoc = m.DaNghiHoc,
                    ngayNghiHoc = m.NgayNghiHoc,
                    hoTenCha = m.HoTenCha,
                    ngaySinhCha = m.NgaySinhCha,
                    ngheNghiepCha = m.NgheNghiepCha,
                    hoTenMe = m.HoTenMe,
                    ngaySinhMe = m.NgaySinhMe,
                    ngheNghiepMe = m.NgheNghiepMe,
                    dienThoaiLienHe = m.DienThoaiLienHe,
                    emailLienHe = m.EmailPhuHuynh,
                    hinhAnh = m.HinhAnh,
                    tenThuongGoi = m.TenThuongGoi,
                    tinhCach = m.TinhCach,
                    hoanThanhMauGiao = m.HoanThanhMN,
                    ghiChu = m.GhiChu
                }).OrderByDescending(m => m.maHocSinh).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                if (!String.IsNullOrEmpty(tukhoa))
                {
                    list = (from m in db.tbl_hocsinh
                            join u in db.tbl_doituonguutien on m.MaUuTien equals u.MaUuTien
                            where (m.MaHS.Contains(tukhoa)||m.HoTen.Contains(tukhoa)||m.QueQuan.Contains(tukhoa))
                            select new HocSinhData
                            {
                                maHocSinh = m.MaHS,
                                hoTen = m.HoTen,
                                ngaySinh = m.NgaySinh,
                                gioiTinh = m.GioiTinh,
                                danToc = m.DanToc,
                                tonGiao = m.TonGiao,
                                queQuan = m.QueQuan,
                                diaChiTamTru = m.DiaChiTamTru,
                                maUuTien = m.MaUuTien,
                                tenDTUuTien = u.LoaiDoiTuongUuTien,
                                ngayVaoHoc = m.NgayVaoHoc,
                                daNghiHoc = m.DaNghiHoc,
                                ngayNghiHoc = m.NgayNghiHoc,
                                hoTenCha = m.HoTenCha,
                                ngaySinhCha = m.NgaySinhCha,
                                ngheNghiepCha = m.NgheNghiepCha,
                                hoTenMe = m.HoTenMe,
                                ngaySinhMe = m.NgaySinhMe,
                                ngheNghiepMe = m.NgheNghiepMe,
                                dienThoaiLienHe = m.DienThoaiLienHe,
                                emailLienHe = m.EmailPhuHuynh,
                                hinhAnh = m.HinhAnh,
                                tenThuongGoi = m.TenThuongGoi,
                                tinhCach = m.TinhCach,
                                hoanThanhMauGiao = m.HoanThanhMN,
                                ghiChu = m.GhiChu
                            }).OrderByDescending(m => m.maHocSinh).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
            }
            return list;
        }////
        //lấy id, tên học sinh
        public List<HocSinhData> ListHS()
        {
            List<HocSinhData> list = new List<HocSinhData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_hocsinh
                        select new HocSinhData

                {
                    maHocSinh = m.MaHS,
                    hoTen = m.HoTen,
                    maTen = m.MaHS + "-" + m.HoTen
                }).OrderByDescending(m => m.maHocSinh).ToList();
            }
            return list;
        }
        //lấy dữ liệu một danh mục
        public HocSinhData LayChiTietHS(string id)
        {
            HocSinhData HSD = new HocSinhData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var hs = db.tbl_hocsinh.Find(id);
                if (hs != null)
                {
                    HSD.maHocSinh = hs.MaHS;
                    HSD.hoTen = hs.HoTen;
                    HSD.ngaySinh = hs.NgaySinh;
                    HSD.gioiTinh = hs.GioiTinh;
                    HSD.danToc = hs.DanToc;
                    HSD.tonGiao = hs.TonGiao;
                    HSD.queQuan = hs.QueQuan;
                    HSD.diaChiTamTru = hs.DiaChiTamTru;
                    HSD.maUuTien = hs.MaUuTien;
                    //lấy tên loại đối tượng ưu tiên
                    var ut = db.tbl_doituonguutien.Find(hs.MaUuTien);
                    HSD.tenDTUuTien = ut.LoaiDoiTuongUuTien;
                    HSD.dinhMucGiam = ut.DinhMucGiam;
                    HSD.ngayVaoHoc = hs.NgayVaoHoc;
                    HSD.daNghiHoc = hs.DaNghiHoc;
                    HSD.ngayNghiHoc = hs.NgayNghiHoc;
                    HSD.hoTenCha = hs.HoTenCha;
                    HSD.ngaySinhCha = hs.NgaySinhCha;
                    HSD.ngheNghiepCha = hs.NgheNghiepCha;
                    HSD.hoTenMe = hs.HoTenMe;
                    HSD.ngaySinhMe = hs.NgaySinhMe;
                    HSD.ngheNghiepMe = hs.NgheNghiepMe;
                    HSD.dienThoaiLienHe = hs.DienThoaiLienHe;
                    HSD.emailLienHe = hs.EmailPhuHuynh;
                    HSD.hinhAnh = hs.HinhAnh;
                    HSD.tenThuongGoi = hs.TenThuongGoi;
                    HSD.tinhCach = hs.TinhCach;
                    HSD.hoanThanhMauGiao = hs.HoanThanhMN;
                    HSD.ghiChu = hs.GhiChu;
                }
            }
            return HSD;
        }///
        //lấy dữ liệu một danh mục
        public void ThemMoiHS(HocSinhData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_hocsinh hocsinh = new tbl_hocsinh();

                    hocsinh.MaHS = obj.maHocSinh;
                    hocsinh.HoTen = obj.hoTen;
                    hocsinh.NgaySinh = obj.ngaySinh;
                    hocsinh.GioiTinh = obj.gioiTinh;
                    hocsinh.DanToc = obj.danToc;
                    hocsinh.TonGiao = obj.tonGiao;
                    hocsinh.QueQuan = obj.queQuan;
                    hocsinh.DiaChiTamTru = obj.diaChiTamTru;
                    hocsinh.MaUuTien = obj.maUuTien;
                    hocsinh.NgayVaoHoc = obj.ngayVaoHoc;
                    hocsinh.DaNghiHoc = obj.daNghiHoc;
                    hocsinh.NgayNghiHoc = obj.ngayNghiHoc;
                    hocsinh.HoTenCha = obj.hoTenCha;
                    hocsinh.NgaySinhCha = obj.ngaySinhCha;
                    hocsinh.NgheNghiepCha = obj.ngheNghiepCha;
                    hocsinh.HoTenMe = obj.hoTenMe;
                    hocsinh.NgaySinhMe = obj.ngaySinhMe;
                    hocsinh.NgheNghiepMe = obj.ngheNghiepMe;
                    hocsinh.DienThoaiLienHe = obj.dienThoaiLienHe;
                    hocsinh.EmailPhuHuynh = obj.emailLienHe;
                    hocsinh.HinhAnh = obj.hinhAnh;
                    hocsinh.TenThuongGoi = obj.tenThuongGoi;
                    hocsinh.TinhCach = obj.tinhCach;
                    hocsinh.HoanThanhMN = obj.hoanThanhMauGiao;
                    hocsinh.GhiChu = obj.ghiChu;

                    db.tbl_hocsinh.Add(hocsinh);

                    db.SaveChanges();

                    ////thêm tài khoản vào bảng phụ huynh
                    //tbl_phuhuynh ph = new tbl_phuhuynh();
                    //ph.TenDangNhap = hocsinh.MaHS;
                    //ph.MatKhau = Encryptor.MD5Hash("123456");

                    //db.tbl_phuhuynh.Add(ph);
                    //db.SaveChanges();
                    //thêm dữ liệu vào bảng tài khoảvà phân quyền cho người dùng
                    tbl_taikhoan tk = new tbl_taikhoan();

                    tk.TenDangNhap = hocsinh.MaHS;
                    tk.MatKhau = Encryptor.MD5Hash("123456");
                    tk.MaQuyen = 6;//xét quyền phụ huynh
                    tk.DaHoatDong = true;
                    tk.NgayTao = DateTime.Now;
                    tk.NgaySua = DateTime.Now;

                    db.tbl_taikhoan.Add(tk);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        // chỉnh sửa danh mục
        public void SuaHocSinh(HocSinhData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_hocsinh.Where(o => o.MaHS == obj.maHocSinh);
                        if (timkiem.Any())
                        {
                            tbl_hocsinh hocsinh = timkiem.FirstOrDefault();

                            hocsinh.MaHS = obj.maHocSinh;
                            hocsinh.HoTen = obj.hoTen;
                            hocsinh.NgaySinh = obj.ngaySinh;
                            hocsinh.GioiTinh = obj.gioiTinh;
                            hocsinh.DanToc = obj.danToc;
                            hocsinh.TonGiao = obj.tonGiao;
                            hocsinh.QueQuan = obj.queQuan;
                            hocsinh.DiaChiTamTru = obj.diaChiTamTru;
                            hocsinh.MaUuTien = obj.maUuTien;
                            hocsinh.NgayVaoHoc = obj.ngayVaoHoc;
                            hocsinh.DaNghiHoc = obj.daNghiHoc;
                            hocsinh.NgayNghiHoc = obj.ngayNghiHoc;
                            hocsinh.HoTenCha = obj.hoTenCha;
                            hocsinh.NgaySinhCha = obj.ngaySinhCha;
                            hocsinh.NgheNghiepCha = obj.ngheNghiepCha;
                            hocsinh.HoTenMe = obj.hoTenMe;
                            hocsinh.NgaySinhMe = obj.ngaySinhMe;
                            hocsinh.NgheNghiepMe = obj.ngheNghiepMe;
                            hocsinh.DienThoaiLienHe = obj.dienThoaiLienHe;
                            hocsinh.EmailPhuHuynh = obj.emailLienHe;
                            hocsinh.HinhAnh = obj.hinhAnh;
                            hocsinh.TenThuongGoi = obj.tenThuongGoi;
                            hocsinh.TinhCach = obj.tinhCach;
                            hocsinh.HoanThanhMN = obj.hoanThanhMauGiao;
                            hocsinh.GhiChu = obj.ghiChu;

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
        //xóa  một bản ghi
        public int XoaHocSinh(string maHocSinh)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //kiểm tra hs này có trong bảng tbl_hocsinhlop hay không
                        var hsl = db.tbl_hocsinhlop.Where(o => o.MaHocSinh == maHocSinh);
                        if (hsl.Any())
                        {
                            return 1;
                        }
                        //kiểm tra học sinh có nằm trong bảng học phí hay không
                        var hsp = db.tbl_hocphi.Where(o => o.MaHocSinh == maHocSinh);
                        if (hsp.Any())
                        {
                            return 2;
                        }
                        //kiểm tra
                        else
                        {
                            //Xóa ID đã chọn trong bảng tài khoản
                            var SUP = db.tbl_taikhoan.Where(o => o.TenDangNhap == maHocSinh);
                            if (SUP.Any())
                            {
                                db.tbl_taikhoan.Remove(SUP.FirstOrDefault());
                                db.SaveChanges();
                            }
                            ////Xóa ID đã chọn trong bảng tài khoản
                            //var SUP = db.tbl_phuhuynh.Where(o => o.TenDangNhap == maHocSinh);
                            //if (SUP.Any())
                            //{
                            //    db.tbl_phuhuynh.Remove(SUP.FirstOrDefault());
                            //    db.SaveChanges();
                            //}

                            var ID = db.tbl_hocsinh.Where(o => o.MaHS == maHocSinh);
                            //Xóa id danh mục đã chọn trong bảng
                            if (ID.Any())
                            {
                                db.tbl_hocsinh.Remove(ID.FirstOrDefault());
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
        }//kết thúc xóa một danh mục 
    }
}