using Project.Models.BUS;
using Project.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Areas.Backends.Controllers
{
    public class GrantPermissionsController : Controller
    {
        //
        // GET: /Backends/GrantPermissions/
        private Entitys db = new Entitys();
        public ActionResult Index(int id)
        {
            var listcontrol = db.Businesses.AsEnumerable();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in listcontrol)
            {
                items.Add(new SelectListItem() { Text = item.BusinessName, Value = item.BusinessId });

            }
            ViewBag.items = items;
            var listgranted = from g in db.GrantPermissions
                              join p in db.Permissions on g.PermissionId equals p.PermissionId
                              where g.UserId == id
                              select new SelectListItem()
                              {
                                  Value = p.PermissionId.ToString(),
                                  Text = p.Description
                              };
            ViewBag.listgranted = listgranted;
            Session["usergrant"] = id;
            var usergrant = db.Administrators.Find(id);
            ViewBag.usergrant = usergrant.Username + "(" + usergrant.FullName + ")";
            return View();
        }
        public JsonResult getPermissions(string id, int usertemp)
        {
            // lấy tất cả các permission của user và của bussiness
            var listgranted = (from g in db.GrantPermissions
                               join p in db.Permissions on g.PermissionId equals p.PermissionId
                               where g.UserId == usertemp && p.BusinessId == id
                               select new PermissionAction { PermissionId = p.PermissionId, PermissionName = p.PermissionName, Description = p.Description, IsGranted = true }
                               ).ToList();
            // lấy ra tất cả các permission của business hiện tại
            var listpermission = from p in db.Permissions
                                 where p.BusinessId == id
                                 select new PermissionAction { PermissionId = p.PermissionId, PermissionName = p.PermissionName, Description = p.Description, IsGranted = false };

            // lấy các id của permission đã được gán cho người dùng
            var listpermissionId = listgranted.Select(p => p.PermissionId);


            foreach (var item in listpermission)
            {
                if (!listpermissionId.Contains(item.PermissionId))
                    listgranted.Add(item);
            }
            return Json(listgranted.OrderBy(x => x.Description), JsonRequestBehavior.AllowGet);
        }

        public string updatePermission(int id, int usertemp)
        {
            string msg = "";
            var grant = db.GrantPermissions.Find(id, usertemp);
            if (grant == null)
            {
                GrantPermission g = new GrantPermission() { PermissionId = id, UserId = usertemp, Description = "" };
                db.GrantPermissions.Add(g);
                msg = "<div class='alert alert-success'>Quyền cấp thành công</div>";
            }
            else
            {
                db.GrantPermissions.Remove(grant);
                msg = "<div class='alert alert-danger'>Quyền hủy thành công</div>";
            }
            db.SaveChanges();
            return msg;
        }
	}
}