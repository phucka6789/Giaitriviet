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
    public class CatVideosController : Controller
    {
        private Entitys db = new Entitys();

        // GET: Backends/CatVideos
       
        public ActionResult Index(bool status = true)
        {
            List<CatVideo> list = (from t in db.CatVideos where t.DeleteCateVideo == false orderby t.CatDepth select t).ToList();
            if (status == false)
                list = (from t in list where t.CatDepth == 0 select t).ToList();
            return View(list);
        }
        public ActionResult IndexFilter(int status = 0)
        {
            List<CatVideo> list = (from t in db.CatVideos where t.DeleteCateVideo == false orderby t.CatDepth select t).ToList();
            if (status == 0)
                list = (from t in list where t.CatDepth == 0 select t).ToList();
            return View(list);
        }

        // GET: Backends/CatVideos/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatVideo catVideo = db.CatVideos.Find(id);
            if (catVideo == null)
            {
                return HttpNotFound();
            }
            return View(catVideo);
        }

        // GET: Backends/CatVideos/Create
        public ActionResult Create()
        {
            List<CatVideo> list = (from t in db.CatVideos where t.DeleteCateVideo == false && t.Status == true orderby t.CatDepth select t).ToList();
            foreach (CatVideo item in list)
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

        // POST: Backends/CatVideos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,MetaTitle,ParentID,DisplayOrder,SeoTitle,CatDepth,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescriptions,Status,DeleteCateVideo")] CatVideo catVideo)
        {
            if (ModelState.IsValid)
            {
                ArticleCatManager aMNG = new ArticleCatManager();
                int catid = 0;
                if (catVideo.ParentID != null)
                    catid = int.Parse(catVideo.ParentID.ToString());
                int maxOrder = int.Parse(aMNG.GetMaxOrderVideo(catid).ToString());
                catVideo.DisplayOrder = maxOrder + 1;
                if (catVideo.ParentID == null)
                {
                    catVideo.CatDepth = 0;
                    catVideo.ParentID = 0;
                }
                else
                {
                    catVideo.CatDepth = 1;
                }
                catVideo.DeleteCateVideo = false;
                db.CatVideos.Add(catVideo);
                db.SaveChanges();
                SetAlert("Thêm chủ đề thành công", "success");
                return RedirectToAction("Index");
            }
            return View(catVideo);
        }

        // GET: Backends/CatVideos/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                SetAlert("Vui Lòng kiểm tra lại", "danger");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            List<ArticleCatViewModel> list = (from t in db.CatVideos orderby t.CatDepth select new ArticleCatViewModel { ID = t.ID, Name = t.Name, CatDepth = t.CatDepth }).ToList();
            foreach (ArticleCatViewModel item in list)
            {
                if (item.CatDepth == 1)
                    item.Name = "---" + item.Name;
                else if (item.CatDepth == 2)
                    item.Name = "------" + item.Name;
            }


            CatVideo articlecat = db.CatVideos.Find(id);
            if (articlecat == null)
            {
                SetAlert("Không tìm thấy danh mục này", "danger");
                return RedirectToAction("Index");
            }
            ViewBag.ParentID = new SelectList(list, "ID", "Name", articlecat.ParentID);
            return View(articlecat);
        }

        // POST: Backends/CatVideos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,MetaTitle,ParentID,DisplayOrder,SeoTitle,CatDepth,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescriptions,Status,DeleteCateVideo")] CatVideo catVideo)
        {
            if (ModelState.IsValid)
            {
                if (catVideo.ParentID == null)
                {
                    catVideo.ParentID = 0;
                    catVideo.CatDepth = 0;
                }
                else
                {
                    catVideo.CatDepth = 1;
                }
                catVideo.DeleteCateVideo = false;
                catVideo.ModifiedDate = DateTime.Now;
                db.Entry(catVideo).State = EntityState.Modified;
                db.SaveChanges();
                SetAlert("Update chủ đề thành công", "success");
                return RedirectToAction("Index");
            }
            return View(catVideo);
        }

        // GET: Backends/CatVideos/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatVideo catVideo = db.CatVideos.Find(id);
            if (catVideo == null)
            {
                return HttpNotFound();
            }
            return View(catVideo);
        }

        // POST: Backends/CatVideos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            CatVideo catVideo = db.CatVideos.Find(id);
            db.CatVideos.Remove(catVideo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Xoabaiviet(long id)
        {
            var model = db.CatVideos.Find(id);
            db.CatVideos.Remove(model);
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
    }
}
