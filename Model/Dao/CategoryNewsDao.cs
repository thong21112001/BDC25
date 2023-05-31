using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Model.EF;
using PagedList;

namespace Model.Dao
{
    public class CategoryNewsDao
    {
        BDC25DbContext db = null;
        public CategoryNewsDao()
        {
            db = new BDC25DbContext();
        }

        //Lấy toàn bộ danh sách trong DB 
        public IEnumerable<CATEGORY_NEWS> ListALLCATE(string searchString, int page, int pageSize)
        {
            IQueryable<CATEGORY_NEWS> model = db.CATEGORY_NEWS;

            if (!string.IsNullOrEmpty(searchString))
            {
                string key = addMinus(convertToUnSign3(searchString));
                return model.Where(x => x.StatusDel == false && x.UrlTitle.Contains(key)).OrderBy(x => x.IDCateNews).ToPagedList(page, pageSize);
            }
            return model.Where(x => x.StatusDel == false).OrderBy(x => x.IDCateNews).ToPagedList(page, pageSize);
        }
        //Trang danh sách xóa trong admin
        public IEnumerable<CATEGORY_NEWS> ListALLCATEDel(string searchString, int page, int pageSize)
        {
            IQueryable<CATEGORY_NEWS> model = db.CATEGORY_NEWS;
            if (!string.IsNullOrEmpty(searchString))
            {
                string key = addMinus(convertToUnSign3(searchString));
                return model.Where(x => x.StatusDel == true && x.UrlTitle.Contains(key)).OrderBy(x => x.IDCateNews).ToPagedList(page, pageSize);
            }
            return model.Where(x => x.StatusDel == true).OrderBy(x => x.IDCateNews).ToPagedList(page, pageSize);
        }
        public List<CATEGORY_NEWS> ListByGroupID()  //Hiển thị lên Nav khi AnHien = true( hiển thị ) và StatusDel = false(không xóa)
        {
            return db.CATEGORY_NEWS.Where(x => x.AnHien == true && x.StatusDel == false).ToList();
        }

        public List<CATEGORY_NEWS> ListDropALL() //Hiển thị lên drop khi AnHien = true( hiển thị ) và StatusDel = false(không xóa)
        {
            return db.CATEGORY_NEWS.Where(x => x.AnHien == true && x.StatusDel == false).ToList();
        }

        public string GetUrlPostNew()
        {
            int id = 2;
            var cate_news = db.CATEGORY_NEWS.Find(id);
            string url = cate_news.UrlTitle;
            return url;
        }

        public CATEGORY_NEWS ViewDetail(int id)
        {
            return db.CATEGORY_NEWS.Find(id);
        }

        public long Insert(CATEGORY_NEWS entity)
        {
            db.CATEGORY_NEWS.Add(entity);
            entity.StatusDel = false;
            db.SaveChanges();
            return entity.IDCateNews;
        }

        public bool Update(CATEGORY_NEWS entity)
        {
            try
            {
                var cate_news = db.CATEGORY_NEWS.Find(entity.IDCateNews);

                cate_news.Ten = entity.Ten;
                cate_news.UrlTitle = addMinus(convertToUnSign3(entity.Ten));
                cate_news.NgaySua = DateTime.Now;
                cate_news.AnHien = entity.AnHien;

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var cate = db.CATEGORY_NEWS.Find(id);
                cate.StatusDel = true;
                cate.AnHien = false;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public bool ReDelete(int id)
        {
            try
            {
                var cate = db.CATEGORY_NEWS.Find(id);
                cate.StatusDel = false;
                cate.AnHien = false;
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
            //Ghi thường và loại bỏ khoảng trắng
            string cat = s.ToLower().Trim();
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
