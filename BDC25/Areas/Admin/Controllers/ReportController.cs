using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Model.Dao;
using Model.EF;
using PagedList;

namespace BDC25.Areas.Admin.Controllers
{
    public class ReportController : BaseController
    {
        // GET: Admin/Report
        BDC25DbContext db = new BDC25DbContext();
        public ActionResult Index(int? idLoaiDM, string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new CommentDao();
            var model = dao.ListCmtAd(searchString, page, pageSize);
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

        public ActionResult DelCmt(int? idLoaiDM, string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new CommentDao();
            var model = dao.ListCmtAdRip(searchString, page, pageSize);
            var model2 = dao.ListALLDropRip(idLoaiDM, page, pageSize);

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
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var cmt = db.COMMENTs.Find(id);

            if (cmt == null)
            {
                return HttpNotFound();
            }

            cmt.AnHien = false;
            cmt.IsReport = false;
            db.SaveChanges();
            return View();
        }
    }
}