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
    public class HoanTraHocPhiManager
    {
        //lấy danh sách dinh mức học phí theo từng nhóm lớp
        public List<HoanTraHocPhiData> DanhSachThongTin(string maHocSinh, string namHoc, ref int totalRecord, int pageIndex = 1, int pageSize = 10)
        {
            List<HoanTraHocPhiData> list = new List<HoanTraHocPhiData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                //ref :: ở bên ngoài truyền vào
                //lấy tổng bản ghi
                totalRecord = db.tbl_hoantrahocphi.Count();
                //lấy danh sách học sinh
                var query = (from m in db.tbl_hoantrahocphi
                             join n in db.tbl_lop on m.MaLop equals n.MaLop
                             join h in db.tbl_hocsinh on m.MaHocSinh equals h.MaHS
                             select new
                             {
                                 maHTHP = m.MaHoanTraHP,
                                 maHocSinh = m.MaHocSinh,
                                 tenHocSinh = h.HoTen,
                                 tenLop = n.TenLop,
                                 namHoc = m.NamHoc,
                                 tienHoanTra = m.TienHoanTra,
                                 lyDoTra = m.LyDoTra,
                                 ngayLap = m.NgayLap,
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
                var result = (from m in query
                              select new HoanTraHocPhiData
                              {
                                  maHTHP = m.maHTHP,
                                  maHocSinh = m.maHocSinh,
                                  tenHocSinh = m.tenHocSinh,
                                  tenLop = m.tenLop,
                                  namHoc = m.namHoc,
                                  tienHoanTra = m.tienHoanTra,
                                  lyDoTra = m.lyDoTra,
                                  ngayLap = m.ngayLap,
                                  ghiChu = m.ghiChu
                              }).OrderByDescending(m => m.maHTHP).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                list = result;

            }
            return list;
        }
        //lấy chi tiết thông tin bản ghi
        public HoanTraHocPhiData LayChiTiet(int id)
        {
            HoanTraHocPhiData HPD = new HoanTraHocPhiData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var hp = db.tbl_hoantrahocphi.Find(id);
                if (hp != null)
                {
                    HPD.maHTHP = hp.MaHoanTraHP;
                    HPD.maHocSinh = hp.MaHocSinh;
                    HPD.maLop = hp.MaLop;
                    HPD.namHoc = hp.NamHoc;
                    HPD.tienHoanTra = hp.TienHoanTra;
                    HPD.lyDoTra = hp.LyDoTra;
                    HPD.ngayLap = hp.NgayLap;
                    HPD.ghiChu = hp.GhiChu;
                }
            }
            return HPD;
        }///
        //thêm mới một bản ghi
        public void ThemThongTin(HoanTraHocPhiData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_hoantrahocphi data = new tbl_hoantrahocphi();

                    data.MaHoanTraHP = obj.maHTHP;
                    data.MaHocSinh = obj.maHocSinh;
                    data.MaLop = obj.maLop;
                    data.NamHoc = obj.namHoc;
                    data.TienHoanTra = obj.tienHoanTra;
                    data.LyDoTra = obj.lyDoTra;
                    data.NgayLap = obj.ngayLap;
                    data.GhiChu = obj.ghiChu;

                    db.tbl_hoantrahocphi.Add(data);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        //sửa thông tin bản ghi 
        public void SuaThongTin(HoanTraHocPhiData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_hoantrahocphi.Where(o => o.MaHoanTraHP == obj.maHTHP);
                        if (timkiem.Any())
                        {
                            tbl_hoantrahocphi data = timkiem.FirstOrDefault();

                            data.MaHoanTraHP = obj.maHTHP;
                            data.MaHocSinh = obj.maHocSinh;
                            data.MaLop = obj.maLop;
                            data.NamHoc = obj.namHoc;
                            data.TienHoanTra = obj.tienHoanTra;
                            data.LyDoTra = obj.lyDoTra;
                            data.NgayLap = obj.ngayLap;
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
        //xóa  một danh mục nhóm tin
        public void XoaThongTin(int maHTHP)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var ID = db.tbl_hoantrahocphi.Where(o => o.MaHoanTraHP == maHTHP);
                        //Xóa id danh mục đã chọn trong bảng
                        if (ID.Any())
                        {
                            db.tbl_hoantrahocphi.Remove(ID.FirstOrDefault());
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