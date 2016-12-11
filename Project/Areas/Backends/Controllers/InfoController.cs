using Project.Models.BUS;
using Project.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Areas.Backends.Controllers
{
    public class InfoController : Controller
    {
        //
        // GET: /Backends/Info/
        Entitys db = new Entitys();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            string passwordMD5 = Common.EncryptMD5(username + password);
            var user = db.Administrators.SingleOrDefault(a =>a.Username == username && a.Password == passwordMD5 && a.Allowed == 1);
            if (user != null)
            {
                Session["userid"] = user.UserId;
                Session["username"] = user.Username;
                Session["fullname"] = user.FullName;
                Session["avatar"] = user.Avatar;
                return RedirectToAction("Index");
            }
            ViewBag.error = "dang nhap khong thanh cong";
            return View();
        }
        public ActionResult Logout()
        {
            Session["userid"] = null;
            Session["username"] = null;
            Session["fullname"] = null;
            Session["avatar"] = null;
            return RedirectToAction("Login");
        }
        public ActionResult NotificationAuthorize()
        {
            return View();
        }
        public EmptyResult Alive()
        {
            return new EmptyResult();
        }
	}
}