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
    public class ParentManager
    {
        //lấy mật khẩu của người dùng đăng nhập
        public string GetPassWord(string tenDangNhap)
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
    }
}