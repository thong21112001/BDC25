using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CommentDao
    {
        BDC25DbContext db = null;
        public CommentDao()
        {
            db = new BDC25DbContext();
        }

        public List<COMMENT> ListCmt(int id)
        {
            return db.COMMENTs.Where(x => x.IDCommunity == id && x.AnHien==true).OrderByDescending(x => x.NgayTao).ToList();
        }

        public IEnumerable<COMMENT> ListALLDrop(int? id, int page, int pageSize)
        {
            List<COMMENT> ds = (from m in db.COMMENTs
                                join idPost in db.COMMUNITies on m.IDCommunity equals idPost.IDCommunity
                                where ((idPost.IDTagsCommunity == id) && (idPost.AnHien == true) && m.AnHien == true && m.IsReport == false)
                                         select m
                                    ).ToList();
            return ds.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
        }

        public IEnumerable<COMMENT> ListCmtAd(string searchString, int page, int pageSize)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                List<COMMENT> ds = (from m in db.COMMENTs
                                      join idUs in db.USERWEBs on m.IDUser equals idUs.IDUser
                                      where (idUs.AnHien == true && idUs.TaiKhoan.Contains(searchString) && m.AnHien == true && m.IsReport == false)
                                      select m
                                    ).ToList();
                return ds.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
            }
            List<COMMENT> dsCmt = (from m in db.COMMENTs
                                         join idPost in db.COMMUNITies on m.IDCommunity equals idPost.IDCommunity
                                         where (idPost.AnHien == true && m.AnHien == true && m.IsReport == false)
                                         select m
                                    ).ToList();

            return dsCmt.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
        }

        public IEnumerable<COMMENT> ListALLDropRip(int? id, int page, int pageSize)
        {
            List<COMMENT> ds = (from m in db.COMMENTs
                                join idPost in db.COMMUNITies on m.IDCommunity equals idPost.IDCommunity
                                where ((idPost.IDTagsCommunity == id) && (idPost.AnHien == true) && m.AnHien == true && m.IsReport == true)
                                select m
                                    ).ToList();
            return ds.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
        }
        public IEnumerable<COMMENT> ListCmtAdRip(string searchString, int page, int pageSize)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                List<COMMENT> ds = (from m in db.COMMENTs
                                    join idUs in db.USERWEBs on m.IDUser equals idUs.IDUser
                                    where (idUs.AnHien == true && idUs.TaiKhoan.Contains(searchString) && m.AnHien == true && m.IsReport == true)
                                    select m
                                    ).ToList();
                return ds.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
            }
            List<COMMENT> dsCmt = (from m in db.COMMENTs
                                   join idPost in db.COMMUNITies on m.IDCommunity equals idPost.IDCommunity
                                   where (idPost.AnHien == true && m.AnHien == true && m.IsReport == true)
                                   select m
                                    ).ToList();

            return dsCmt.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
        }
    }
}
