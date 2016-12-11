using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Models.Database;
using Project.Models.BUS;
using Project.Areas.Backends.Models;

namespace Project.Areas.Backends.Controllers
{
    public class CatNewController : Controller
    {
        private Entitys db = new Entitys();

        // GET: /Backends/CatNew/
        public ActionResult Index(bool status = true)
        {
            List<CatNew> list = (from t in db.CatNews where t.DeleteCateVideo == false orderby t.CatDepth select t).ToList();
            if (status == false)
                list = (from t in list where t.CatDepth == 0 select t).ToList();
            return View(list);
        }
        public ActionResult IndexFilter(int status = 0)
        {
            List<CatNew> list = (from t in db.CatNews where t.DeleteCateVideo == false orderby t.CatDepth select t).ToList();
            if (status == 0)
                list = (from t in list where t.CatDepth == 0 select t).ToList();
            return View(list);
        }

        // GET: /Backends/CatNew/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatNew catnew = db.CatNews.Find(id);
            if (catnew == null)
            {
                return HttpNotFound();
            }
            return View(catnew);
        }

        // GET: /Backends/CatNew/Create
        public ActionResult Create()
        {
            List<CatNew> list = (from t in db.CatNews where t.DeleteCateVideo == false && t.Status == true orderby t.CatDepth select t).ToList();
            foreach (CatNew item in list)
            {
                if (item.CatDepth == 1)
                    item.Name = "---" + item.Name;
                else if (item.CatDepth == 2)
                    item.Name = "------" + item.Name;
                else if (item.CatDepth == 3)
                    item.Name = "----------" + item.Name;
            }
            ViewBag.ParentID = new SelectList(list, "ID", "Name");         
            return View();
        }

        // POST: /Backends/CatNew/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,MetaTitle,ParentID,DisplayOrder,SeoTitle,CatDepth,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescriptions,Status,DeleteCateVideo")] CatNew catnew)
        {
            if (ModelState.IsValid)
            {
                ArticleCatManager aMNG = new ArticleCatManager();
                int catid = 0;
                if (catnew.ParentID != null)
                    catid = int.Parse(catnew.ParentID.ToString());
                int maxOrder = int.Parse(aMNG.GetMaxOrder(catid).ToString());
                catnew.DisplayOrder = maxOrder + 1;
                if (catnew.ParentID == null)
                {
                    catnew.CatDepth = 0;
                    catnew.ParentID = 0;
                }
                else
                {
                    catnew.CatDepth = 1;
                }
                catnew.DeleteCateVideo = false;
                db.CatNews.Add(catnew);
                db.SaveChanges();
                SetAlert("Thêm chủ đề thành công", "success");
                return RedirectToAction("Index");
            }

           
            return View(catnew);
        }

        // GET: /Backends/CatNew/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                SetAlert("Vui Lòng kiểm tra lại", "danger");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            List<ArticleCatViewModel> list = (from t in db.CatNews orderby t.CatDepth select new ArticleCatViewModel { ID = t.ID, Name = t.Name, CatDepth = t.CatDepth }).ToList();
            foreach (ArticleCatViewModel item in list)
            {
                if (item.CatDepth == 1)
                    item.Name = "---" + item.Name;
                else if (item.CatDepth == 2)
                    item.Name = "------" + item.Name;
            }


            CatNew articlecat = db.CatNews.Find(id);
            if (articlecat == null)
            {
                SetAlert("Không tìm thấy danh mục này", "danger");
                return RedirectToAction("Index");
            }
            ViewBag.ParentID = new SelectList(list, "ID", "Name", articlecat.ParentID);
            return View(articlecat);
        }

        // POST: /Backends/CatNew/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,MetaTitle,ParentID,DisplayOrder,SeoTitle,CatDepth,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescriptions,Status,DeleteCateVideo")] CatNew catnew)
        {
            if (ModelState.IsValid)
            {
                if (catnew.ParentID == null)
                {
                    catnew.ParentID = 0;
                    catnew.CatDepth = 0;
                }
                else
                {
                    catnew.CatDepth = 1;
                }
                catnew.DeleteCateVideo = false;
                catnew.ModifiedDate = DateTime.Now;
                db.Entry(catnew).State = EntityState.Modified;
                db.SaveChanges();
                SetAlert("Update chủ đề thành công", "success");
                return RedirectToAction("Index");
            }           
            return View(catnew);
        }

        // GET: /Backends/CatNew/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatNew catnew = db.CatNews.Find(id);
            if (catnew == null)
            {
                return HttpNotFound();
            }
            return View(catnew);
        }

        // POST: /Backends/CatNew/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            CatNew catnew = db.CatNews.Find(id);
            db.CatNews.Remove(catnew);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Xoabaiviet(long id)
        {
            var model = db.CatNews.Find(id);           
            db.CatNews.Remove(model);
            db.SaveChanges();
            SetAlert("Xóa Bài Viết Thành Công", "success");
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public bool Update(CatNew item)
        {
            try
            {
                CatNew old = (from c in db.CatNews where c.ID == item.ID select c).FirstOrDefault();
                if (old == null)
                    return false;
                old.ParentID = item.ParentID;
                old.Name = item.Name;
                old.DisplayOrder = item.DisplayOrder;
                old.MetaTitle = item.MetaTitle; ;
                old.MetaDescriptions = item.MetaDescriptions;
                old.MetaKeywords = item.MetaKeywords;
                old.Status = item.Status;
                old.DeleteCateVideo = item.DeleteCateVideo;
                db.SaveChanges();
                return true;

            }
            catch (Exception ex) { }
            return false;
        }


        protected void SetAlert(string message, string type)
        {
            TempData["AlertMessage"] = message;
            if (type == "success")
            {
                TempData["AlerType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlerType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlerType"] = "alert-danger";
            }
        }
    }
}
