using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model.EF;
using PagedList;
using System.Text.RegularExpressions;

namespace Model.Dao
{
    public class NewsDao
    {
        BDC25DbContext db = null;
        public NewsDao()
        {
            db = new BDC25DbContext();
        }

        public IEnumerable<NEWS> ListALLCATE(string searchString, int page, int pageSize)
        {
            //nếu như tìm kiếm không rỗng
            if (!string.IsNullOrEmpty(searchString))
            {
               string key = addMinus(convertToUnSign3(searchString));

                List<NEWS> ds = (from m in db.NEWS
                                        join idCate in db.CATEGORY_NEWS on m.IDCateNews equals idCate.IDCateNews
                                        where (idCate.StatusDel == false && m.UrlTitle.Contains(key)) 
                                        select m
                                    ).ToList();
                return ds.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
            }

            List<NEWS> dsBaiViet = (from m in db.NEWS
                             join idCate in db.CATEGORY_NEWS on m.IDCateNews equals idCate.IDCateNews
                             where (idCate.StatusDel == false)
                             select m
                                    ).ToList();

            return dsBaiViet.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
        }


        public List<NEWS> Searchkey(string key, ref int totalRecord)
        {

            totalRecord = db.NEWS.Where(x => x.TieuDe.Contains(key)).Count();

            string keyW = addMinus(convertToUnSign3(key));
            List<NEWS> ds = (from m in db.NEWS
                             join idCate in db.CATEGORY_NEWS on m.IDCateNews equals idCate.IDCateNews
                             where (idCate.StatusDel == false && m.UrlTitle.Contains(keyW))
                             select m
                                ).ToList();

            return ds.OrderByDescending(x => x.IDNews).ToList();
        }

        //Lấy danh sách gợi ý để tìm kiếm // chức năng không hoạt động
        public List<string> ListName(string Keyw)
        {
            return db.NEWS.Where(x=>x.TieuDe.Contains(Keyw)).Select(x=>x.TieuDe).ToList();
        }

        //Dùng cho admin
        //Dùng js để kích hoạt 
        public bool ChangeStatus(int id)
        {
            var bv = db.NEWS.Find(id);
            bv.AnHien = !bv.AnHien;
            db.SaveChanges();
            return bv.AnHien;
        }

        public IEnumerable<NEWS> ListALLDrop(int? id, int page, int pageSize)
        {
            List<NEWS> dsBaiViet = (from m in db.NEWS
                                    join idCate in db.CATEGORY_NEWS on m.IDCateNews equals idCate.IDCateNews
                                    where  (idCate.IDCateNews == id)
                                    select m
                                    ).ToList();
            return dsBaiViet.OrderBy(x => x.NgayTao).ToPagedList(page, pageSize); 
        }

        //Dùng trong post ad
        public List<NEWS> ListNewsLienQuan(int id)
        {
            int top = 3;
            var list = db.NEWS.Where(x => x.IDNews == id).FirstOrDefault();
            var listIDcate = db.CATEGORY_NEWS.Where(x=>x.IDCateNews == list.IDCateNews).FirstOrDefault();

            return db.NEWS.Where(x => x.IDCateNews == listIDcate.IDCateNews).OrderByDescending(x => x.NgayTao).Take(top).ToList();
        }

        //Dùng cho trang home bên user danh mục
        public List<NEWS> ListNewsChiaSe(int top = 4)
        {
            List<NEWS> ds = (from m in db.NEWS
                                  join idCate in db.CATEGORY_NEWS on m.IDCateNews equals idCate.IDCateNews
                                  where (idCate.StatusDel == false && idCate.AnHien == true)
                                  select m
                                    ).ToList();

            return ds.Where(x=>x.IDCateNews == 10 && x.AnHien == true).OrderByDescending(x => x.IDNews).Take(top).ToList();
        }

        public List<NEWS> ListNewsKienThuc(int top = 4)
        {
            List<NEWS> ds = (from m in db.NEWS
                             join idCate in db.CATEGORY_NEWS on m.IDCateNews equals idCate.IDCateNews
                             where (idCate.StatusDel == false && idCate.AnHien == true)
                             select m
                                    ).ToList();

            return ds.Where(x => x.IDCateNews == 7 && x.AnHien == true).OrderByDescending(x => x.IDNews).Take(top).ToList();
        }

        public List<NEWS> ListNewsCuuHo(int top = 9)
        {
            List<NEWS> ds = (from m in db.NEWS
                             join idCate in db.CATEGORY_NEWS on m.IDCateNews equals idCate.IDCateNews
                             where (idCate.StatusDel == false && idCate.AnHien == true)
                             select m
                                    ).ToList();

            return ds.Where(x => x.IDCateNews == 2 && x.AnHien == true).OrderByDescending(x => x.IDNews).Take(top).ToList();
        }

        //End Dùng cho trang home bên user danh mục

        //Hiển thị các bài viết có cùng id danh mục của admin
        public List<NEWS> ListByGroupID(int id, ref int totalRecord)
        {
            totalRecord = db.NEWS.Where(x => x.IDCateNews == id).Count();
            //Đầu tiên lấy ra toàn bộ bài viết có id danh mục bằng id danh mục truyền vào (db.NEWS.Where(x => x.IDCateNews == id))
            var model = db.NEWS.Where(x => x.IDCateNews == id).OrderByDescending(x => x.IDNews).ToList();
            return model;
        }

        //Dùng cho admin
        public NEWS ViewNews(int id)
        {
            IQueryable<NEWS> listUser = db.NEWS;
            var list = listUser.Where(x => x.IDNews == id).FirstOrDefault();
            return list;
        }

        public NEWS ViewDetail(int id)
        {
            return db.NEWS.Find(id);
        }

        public long Insert(NEWS entity)
        {
            DateTime createDay = DateTime.Now;
            entity.NgayTao = createDay;
            string s = entity.TieuDe.ToUpper();
            entity.TieuDe = s;
            db.NEWS.Add(entity);
            db.SaveChanges();
            return entity.IDNews;
        }

        public bool Update(NEWS entity)
        {
            try
            {
                var news = db.NEWS.Find(entity.IDNews);

                news.TieuDe = entity.TieuDe.ToUpper();
                news.UrlTitle = addMinus(convertToUnSign3(entity.TieuDe));
                news.TomTat = entity.TomTat;
                news.GioiThieu= entity.GioiThieu;
                news.Image = entity.Image;
                news.NoiDung= entity.NoiDung;
                news.NgaySua = DateTime.Now;
                news.AnHien = entity.AnHien;
                news.IDCateNews= entity.IDCateNews;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        //Hàm chuyển có dấu sang không dấu để tạo meta title
        public string convertToUnSign3(string s)
        {
            char[] MyChar = { '?',':',' '};
            string newS = s.TrimEnd(MyChar);
            //Ghi thường và loại bỏ khoảng trắng
            string cat = newS.ToLower().Trim();
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = cat.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }
        //Hàm thêm "-" để tạo title
        public string addMinus(string s)
        {
            while (s.IndexOf(" ") != -1)
            {
                s = s.Replace(" ", "-");
            }
            return s;
        }
    }
}
