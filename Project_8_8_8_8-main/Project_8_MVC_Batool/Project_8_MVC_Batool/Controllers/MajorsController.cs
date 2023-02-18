using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Project_8_MVC_Batool;

namespace Project_8_MVC_Batool.Controllers
{
    public class MajorsController : Controller
    {
        private Project_8Entities db = new Project_8Entities();

        // GET: Majors
        public ActionResult Index()
        {
            var majors = db.Majors.Include(m => m.Faculty);
            return View(majors.ToList());
        }

        public ActionResult Index1(int id)
        {
            var majors = db.Majors.Where(a => a.Faculity_ID == id).Include(a => a.Faculty);
            return View(majors.ToList());
        }



        // GET: Majors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Major major = db.Majors.Find(id);
            if (major == null)
            {
                return HttpNotFound();
            }
            return View(major);
        }

        public ActionResult detailMajors(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Major major = db.Majors.Find(id);
            if (major == null)
            {
                return HttpNotFound();
            }
            return View(major);
        }



        // GET: Majors/Create
        public ActionResult Create()
        {
            ViewBag.Faculity_ID = new SelectList(db.Faculties, "FaculityID", "FaculityName");
            return View();
        }

        // POST: Majors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Major major , HttpPostedFileBase MajorImage)
        {
            if (ModelState.IsValid)
            {

                if (MajorImage != null)
                {
                    //string fileName = Path.GetFileName(image.FileName);
                    string path = Server.MapPath("~/image/") + MajorImage.FileName;
                    MajorImage.SaveAs(path);
                    major.MajorImage = MajorImage.FileName;
                }


               
                db.Majors.Add(major);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Faculity_ID = new SelectList(db.Faculties, "FaculityID", "FaculityName", major.Faculity_ID);
            return View(major);
        }

        // GET: Majors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Major major = db.Majors.Find(id);
            Session["image"] = major.MajorImage;
            if (major == null)
            {
                return HttpNotFound();
            }
          ViewBag.Faculity_ID = new SelectList(db.Faculties, "FaculityID", "FaculityName", major.Faculity_ID);
            return View(major);
        }

        // POST: Majors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Major major , HttpPostedFileBase MajorImage)
        {

            if (ModelState.IsValid)
            {
                string pathpic = "";
              major.MajorImage = Session["image"].ToString();

                if (MajorImage != null)
                {
                    pathpic = Path.GetFileName(MajorImage.FileName);
                    MajorImage.SaveAs(Path.Combine(Server.MapPath("~/image/"), MajorImage.FileName));
                    major.MajorImage = pathpic;
                }




                db.Entry(major).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }



              ViewBag.Faculity_ID = new SelectList(db.Faculties, "FaculityID", "FaculityName", major.Faculity_ID);
            return View(major);
        }

        // GET: Majors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Major major = db.Majors.Find(id);
            if (major == null)
            {
                return HttpNotFound();
            }
            return View(major);
        }

        // POST: Majors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Major major = db.Majors.Find(id);
            db.Majors.Remove(major);
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
