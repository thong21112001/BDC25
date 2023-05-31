using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using Model.EF;
using System.IO;
using System.Net;
using System.Data.Entity;


namespace BDC25.Areas.Admin.Controllers
{
    public class DonateController : BaseController
    {
        private BDC25DbContext db = new BDC25DbContext();
        // GET: Admin/Donate
        public ActionResult Index(string searchString)
        {
            var dao = new DonateDao();
            var model = dao.ListALLCATE(searchString);
            if (searchString != null)
            {
                //Giữ nguyên giá trị tìm kiếm trên seacrh
                ViewBag.SearchString = searchString;
                ViewBag.ListFile = model;
                return View(model);
            }
            ViewBag.ListFile = model;
            return View(model);
        }

        public FileResult Download(string fileName)
        {
            string fileN = Path.GetFileName(fileName);
            string fullPath = Path.Combine(Server.MapPath("~/UploadFile/"), fileN);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fullPath, "application/force-download", Path.GetFileName(fullPath));
        }

        private string GetFileTypeByExtension(string extension)
        {
            switch (extension.ToLower())
            {
                case ".docx":
                case ".doc":
                    return "Microsoft Word Document";
                case ".xlsx":
                case ".xls":
                    return "Microsoft Excel Document";
                case ".txt":
                    return "Text Document";
                case ".jpg":
                case ".png":
                    return "Image";
                default:
                    return "Unknown";
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DONATE doc,HttpPostedFileBase files)
        {
            var dao = new DonateDao();
            if (files.ContentLength > 0)
            {
                string fileName = Path.GetFileName(files.FileName);   //Lấy tên file
                string DayFileName = DateTime.Now.ToString("ddMMyyyyHHmm") + "_" + fileName;  //Gán them datime vào tên file
                string path = Path.Combine(Server.MapPath("~/UploadFile/"), DayFileName);    //Lấy đường dẫn
                FileInfo fi = new FileInfo(fileName);

                doc.UrlTitle = dao.addMinus(dao.convertToUnSign3(doc.TieuDe));
                doc.File = "~/UploadFile/" + DayFileName;
                doc.Type = GetFileTypeByExtension(fi.Extension);

                long id = dao.Insert(doc);

                if (id > 0)
                {
                    files.SaveAs(path);
                    SetAlert("Thêm thành công", "success");
                    return RedirectToAction("Index", "Donate");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm thất bai");
                }

            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var dao = new DonateDao().ViewDetail(id);
            Session["FilePath"] = dao.File;
            return View(dao);
        }

        [HttpPost]
        public ActionResult Edit(DONATE doc, HttpPostedFileBase files)
        {
            var dao = new DonateDao();
            if (ModelState.IsValid)
            {
                if (files != null)
                {
                    string fileName = Path.GetFileName(files.FileName);   //Lấy tên file
                    string DayFileName = DateTime.Now.ToString("ddMMyyyyHHmm") + "_" + fileName;  //Gán them datime vào tên file
                    string path = Path.Combine(Server.MapPath("~/UploadFile/"), DayFileName);    //Lấy đường dẫn
                    FileInfo fi = new FileInfo(fileName);

                    doc.UrlTitle = dao.addMinus(dao.convertToUnSign3(doc.TieuDe));
                    doc.File = "~/UploadFile/" + DayFileName;
                    doc.Type = GetFileTypeByExtension(fi.Extension);

                    db.Entry(doc).State = EntityState.Modified;
                    var res = dao.Update(doc);
                    string oldFile = Request.MapPath(Session["FilePath"].ToString());
                    if (res)
                    {
                        files.SaveAs(path);
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                        SetAlert("Sửa thành công", "success");
                        return RedirectToAction("Index", "Donate");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Sửa thất bai");
                    }
                }
                else
                {
                    DateTime createDay = DateTime.Now;
                    doc.NgaySua = createDay;
                    doc.File = Session["FilePath"].ToString();
                    var res = dao.UpdateNotFile(doc);
                    if (res)
                    {
                        SetAlert("Sửa thành công", "success");
                        return RedirectToAction("Index", "Donate");
                    }
                }
            }
            return View();
        }

        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var donate = db.DONATEs.Find(id);

            if (donate == null)
            {
                return HttpNotFound();
            }
            string curImg = Request.MapPath(donate.File);
            db.Entry(donate).State = EntityState.Deleted;
            if (System.IO.File.Exists(curImg))
            {
                if (db.SaveChanges() > 0)
                {
                    System.IO.File.Delete(curImg);
                    SetAlert("Xóa thành công", "success");
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
    }
}