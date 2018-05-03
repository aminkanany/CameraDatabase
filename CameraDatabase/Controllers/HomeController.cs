using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CameraDatabase.Models;
using System.Data;
using System.Data.Entity;
using System.Net;

namespace CameraDatabase.Controllers
{
    public class HomeController : Controller
    {
        private CameraDBEntities db = new CameraDBEntities();

        // GET: Cameras
        public ActionResult Index(string searchModel, string searchStyle, string searchManufacturer)
        {
            //Search By Model
            var cameras = from c in db.Cameras
                          select c;
            if (!String.IsNullOrEmpty(searchModel))
            {
                cameras = cameras.Where(s => s.Model.Contains(searchModel));
            }

            //Search by Style
            var styleList = new List<string>();
            var styleQry = from d in db.Cameras
                           orderby d.Style
                           select d.Style;
            styleList.AddRange(styleQry.Distinct());
            ViewBag.searchStyle = new SelectList(styleList);
            if (!String.IsNullOrEmpty(searchStyle))
            {
                cameras = cameras.Where(x => x.Style == searchStyle);
            }

            //Search by Manufacturer
            var manufacturerList = new List<string>();
            var manufacturerQry = from d in db.Cameras
                                  orderby d.Manufacturer
                                  select d.Manufacturer;
            manufacturerList.AddRange(manufacturerQry.Distinct());
            ViewBag.searchManufacturer = new SelectList(manufacturerList);
            if (!String.IsNullOrEmpty(searchManufacturer))
            {
                cameras = cameras.Where(x => x.Manufacturer == searchManufacturer);
            }

            return View(cameras);
        }

        //LIKE
        public ActionResult Like(int id)
        {
            Camera cam = db.Cameras.Find(id);
            cam.Rate++;
            if (ModelState.IsValid)
            {
                db.Entry(cam).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //DISLIKE
        public ActionResult Dislike(int id)
        {
            Camera cam = db.Cameras.Find(id);
            cam.Rate--;
            if (ModelState.IsValid)
            {
                db.Entry(cam).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        // GET: Cameras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Camera camera = db.Cameras.Find(id);
            if (camera == null)
            {
                return HttpNotFound();
            }
            return View(camera);
        }

        // GET: Cameras/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cameras/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Manufacturer,Model,Style,Megapixels,Sensor,ISO,Shutter,FocusingPoints,Processor,Flash,Memory,Interface,Battery,Dimensions,Weight,Price,Image,SampleImage,Rate")] Camera camera)
        {
            if (ModelState.IsValid)
            {
                //Default images
                if (camera.Image == null)
                    camera.Image = "http://camdb.1coder.net/img/na.png";
                if (camera.SampleImage == null)
                    camera.SampleImage = "http://camdb.1coder.net/img/na.png";

                db.Cameras.Add(camera);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(camera);
        }

        // GET: Cameras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Camera camera = db.Cameras.Find(id);
            if (camera == null)
            {
                return HttpNotFound();
            }
            return View(camera);
        }

        // POST: Cameras/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Manufacturer,Model,Style,Megapixels,Sensor,ISO,Shutter,FocusingPoints,Processor,Flash,Memory,Interface,Battery,Dimensions,Weight,Price,Image,SampleImage,Rate")] Camera camera)
        {
            if (ModelState.IsValid)
            {
                //Default images
                if (camera.Image == null)
                    camera.Image = "http://camdb.1coder.net/img/na.png";
                if (camera.SampleImage == null)
                    camera.SampleImage = "http://camdb.1coder.net/img/na.png";

                db.Entry(camera).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(camera);
        }

        // GET: Cameras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Camera camera = db.Cameras.Find(id);
            if (camera == null)
            {
                return HttpNotFound();
            }
            return View(camera);
        }

        // POST: Cameras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Camera camera = db.Cameras.Find(id);
            db.Cameras.Remove(camera);
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

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}