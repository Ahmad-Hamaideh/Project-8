using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_8_MVC_Batool;

namespace Project_8_MVC_Batool.Controllers
{
    public class FacultiesController : Controller
    {
        private Project_8Entities db = new Project_8Entities();

        // GET: Faculties
        public ActionResult Index()
        {
            return View(db.Faculties.ToList());
        }

        // GET: Faculties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faculty faculty = db.Faculties.Find(id);
            if (faculty == null)
            {
                return HttpNotFound();
            }
            return View(faculty);
        }

        // GET: Faculties/Create
        public ActionResult Create()
        {
           
            return View();
        }

        // POST: Faculties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Faculty faculty, HttpPostedFileBase Faculity_Image)
        {
            if (ModelState.IsValid)
            {
                if (Faculity_Image != null)
                {
                    //string fileName = Path.GetFileName(image.FileName);
                    string path = Server.MapPath("~/image/") + Faculity_Image.FileName;
                    Faculity_Image.SaveAs(path);
                    faculty.Faculity_Image = Faculity_Image.FileName;
                }

               
                db.Faculties.Add(faculty);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(faculty);
        }

            // GET: Faculties/Edit/5
            public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            Faculty faculty = db.Faculties.Find(id);
            Session["image"] = faculty.Faculity_Image;
            if (faculty == null)
            {
                return HttpNotFound();
            }
            return View(faculty);
        }

        // POST: Faculties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Faculty faculty , HttpPostedFileBase Faculity_Image)
        {
            faculty.Faculity_Image = Session["image"].ToString();
            if (ModelState.IsValid)
            {

                if (Faculity_Image != null)
                {
                    //string fileName = Path.GetFileName(image.FileName);
                    string path = Server.MapPath("~/image/") + Faculity_Image.FileName;
                    Faculity_Image.SaveAs(path);
                    faculty.Faculity_Image = Faculity_Image.FileName;
                }


               


                db.Entry(faculty).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(faculty);
        }

        // GET: Faculties/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Faculty faculty = db.Faculties.Find(id);
            if (faculty == null)
            {
                return HttpNotFound();
            }
            return View(faculty);
        }

        // POST: Faculties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Faculty faculty = db.Faculties.Find(id);
            db.Faculties.Remove(faculty);
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
    }
}
