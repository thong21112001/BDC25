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
    public class TagDao
    {
        BDC25DbContext db = null;
        public TagDao()
        {
            db = new BDC25DbContext();
        }

        //Danh sách tag chưa xóa hiển thị lên admin
        public IEnumerable<TAGS_COMMUNITY> ListALLCATE(string searchString, int page, int pageSize)
        {
            IQueryable<TAGS_COMMUNITY> model = db.TAGS_COMMUNITY;
            if (!string.IsNullOrEmpty(searchString))
            {
                string key = addMinus(convertToUnSign3(searchString));
                return model.Where(x => x.StatusDel == false && x.UrlTitle.Contains(key)).OrderByDescending(x => x.IDTagsCommunity).ToPagedList(page, pageSize);
            }
            return model.Where(x => x.StatusDel == false).OrderBy(x => x.IDTagsCommunity).ToPagedList(page, pageSize);
        }

        //Danh sách tag đã xóa hiển thị lên admin
        public IEnumerable<TAGS_COMMUNITY> ListALLCATEDel(string searchString, int page, int pageSize)
        {
            IQueryable<TAGS_COMMUNITY> model = db.TAGS_COMMUNITY;
            if (!string.IsNullOrEmpty(searchString))
            {
                string key = addMinus(convertToUnSign3(searchString));
                return model.Where(x => x.StatusDel == true && x.UrlTitle.Contains(key)).OrderByDescending(x => x.IDTagsCommunity).ToPagedList(page, pageSize);
            }
            return model.Where(x => x.StatusDel == true).OrderBy(x => x.IDTagsCommunity).ToPagedList(page, pageSize);
        }

        public TAGS_COMMUNITY ViewDetail(int id)
        {
            return db.TAGS_COMMUNITY.Find(id);
        }

        public long Insert(TAGS_COMMUNITY entity)
        {
            db.TAGS_COMMUNITY.Add(entity);
            entity.Ten = entity.Ten.ToUpper();
            entity.StatusDel = false;
            db.SaveChanges();
            return entity.IDTagsCommunity;
        }

        public bool Update(TAGS_COMMUNITY entity)
        {
            try
            {
                var tag = db.TAGS_COMMUNITY.Find(entity.IDTagsCommunity);

                tag.Ten = entity.Ten;
                tag.UrlTitle = addMinus(convertToUnSign3(entity.Ten));
                tag.NgaySua = DateTime.Now;
                tag.AnHien = entity.AnHien;

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
                var tag = db.TAGS_COMMUNITY.Find(id);
                tag.StatusDel = true;
                tag.AnHien = false;
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
                var tag = db.TAGS_COMMUNITY.Find(id);
                tag.StatusDel = false;
                tag.AnHien = false;
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
