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
    public class ConfigManager
    {
        //lấy tất cả danh sách bản ghi trong cơ sở dữ liệu
        public List<ConfigData> ListDataType(string type)
        {
            List<ConfigData> list = new List<ConfigData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_config
                        where m.Type == type
                        select new ConfigData
                        {
                            id = m.ID,
                            name = m.Name,
                            type = m.Type,
                            image = m.Image,
                            content = m.Content,
                            ord = m.Ord,
                            temp1 = m.temp1
                        }).OrderByDescending(m => m.id).ToList();
            }
            return list;
        }
        //
        public List<ConfigData> ListConfig()
        {
            List<ConfigData> list = new List<ConfigData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_config
                        select new ConfigData
                        {
                            id = m.ID,
                            name = m.Name,
                            type = m.Type,
                            image = m.Image,
                            content = m.Content,
                            ord = m.Ord,
                            temp1 = m.temp1
                        }).OrderByDescending(m => m.id).ToList();
            }
            return list;
        }
        //lấy những slide hiển thị ra màn hình
        public List<ConfigData> ListDataShow(string type)
        {
            List<ConfigData> list = new List<ConfigData>();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                list = (from m in db.tbl_config
                        where m.Type == type && m.Ord == 1
                        select new ConfigData
                        {
                            id = m.ID,
                            name = m.Name,
                            type = m.Type,
                            image = m.Image,
                            content = m.Content,
                            ord = m.Ord
                        }).OrderByDescending(m => m.id).ToList();
            }
            return list;
        }
        //lấy tin tức nôi bật trong ngày
        public ConfigData GetNewsHot()
        {
            ConfigData data = new ConfigData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var find = from n in db.tbl_config where n.Type == "news" && n.Ord == 1 select n;
                if (find != null)
                {
                    var result = find.FirstOrDefault();
                    data.id = result.ID;
                    data.name = result.Name;
                    data.type = result.Type;
                    data.image = result.Image;
                    data.content = result.Content;
                    data.ord = result.Ord;
                    data.temp1 = result.temp1;
                }
            }
            return data;
        }
        //lấy dữ liệu một bản ghi
        public ConfigData GetData(int id)
        {
            ConfigData data = new ConfigData();
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                var find = db.tbl_config.Find(id);
                if (find != null)
                {
                    data.id = find.ID;
                    data.name = find.Name;
                    data.type = find.Type;
                    data.image = find.Image;
                    data.content = find.Content;
                    data.ord = find.Ord;
                }
            }
            return data;
        }
        //xem slide

        //thêm mới bản ghi
        public void AddData(ConfigData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                try
                {
                    tbl_config data = new tbl_config();
                    data.ID = obj.id;
                    data.Name = obj.name;
                    data.Type = obj.type;
                    data.Image = obj.image;
                    data.Content = obj.content;
                    data.Ord = obj.ord;
                    data.temp1 = obj.temp1;

                    db.tbl_config.Add(data);
                    db.SaveChanges();

                }
                catch (DbEntityValidationException e)
                {
                    Console.Write(e);
                }
            }
        }
        // chỉnh sửa danh mục  nhóm thực phẩm
        public void UpdateData(ConfigData obj)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var find = db.tbl_config.Where(o => o.ID == obj.id);
                        if (find.Any())
                        {
                            tbl_config data = find.FirstOrDefault();

                            data.Name = obj.name;
                            data.Type = obj.type;
                            data.Image = obj.image;
                            data.Content = obj.content;
                            data.Ord = obj.ord;
                            data.temp1 = obj.temp1;

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
        public void DeleteData(int id)
        {
            using (DoAnTotNghiepEntities db = new DoAnTotNghiepEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var ID = db.tbl_config.Where(o => o.ID == id);
                        //Xóa id đã chọn trong bảng
                        if (ID.Any())
                        {
                            db.tbl_config.Remove(ID.FirstOrDefault());
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
        ////
    }
}