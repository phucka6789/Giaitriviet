using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Models.Database;
using System.Configuration;
using System.IO;

namespace Project.Areas.Backends.Controllers
{
    public class VideosController : Controller
    {
        private Entitys db = new Entitys();

        // GET: Backends/Videos
        public ActionResult Index()
        {
            List<CatVideo> list = (from t in db.CatVideos where t.DeleteCateVideo == false orderby t.CatDepth select t).ToList();
            foreach (CatVideo item in list)
            {
                if (item.CatDepth == 1)
                    item.Name = "---" + item.Name;
                else if (item.CatDepth == 2)
                    item.Name = "------" + item.Name;
            }
            ViewBag.ParentID = new SelectList(list, "ID", "Name");
            List<Video> temp = (from t in db.Videos where t.DeleteVideo == false && t.TopHot == false orderby t.CreatedDate descending select t).ToList();
            return View(temp);
        }

        // GET: Backends/Videos/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // GET: Backends/Videos/Create
        public ActionResult Create()
        {
            ViewBag.Date = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
            List<CatVideo> list = (from t in db.CatVideos where t.DeleteCateVideo == false orderby t.CatDepth select t).ToList();
            foreach (CatVideo item in list)
            {
                if (item.CatDepth == 1)
                    item.Name = "---" + item.Name;
                else if (item.CatDepth == 2)
                    item.Name = "------" + item.Name;
            }
            ViewBag.CategoryID = new SelectList(list, "ID", "Name");
            return View();
        }

        // POST: Backends/Videos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Name,MetaTitle,Description,Image,CategoryID,Keyoutube,Detail,Warranty,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescriptions,ViewCount,TopHot,Status,Tags,DeleteVideo")] Video video)
        {
            if (ModelState.IsValid)
            {
                string img = UploadImage();
                if (!String.IsNullOrEmpty(img))
                {
                    video.Image = img;
                }
                video.DeleteVideo = false;
                db.Videos.Add(video);
                db.SaveChanges();
                SetAlert("Thêm Bài Viết Thành Công", "success");
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.CatVideos, "ID", "Name", video.CategoryID);
            return View(video);
        }

        // GET: Backends/Videos/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {

                SetAlert("Xảy ra lỗi.", "danger");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video article = db.Videos.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImageSavePath = ConfigurationManager.AppSettings["ImageSavePath"];

            List<CatVideo> list = (from t in db.CatVideos where t.DeleteCateVideo == false orderby t.CatDepth select t).ToList();
            foreach (CatVideo item in list)
            {
                if (item.CatDepth == 1)
                    item.Name = "---" + item.Name;
                else if (item.CatDepth == 2)
                    item.Name = "------" + item.Name;
            }

            ViewBag.CategoryID = new SelectList(list, "ID", "Name", article.CategoryID);
            return View(article);
        }

        // POST: Backends/Videos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Name,MetaTitle,Description,Image,CategoryID,Keyoutube,Detail,Warranty,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescriptions,ViewCount,TopHot,Status,Tags,DeleteVideo")] Video video)
        {
            if (ModelState.IsValid)
            {
                string img = UploadImage();
                if (!String.IsNullOrEmpty(img))
                {
                    deleteImg(video.Image);
                    video.Image = img;
                }
                video.DeleteVideo = false;
                video.ModifiedDate = DateTime.Now;
                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();
                SetAlert("Update Bài Viết Thành Công", "success");
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.CatVideos, "ID", "Name", video.Id);
            return View(video);
        }

        // GET: Backends/Videos/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // POST: Backends/Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Video video = db.Videos.Find(id);
            db.Videos.Remove(video);
            db.SaveChanges();
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
        public ActionResult Xoabaiviet(long id)
        {
            var model = db.Videos.Find(id);
            deleteImg(model.Image);
            db.Videos.Remove(model);
            db.SaveChanges();
            SetAlert("Xóa Bài Viết Thành Công", "success");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult DeleteMulti(List<long> listDelete)
        {
            try
            {

                List<Video> list = (from t in db.Videos where listDelete.Contains(t.Id) select t).ToList();
                try
                {
                    foreach (Video item in list)
                    {
                        deleteImg(item.Image);
                        db.Videos.Remove(item);
                        db.SaveChanges();
                        // aMNG.Delete(item.ArticleId);

                    }

                    SetAlert("Xóa Bài Viết Thành Công", "success");
                }
                catch
                {

                    SetAlert("Xóa Bài Viết Xảy ra lỗi", "danger");
                }

            }
            catch
            {

                SetAlert("Vui lòng chọn bài viết để xóa", "danger");

            }

            return RedirectToAction("Index");

        }
        public void deleteImg(String ArticleImg)
        {
            FileInfo file2 = new FileInfo(Server.MapPath(ConfigurationManager.AppSettings["ImageSavePath"] + "/Youtube/" + ArticleImg));
            if (file2.Exists)
            {
                System.IO.File.Delete(Server.MapPath(ConfigurationManager.AppSettings["ImageSavePath"] + "/Youtube/" + ArticleImg));
            }
        }
        public string UploadImage()
        {
            string dbSaveUrl = "";
            if (this.Request.Files.Count > 0)
            {
                HttpPostedFileBase files = this.Request.Files[0];
                if (files.FileName == "") return "";
                string file_name = files.FileName;
                string saveDB = ConfigurationManager.AppSettings["ImageSavePath"] + "/Youtube/";
                string savePath = Server.MapPath(saveDB);

                if (System.IO.Directory.Exists(savePath) == false)
                {
                    System.IO.Directory.CreateDirectory(savePath);
                }

                file_name = Path.GetFileNameWithoutExtension(files.FileName) + "_" + DateTime.Now.Ticks.ToString() + Path.GetExtension(files.FileName);
                files.SaveAs(savePath + file_name);
                dbSaveUrl = file_name;
            }
            return dbSaveUrl.ToString();
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
