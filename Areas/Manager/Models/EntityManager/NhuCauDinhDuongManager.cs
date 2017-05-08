using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestDA.DB;
using System.Data.Entity.Validation;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using TestDA.Common;
using TestDA.Areas.Manager.Models.ViewModel;

namespace TestDA.Areas.Manager.Models.EntityManager
{
    public class NhuCauDinhDuongManager
    {
        //lấy tất cả danh sách trong cơ sở dữ liệu
        public List<NhuCauDinhDuongData> LayDSNCDD()
        {
            List<NhuCauDinhDuongData> list = new List<NhuCauDinhDuongData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = db.tbl_nhucaudinhduong.Select(m => new NhuCauDinhDuongData
                {
                    maNhuCauDD = m.MaNhuCauDD,
                    tuoi = m.Tuoi,
                    kcalo = m.Kcalo,
                    protein = m.Protein,
                    calsi = m.Calsi,
                    sat = m.Sat,
                    vitaminA = m.VitaminA,
                    vitaminB1 = m.VitaminB1,
                    vitaminB2 = m.VitaminB2,
                    vitaminPP = m.VitaminPP,
                    vitaminC = m.VitaminC,
                    ghiChu = m.GhiChu
                }).ToList();

            }
            return list;

        }
        //lấy dữ liệu một danh mục
        public NhuCauDinhDuongData LayChiTietNCDD(int id)
        {
            NhuCauDinhDuongData NCDD = new NhuCauDinhDuongData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var ncdd = db.tbl_nhucaudinhduong.Find(id);
                if (ncdd != null)
                {
                    NCDD.maNhuCauDD = ncdd.MaNhuCauDD;
                    NCDD.tuoi = ncdd.Tuoi;
                    NCDD.kcalo = ncdd.Kcalo;
                    NCDD.protein = ncdd.Protein;
                    NCDD.calsi = ncdd.Calsi;
                    NCDD.sat = ncdd.Sat;
                    NCDD.vitaminA = ncdd.VitaminA;
                    NCDD.vitaminB1 = ncdd.VitaminB1;
                    NCDD.vitaminB2 = ncdd.VitaminB2;
                    NCDD.vitaminPP = ncdd.VitaminPP;
                    NCDD.vitaminC = ncdd.VitaminC;
                    NCDD.ghiChu = ncdd.GhiChu;
                }
            }
            return NCDD;
        }
        //Thêm mới một danh mục
        public void ThemMoiNhuCauDD(NhuCauDinhDuongData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_nhucaudinhduong ncdd = new tbl_nhucaudinhduong();

                    ncdd.MaNhuCauDD = obj.maNhuCauDD;
                    ncdd.Tuoi = obj.tuoi;
                    ncdd.Kcalo = obj.kcalo;
                    ncdd.Protein = obj.protein;
                    ncdd.Calsi = obj.calsi;
                    ncdd.Sat = obj.sat;
                    ncdd.VitaminA = obj.vitaminA;
                    ncdd.VitaminB1 = obj.vitaminB1;
                    ncdd.VitaminB2 = obj.vitaminB2;
                    ncdd.VitaminPP = obj.vitaminPP;
                    ncdd.VitaminC = obj.vitaminC;
                    ncdd.GhiChu = obj.ghiChu;

                    db.tbl_nhucaudinhduong.Add(ncdd);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        // chỉnh sửa danh mục 
        public void SuaNhuCauDD(NhuCauDinhDuongData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_nhucaudinhduong.Where(o => o.MaNhuCauDD == obj.maNhuCauDD);
                        if (timkiem.Any())
                        {
                            tbl_nhucaudinhduong kq = timkiem.FirstOrDefault();

                            kq.MaNhuCauDD = obj.maNhuCauDD;
                            kq.Tuoi = obj.tuoi;
                            kq.Kcalo = obj.kcalo;
                            kq.Protein = obj.protein;
                            kq.Calsi = obj.calsi;
                            kq.Sat = obj.sat;
                            kq.VitaminA = obj.vitaminA;
                            kq.VitaminB1 = obj.vitaminB1;
                            kq.VitaminB2 = obj.vitaminB2;
                            kq.VitaminPP = obj.vitaminPP;
                            kq.VitaminC = obj.vitaminC;
                            kq.GhiChu = obj.ghiChu;

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
        //xóa  một danh mục nhu cầu dinh dưỡng
        public int XoaNhuCauDinhDuong(int maNCDD)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var ID = db.tbl_nhucaudinhduong.Where(o => o.MaNhuCauDD == maNCDD);
                        if (ID.Any())
                        {
                            db.tbl_nhucaudinhduong.Remove(ID.FirstOrDefault());
                            db.SaveChanges();
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
        }//kết thúc xóa một danh mục loại món ăn
    }
}