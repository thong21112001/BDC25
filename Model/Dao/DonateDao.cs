using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI;
using Model.EF;
using PagedList;

namespace Model.Dao
{
    public class DonateDao
    {
        BDC25DbContext db = null;

        public DonateDao()
        {
            db = new BDC25DbContext();
        }
        public IEnumerable<DONATE> ListALLCATE(string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                string key = addMinus(convertToUnSign3(search));

                List<DONATE> ds = (from m in db.DONATEs
                                 where (m.AnHien==true && m.UrlTitle.Contains(key))
                                 select m
                                    ).ToList();
                return ds.OrderByDescending(x => x.NgayTao).ToList();
            }

            List<DONATE> dsDonate = (from m in db.DONATEs
                                    select m
                                    ).ToList();

            return dsDonate.OrderByDescending(x => x.NgayTao).ToList();
        }
        public IEnumerable<DONATE> ListDonate()
        {
            List<DONATE> dsDonate = (from m in db.DONATEs
                                     where (m.AnHien == true)
                                     select m
                                    ).ToList();

            return dsDonate.OrderByDescending(x => x.NgayTao).ToList();
        }

        public DONATE ViewDetail(int id)
        {
            return db.DONATEs.Find(id);
        }

        public long Insert(DONATE entity)
        {
            DateTime createDay = DateTime.Now;
            entity.NgayTao = createDay;
            string s = entity.TieuDe.ToUpper();
            entity.TieuDe = s;
            db.DONATEs.Add(entity);
            db.SaveChanges();
            return entity.IDDonate;
        }

        public bool Update(DONATE entity)
        {
            try
            {
                var news = db.DONATEs.Find(entity.IDDonate);

                news.TieuDe = entity.TieuDe.ToUpper();
                news.File = entity.File;
                news.Type = entity.Type;
                news.UrlTitle = addMinus(convertToUnSign3(entity.TieuDe));
                news.NgaySua = DateTime.Now;
                news.AnHien = entity.AnHien;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool UpdateNotFile(DONATE entity)
        {
            try
            {
                var news = db.DONATEs.Find(entity.IDDonate);

                news.TieuDe = entity.TieuDe.ToUpper();
                news.UrlTitle = addMinus(convertToUnSign3(entity.TieuDe));
                news.NgaySua = DateTime.Now;
                news.AnHien = entity.AnHien;
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
            char[] MyChar = { '?', ':', ' ' };
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
