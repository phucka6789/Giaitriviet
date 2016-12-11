
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
           
            return View();
        }
        public ActionResult PageVideo()
        {
            return View();
        }
        public ActionResult DetailPageVideo()
        {
            return View();
        }
        public ActionResult PateNew()
        {
            return View();
        }
        public ActionResult DetailNew()
        {
            return View();
        }
        
    }
}