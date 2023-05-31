using Model.Dao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDC25.Controllers
{
    public class DonateController : Controller
    {
        // GET: Donate
        public ActionResult Index()
        {
            var dao = new DonateDao();
            var model = dao.ListDonate();
            ViewBag.ListFile = model;
            return View();
        }

        public FileResult Download(string fileName)
        {
            string fileN = Path.GetFileName(fileName);
            string fullPath = Path.Combine(Server.MapPath("~/UploadFile/"), fileN);
            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fullPath, "application/force-download", Path.GetFileName(fullPath));
        }
    }
}