using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BDC25.Models;
using CKFinder.Settings;
using PagedList;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data.Entity;
using System.Threading;
using System.Runtime.InteropServices;
using System.Web.Services.Description;
using System.IO;
using System.Xml.Linq;

namespace BDC25.Controllers
{
    public class CommunityController : Controller
    {
        // GET: Community
        BDC25DbContext db = new BDC25DbContext();
        public ActionResult Index(int? idGet,int page = 1, int pageSize = 3)
        {
            var dao = new CommunityDao();
            var model = dao.ListByGroupID();

            //Lấy ra TagName và số bài post từ Commun theo TagID
            var tongPostInTag = (from s in db.TAGS_COMMUNITY
                                 where s.AnHien
                                 group s by s.IDTagsCommunity into g
                                 select new CountPostFlowTag()
                                 {
                                     IDTag = g.FirstOrDefault().IDTagsCommunity,
                                     Tagname = g.FirstOrDefault().Ten,
                                     Count = g.FirstOrDefault().COMMUNITies.Where(x=>x.AnHien == true).Count()
                                 }).OrderByDescending(x=>x.Count).Take(10).ToList();

            ViewBag.CountPostInTag = tongPostInTag;

            if (idGet == 1)
            {
                ViewBag.IDChild = idGet;
                SetAlert("Đã lọc bài mới nhất", 1);
            }
            else if (idGet == 2)
            {
                ViewBag.IDChild = idGet;
                SetAlert("Đã lọc bài cũ nhất", 1);
            }
            else if (idGet == 3)
            {
                ViewBag.IDChild = idGet;
                SetAlert("Đã lọc bài theo đánh giá cao nhất", 1);
            }

            ViewBag.ListPostUserCom = dao.ListPostUserCom();
            ViewBag.ListLikePost = dao.ListLikePost();
            return View(model);
        }

        //Hiển thị ra tất cả tag và số bài viết của tag
        public ActionResult AllTag()
        {
            var dao = new CommunityDao();
            //Lấy ra TagName và số bài post từ Commun theo TagID
            var tongPostInTag = (from s in db.TAGS_COMMUNITY
                                 where s.AnHien
                                 group s by s.IDTagsCommunity into g
                                 select new CountPostFlowTag()
                                 {
                                     IDTag = g.FirstOrDefault().IDTagsCommunity,
                                     Tagname = g.FirstOrDefault().Ten,
                                     Count = g.FirstOrDefault().COMMUNITies.Where(x => x.AnHien == true).Count()
                                 }).OrderByDescending(x => x.Count).ToList();

            ViewBag.CountPostInTag = tongPostInTag;
            return View();
        }

        public ActionResult PostOfTag(int idTag)
        {
            var model = new CommunityDao().ListPostTag(idTag);
            ViewBag.IDTAG = idTag;
            return View(model);
        }

        //Hiển thị thông tin chi tiết bài viết us
        public ActionResult PostUser(int IdPostUser)
        {
            string nameUs = Session["NameUser"]?.ToString();
            if (nameUs != null)
            {
                var loai = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == nameUs);
                ViewBag.User = loai.IDUser;
                ViewBag.UserN = loai.TaiKhoan;
            }

            var dao = new CommunityDao();
            var cmtDao = new CommentDao();
            var likeP = new ComlikeDao();
            var model = dao.ViewPost(IdPostUser);

            ViewBag.AllCMT = dao.CountCMT(IdPostUser);  //Đếm số cmt trong post
            ViewBag.like = likeP.Getlikecounts(IdPostUser); //lấy số like trong post
            ViewBag.AllUserlikedislike = likeP.GetallUser(IdPostUser);
            ViewBag.ListPostLienQuan = dao.ListPostLienQuan(IdPostUser);    //Lấy các post liên quan của user đó
            ViewBag.IDPostUser = IdPostUser;
            ViewBag.ListLikePost = dao.ListLikePost();  //Lấy bài viết đánh giá cao
            ViewBag.ListCMT = cmtDao.ListCmt(IdPostUser);   //Lấy toàn bộ cmt trong bài viết
            
            return View(model);
        }

        //Chức năng like
        public ActionResult Like(int postId)
        {
            var Post = db.COMMUNITies.Find(postId);
            string nameUs = Session["NameUser"]?.ToString();
            var loai = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == nameUs);
            var LikeP = db.COMLIKEs.FirstOrDefault(x => x.IDCommunity == postId && x.IDUser == loai.IDUser);

            if (LikeP == null)
            {
                LikeP = new COMLIKE();
                LikeP.IDUser = loai.IDUser;
                LikeP.IsLike = true;
                LikeP.IDCommunity = postId;
                db.COMLIKEs.Add(LikeP);

                if (LikeP != null)
                {
                    Post.DanhGia = Post.DanhGia + 1;
                    db.SaveChanges();
                    SetAlert("Đã thích bài viết", 1);
                    return Redirect("/chi-tiet-bai-viet-cong-dong/" + Post.UrlTitle + "-" + postId);
                }
            }

