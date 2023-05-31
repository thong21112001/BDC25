using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDC25.Areas.Admin.Controllers
{
    public class CommunityController : BaseController
    {
        // GET: Admin/Community
        BDC25DbContext db = new BDC25DbContext();
        public ActionResult Index(int? idLoaiDM, string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new CommunityDao();
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

        public ActionResult ChangePost(int? idLoaiDM, string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new CommunityDao();
            var model = dao.ListALLCATEChange(searchString, page, pageSize);
            var model2 = dao.ListALLDropChange(idLoaiDM, page, pageSize);
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

        public ActionResult PostDelUs(int? idLoaiDM, string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new CommunityDao();
            var model = dao.ListALLCATEChangeDel(searchString, page, pageSize);
            var model2 = dao.ListALLDropChangeDel(idLoaiDM, page, pageSize);
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

        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            var tintuc = db.COMMUNITies.Find(id);

            if (tintuc == null)
            {
                return HttpNotFound();
            }

            tintuc.AnHien = false;
            tintuc.DanhGia = 0;
            db.SaveChanges();
            return View();
        }

        public ActionResult Details(int id)
        {
            var dao = new CommunityDao();
            var model = dao.ViewPostOfUser(id);
            ViewBag.IDPost = id;
            return View(model);
        }

        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var res = new CommunityDao().ChangeStatus(id);
            return Json(new
            {
                status = res
            });
        }
    }
}