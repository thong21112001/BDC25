using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Model.Dao;
using Model.EF;
using System.Data.Entity;
using System.Web.UI.WebControls;

namespace BDC25.Areas.Admin.Controllers
{
    public class NewsController : BaseController
    {
        // GET: Admin/News
        BDC25DbContext db = new BDC25DbContext();
        public ActionResult Index(int? idLoaiDM, string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new NewsDao(); 
            var model = dao.ListALLCATE(searchString, page, pageSize);
            var model2 = dao.ListALLDrop(idLoaiDM, page, pageSize);
            if (searchString != null)
            {
                //Giữ nguyên giá trị tìm kiếm trên seacrh
                ViewBag.SearchString = searchString;
                return View(model);
            }
            if (idLoaiDM != null)
            {
                ViewBag.idLoaiDM = idLoaiDM;
                return View(model2);
            }
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var dao = new NewsDao();
            var model = dao.ViewNews(id);
            ViewBag.IDNewsAD = id;
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            NEWS img= new NEWS();
            SetViewBag();
            return View(img);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(NEWS news,HttpPostedFileBase ImageUpload)
        {
            var dao = new NewsDao();
            if (ImageUpload.ContentLength > 0)
            {
                if (news.NoiDung == null)
                {
                    news.NoiDung = "Đoạn nội dung được thêm vào để tránh lỗi";
                }

                string fileName = Path.GetFileName(ImageUpload.FileName);   //Lấy tên file
                string DayFileName = DateTime.Now.ToString("ddMMyyyyHHmm") + "_" + fileName;  //Gán them datime vào tên file
                string path = Path.Combine(Server.MapPath("~/DataImg/images/SubImg/"), DayFileName);    //Lấy đường dẫn
                news.UrlTitle = dao.addMinus(dao.convertToUnSign3(news.TieuDe));
                news.Image = "~/DataImg/images/SubImg/" + DayFileName;

                long id = dao.Insert(news);
                if (id > 0)
                {
                    ImageUpload.SaveAs(path);
                    SetAlert("Thêm thành công", "success");
                    return RedirectToAction("Index", "News");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm thất bai");
                }
                
            }
            SetViewBag();
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dao = new NewsDao().ViewDetail(id);
            Session["imgPath"] = dao.Image;
            SetViewBag(dao.IDCateNews);
            return View(dao);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(NEWS news,HttpPostedFileBase ImageUpload)
        {
            var dao = new NewsDao();
            if (ModelState.IsValid)
            {
                if (ImageUpload != null)
                {
                    string fileName = Path.GetFileName(ImageUpload.FileName);   //Lấy tên file
                    string DayFileName = DateTime.Now.ToString("ddMMyyyyHHmm") + "_" + fileName;  //Gán them datime vào tên file
                    string path = Path.Combine(Server.MapPath("~/DataImg/images/SubImg/"), DayFileName);    //Lấy đường dẫn
                    news.Image = "~/DataImg/images/SubImg/" + DayFileName;
                    news.UrlTitle = dao.addMinus(dao.convertToUnSign3(news.TieuDe));
                    db.Entry(news).State = EntityState.Modified;
                    var res = dao.Update(news);
                    string oldImg = Request.MapPath(Session["imgPath"].ToString());
                    if (res)
                    {
                        ImageUpload.SaveAs(path);
                        if (System.IO.File.Exists(oldImg))
                        {
                            System.IO.File.Delete(oldImg);
                        }
                        SetAlert("Sửa thành công", "success");
                        return RedirectToAction("Index", "News");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sửa thất bai");
                    }
                }
                else
                {
                    DateTime createDay = DateTime.Now;
                    news.NgaySua = createDay;
                    news.Image = Session["imgPath"].ToString();
                    news.UrlTitle = dao.addMinus(dao.convertToUnSign3(news.TieuDe));
                    var res = dao.Update(news);
                    if (res)
                    {
                        SetAlert("Sửa thành công", "success");
                        return RedirectToAction("Index", "News");
                    }
                }
            }
            SetViewBag(news.IDCateNews);
            return View();
        }

        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var tintuc = db.NEWS.Find(id);

            if (tintuc == null)
            {
                return HttpNotFound();
            }
            string curImg = Request.MapPath(tintuc.Image);
            db.Entry(tintuc).State = EntityState.Deleted;
            if (System.IO.File.Exists(curImg))
            {
                if (db.SaveChanges() > 0)
                {
                    System.IO.File.Delete(curImg);
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        //Lấy viewbag gán vào select của drop
        public void SetViewBag(int? selectedID = null)
        {
            var dao = new CategoryNewsDao();
            ViewBag.IDCateNews = new SelectList(dao.ListDropALL(), "IDCateNews", "Ten", selectedID);
        }

        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var res = new NewsDao().ChangeStatus(id);
            return Json(new
            {
                status = res
            });
        }
    }
}