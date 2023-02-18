using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project_8_MVC_Batool;

namespace Project_8_MVC_Batool.Controllers
{
    public class DelaysController : Controller
    {
        private Project_8Entities db = new Project_8Entities();

        // GET: Delays
        public ActionResult Index()
        {
            var delays = db.Delays.Include(d => d.AspNetUser);
            return View(delays.ToList());
        }

        // GET: Delays/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delay delay = db.Delays.Find(id);
            if (delay == null)
            {
                return HttpNotFound();
            }
            return View(delay);
        }

        // GET: Delays/Create
        public ActionResult Create()
        {
            ViewBag.User_ID = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Delays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Delay,Description,User_ID,Semester,Delay_Hour")] Delay delay)
        {
            if (ModelState.IsValid)
            {
                db.Delays.Add(delay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.User_ID = new SelectList(db.AspNetUsers, "Id", "Email", delay.User_ID);
            return View(delay);
        }

        // GET: Delays/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delay delay = db.Delays.Find(id);
            Session["Description"] = delay.Description;
            Session["Semester"] = delay.Semester;
            Session["delay"] = delay.Delay_Hour;
            if (delay == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_ID = new SelectList(db.AspNetUsers, "Id", "Email", delay.User_ID);
            return View(delay);
        }

        // POST: Delays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Delay,Description,User_ID,Semester,Delay_Hour")] Delay delay)
        {


            delay.Description = Session["Description"].ToString();
            delay.Semester = Convert.ToInt32( Session["Semester"]);
          delay.Delay_Hour= Convert.ToInt32( Session["delay"]);
            if (ModelState.IsValid)
            {
                var user = delay.User_ID;
                db.AspNetUsers.Find(user).stutus_ofDelay = Convert.ToInt32(Request["AspNetUser.stutus_ofDelay"]);
              //  delay.AspNetUser.stutus_ofDelay = ;
                db.Entry(delay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User_ID = new SelectList(db.AspNetUsers, "Id", "Email", delay.User_ID);
            return View(delay);
        }

        // GET: Delays/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Delay delay = db.Delays.Find(id);
            if (delay == null)
            {
                return HttpNotFound();
            }
            return View(delay);
        }

        // POST: Delays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Delay delay = db.Delays.Find(id);
            db.Delays.Remove(delay);
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
