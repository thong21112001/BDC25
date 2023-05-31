using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;

namespace BDC25.Areas.Admin.Controllers
{
    public class CategoryNewsController : BaseController
    {
        // GET: Admin/CategoryNews
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new CategoryNewsDao();
            var model = dao.ListALLCATE(searchString, page, pageSize);
            if (searchString != null)
            {
                //Giữ nguyên giá trị tìm kiếm trên seacrh
                ViewBag.SearchString = searchString;
                return View(model);
            }
            return View(model);
        }

        public ActionResult ListDel(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new CategoryNewsDao();
            var model = dao.ListALLCATEDel(searchString, page, pageSize);
            if (searchString != null)
            {
                //Giữ nguyên giá trị tìm kiếm trên seacrh
                ViewBag.SearchString = searchString;
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CATEGORY_NEWS cate)
        {
            if (ModelState.IsValid)
            {
                var dao = new CategoryNewsDao();
                DateTime createDay = DateTime.Now;
                cate.NgayTao = createDay;
                cate.UrlTitle = dao.addMinus(dao.convertToUnSign3(cate.Ten));

                long id = dao.Insert(cate);

                if (id > 0)
                {
                    SetAlert("Thêm thành công", "success");
                    return RedirectToAction("Index", "CategoryNews");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm thất bại");
                }
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            var dao = new CategoryNewsDao().ViewDetail(id);
            return View(dao);
        }

        [HttpPost]
        public ActionResult Edit(CATEGORY_NEWS cate)
        {
            if (ModelState.IsValid)
            {
                var dao = new CategoryNewsDao();

                var res = dao.Update(cate);

                if (res)
                {
                    SetAlert("Sửa thành công", "success");
                    return RedirectToAction("Index", "CategoryNews");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhập thất bại");
                }
            }
            return View();
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new CategoryNewsDao().Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ReDelete(int id)
        {
            new CategoryNewsDao().ReDelete(id);
            return RedirectToAction("Index");
        }
    }
}