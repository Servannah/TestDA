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

namespace TestDA.Areas.Manager.Models.ViewModel
{
    public class OptionManager
    {
        //set page
        //lấy danh sách tất cả những trang trong cơ sở dữ liệu
        public List<OptionData> ListData(string type)
        {
            List<OptionData> list = new List<OptionData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_option
                        where m.Type == type
                        select new OptionData
                        {
                            id = m.ID,
                            name = m.Name,
                            link = m.Link,
                            description = m.Descriptiton,
                            order = m.Ord,
                            content = m.Content

                        }).OrderByDescending(m=>m.id).ToList();
            }
            return list;
        }
        public List<OptionData> ListOptionData()
        {
            List<OptionData> list = new List<OptionData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_option
                        select new OptionData
                        {
                            id = m.ID,
                            name = m.Name,
                            link = m.Link,
                            description = m.Descriptiton,
                            order = m.Ord,
                            type = m.Type,
                            content = m.Content

                        }).ToList();
            }
            return list;
        }
        //lấy thông tin dữ liệu không phải dạng page
        public List<OptionData> ListSocial()
        {
            List<OptionData> list = new List<OptionData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_option
                        where (m.Type =="info"||m.Type=="social"||m.Type=="address")
                        select new OptionData
                        {
                            id = m.ID,
                            name = m.Name,
                            link = m.Link,
                            description = m.Descriptiton,
                            order = m.Ord,
                            type =m.Type,
                            content = m.Content

                        }).ToList();
            }
            return list;
        }

        //Lấy thông tin chi tiết
        public OptionData GetData(int id)
        {
            OptionData LOP = new OptionData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var lop = db.tbl_option.Find(id);
                if (lop != null)
                {
                    LOP.id = lop.ID;
                    LOP.name = lop.Name;
                    LOP.link = lop.Link;
                    LOP.description = lop.Descriptiton;
                    LOP.order = lop.Ord;
                    LOP.content = lop.Content;
                }
            }
            return LOP;
        }
        //thêm mới một bản ghi trong cơ sở dữ liệu
        public void AddData(OptionData obj, string type)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_option data = new tbl_option();
                    data.ID = obj.id;
                    data.Name = obj.name;
                    data.Link = obj.link;
                    data.Descriptiton = obj.description;
                    data.Ord = obj.order;
                    data.Content = obj.content;
                    data.Type = type;

                    db.tbl_option.Add(data);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        //sửa thông tin bản ghi trong cơ sở dữ liệu
        public void UpdateData(OptionData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var timkiem = db.tbl_option.Where(o => o.ID == obj.id);
                        if (timkiem.Any())
                        {
                            tbl_option kq = timkiem.FirstOrDefault();

                            kq.Name = obj.name;
                            kq.Link = obj.link;
                            kq.Descriptiton = obj.description;
                            kq.Ord = obj.order;
                            kq.Content = obj.content;

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
        //xóa thông tin bản ghi trong cơ sở dữ liệu
        public void DeleteData(int id)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var ID = db.tbl_option.Where(o => o.ID == id);
                        //Xóa id danh mục đã chọn trong bảng
                        if (ID.Any())
                        {
                            db.tbl_option.Remove(ID.FirstOrDefault());
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
        //end page
    }
}