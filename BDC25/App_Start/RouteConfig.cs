using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BDC25
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "Bot",
            url: "api/messages",
            defaults: new { controller = "Bot", action = "Messages", id = UrlParameter.Optional },
            namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho đăng nhập ở Login Controller
            routes.MapRoute(
                name: "Home",
                url: "trang-chu",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho đăng nhập ở Login Controller
            routes.MapRoute(
                name: "Login",
                url: "dang-nhap",
                defaults: new { controller = "Login", action = "LoginUser", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho đăng ký ở Login Controller
            routes.MapRoute(
                name: "Register",
                url: "dang-ky",
                defaults: new { controller = "Login", action = "Register", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho quên mật khẩu ở Login Controller
            routes.MapRoute(
                name: "ForgotPass",
                url: "quen-mat-khau",
                defaults: new { controller = "Login", action = "ForgotPassword", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho NewsController ở User NewsDetails
            routes.MapRoute(
                name: "News All",
                url: "danh-muc/{UrlTitle}-{id}",
                defaults: new { controller = "News", action = "NewsDetails", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho danh mục game ở game Controller
            routes.MapRoute(
                name: "List Game",
                url: "danh-muc-game",
                defaults: new { controller = "Game", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho game mot ở game Controller
            routes.MapRoute(
                name: "Game One",
                url: "danh-muc-game/the-cheems",
                defaults: new { controller = "Game", action = "GameMot", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho game hai ở game Controller
            routes.MapRoute(
                name: "Game Two",
                url: "danh-muc-game/shipper",
                defaults: new { controller = "Game", action = "GameHai", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho game ba ở game Controller
            routes.MapRoute(
                name: "Game Three",
                url: "danh-muc-game/the-cheems",
                defaults: new { controller = "Game", action = "GameBa", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho cẩm nang video ở video Controller
            routes.MapRoute(
                name: "Video Gallary",
                url: "huong-dan",
                defaults: new { controller = "Video", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho tìm kiếm
            routes.MapRoute(
                name: "Search",
                url: "tim-kiem",
                defaults: new { controller = "News", action = "SearchKey", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho all thẻ cộng đồng
            routes.MapRoute(
                name: "Tag All",
                url: "the-cong-dong",
                defaults: new { controller = "Community", action = "AllTag", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho post all thẻ cộng đồng
            routes.MapRoute(
                name: "Post of Tag",
                url: "danh-sach-bai-cong-dong/{UrlTitle}-{idTag}",
                defaults: new { controller = "Community", action = "PostOfTag", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho ủng hộ
            routes.MapRoute(
                name: "Donate",
                url: "ung-ho",
                defaults: new { controller = "Donate", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho NewsController ở User PostAdmin
            routes.MapRoute(
                name: "News Details",
                url: "chi-tiet-bai-viet/{UrlTitle}-{id}",
                defaults: new { controller = "News", action = "PostAdmin", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho InfoUserController ở User - CreatePost
            routes.MapRoute(
                name: "Create Post",
                url: "tao-bai-viet/{TaiKhoan}",
                defaults: new { controller = "InfoUser", action = "CreatePost", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho InfoUserController ở User - EditPost
            routes.MapRoute(
                name: "Edit Post",
                url: "chinh-sua-bai-viet/{UrlTitle}-{IdPost}",
                defaults: new { controller = "InfoUser", action = "EditPost", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho InfoUserController ở User - Index
            routes.MapRoute(
                name: "Information User",
                url: "thong-tin-ca-nhan/{TaiKhoan}",
                defaults: new { controller = "InfoUser", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho InfoUserController ở User - EditProfile
            routes.MapRoute(
                name: "Edit User",
                url: "thong-tin-ca-nhan/chinh-sua/{IDUser}",
                defaults: new { controller = "InfoUser", action = "EditProfile", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho CommunityController ở User Index
            routes.MapRoute(
                name: "Community",
                url: "cong-dong",
                defaults: new { controller = "Community", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            //Cái này dành cho NewsController ở User NewsDetails
            routes.MapRoute(
                name: "Details Post User",
                url: "chi-tiet-bai-viet-cong-dong/{UrlTitle}-{IdPostUser}",
                defaults: new { controller = "Community", action = "PostUser", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BDC25.Controllers" }
            );
        }
    }
}
