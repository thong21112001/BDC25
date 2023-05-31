using BDC25.Common;
using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace BDC25.Controllers
{
    public class InfoUserController : Controller
    {
        // GET: InfoUser
        BDC25DbContext db = new BDC25DbContext();
        public ActionResult Index(string TaiKhoan)
        {
            var dao = new CommunityDao();
            string nameUs = Session["NameUser"]?.ToString();
            var usFLw = new FollowDao();

            if (nameUs != null && nameUs == TaiKhoan)   //Khi đăng nhập nick mình và xem tài khoản mình
            {
                var loai = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == nameUs);

                Session["IDUserLogin"] = loai.IDUser;

                ViewBag.CheckUser = dao.CheckUserPost(loai.IDUser);
                ViewBag.ListBvChuaDuyet = dao.ListPostNotActiveUser(loai.IDUser);
                ViewBag.ListBvDaDuyet = dao.ListPosttActiveUser(loai.IDUser);
                ViewBag.SoLuongDuyet = dao.SoLuongBvDuyet(loai.IDUser);
                ViewBag.SoLuongKhongDuyet = dao.SoLuongBvKhongDuyet(loai.IDUser);

                ViewBag.PostFollow = dao.GetPostFollow(loai.IDUser);

                ViewBag.AllUserFlow = usFLw.GetallUser(loai.IDUser);

                ViewBag.IDUSer = loai.IDUser;
            }
            if (nameUs != null && nameUs != TaiKhoan)   //Khi đăng nhập nick mình và xem tài khoản người khác
            {
                var loai = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == nameUs);
                var loaiKhac = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == TaiKhoan);
                var FloUS = db.USERFOLLOWs.FirstOrDefault(x => x.IDUserFollow == loaiKhac.IDUser && x.IDUser == loai.IDUser);

                Session["IDUserLogin"] = loai.IDUser;
                Session["IDUserNotLogin"] = loaiKhac.IDUser;    //Dùng cho tài khoản khác

                ViewBag.CheckUser = dao.CheckUserPost(loaiKhac.IDUser);
                ViewBag.ListBvChuaDuyet = dao.ListPostNotActiveUser(loaiKhac.IDUser);
                ViewBag.ListBvDaDuyet = dao.ListPosttActiveUser(loaiKhac.IDUser);
                ViewBag.SoLuongDuyet = dao.SoLuongBvDuyet(loaiKhac.IDUser);
                ViewBag.SoLuongKhongDuyet = dao.SoLuongBvKhongDuyet(loaiKhac.IDUser);

                ViewBag.AllUserFlow = usFLw.GetallUser(loaiKhac.IDUser);

                if (FloUS != null)
                {
                    ViewBag.IsFL = FloUS.IsFollow;
                }

                ViewBag.IDUSer = loai.IDUser;
                ViewBag.IDUSerNotLogin = loaiKhac.IDUser;
            }
            else//Khi không đăng nhập nhưng xem tài khoản
            {
                var loai = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == TaiKhoan);
                Session["IDUserNotLogin"] = loai.IDUser;

                ViewBag.CheckUser = dao.CheckUserPost(loai.IDUser);
                ViewBag.ListBvChuaDuyet = dao.ListPostNotActiveUser(loai.IDUser);
                ViewBag.ListBvDaDuyet = dao.ListPosttActiveUser(loai.IDUser);
                ViewBag.SoLuongDuyet = dao.SoLuongBvDuyet(loai.IDUser);
                ViewBag.SoLuongKhongDuyet = dao.SoLuongBvKhongDuyet(loai.IDUser);
                ViewBag.AllUserFlow = usFLw.GetallUser(loai.IDUser);
                ViewBag.IDUSerNotLogin = loai.IDUser;
            }

            return View();
        }

        [HttpGet]
        public ActionResult CreatePost(int? idLoaiDM, string TaiKhoan)
        {
            COMMUNITY img = new COMMUNITY();
            ViewBag.idLoaiDM = idLoaiDM;
            return View(img);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreatePost(int idLoaiDM,COMMUNITY com, HttpPostedFileBase ImageUpload)
        {
            var dao = new CommunityDao();
            string nameUs = Session["NameUser"].ToString();
            var loai = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == nameUs);

            if (ImageUpload.ContentLength > 0)
            {
                if (com.NoiDung == null)
                {
                    com.NoiDung = "Đoạn nội dung được thêm vào để tránh lỗi";
                }

                string fileName = Path.GetFileName(ImageUpload.FileName);   //Lấy tên file
                string DayFileName = DateTime.Now.ToString("ddMMyyyyHHmm") + "_" + fileName;  //Gán them datime vào tên file
                string path = Path.Combine(Server.MapPath("~/DataImg/images/SubImg/"), DayFileName);    //Lấy đường dẫn
                com.UrlTitle = dao.addMinus(dao.convertToUnSign3(com.TieuDe));
                com.Image = "~/DataImg/images/SubImg/" + DayFileName;
                com.IDTagsCommunity = idLoaiDM;
                com.DanhGia = 1;
                com.IDUser = loai.IDUser;

                long id = dao.Insert(com);
                if (id > 0)
                {
                    ImageUpload.SaveAs(path);
                    SetAlert("Đăng bài viết thành công đợi duyệt", 1);
                    return Redirect("/thong-tin-ca-nhan/" + loai.TaiKhoan);
                }
                else
                {
                    ModelState.AddModelError("", "Thêm thất bai");
                }

            }
            return View();
        }

        [HttpGet]
        public ActionResult EditPost(int? idLoaiDM, int IdPost)
        {
            var dao = new CommunityDao().ViewDetail(IdPost);
            Session["PathImgPost"] = dao.Image;
            ViewBag.idLoaiDM = dao.IDTagsCommunity;
            ViewBag.idPost = IdPost;
            return View(dao);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditPost(int idLoaiDM, COMMUNITY com, HttpPostedFileBase ImageUpload)
        {
            var dao = new CommunityDao();
            string nameUs = Session["NameUser"].ToString();
            var loai = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == nameUs);

            if (ImageUpload != null)
            {
                if (com.NoiDung == null)
                {
                    com.NoiDung = "Đoạn nội dung được thêm vào để tránh lỗi";
                }

                string fileName = Path.GetFileName(ImageUpload.FileName);   //Lấy tên file
                string DayFileName = DateTime.Now.ToString("ddMMyyyyHHmm") + "_" + fileName;  //Gán them datime vào tên file
                string path = Path.Combine(Server.MapPath("~/DataImg/images/SubImg/"), DayFileName);    //Lấy đường dẫn
                com.Image = "~/DataImg/images/SubImg/" + DayFileName;

                com.UrlTitle = dao.addMinus(dao.convertToUnSign3(com.TieuDe));
                com.IDTagsCommunity = idLoaiDM;

                var res = dao.Update(com);
                string oldImg = Request.MapPath(Session["PathImgPost"].ToString());
                if (res)
                {
                    ImageUpload.SaveAs(path);
                    if (System.IO.File.Exists(oldImg))
                    {
                        System.IO.File.Delete(oldImg);
                    }
                    SetAlert("Sửa bài viết thành công đợi duyệt", 1);
                    return Redirect("/thong-tin-ca-nhan/" + loai.TaiKhoan);
                }
                else
                {
                    ModelState.AddModelError("", "Sửa thất bai");
                }

            }
            else
            {
                com.Image = Session["PathImgPost"].ToString();
                com.UrlTitle = dao.addMinus(dao.convertToUnSign3(com.TieuDe));
                com.IDTagsCommunity = idLoaiDM;
                var res = dao.Update(com);
                if (res)
                {
                    SetAlert("Sửa bài viết thành công đợi duyệt", 1);
                    return Redirect("/thong-tin-ca-nhan/" + loai.TaiKhoan);
                }
            }
            return View();
        }

        //public ActionResult AnPost(int id)
        //{
        //    string nameUs = Session["NameUser"].ToString();
        //    var loai = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == nameUs);

        //    bool dao = new CommunityDao().AnPost(id);
        //    if (dao == true)
        //    {
        //        SetAlert("Ẩn bài viết thành công", 1);
        //        return Redirect("/thong-tin-ca-nhan/" + loai.TaiKhoan);
        //    }
        //    SetAlert("Ẩn bài viết thất bại", 3);
        //    return Redirect("/thong-tin-ca-nhan/" + loai.TaiKhoan);
        //}


        public ActionResult Delete(int? id)
        {
            string nameUs = Session["NameUser"].ToString();
            var loai = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == nameUs);
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var tintuc = db.COMMUNITies.Find(id);
            var like = db.COMLIKEs.Find(id);
            var cmt = db.COMMENTs.Find(id);

            if (tintuc == null)
            {
                return HttpNotFound();
            }
           

            tintuc.AnHien = false;
            tintuc.DanhGia = 0;
            db.SaveChanges();
            SetAlert("Xóa bài viết thành công", 1);
            return Redirect("/thong-tin-ca-nhan/" + loai.TaiKhoan);
        }

        [HttpGet]
        public ActionResult EditProfile(int IDUser)
        {
            var dao = new UserDao().ViewDetail(IDUser);
            ViewBag.IDUSer = IDUser;
            Session["imgPath"] = dao.Image;
            return View(dao);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditProfile(int IDUser,USERWEB us, HttpPostedFileBase ImageUpload)
        {
            var dao = new UserDao();
            string oldImg = null;
            var check = Session["imgPath"]?.ToString();

            if (check != null && us.ImageUpload != null)//Dùng để edit thông tin khi có ảnh cũ và thay thành ảnh mới
            {
                if (ImageUpload != null && ImageUpload.ContentLength > 0)//Dùng cập nhập toàn bộ bao gồm thay đổi cả ảnh cũ
                {
                    string fileName = Path.GetFileName(ImageUpload.FileName);   //Lấy tên file
                    string DayFileName = DateTime.Now.ToString("ddMMyyyyHHmm") + "_" + fileName;  //Gán them datime vào tên file
                    string path = Path.Combine(Server.MapPath("~/DataImg/images/SubImg/"), DayFileName);    //Lấy đường dẫn
                    us.Image = "~/DataImg/images/SubImg/" + DayFileName;

                    var res = dao.Update(us);
                    if (check != null)
                    {
                        oldImg = Request.MapPath(Session["imgPath"].ToString());
                    }

                    if (res)
                    {
                        ImageUpload.SaveAs(path);
                        if (oldImg != null)
                        {
                            if (System.IO.File.Exists(oldImg))
                            {
                                System.IO.File.Delete(oldImg);
                            }
                        }
                        SetAlert("Cập nhập thông tin thành công", 1);
                        return RedirectToAction("EditProfile", "InfoUser");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sửa thất bai");
                    }
                }
            }
            else if (check != null && us.ImageUpload == null)//Dùng để edit thông tin khi có ảnh cũ và không thay đổi ảnh
            {
                var resE1 = dao.UpdateNotImage(us);
                if (resE1)
                {
                    SetAlert("Cập nhập thông tin thành công", 1);
                    return RedirectToAction("EditProfile", "InfoUser");
                }
                else
                {
                    ModelState.AddModelError("", "Sửa thất bai");
                }
            }
            else if (check == null && us.ImageUpload != null)//Dùng để edit thông tin khi không có ảnh cũ và up ảnh mới + thay đổi thông tin
            {
                string fileName = Path.GetFileName(ImageUpload.FileName);   //Lấy tên file
                string DayFileName = DateTime.Now.ToString("ddMMyyyyHHmm") + "_" + fileName;  //Gán them datime vào tên file
                string path = Path.Combine(Server.MapPath("~/DataImg/images/SubImg/"), DayFileName);    //Lấy đường dẫn
                us.Image = "~/DataImg/images/SubImg/" + DayFileName;

                var res = dao.Update(us);
                if (check != null)
                {
                    oldImg = Request.MapPath(Session["imgPath"].ToString());
                }

                if (res)
                {
                    ImageUpload.SaveAs(path);
                    if (oldImg != null)
                    {
                        if (System.IO.File.Exists(oldImg))
                        {
                            System.IO.File.Delete(oldImg);
                        }
                    }
                    SetAlert("Cập nhập thông tin thành công", 1);
                    return RedirectToAction("EditProfile", "InfoUser");
                }
                else
                {
                    ModelState.AddModelError("", "Sửa thất bai");
                }
            }
            else //Dùng khi không úp ảnh nhưng không có ảnh cũ chỉ cập nhập thông tin
            {
                var resE2 = dao.UpdateNotImage(us);
                if (resE2)
                {
                    SetAlert("Cập nhập thông tin thành công", 1);
                    return RedirectToAction("EditProfile", "InfoUser");
                }
                else
                {
                    ModelState.AddModelError("", "Sửa thất bai");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult PVUpdatePass(int IDUser, USERWEB us, string newPass, string checkNewPass)
        {
            var dao = new UserDao();

            var loai = db.USERWEBs.Find(IDUser);

            bool checkPass = dao.CheckPass(IDUser,Encryptor.MD5Hash(us.MatKhau));
            if (checkPass == true)
            {
                if (newPass == checkNewPass)
                {
                    bool UpDatePass = dao.UpdatePass(loai.TaiKhoan, Encryptor.MD5Hash(newPass));
                    if (UpDatePass == true)
                    {
                        SetAlert("Đổi mật khẩu thành công", 1);
                        return Redirect("/thong-tin-ca-nhan/chinh-sua/" + IDUser);
                    }
                    else
                    {
                        SetAlert("Đổi mật khẩu thất bại", 3);
                        return Redirect("/thong-tin-ca-nhan/chinh-sua/" + IDUser);
                    }
                }
                else
                {
                    SetAlert("Mật khẩu mới không trùng", 2);
                    return Redirect("/thong-tin-ca-nhan/chinh-sua/" + IDUser);
                }
            }
            else
            {
                SetAlert("Mật khẩu cũ không đúng", 2);
                return Redirect("/thong-tin-ca-nhan/chinh-sua/"+IDUser);
            }
        }

        public ActionResult Follow(int UserIDFollow)
        {
            var usID = db.USERWEBs.Find(UserIDFollow);  //Tìm cái ID user mình cần follow
            string nameUs = Session["NameUser"]?.ToString();    //Lấy tên tài khoản hiện tại đăng nhập
            var loai = db.USERWEBs.SingleOrDefault(x => x.TaiKhoan == nameUs); //Kiểm tra tên tài khoản hiện tại đăng nhập
            var FloUS = db.USERFOLLOWs.FirstOrDefault(x => x.IDUserFollow == UserIDFollow && x.IDUser == loai.IDUser);
           
            if (FloUS != null)
            {
                bool status = FloUS.IsFollow;
                if (status == true)
                {
                    FloUS.IsFollow = false;
                    FloUS.IDUser = loai.IDUser;
                    FloUS.IDUserFollow = UserIDFollow;
                    db.SaveChanges();
                    SetAlert("Đã hủy theo dõi người dùng", 1);
                    return Redirect("/thong-tin-ca-nhan/" + usID.TaiKhoan);
                }
                else
                {
                    FloUS.IsFollow = true;
                    FloUS.IDUser = loai.IDUser;
                    FloUS.IDUserFollow = UserIDFollow;
                    FloUS.NgayFollow = DateTime.Now;
                    db.SaveChanges();
                    SetAlert("Đã theo dõi người dùng", 1);
                    return Redirect("/thong-tin-ca-nhan/" + usID.TaiKhoan);
                }
            }

            if (FloUS == null)
            {
                FloUS = new USERFOLLOW();
                FloUS.IDUser = loai.IDUser;
                FloUS.IDUserFollow = UserIDFollow;
                FloUS.IsFollow = true;
                FloUS.NgayFollow = DateTime.Now;
                db.USERFOLLOWs.Add(FloUS);

                if (FloUS != null)
                {
                    db.SaveChanges();
                }
            }
            Session["IDUserFl"] = UserIDFollow;
            SetAlert("Đã theo dõi người dùng", 1);
            return Redirect("/thong-tin-ca-nhan/" + usID.TaiKhoan);
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