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
    public class ContenNewController : Controller
    {
        private Entitys db = new Entitys();

        // GET: /Backends/ContenNew/
        public ActionResult Index()
        {
            List<CatNew> list = (from t in db.CatNews where t.DeleteCateVideo == false orderby t.CatDepth select t).ToList();
            foreach (CatNew item in list)
            {
                if (item.CatDepth == 1)
                    item.Name = "---" + item.Name;
                else if (item.CatDepth == 2)
                    item.Name = "------" + item.Name;
            }
            ViewBag.ParentID = new SelectList(list, "ID", "Name");
            List<ContenNew> temp = (from t in db.ContenNews where t.DeleteVideo == false && t.TopHot == false orderby t.CreatedDate descending select t).ToList();
            return View(temp);
        }

        [HttpGet]
        public ActionResult Filter(String title, int catid)
        {
            List<ContenNew> temp = (from t in db.ContenNews where t.DeleteVideo == false orderby t.CreatedDate descending select t).ToList();
            if (!String.IsNullOrEmpty(title))
                temp = temp.Where(p => p.MetaTitle.ToLower().Contains(title.ToLower())).ToList();
            if (catid != 0)
                temp = temp.Where(p => p.CategoryID == catid).ToList();
            return View(temp);
        }

        // GET: /Backends/ContenNew/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContenNew contennew = db.ContenNews.Find(id);
            if (contennew == null)
            {
                return HttpNotFound();
            }
            return View(contennew);
        }

        // GET: /Backends/ContenNew/Create
        public ActionResult Create()
        {
            ViewBag.Date = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
            List<CatNew> list = (from t in db.CatNews where t.DeleteCateVideo == false orderby t.CatDepth select t).ToList();
            foreach (CatNew item in list)
            {
                if (item.CatDepth == 1)
                    item.Name = "---" + item.Name;
                else if (item.CatDepth == 2)
                    item.Name = "------" + item.Name;
            }
            ViewBag.CategoryID = new SelectList(list, "ID", "Name");
            return View();
           
        }

        // POST: /Backends/ContenNew/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include="Id,Name,MetaTitle,Description,Image,CategoryID,Detail,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescriptions,ViewCount,TopHot,Status,Tags,DeleteVideo")] ContenNew contennew)
        {
           
            if (ModelState.IsValid)
            {
                string img = UploadImage();
                if (!String.IsNullOrEmpty(img))
                {
                    contennew.Image = img;
                }
                contennew.DeleteVideo = false;
                db.ContenNews.Add(contennew);
                db.SaveChanges();
                SetAlert("Thêm Bài Viết Thành Công", "success");
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.CatNews, "ID", "Name", contennew.CategoryID);
            return View(contennew);

        }

        // GET: /Backends/ContenNew/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {

                SetAlert("Xảy ra lỗi.", "danger");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContenNew article = db.ContenNews.Find(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            ViewBag.ImageSavePath = ConfigurationManager.AppSettings["ImageSavePath"];

            List<CatNew> list = (from t in db.CatNews where t.DeleteCateVideo == false orderby t.CatDepth select t).ToList();
            foreach (CatNew item in list)
            {
                if (item.CatDepth == 1)
                    item.Name = "---" + item.Name;
                else if (item.CatDepth == 2)
                    item.Name = "------" + item.Name;
            }

            ViewBag.CategoryID = new SelectList(list, "ID", "Name", article.CategoryID);
            return View(article);
        }

        // POST: /Backends/ContenNew/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include="Id,Name,MetaTitle,Description,Image,CategoryID,Detail,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDescriptions,ViewCount,TopHot,Status,Tags,DeleteVideo")] ContenNew contennew)
        {
            if (ModelState.IsValid)
            {
                string img = UploadImage();
                if (!String.IsNullOrEmpty(img))
                {
                    deleteImg(contennew.Image);
                    contennew.Image = img;
                }
                contennew.DeleteVideo = false;
                contennew.ModifiedDate = DateTime.Now;
                db.Entry(contennew).State = EntityState.Modified;
                db.SaveChanges();
                SetAlert("Update Bài Viết Thành Công", "success");
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.CatNews, "ID", "Name", contennew.CategoryID);
            return View(contennew);
        }

        // GET: /Backends/ContenNew/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContenNew contennew = db.ContenNews.Find(id);
            if (contennew == null)
            {
                return HttpNotFound();
            }
            return View(contennew);
        }

        // POST: /Backends/ContenNew/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            ContenNew contennew = db.ContenNews.Find(id);
            db.ContenNews.Remove(contennew);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Xoabaiviet(long id)
        {
            var model = db.ContenNews.Find(id);
            deleteImg(model.Image);
            db.ContenNews.Remove(model);
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
        [HttpPost]
        public ActionResult DeleteMulti(List<long> listDelete)
        {
            try
            {
               
                List<ContenNew> list = (from t in db.ContenNews where listDelete.Contains(t.Id) select t).ToList();
                try
                {
                    foreach (ContenNew item in list)
                    {
                        deleteImg(item.Image);
                        db.ContenNews.Remove(item);
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
            FileInfo file2 = new FileInfo(Server.MapPath(ConfigurationManager.AppSettings["ImageSavePath"] + "/articles/" + ArticleImg));
            if (file2.Exists)
            {
                System.IO.File.Delete(Server.MapPath(ConfigurationManager.AppSettings["ImageSavePath"] + "/articles/" + ArticleImg));
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
                string saveDB = ConfigurationManager.AppSettings["ImageSavePath"] + "/articles/";
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