            if (LikeP != null)
            {
                LikeP.IDUser = loai.IDUser;
                LikeP.IsLike = true;
                LikeP.IDCommunity = postId;

                if (LikeP != null)
                {
                    Post.DanhGia = Post.DanhGia + 1;
                    db.SaveChanges();
                }
            }
            SetAlert("Đã thích bài viết", 1);
            return Redirect("/chi-tiet-bai-viet-cong-dong/" + Post.UrlTitle + "-" + postId);
        }

        //Chức năng dislike
        public ActionResult Dislike(int postId)
        {
            var Post = db.COMMUNITies.Find(postId);
            string nameUs = Session["NameUser"]?.ToString();
            var loai = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == nameUs);
            var LikeP = db.COMLIKEs.FirstOrDefault(x => x.IDCommunity == postId && x.IDUser == loai.IDUser);

            if (LikeP == null)
            {
                LikeP = new COMLIKE();
                LikeP.IDUser = loai.IDUser;
                LikeP.IsLike = false;
                LikeP.IDCommunity = postId;
                db.COMLIKEs.Add(LikeP);

                if (LikeP != null)
                {
                    if (Post.DanhGia == 0 || Post.DanhGia < 0)
                    {
                        Post.DanhGia = 0;
                    }
                    else
                    {
                        Post.DanhGia = Post.DanhGia - 1;
                    }
                    db.SaveChanges();
                    SetAlert("Đã hủy thích bài viết", 1);
                    return Redirect("/chi-tiet-bai-viet-cong-dong/" + Post.UrlTitle + "-" + postId);
                }
            }

            if (LikeP != null)
            {
                LikeP.IDUser = loai.IDUser;
                LikeP.IsLike = false;
                LikeP.IDCommunity = postId;

                if (LikeP != null)
                {
                    if (Post.DanhGia == 0 || Post.DanhGia < 0)
                    {
                        Post.DanhGia = 0;
                    }
                    else
                    {
                        Post.DanhGia = Post.DanhGia - 1;
                    }
                    db.SaveChanges();
                }
            }
            SetAlert("Đã hủy thích bài viết", 1);
            return Redirect("/chi-tiet-bai-viet-cong-dong/" + Post.UrlTitle + "-" + postId);
            //return Content();
        }

        //Bình luận bài viết
        public ActionResult CommentPost(string cmt,int postId)
        {
            string nameUs = Session["NameUser"]?.ToString();
            var Post = db.COMMUNITies.Find(postId);
            var loai = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == nameUs);
            var dao = new CommentDao();
            var CommtP = db.COMMENTs.FirstOrDefault(x => x.IDCommunity == postId);

            if (nameUs != null)
            {
                if (cmt != null && cmt != "")
                {
                    CommtP = new COMMENT();
                    CommtP.NoiDung = cmt;
                    CommtP.NgayTao = DateTime.Now;
                    CommtP.IDCommunity = postId;
                    CommtP.IDUser = loai.IDUser;
                    CommtP.AnHien = true;
                    CommtP.IsReport = false;
                    db.COMMENTs.Add(CommtP);
                    db.SaveChanges();
                    return Redirect("/chi-tiet-bai-viet-cong-dong/" + Post.UrlTitle + "-" + postId);
                }
                else
                {
                    return Redirect("/chi-tiet-bai-viet-cong-dong/" + Post.UrlTitle + "-" + postId);
                }
            }
            return Redirect("/chi-tiet-bai-viet-cong-dong/" + Post.UrlTitle + "-" + postId);
        }

        //Trả lời bình luận lớn
        public ActionResult ReplyCmt(string cmt, int idCmt, int postID)
        {
            string nameUs = Session["NameUser"]?.ToString();
            var Post = db.COMMUNITies.Find(postID);
            var loai = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == nameUs);
            var dao = new CommentDao();
            var CommtP = db.COMMENT_REPLY.FirstOrDefault(x => x.IDComment == idCmt);

            if (nameUs != null)
            {
                CommtP = new COMMENT_REPLY();
                CommtP.NoiDung = cmt;
                CommtP.NgayTao = DateTime.Now;
                CommtP.IDUser = loai.IDUser;
                CommtP.AnHien = true;
                CommtP.IDComment = idCmt;
                CommtP.IsReport = false;
                db.COMMENT_REPLY.Add(CommtP);
                db.SaveChanges();
                return Redirect("/chi-tiet-bai-viet-cong-dong/" + Post.UrlTitle + "-" + postID);
            }
            return Redirect("/chi-tiet-bai-viet-cong-dong/" + Post.UrlTitle + "-" + postID);
        }

        //Dùng để xóa cmt lớn
        public ActionResult DelCmt(int idCmt,int postID)
        {
            var Post = db.COMMUNITies.Find(postID);
            var CommtP = db.COMMENTs.FirstOrDefault(x => x.IDComment == idCmt);
            CommtP.AnHien = false;
            db.SaveChanges();
            return Redirect("/chi-tiet-bai-viet-cong-dong/" + Post.UrlTitle + "-" + postID);
        }

        //Dùng để xóa cmt nhỏ trong cmt lớn
        public ActionResult SubDelCmt(int idCmt, int postID)
        {
            var Post = db.COMMUNITies.Find(postID);
            var CommtP = db.COMMENT_REPLY.FirstOrDefault(x => x.IDCommentReply == idCmt);
            CommtP.AnHien = false;
            db.SaveChanges();
            return Redirect("/chi-tiet-bai-viet-cong-dong/" + Post.UrlTitle + "-" + postID);
        }

        //Dùng để chỉnh sủa cmt lớn
        public ActionResult EditCmt(string cmt, int idCmt, int postID)
        {
            string nameUs = Session["NameUser"]?.ToString();
            var Post = db.COMMUNITies.Find(postID);
            var loai = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == nameUs);
            var dao = new CommentDao();
            var CommtP = db.COMMENTs.FirstOrDefault(x => x.IDComment == idCmt);

            if (nameUs != null)
            {
                if (CommtP != null)
                {
                    CommtP.NoiDung = cmt;
                    CommtP.NgaySua = DateTime.Now;
                    db.SaveChanges();
                    return Redirect("/chi-tiet-bai-viet-cong-dong/" + Post.UrlTitle + "-" + postID);
                }
            }
            return Redirect("/chi-tiet-bai-viet-cong-dong/" + Post.UrlTitle + "-" + postID);
        }

        //Dùng để chỉnh sủa cmt nhỏ
        public ActionResult EditCmtSub(string cmt, int idCmt, int postID)
        {
            string nameUs = Session["NameUser"]?.ToString();
            var Post = db.COMMUNITies.Find(postID);
            var loai = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == nameUs);
            var dao = new CommentDao();
            var CommtP = db.COMMENT_REPLY.FirstOrDefault(x => x.IDCommentReply == idCmt);
            if (nameUs != null)
            {
                if (CommtP != null)
                {
                    CommtP.NoiDung = cmt;
                    CommtP.NgaySua = DateTime.Now;
                    db.SaveChanges();
                    return Redirect("/chi-tiet-bai-viet-cong-dong/" + Post.UrlTitle + "-" + postID);
                }
            }
            return Redirect("/chi-tiet-bai-viet-cong-dong/" + Post.UrlTitle + "-" + postID);
        }

        //Dùng để báo cáo cmt lớn
        public ActionResult RipCmt(int idCmt, int postID)
        {
            var Post = db.COMMUNITies.Find(postID);
            var CommtP = db.COMMENTs.FirstOrDefault(x => x.IDComment == idCmt);
            CommtP.IsReport = true;
            db.SaveChanges();
            return Redirect("/chi-tiet-bai-viet-cong-dong/" + Post.UrlTitle + "-" + postID);
        }

        //Dùng để báo cáo cmt nhỏ trong cmt lớn
        public ActionResult RipSubCmt(int idCmt, int postID)
        {
            var Post = db.COMMUNITies.Find(postID);
            var CommtP = db.COMMENT_REPLY.FirstOrDefault(x => x.IDCommentReply == idCmt);
            CommtP.IsReport = true;
            db.SaveChanges();
            return Redirect("/chi-tiet-bai-viet-cong-dong/" + Post.UrlTitle + "-" + postID);
        }

        //Trả về PartialView của một trang
        [ChildActionOnly]
        public ActionResult _ChildComment(int id,int idUs)
        {
            var data = db.COMMENT_REPLY.Where(s => s.IDComment == id && s.AnHien == true).ToList();
            if (idUs != 0)
            {
                var loai = db.USERWEBs.SingleOrDefault(x => x.IDUser == idUs);
                ViewBag.PVUser = loai.IDUser;
                ViewBag.PVUserN = loai.TaiKhoan;
            }
            return PartialView("_ChildComment", data);
        }

        //Trả về các bài post khi lọc và return dữ liệu về PartialView
        [ChildActionOnly]
        public ActionResult _ChildPostComMain(int? id)
        {
            if (id == 1)
            {
                var data = db.COMMUNITies.Where(x => x.AnHien == true).OrderByDescending(x => x.NgayTao).ToList();
                return PartialView("_ChildPostCom", data);
            }
            if (id == 2)
            {
                var data = db.COMMUNITies.Where(x => x.AnHien == true).OrderBy(x => x.IDCommunity).ToList();
                return PartialView("_ChildPostCom", data);
            }
            if (id == 3)
            {
                var data = db.COMMUNITies.Where(x => x.AnHien == true).OrderByDescending(x => x.DanhGia).ToList();
                return PartialView("_ChildPostCom", data);
            }
            var list = db.COMMUNITies.Where(x => x.AnHien == true).OrderByDescending(x => x.NgayTao).ToList();
            return PartialView("_ChildPostCom", list);
        }


        //Thông báo
        protected void SetAlert(string message, int type)
        {
            TempData["AlertMessage"] = message;
            if (type == 1)
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == 2)
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == 3)
            {
                TempData["AlertType"] = "alert-danger";
            }
            else
            {
                TempData["AlertType"] = "alert-info";
            }
        }
    }
}