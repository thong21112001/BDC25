using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;

namespace BDC25.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var newsDao = new NewsDao();
            var comDao = new CommunityDao();
            var catDao = new CategoryNewsDao();
            ViewBag.ListPostUser = comDao.ListPostUser();
            ViewBag.ListNewsChiaSe = newsDao.ListNewsChiaSe();
            ViewBag.ListNewsKienThuc = newsDao.ListNewsKienThuc();
            ViewBag.ListNewsCuuHo = newsDao.ListNewsCuuHo();
            ViewBag.GetUrl = catDao.GetUrlPostNew();
            return View();
        }

        [ChildActionOnly]
        public ActionResult MenuNav()
        {
            var model = new CategoryNewsDao().ListByGroupID();
            return PartialView(model);
        }
    }
}