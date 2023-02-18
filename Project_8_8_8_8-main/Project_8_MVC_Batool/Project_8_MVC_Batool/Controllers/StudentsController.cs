using Microsoft.AspNet.Identity;
using Project_8_MVC_Batool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Project_8_MVC_Batool.Controllers
{
   

    public class StudentsController : Controller
    {
        private Project_8Entities db = new Project_8Entities();


        [Authorize (Roles ="Admin")]

        public ActionResult statistics()
        {
            var countmg = db.Majors.Count();
            var count = db.AspNetUsers.Count();
            var fcc = db.Faculties.Count();
            var std = db.AspNetUsers.Count();
            var co = db.Courses.Count();
            var payy = db.AspNetUsers.Count(b => b.Stutus_OfPayment == 1);
            var model = new statisticss
            {
                Count = count,
                Countmg = countmg,
                fuc = fcc,
                sty = std,
                cor = co,
                pay = payy

            };

            return View(model);
        }





        [Authorize (Roles = "Student")]
        public ActionResult CourseRegist()
        {
            var courses = db.Courses.ToList();

            return View(courses);
        }


        public ActionResult AddCourse(string secId)
        {

            int secid = Convert.ToInt32(secId);
            string userid = User.Identity.GetUserId().ToString();

            var myCousrses = db.InRolements.Select(item => item).Where(item => item.UserID.Equals(userid));
            bool isExist = false;
            bool isConflict = false;
            int courseID = Convert.ToInt32(db.Times.Find(secid).CourseID);

            var addSection = db.Times.Find(secid);
            foreach (var item in myCousrses)
            {
                if (item.Time.Course_Time == addSection.Course_Time && item.Time.Course_Day == addSection.Course_Day)
                    isConflict = true;
            }
            foreach (var item in myCousrses)
            {
                if (item.Time.CourseID == courseID)
                    isExist = true;
            }

            if (isConflict)
            {

                TempData["Message"] = "Time Conflict in action";
                return Redirect("CourseRegist");
            }

            if (isExist)
            {
                return Redirect("CourseRegist");
            }


            var user = db.AspNetUsers.Find(userid);
            var section = db.Times.Find(secid);
            var course = db.Courses.Find(section.CourseID);
            var major = db.Majors.Find(course.Major_ID);

            double coursePrice = Convert.ToDouble(course.Number_OfHour) * Convert.ToDouble(major.Price_OfHour);

            if (user.stutus_ofDelay == 1)
            {
                user.debet += coursePrice;
                courseRegisteration(secid);
            }
            if (user.Balance >= coursePrice && user.stutus_ofDelay == 0)
            {
                user.Balance -= coursePrice;
                courseRegisteration(secid);
            }
            if (user.Balance < coursePrice && user.stutus_ofDelay == 0) {

                TempData["notEnough"] = "Your balance is not enough to register this course";
            
            }

            return Redirect("CourseRegist");
        }


        public void courseRegisteration(int secid)
        {

            string userid = User.Identity.GetUserId().ToString();
            InRolement inr = new InRolement();
            inr.Time_ID= secid;
            inr.UserID = userid;
            db.InRolements.Add(inr);
            db.SaveChanges();


        }

        public ActionResult removeCourse(string enrolID)
        {
            db.InRolements.Remove(db.InRolements.Find(Convert.ToInt32(enrolID)));
            db.SaveChanges();

            return Redirect("CourseRegist");
        }
        public PartialViewResult courseScudule()
        {
            string userID = User.Identity.GetUserId();
            var myCousrses = db.InRolements.Select(item => item).Where(item => item.UserID.Equals(userID));

            return PartialView( myCousrses);
        }



        public ActionResult batool()
        {
            return View();
        }


        [Authorize(Roles = "Student")]
        // GET: Students
        public ActionResult pay2()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        [Authorize (Roles ="Student")]
        public ActionResult pay2([Bind(Include = "Payment_ID,Status_OfPayment,Money_Payment,Number_OfPayment,Status_OfDelay,Debit,UserID")] Payment payment)
        {
            var z = Request["Money_Payment"];

            if (ModelState.IsValid)
            {
                var id = User.Identity.GetUserId();
                db.AspNetUsers.Find(id).Stutus_OfPayment = 1;
                payment.UserID = id;
                db.AspNetUsers.Find(id).Balance += Convert.ToDouble(z);
                db.Payments.Add(payment);
                db.SaveChanges();
                var email = db.AspNetUsers.Find(id).Email;
                MailMessage mail = new MailMessage();
                mail.To.Add($"{email}".ToString().Trim());
                mail.From = new MailAddress("iahmed.hamaideh@gmail.com");
                mail.Subject = "Hello " + email;
                mail.Body = " <!DOCTYPE html>\r\n<html lang=\"en\" >\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  <title>CodePen - New Account Email Template</title>\r\n  \r\n\r\n</head>\r\n<body>\r\n<!-- partial:index.partial.html -->\r\n<!doctype html>\r\n<html lang=\"en-US\">\r\n\r\n<head>\r\n    <meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />\r\n    <title>New Account Email Template</title>\r\n    <meta name=\"description\" content=\"New Account Email Template.\">\r\n    <style type=\"text/css\">\r\n        a:hover {text-decoration: underline !important;}\r\n    </style>\r\n</head>\r\n\r\n<body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px; background-color: #f2f3f8;\" leftmargin=\"0\">\r\n    <!-- 100% body table -->\r\n    <table cellspacing=\"0\" border=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#f2f3f8\"\r\n        style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\">\r\n        <tr>\r\n            <td>\r\n                <table style=\"background-color: #f2f3f8; max-width:670px; margin:0 auto;\" width=\"100%\" border=\"0\"\r\n                    align=\"center\" cellpadding=\"0\" cellspacing=\"0\">\r\n                    <tr>\r\n                        <td style=\"height:80px;\">&nbsp;</td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"text-align:center;\">\r\n                            <a href=\"https://rakeshmandal.com\" title=\"logo\" target=\"_blank\">\r\n                            <img width=\"60\" src=\"https://i.ibb.co/hL4XZp2/android-chrome-192x192.png\" title=\"logo\" alt=\"logo\">\r\n                          </a>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"height:20px;\">&nbsp;</td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td>\r\n                            <table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                style=\"max-width:670px; background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);\">\r\n                                <tr>\r\n                                    <td style=\"height:40px;\">&nbsp;</td>\r\n                                </tr>\r\n                                <tr>\r\n                                    <td style=\"padding:0 35px;\">\r\n                                        <h1 style=\"color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;\">Payment Done\r\n                                        </h1>\r\n                                        <p style=\"font-size:15px; color:#455056; margin:8px 0 0; line-height:24px;\">\r\n  The payment process has been completed successfully  . Now go to the university website to register the courses.\r\n <br><strong>  We wish you success\r\n    .</p>\r\n                                        <span\r\n                                            style=\"display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;\"></span>\r\n                                        <p\r\n                                            style=\"color:#455056; font-size:18px;line-height:20px; margin:0; font-weight: 500;\">\r\n                                            <strong\r\n                                                                      <strong\r\n                                                style=\"display: block; font-size: 13px; margin: 24px 0 4px 0; f\r\n\r\n                                        <a href=\"https://localhost:44370/Home/Index\"\r  style=\"background:#20e277;text-decoration:none !important; display:inline-block; font-weight:500; margin-top:24px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\"> Go to the university website\r\n </a>\r\n                                    </td>\r\n                                </tr>\r\n                                <tr>\r\n                                    <td style=\"height:40px;\">&nbsp;</td>\r\n                                </tr>\r\n                            </table>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"height:20px;\">&nbsp;</td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"text-align:center;\">\r\n                            <p style=\"font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;\">&copy; <strong>www.Hamaideh_university.com</strong> </p>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"height:80px;\">&nbsp;</td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n    <!--/100% body table-->\r\n</body>\r\n\r\n</html>\r\n<!-- partial -->\r\n  \r\n</body>\r\n</html>\r\n";
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587; // 25 465
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential("iahmed.hamaideh@gmail.com", "yheigdkvtpoeqrhl");

                smtp.Send(mail);
            }
            else
            {

            }


            ViewBag.UserID = new SelectList(db.AspNetUsers, "Id", "Email", payment.UserID);
            return View("pay2");
        }


        [Authorize(Roles = "Student")]

        public ActionResult delay()
        {
            return View();
        }
        // POST: Delays/Create


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student")]

        public ActionResult delay([Bind(Include = "ID_Delay,Description,stutus_ofDelay,debet,User_ID,Semester,Delay_Hour")] Delay delay)
        {
            if (ModelState.IsValid)
            {
                var id = User.Identity.GetUserId();
                delay.User_ID = id;
                int price = Convert.ToInt32(db.Majors.Find(db.AspNetUsers.Find(id).Major_id).Price_OfHour);
                int hour = Convert.ToInt32(Request["Delay_Hour"]);
                db.AspNetUsers.Find(id).debet += hour * price;
                db.AspNetUsers.Find(id).stutus_ofDelay = 0;
                db.Delays.Add(delay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(delay);
        }


    }
}