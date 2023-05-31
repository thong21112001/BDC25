using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI;


namespace Model.Dao
{
    public class CommunityDao
    {
        BDC25DbContext db = null;
        public CommunityDao()
        {
            db = new BDC25DbContext();
        }
        //Lấy toàn bộ danh sách trong DB
        public IEnumerable<COMMUNITY> ListALLCATE(string searchString, int page, int pageSize)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                string key = addMinus(convertToUnSign3(searchString));
                List<COMMUNITY> ds = (from m in db.COMMUNITies
                                 join idCate in db.TAGS_COMMUNITY on m.IDTagsCommunity equals idCate.IDTagsCommunity
                                 where (idCate.StatusDel == false && m.UrlTitle.Contains(key) && m.AnHien == true)
                                 select m
                                    ).ToList();
                return ds.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
            }
            List<COMMUNITY> dsBaiViet = (from m in db.COMMUNITies
                                    join idCate in db.TAGS_COMMUNITY on m.IDTagsCommunity equals idCate.IDTagsCommunity
                                    where (idCate.StatusDel == false && m.AnHien == true)
                                    select m
                                    ).ToList();

            return dsBaiViet.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
        }

        //Lấy toàn bộ danh sách đợi duyệt trong DB
        public IEnumerable<COMMUNITY> ListALLCATEChange(string searchString, int page, int pageSize)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                string key = addMinus(convertToUnSign3(searchString));
                List<COMMUNITY> ds = (from m in db.COMMUNITies
                                      join idCate in db.TAGS_COMMUNITY on m.IDTagsCommunity equals idCate.IDTagsCommunity
                                      where (idCate.StatusDel == false && m.UrlTitle.Contains(key) && m.AnHien == false)
                                      select m
                                    ).ToList();
                return ds.Where(x => x.DanhGia > 0).OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
            }
            List<COMMUNITY> dsBaiViet = (from m in db.COMMUNITies
                                         join idCate in db.TAGS_COMMUNITY on m.IDTagsCommunity equals idCate.IDTagsCommunity
                                         where (idCate.StatusDel == false && m.AnHien == false)
                                         select m
                                    ).ToList();

            return dsBaiViet.Where(x => x.DanhGia > 0).OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
        }

        public IEnumerable<COMMUNITY> ListALLCATEChangeDel(string searchString, int page, int pageSize)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                string key = addMinus(convertToUnSign3(searchString));
                List<COMMUNITY> ds = (from m in db.COMMUNITies
                                      join idCate in db.TAGS_COMMUNITY on m.IDTagsCommunity equals idCate.IDTagsCommunity
                                      where (idCate.StatusDel == false && m.UrlTitle.Contains(key) && m.AnHien == false)
                                      select m
                                    ).ToList();
                return ds.Where(x => x.DanhGia <= 0).OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
            }
            List<COMMUNITY> dsBaiViet = (from m in db.COMMUNITies
                                         join idCate in db.TAGS_COMMUNITY on m.IDTagsCommunity equals idCate.IDTagsCommunity
                                         where (idCate.StatusDel == false && m.AnHien == false)
                                         select m
                                    ).ToList();

            return dsBaiViet.Where(x => x.DanhGia <= 0).OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
        }

        //Lấy bài viết ra trang cộng đồng
        public List<COMMUNITY> ListByGroupID()
        {
            //Đầu tiên lấy ra toàn bộ bài viết có id danh mục bằng id danh mục truyền vào (db.NEWS.Where(x => x.IDCateNews == id))
            var model = db.COMMUNITies.Where(x => x.AnHien == true).OrderByDescending(x => x.TieuDe).ToList();
            return model;
        }


        //Hiển thị ra luôn có 
        public List<COMMUNITY> SearchkeyCom(string key, ref int totalRecord)
        {

            totalRecord = db.COMMUNITies.Where(x => x.TieuDe.Contains(key)).Count();

            string keyW = addMinus(convertToUnSign3(key));
            List<COMMUNITY> ds = (from m in db.COMMUNITies
                             join idCate in db.TAGS_COMMUNITY on m.IDTagsCommunity equals idCate.IDTagsCommunity
                             where (idCate.StatusDel == false && m.UrlTitle.Contains(keyW) && m.AnHien == true)
                             select m
                                ).ToList();

            return ds.OrderByDescending(x => x.NgayTao).ToList();
        }

        //Info User
        public List<COMMUNITY> GetPostFollow(int idUser)
        {
            List<COMMUNITY> ds = (from m in db.COMMUNITies
                                  join fl in db.USERFOLLOWs on m.IDUser equals fl.IDUserFollow
                                  where (fl.IsFollow == true && fl.IDUser == idUser && fl.IDUserFollow == m.IDUser && m.AnHien == true && fl.NgayFollow <= m.NgayTao)
                                  select m
                                ).OrderByDescending(x => x.NgayTao).ToList();
            return ds;
        }

        public List<COMMUNITY> ListPostNotActiveUser(int idUser)
        {
            int? idPostUser = null;

            var listUser = db.USERWEBs.Where(x => x.IDUser == idUser).FirstOrDefault();

            if (listUser.IDUser != 0)
            {
                return db.COMMUNITies.Where(x => x.IDUser == listUser.IDUser && x.AnHien == false && x.DanhGia > 0).OrderByDescending(x => x.NgayTao).ToList();
            }

            return db.COMMUNITies.Where(x => x.IDUser == idPostUser && x.DanhGia > 0).ToList();
        }
        public int SoLuongBvKhongDuyet(int idUser)
        {
            var listPostIdUSEr = db.USERWEBs.Where(x => x.IDUser == idUser).FirstOrDefault();
            return db.COMMUNITies.Where(x => x.IDUser == listPostIdUSEr.IDUser && x.AnHien == false && x.DanhGia > 0).Count();
        }

        public List<COMMUNITY> ListPosttActiveUser(int idUser)
        {
            int? idPostUser = null;

            var listUser = db.USERWEBs.Where(x => x.IDUser == idUser).FirstOrDefault();

            if (listUser.IDUser != 0)
            {
                return db.COMMUNITies.Where(x => x.IDUser == listUser.IDUser && x.AnHien == true).OrderByDescending(x => x.NgayTao).ToList();
            }

            return db.COMMUNITies.Where(x => x.IDUser == idPostUser).ToList();
        }

        public int SoLuongBvDuyet(int idUser)
        {
            var listPostIdUSEr = db.USERWEBs.Where(x => x.IDUser == idUser).FirstOrDefault();
            return db.COMMUNITies.Where(x => x.IDUser == listPostIdUSEr.IDUser && x.AnHien == true).Count();
        }

        public bool CheckUserPost(int idUser)
        {
            var listPostIdUSEr = db.COMMUNITies.Where(x => x.IDUser == idUser).FirstOrDefault();

            if (listPostIdUSEr == null)
            {
                return false;
            }
            return true;
        }
        //Info User

        //Hiển thị các bài viết đánh giá cao
        public List<COMMUNITY> ListLikePost()
        {
            //int top = 5;
            return db.COMMUNITies.Where(x => x.AnHien == true).OrderByDescending(x => x.DanhGia).Take(5).ToList();
        }

        //hiện các bài viết lên cộng đồng
        public List<COMMUNITY> ListPostUserCom()
        {
            return db.COMMUNITies.Where(x => x.AnHien == true).OrderByDescending(x => x.NgayTao).ToList();
        }

        //Hiện lên home
        public List<COMMUNITY> ListPostUser(int top = 9)
        {
            List<COMMUNITY> dsBaiViet = (from m in db.COMMUNITies
                                    join idCate in db.TAGS_COMMUNITY on m.IDTagsCommunity equals idCate.IDTagsCommunity
                                    where (idCate.StatusDel == false && idCate.AnHien == true)
                                    select m
                                    ).ToList();

            return dsBaiViet.Where(x => x.AnHien == true).OrderByDescending(x => x.NgayTao).Take(top).ToList();
        }

        //Hiển thị số lượt bình luận trong bài viết
        public int CountCMT(int idPost)
        {
            var postUS = db.COMMUNITies.Find(idPost);
            int countCmtRep = db.COMMENTs.Where(x => x.IDCommunity == idPost && x.AnHien == true).Count();  // lấy các bình luận chưa xóa
            
            List<COMMENT_REPLY> countRep = (from rep in db.COMMENT_REPLY
                                            join idC in db.COMMENTs on rep.IDComment equals idC.IDComment
                                            join c in db.COMMUNITies on idC.IDCommunity equals c.IDCommunity
                                            where (c.AnHien == true && c.IDCommunity == idPost && c.IDCommunity == idC.IDCommunity && idC.IDComment == rep.IDComment && 
                                                    idC.AnHien == true && rep.AnHien == true)
                                            select rep
                                            ).ToList();

            int countCmtReply = countRep.Count();

            int allCMT = countCmtRep + countCmtReply;

            return allCMT;
        }

        public long Insert(COMMUNITY entity)
        {
            DateTime createDay = DateTime.Now;
            entity.NgayTao = createDay;
            string s = entity.TieuDe.ToUpper();
            entity.TieuDe = s;
            entity.AnHien = false;
            db.COMMUNITies.Add(entity);
            db.SaveChanges();
            return entity.IDCommunity;
        }

        //Update bài viết usser
        public bool Update(COMMUNITY entity)
        {
            try
            {
                var news = db.COMMUNITies.Find(entity.IDCommunity);

                news.TieuDe = entity.TieuDe.ToUpper();
                news.UrlTitle = entity.UrlTitle;
                news.TomTat = entity.TomTat;
                news.GioiThieu = entity.GioiThieu;
                news.Image = entity.Image;
                news.NoiDung = entity.NoiDung;
                news.NgaySua = DateTime.Now;
                news.AnHien = false;
                news.IDTagsCommunity = entity.IDTagsCommunity;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }


        //Hiển thị lên droplist tag bài viết
        public IEnumerable<COMMUNITY> ListALLDrop(int? id, int page, int pageSize)
        {
            List<COMMUNITY> dsBaiViet = (from m in db.COMMUNITies
                                    join idTAG in db.TAGS_COMMUNITY on m.IDTagsCommunity equals idTAG.IDTagsCommunity
                                    where ((idTAG.IDTagsCommunity == id) && (idTAG.AnHien == true) && m.AnHien == true)
                                    select m
                                    ).ToList();
            return dsBaiViet.OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
        }
        public IEnumerable<COMMUNITY> ListALLDropChange(int? id, int page, int pageSize)
        {
            List<COMMUNITY> dsBaiViet = (from m in db.COMMUNITies
                                         join idTAG in db.TAGS_COMMUNITY on m.IDTagsCommunity equals idTAG.IDTagsCommunity
                                         where ((idTAG.IDTagsCommunity == id) && (idTAG.AnHien == true) && m.AnHien == false)
                                         select m
                                    ).ToList();
            return dsBaiViet.Where(x => x.DanhGia > 0).OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
        }

        public IEnumerable<COMMUNITY> ListALLDropChangeDel(int? id, int page, int pageSize)
        {
            List<COMMUNITY> dsBaiViet = (from m in db.COMMUNITies
                                         join idTAG in db.TAGS_COMMUNITY on m.IDTagsCommunity equals idTAG.IDTagsCommunity
                                         where ((idTAG.IDTagsCommunity == id) && (idTAG.AnHien == true) && m.AnHien == false)
                                         select m
                                    ).ToList();
            return dsBaiViet.Where(x => x.DanhGia <= 0).OrderByDescending(x => x.NgayTao).ToPagedList(page, pageSize);
        }
        //Hiển thị ra bài viết thuộc tag đó
        public List<COMMUNITY> ListPostTag(int id)
        {
            //Đầu tiên lấy ra toàn bộ bài viết có id danh mục bằng id danh mục truyền vào (db.NEWS.Where(x => x.IDCateNews == id))
            var model = db.COMMUNITies.Where(x => x.IDTagsCommunity == id && x.DanhGia > 0 && x.AnHien == true).OrderByDescending(x => x.NgayTao).ToList();
            return model;
        }

        //Dùng cho user xem bài viết 
        public COMMUNITY ViewPost(int id)
        {
            IQueryable<COMMUNITY> listPost = db.COMMUNITies;
            var list = listPost.Where(x => x.IDCommunity == id).FirstOrDefault();
            return list;
        }
        //Các bài liên quan trong post
        public List<COMMUNITY> ListPostLienQuan(int id)
        {
            int top = 3;
            var idPost = db.COMMUNITies.Where(x => x.IDCommunity == id).FirstOrDefault();

            var listIDcate = db.COMMUNITies.Where(x => x.IDTagsCommunity == idPost.IDTagsCommunity).FirstOrDefault();

            return db.COMMUNITies.Where(x => x.IDTagsCommunity == listIDcate.IDTagsCommunity).OrderByDescending(x => x.NgayTao).Take(top).ToList();
        }

        //Dùng cho admin xem chi tiết bài viết user
        public COMMUNITY ViewPostOfUser(int id)
        {
            IQueryable<COMMUNITY> listbvOfUser = db.COMMUNITies;
            var listBaiViet = listbvOfUser.Where(x => x.IDCommunity == id).FirstOrDefault();
            return listBaiViet;
        }

        //Dùng cho user xem và chỉnh sửa
        public COMMUNITY ViewDetail(int id)
        {
            return db.COMMUNITies.Find(id);
        }

        public bool AnPost(int id)
        {
            try
            {
                var news = db.COMMUNITies.Find(id);
                news.AnHien = false;
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

        //Dùng cho admin
        //Dùng js để kích hoạt 
        public bool ChangeStatus(int id)
        {
            var bv = db.COMMUNITies.Find(id);
            bv.AnHien = !bv.AnHien;
            db.SaveChanges();
            return bv.AnHien;
        }
    }
}
