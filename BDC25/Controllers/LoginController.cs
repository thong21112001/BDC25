using BDC25.Models;
using BDC25.Common;
using Model.Dao;
using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Model.EF;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

namespace BDC25.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        BDC25DbContext db = new BDC25DbContext();
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(USERWEB us)
        {
            var dao = new UserDao();
            if (us.TaiKhoan != null || us.MatKhau != null || us.Email != null || us.HoTen != null || us.DienThoai != null)
            {
                bool checkEmail = dao.IsEmail(us.Email);
                if (checkEmail == false) { 
                    ModelState.AddModelError("", "Email sai định dạng");
                }
                else
                {
                    bool checkPhone = dao.Isphone(us.DienThoai);
                    if (checkPhone == false)
                    {
                        ModelState.AddModelError("", "Số điện thoại sai định dạng");
                    }
                    else
                    {
                        bool checkPass = dao.IsPass(us.MatKhau);
                        if (checkPass == false)
                        {
                            ModelState.AddModelError("", "Mật khẩu cần chứa 1 ký tự hoa,1 số và trên 8 ký tự");
                        }
                        else
                        {
                            bool checkEmailDef = dao.CheckEmail(us.Email);
                            if (checkEmailDef == false)
                            {
                                ModelState.AddModelError("", "Email đã được sử dụng");
                            }
                            else
                            {
                                bool checkTKlDef = dao.CheckTK(us.TaiKhoan);
                                if (checkTKlDef == false)
                                {
                                    ModelState.AddModelError("", "Tài khoản đã được sử dụng");
                                }
                                else
                                {
                                    DateTime createDay = DateTime.Now;
                                    us.NgayTao = createDay;
                                    us.MatKhau = Encryptor.MD5Hash(us.MatKhau);
                                    us.AnHien = true;
                                    us.Image = null;
                                    long id = dao.Register(us);

                                    if (id > 0)
                                    {
                                        string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/User/template/WelcomeWeb.html"));

                                        content = content.Replace("{{UserName}}", us.TaiKhoan);
                                        content = content.Replace("{{UserEmail}}", us.Email);
                                        content = content.Replace("{{Phone}}", us.DienThoai);

                                        string sub = "Đăng ký tài khoản thành công tại BDC25. Hãy đăng nhập nhé !!!";
                                        new MailHelper().SendWelcome(us.Email, sub, content);

                                        System.Threading.Thread.Sleep(2000);//Dừng lại 2s
                                        return RedirectToAction("LoginUser", "Login");
                                    }
                                    else
                                    {
                                        ModelState.AddModelError("", "Tạo tài khoản thất bại!");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Không bỏ trống các trường !");
            }
            return View();
        }

        public ActionResult LoginUser(LoginModel model)
        {
            var dao = new UserDao();

            if (ModelState.IsValid) //Kiểm tra form rỗng
            {
                var res = dao.Login(model.UserName, Encryptor.MD5Hash(model.PassWord));

                if (res == 1)
                {
                    var user = dao.GetByID(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.TaiKhoan;
                    userSession.UserID = user.IDUser;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    //Lưu session để lấy tên
                    Session["NameUser"] = user.TaiKhoan;
                    return RedirectToAction("Index", "Home");
                }
                else if (res == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                }
                else if (res == -1)
                {
                    bool status = dao.GetStatus(model.UserName); //Xem trạng thái tài khoản
                    int totalDayLock = dao.GetTotalDayLock(model.UserName); //Lấy về số ngày khóa
                    int solanKhoa = dao.CountLock(model.UserName); //Lấy về số lần khóa tài khoản

                    if (solanKhoa <= 3)
                    {
                        if (status == false && totalDayLock < 3)
                        {
                            DateTime dayUnLock = dao.GetDayUnLock(model.UserName);
                            string kq = dayUnLock.ToString("dd/MM/yyyy");
                            ModelState.AddModelError("", "Tài khoản bị khóa đến " + kq);
                        }
                        else
                        {
                            bool GetUnLockAcc = dao.UnLock(model.UserName);
                            if (GetUnLockAcc == true)
                            {
                                var user = dao.GetByID(model.UserName);
                                var userSession = new UserLogin();
                                userSession.UserName = user.TaiKhoan;
                                userSession.UserID = user.IDUser;
                                Session.Add(CommonConstants.USER_SESSION, userSession);
                                //Lưu session để lấy tên
                                Session["NameUser"] = user.TaiKhoan;
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tài khoản bị khóa vĩnh viễn không thể đăng nhập");
                    }
                }
                else if (res == -2)
                {
                    ModelState.AddModelError("", "Sai mật khẩu");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không thành công");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            //Xóa session
            Session[CommonConstants.USER_SESSION] = null;
            Session.Abandon();
            //Xóa session form authent
            FormsAuthentication.SignOut();
            return Redirect("/Home/Index");
        }



        //
        // POST: /Account/ForgotPassword
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        public ActionResult ForgotPassword(BDC25.Models.SendGmail sendM,USERWEB model)
        {
            var dao = new UserDao();
            if (sendM.UserName != null || sendM.Email != null)
            {
                bool res = dao.CheckTK(sendM.UserName);
                var checkEmail = db.USERWEBs.FirstOrDefault(x => x.TaiKhoan == sendM.UserName);
                if (res == true)
                {
                    ModelState.AddModelError("", "Tên đăng nhập không tồn tại");
                }
                else
                {
                    if (checkEmail.Email.ToString() == sendM.Email.ToString())
                    {
                        //Tạo random pass
                        string newPass = dao.RandomPassword(8);
                        //upadte pass truyền mã hóa md5 vào password
                        bool upPass = dao.UpdatePass(sendM.UserName, Encryptor.MD5Hash(newPass));
                        string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/User/template/ResetPass.html"));

                        content = content.Replace("{{UserName}}", sendM.UserName);
                        content = content.Replace("{{UserEmail}}", sendM.Email);
                        content = content.Replace("{{PassNew}}", newPass);

                        string sub = "Email này được dùng để lấy lại mật khẩu tại BDC25";
                        new MailHelper().SendEmail(sendM.Email,sub,content);
                        ViewBag.MessM = "Đã gửi email lấy pass thành công";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email không phải của tài khoản");
                    }
                }
            }
            return View();
        }
    }
}