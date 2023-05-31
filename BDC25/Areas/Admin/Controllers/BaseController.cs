using BDC25.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BDC25.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        //Neu chay admin thi se tu dong ra trang login de dang nhap chu khong vao thang trang
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Kiem tra session nay la null thi tra ve trang dang nhap cua admin
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Login", Action = "Index", Area = "Admin" }));
            }
            base.OnActionExecuting(filterContext);
        }

        protected void SetAlert(string message,string type)
        {
            TempData["AlertMessage"] = message;
            if (type=="success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }
    }
}