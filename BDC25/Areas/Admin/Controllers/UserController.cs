using BDC25.Common;
using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDC25.Areas.Admin.Controllers
{
    //Controller Quản lý user
    public class UserController : BaseController
    {
        // GET: Admin/User
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new UserDao();
            var model = dao.ListALLCATE(searchString, page, pageSize);
            if (searchString != null)
            {
                //Giữ nguyên giá trị tìm kiếm trên seacrh
                ViewBag.SearchString = searchString;
                return View(model);
            }
            return View(model);
        }

        public ActionResult Details(int id)
        {
            var dao = new UserDao();
            var model = dao.ViewDetail(id);
            ViewBag.IDUserAD = id;
            return View(model);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new UserDao().Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ChangeStatus(int id) 
        { 
            var res = new UserDao().ChangeStatus(id);
            return Json(new {
                status = res
            });
        }
    }
}