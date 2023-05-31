using BDC25.Areas.Admin.Data;
using BDC25.Common;
using Model.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BDC25.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid) //Kiểm tra form rỗng
            {
                var dao = new AdminDao();
                var res = dao.Login(model.UserName, Encryptor.MD5Hash(model.PassWord)); //Bên folder Data của admin file LoginModel

                if (res == 1)
                {
                    var user = dao.GetByID(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.TaiKhoan;
                    userSession.UserID = user.IDAdmin;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    //Lưu session để lấy tên
                    Session["NameAdmin"] = user.TaiKhoan;
                    return RedirectToAction("Index", "CategoryNews");
                }
                else if (res == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                }
                else if (res == -1)
                {
                    ModelState.AddModelError("", "Sai mật khẩu");
                }
                else
                {
                   ModelState.AddModelError("", "Đăng nhập không thành công");
                }
            }
            return View("Index");
        }

        public ActionResult Logout()
        {
            //Xóa session
            Session[CommonConstants.USER_SESSION] = null;
            Session.Abandon();
            //Xóa session form authent
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}