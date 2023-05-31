using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDC25.Areas.Admin.Controllers
{
    public class TagController : BaseController
    {
        // GET: Admin/Tag
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new TagDao();
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
            var dao = new TagDao();
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
        public ActionResult Create(TAGS_COMMUNITY tag)
        {
            if (ModelState.IsValid)
            {
                var dao = new TagDao();
                DateTime createDay = DateTime.Now;
                tag.NgayTao = createDay;
                tag.UrlTitle = dao.addMinus(dao.convertToUnSign3(tag.Ten));

                long id = dao.Insert(tag);

                if (id > 0)
                {
                    SetAlert("Thêm thành công", "success");
                    return RedirectToAction("Index", "Tag");
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
            var dao = new TagDao().ViewDetail(id);
            return View(dao);
        }

        [HttpPost]
        public ActionResult Edit(TAGS_COMMUNITY tag)
        {
            if (ModelState.IsValid)
            {
                var dao = new TagDao();

                var res = dao.Update(tag);

                if (res)
                {
                    SetAlert("Sửa thành công", "success");
                    return RedirectToAction("Index", "Tag");
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
            new TagDao().Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ReDelete(int id)
        {
            new TagDao().ReDelete(id);
            return RedirectToAction("Index");
        }
    }
}