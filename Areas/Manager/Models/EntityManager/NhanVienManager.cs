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
    public class NhanVienManager
    {
        public void ThemTaiKhoan(DangKi user)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //thêm dữ liệu vào bảng tài khoản
                tbl_taikhoan obj = new tbl_taikhoan();

                obj.TenDangNhap = user.tenDangNhap;
                obj.MatKhau = Encryptor.MD5Hash(user.matKhau);//mã hóa mật khẩu
                obj.NgayTao = DateTime.Now;
                obj.NgaySua = DateTime.Now;

                db.tbl_taikhoan.Add(obj);
                db.SaveChanges();

                //thêm vào bảng hồ sơ nhân viên
                tbl_nhanvien hsnv = new tbl_nhanvien();

                hsnv.MaNV = obj.TenDangNhap;
                hsnv.Ho = user.ho;
                hsnv.Ten = user.ten;
                hsnv.GioiTinh = user.gioiTinh;
                hsnv.NgayTao = DateTime.Now;
                hsnv.NgaySua = DateTime.Now;

                db.tbl_nhanvien.Add(hsnv);
                db.SaveChanges();
            }
        }
        //lấy danh sách nhân viên 
        public List<NhanVienData> LayDSNhanVien(string inputtext, string daNghiViec, ref int totalRecord, int pageIndex = 1, int pageSize = 10)
        {
            List<NhanVienData> list = new List<NhanVienData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {

                //ref :: ở bên ngoài truyền vào
                //lấy tổng bản ghi
                totalRecord = db.tbl_nhanvien.Count();

                var query = (from m in db.tbl_nhanvien
                             select m);
                if (!String.IsNullOrEmpty(inputtext))
                {
                    query = from q in query where (q.MaNV.Contains(inputtext) || q.Ho.Contains(inputtext) || q.Ten.Contains(inputtext)) select q;
                }
                if (!String.IsNullOrEmpty(daNghiViec))
                {
                    query = from q in query where q.DaNghiViec == daNghiViec select q;
                }
                var result = (from m in query
                              select new NhanVienData
                              {
                                  maNV = m.MaNV,
                                  hoTen = m.Ho + " " + m.Ten,
                                  gioiTinh = m.GioiTinh,
                                  dienThoai = m.DienThoai,
                                  email = m.Email,
                                  chucVu = m.ChucVu,
                                  loaiHopDong = m.LoaiHopDong,
                                  daNghiViec = m.DaNghiViec,
                                  ngayVaoLamViec = m.NgayVaoLam

                              }).OrderByDescending(m => m.maNV).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                list = result;
            }
            return list;
        }
        ///lấy thông tin nhân viên đang làm việc
        public List<NhanVienData> DSNhanVienLamViec()
        {
            List<NhanVienData> list = new List<NhanVienData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_nhanvien
                        where m.DaNghiViec == "1"
                        select new NhanVienData
                        {
                            maNV = m.MaNV,
                            hoTen = m.Ho + " " + m.Ten,
                            dienThoai = m.DienThoai,
                            hinhAnh = m.HinhAnh,
                            email = m.Email,
                            chucVu = m.ChucVu,

                        }).OrderByDescending(m => m.maNV).ToList();
            }
            return list;
        }
        //xóa một nhân viên  trong cơ sở dữ liệu
        public void XoaNhanVien(string maNV)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //Xóa ID đã chọn trong bảng tài khoản
                        var SUP = db.tbl_taikhoan.Where(o => o.TenDangNhap == maNV);
                        if (SUP.Any())
                        {
                            db.tbl_taikhoan.Remove(SUP.FirstOrDefault());
                            db.SaveChanges();
                        }
                        //Xóa ID đã chọn trong bảng nhân viên
                        var SU = db.tbl_nhanvien.Where(o => o.MaNV == maNV);
                        if (SU.Any())
                        {
                            db.tbl_nhanvien.Remove(SU.FirstOrDefault());
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
        //thêm mới một nhân viên trong cơ sở dữ liệu
        public void ThemNhanVien(NhanVienData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_nhanvien data = new tbl_nhanvien();

                    data.MaNV = obj.maNV;
                    data.Ho = obj.ho;
                    data.Ten = obj.ten;
                    data.NgaySinh = obj.ngaySinh;
                    data.GioiTinh = obj.gioiTinh;
                    data.DanToc = obj.danToc;
                    data.TonGiao = obj.tonGiao;
                    data.SoCMND = obj.soCMND;
                    data.NgayCapCMND = obj.ngayCapCMND;
                    data.NoiCapCMND = obj.noiCapCMND;
                    data.DiaChi = obj.diaChi;
                    data.DienThoai = obj.dienThoai;
                    data.Email = obj.email;
                    data.ChucVu = obj.chucVu;
                    data.TrinhDo = obj.trinhDo;
                    data.NgayVaoDang = obj.ngayVaoDang;
                    data.SoTheDang = obj.soTheDang;
                    data.HinhAnh = obj.hinhAnh;
                    data.LoaiHopDong = obj.loaiHopDong;
                    data.NgayVaoLam = obj.ngayVaoLamViec;
                    data.DaNghiViec = obj.daNghiViec;
                    data.NgayNghiViec = obj.ngayNghiViec;
                    data.NgayTao = DateTime.Now;
                    data.NgaySua = DateTime.Now;

                    data.GhiChu = obj.ghiChu;

                    db.tbl_nhanvien.Add(data);
                    db.SaveChanges();

                    //thêm dữ liệu vào bảng tài khoảvà phân quyền cho người dùng
                    tbl_taikhoan tk = new tbl_taikhoan();

                    tk.TenDangNhap = obj.maNV;
                    tk.MatKhau = Encryptor.MD5Hash("123456");
                    tk.MaQuyen = obj.quyenTruyCap;
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
        //lấy chi tiết thông tin nhân viên
        public NhanVienData LayChiTietNV(string id)
        {
            NhanVienData data = new NhanVienData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var nv = db.tbl_nhanvien.Find(id);
                if (nv != null)
                {
                    data.maNV = nv.MaNV;
                    data.ho = nv.Ho;
                    data.ten = nv.Ten;
                    data.hoTen = nv.Ho + " " + nv.Ten;
                    data.ngaySinh = nv.NgaySinh;
                    data.gioiTinh = nv.GioiTinh;
                    data.danToc = nv.DanToc;
                    data.tonGiao = nv.TonGiao;
                    data.soCMND = nv.SoCMND;
                    data.ngayCapCMND = nv.NgayCapCMND;
                    data.noiCapCMND = nv.NoiCapCMND;
                    data.diaChi = nv.DiaChi;
                    data.dienThoai = nv.DienThoai;
                    data.email = nv.Email;
                    data.chucVu = nv.ChucVu;
                    data.trinhDo = nv.TrinhDo;
                    data.ngayVaoDang = nv.NgayVaoDang;
                    data.soTheDang = nv.SoTheDang;
                    data.hinhAnh = nv.HinhAnh;
                    data.loaiHopDong = nv.LoaiHopDong;
                    data.ngayVaoLamViec = nv.NgayVaoLam;
                    data.daNghiViec = nv.DaNghiViec;
                    data.ngayNghiViec = nv.NgayNghiViec;
                    data.ngayTao = nv.NgayTao;
                    data.ngaySua = DateTime.Now;
                    data.ghiChu = nv.GhiChu;

                    //lấy quyền người dùng
                    var tk = db.tbl_taikhoan.Where(m => m.TenDangNhap == nv.MaNV);
                    if (tk.Any())
                    {
                        tbl_taikhoan TaiKhoan = tk.FirstOrDefault();

                        data.quyenTruyCap = TaiKhoan.MaQuyen;

                        //lấy tên quyền 
                        var tq = db.tbl_quyennguoidung.Find(TaiKhoan.MaQuyen);
                        data.tenQuyenTruyCap = tq.TenPQ;
                    }
                }
            }
            return data;
        }
        //sửa thông tin nhân viên
        public void SuaNhanVien(NhanVienData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_nhanvien.Where(o => o.MaNV == obj.maNV);
                        if (timkiem.Any())
                        {
                            tbl_nhanvien data = timkiem.FirstOrDefault();

                            data.MaNV = obj.maNV;
                            data.Ho = obj.ho;
                            data.Ten = obj.ten;
                            data.NgaySinh = obj.ngaySinh;
                            data.GioiTinh = obj.gioiTinh;
                            data.DanToc = obj.danToc;
                            data.TonGiao = obj.tonGiao;
                            data.SoCMND = obj.soCMND;
                            data.NgayCapCMND = obj.ngayCapCMND;
                            data.NoiCapCMND = obj.noiCapCMND;
                            data.DiaChi = obj.diaChi;
                            data.DienThoai = obj.dienThoai;
                            data.Email = obj.email;
                            data.ChucVu = obj.chucVu;
                            data.TrinhDo = obj.trinhDo;
                            data.NgayVaoDang = obj.ngayVaoDang;
                            data.SoTheDang = obj.soTheDang;
                            data.HinhAnh = obj.hinhAnh;
                            data.LoaiHopDong = obj.loaiHopDong;
                            data.NgayVaoLam = obj.ngayVaoLamViec;
                            data.DaNghiViec = obj.daNghiViec;
                            data.NgayNghiViec = obj.ngayNghiViec;
                            data.NgayTao = obj.ngayTao;
                            data.NgaySua = DateTime.Now;
                            data.GhiChu = obj.ghiChu;
                            db.SaveChanges();

                            //sửa thông tin quyền người dùng
                            var tk = db.tbl_taikhoan.Where(m => m.TenDangNhap == obj.maNV);
                            if (tk.Any())
                            {
                                tbl_taikhoan TaiKhoan = tk.FirstOrDefault();

                                TaiKhoan.MaQuyen = obj.quyenTruyCap;
                                TaiKhoan.NgaySua = DateTime.Now;
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
            }
        }
        //kiểm tra tên đăng kí đã tồn tại chưa
        public bool KiemTraTenDangKi(string tenDangNhap)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                return db.tbl_taikhoan.Where(o => o.TenDangNhap.Equals(tenDangNhap)).Any();
            }
        }
        //lấy mật khẩu khi người dùng đăng nhập
        public string LayMatKhau(string tenDangNhap)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var user = db.tbl_taikhoan.Where(o => o.TenDangNhap.ToLower().Equals(tenDangNhap));
                if (user.Any())
                {
                    return user.FirstOrDefault().MatKhau;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        //thay đổi mật khẩu
        public void UpdatePass(UserData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_taikhoan.Where(o => o.TenDangNhap == obj.UserName);
                        if (timkiem.Any())
                        {
                            tbl_taikhoan data = timkiem.FirstOrDefault();

                            data.TenDangNhap = obj.UserName;
                            data.MatKhau = Encryptor.MD5Hash(obj.NewPassword);
                            data.NgaySua = DateTime.Now;

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
        /////
        //lấy quyền của người dùng
        public bool LayQuyenND(string tenDangNhap, string tenQuyen)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                tbl_taikhoan user = db.tbl_taikhoan.Where(o => o.TenDangNhap.ToLower().Equals(tenDangNhap)).FirstOrDefault();

                if (user != null)
                {
                    var roles = from q in db.tbl_taikhoan
                                join r in db.tbl_quyennguoidung on q.MaQuyen equals r.MaPQ
                                where q.TenDangNhap == tenDangNhap && r.TenPQ == tenQuyen
                                select r.TenPQ;
                    if (roles != null)
                    {
                        return roles.Any();
                    }
                }
                return false;
            }
        }
        ////lấy id của người đăng nhập
        public string LayMaDangNhap(string tenDangNhap)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var user = db.tbl_taikhoan.Where(o => o.TenDangNhap.Equals(tenDangNhap));
                if (user.Any())
                {
                    return user.FirstOrDefault().TenDangNhap;
                }
            }
            return "0";
        }
        //lấy danh sách nhân viên gồm mã nhân viên và họ tên trong cơ sở dữ liệu
        public List<NhanVienData> ListNhanVien()
        {
            List<NhanVienData> list = new List<NhanVienData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = db.tbl_nhanvien.Select(m => new NhanVienData
                {
                    maNV = m.MaNV,
                    maHoTen = m.MaNV + " - " + m.Ho + " " + m.Ten
                }).OrderByDescending(m => m.maNV).ToList();
            }
            return list;
        }
    }
}