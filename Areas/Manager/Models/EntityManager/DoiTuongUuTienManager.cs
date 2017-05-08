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
    public class DoiTuongUuTienManager
    {
        public List<DoiTuongUuTienData> LayDSLoaiUT(string inputtext)
        {
            List<DoiTuongUuTienData> list = new List<DoiTuongUuTienData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                if (!String.IsNullOrEmpty(inputtext))
                {
                    list = (from m in db.tbl_doituonguutien
                            where (m.LoaiDoiTuongUuTien.Contains(inputtext))
                            select new DoiTuongUuTienData
                            {
                                maUuTien = m.MaUuTien,
                                loaiDT = m.LoaiDoiTuongUuTien,
                                dinhMucGiam = m.DinhMucGiam,
                                moTa = m.MoTa
                            }).OrderByDescending(m => m.maUuTien).ToList();
                }
                else
                {
                    list = (from m in db.tbl_doituonguutien
                            select new DoiTuongUuTienData
                            {
                                maUuTien = m.MaUuTien,
                                loaiDT = m.LoaiDoiTuongUuTien,
                                dinhMucGiam = m.DinhMucGiam,
                                moTa = m.MoTa
                            }).OrderByDescending(m => m.maUuTien).ToList();
                }
            }
            return list;

        }
        //lấy id và tên đối tượng ưu tiên
        public List<DoiTuongUuTienData> LayDSLoaiUT()
        {
            List<DoiTuongUuTienData> list = new List<DoiTuongUuTienData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_doituonguutien
                        select new DoiTuongUuTienData
                        {
                            maUuTien = m.MaUuTien,
                            loaiDT = m.LoaiDoiTuongUuTien
                        }).ToList();
            }
            return list;
        }
        //lấy dữ liệu một danh mục
        public DoiTuongUuTienData LayChiTietDT(int id)
        {
            DoiTuongUuTienData DTUT = new DoiTuongUuTienData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var data = db.tbl_doituonguutien.Find(id);
                if (data != null)
                {
                    DTUT.maUuTien = data.MaUuTien;
                    DTUT.loaiDT = data.LoaiDoiTuongUuTien;
                    DTUT.dinhMucGiam = data.DinhMucGiam;
                    DTUT.moTa = data.MoTa;
                }
            }
            return DTUT;
        }
        //Thêm mới một danh mục
        public void ThemMoiLoaiDT(DoiTuongUuTienData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_doituonguutien loaiDT = new tbl_doituonguutien();
                    loaiDT.MaUuTien = obj.maUuTien;
                    loaiDT.LoaiDoiTuongUuTien = obj.loaiDT;
                    loaiDT.DinhMucGiam = obj.dinhMucGiam;
                    loaiDT.MoTa = obj.moTa;

                    db.tbl_doituonguutien.Add(loaiDT);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        // chỉnh sửa danh mục 
        public void SuaLoaiDT(DoiTuongUuTienData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_doituonguutien.Where(o => o.MaUuTien == obj.maUuTien);
                        if (timkiem.Any())
                        {
                            tbl_doituonguutien kq = timkiem.FirstOrDefault();
                            kq.LoaiDoiTuongUuTien = obj.loaiDT;
                            kq.DinhMucGiam = obj.dinhMucGiam;
                            kq.MoTa = obj.moTa;
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
        public int XoaLoaiDT(int maUuTien)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //kiểm tra lớp này có trong bảng tbl_hocsinh hay không
                        var DTUT = db.tbl_hocsinh.Where(o => o.MaUuTien == maUuTien);
                        if (DTUT.Any())
                        {
                            return 1;
                        }
                        else
                        {
                            var ID = db.tbl_doituonguutien.Where(o => o.MaUuTien == maUuTien);
                            //Xóa id danh mục đã chọn trong bảng
                            if (ID.Any())
                            {
                                db.tbl_doituonguutien.Remove(ID.FirstOrDefault());
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