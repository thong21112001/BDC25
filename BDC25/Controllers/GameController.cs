using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BDC25.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GameMot()
        {
            return View();
        }

        public ActionResult GameHai()
        {
            return View();
        }

        public ActionResult GameBa()
        {
            return View();
        }
    }
}