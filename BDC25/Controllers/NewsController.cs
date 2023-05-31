using Model.Dao;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace BDC25.Controllers
{
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewsDetails(int id)
        {
            int totalRecord = 0;
            var model = new NewsDao().ListByGroupID(id,ref totalRecord);
            ViewBag.IDCate = id;
            return View(model);
        }


        public ActionResult PostAdmin(int id)
        {
            var dao = new NewsDao();
            var model = dao.ViewNews(id);
            ViewBag.ListLienQuan = dao.ListNewsLienQuan(id);
            ViewBag.IDNewsUserUI = id;
            return View(model);
        }

        public ActionResult SearchKey(string keyword)
        {
            int totalRecord = 0;
            var model = new NewsDao().Searchkey(keyword, ref totalRecord);
            var modelCom = new CommunityDao().SearchkeyCom(keyword, ref totalRecord);
            ViewBag.KeyWord = keyword;
            ViewBag.ListPostUserCom = modelCom;
            return View(model);
        }

        public JsonResult Search(string txtS)
        {
            var data = new NewsDao().ListName(txtS);
            return Json(new
            {
                res = data,
                status = true
            },JsonRequestBehavior.AllowGet);
        }
    }
}