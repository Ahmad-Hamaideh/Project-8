using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Project_8_MVC_Batool;

namespace Project_8_MVC_Batool.Controllers
{

    [Authorize]
    public class AspNetUsersController : Controller
    {
        private Project_8Entities db = new Project_8Entities();

        // GET: AspNetUsers

        [Authorize (Roles ="Admin")]
        public ActionResult Index()
        {
            var aspNetUsers = db.AspNetUsers.Include(a => a.Major);
            return View(aspNetUsers.ToList());
        }



        public ActionResult Logout()
        {
           // Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Abandon();
            Response.Redirect("https://localhost:44370/Home/Index");
            return View();
        }




        public ActionResult UpdateColumn(string id, string newValue)
        {
            var recordToUpdate = db.AspNetUsers.FirstOrDefault(r => r.Id == id);
            recordToUpdate.stutus_ofDelay = Convert.ToInt32( newValue);

            // Save the changes back to the database
            db.SaveChanges();

            return Content("Column updated successfully.");
        }


        public ActionResult Index1()
        {
            var aspNetUsers = db.AspNetUsers.Include(a => a.Major);
            return View(aspNetUsers.ToList());
        }

        public PartialViewResult accepted()
        {
            var accept = User.Identity.GetUserId();
            ViewBag.Accept = accept;
            var x = db.Majors.ToList();
            return PartialView( "" ,x);
        }


        // GET: AspNetUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        [Authorize (Roles = "Student")]
        public ActionResult Details1(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }


        // GET: AspNetUsers/Create
        public ActionResult Create()
        {
            ViewBag.Major_id = new SelectList(db.Majors, "MajorID", "MajorName");
            return View();
        }

        // POST: AspNetUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( AspNetUser aspNetUser ,HttpPostedFileBase Tawjihi_Image , HttpPostedFileBase UserImage)
        {
            if (ModelState.IsValid)
            {
                if (Tawjihi_Image != null)
                {
                    //string fileName = Path.GetFileName(image.FileName);
                    string path = Server.MapPath("~/image/") + Tawjihi_Image.FileName;
                    Tawjihi_Image.SaveAs(path);
                    aspNetUser.Tawjihi_Image = Tawjihi_Image.FileName;
                }

                if (UserImage != null)
                {
                    //string fileName = Path.GetFileName(image.FileName);
                    string path = Server.MapPath("~/image/") + UserImage.FileName;
                    UserImage.SaveAs(path);
                    aspNetUser.UserImage = UserImage.FileName;
                }


                db.AspNetUsers.Add(aspNetUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Major_id = new SelectList(db.Majors, "MajorID", "MajorName", aspNetUser.Major_id);
            ViewBag.major = aspNetUser.Major.minimum_rate;
            return View(aspNetUser);
        }

        // GET: AspNetUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            Session["email"] = aspNetUser.Email;
            Session["Phone"] = aspNetUser.PhoneNumber;
            Session["Name"] = aspNetUser.FullName;
            var id1 = User.Identity.GetUserId();
            Session["UImage"] = aspNetUser.UserImage;
            Session["TImage"] = aspNetUser.Tawjihi_Image;
            Session["SSN"] = aspNetUser.SSN;
            Session["your_study"] = aspNetUser.Year_OfStudy;
            Session["GPA"] = aspNetUser.GPA;
            Session["ECon"] = aspNetUser.UserName;
            Session["pass"] = aspNetUser.PasswordHash;
            Session["sec"] = aspNetUser.SecurityStamp;
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            //int x = Convert.ToInt32(Request["Is_Send"]);
            int? x = db.AspNetUsers.Find(id).Is_Send;
            if (x==1)
            {
                var email = db.AspNetUsers.Find(id).Email;
                MailMessage mail = new MailMessage();
                mail.To.Add($"{email}".ToString().Trim());
                mail.From = new MailAddress("iahmed.hamaideh@gmail.com");
                mail.Subject = "Hello " + email;
                mail.Body = " <!DOCTYPE html>\r\n<html lang=\"en\" >\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  <title>CodePen - New Account Email Template</title>\r\n  \r\n\r\n</head>\r\n<body>\r\n<!-- partial:index.partial.html -->\r\n<!doctype html>\r\n<html lang=\"en-US\">\r\n\r\n<head>\r\n    <meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />\r\n    <title>New Account Email Template</title>\r\n    <meta name=\"description\" content=\"New Account Email Template.\">\r\n    <style type=\"text/css\">\r\n        a:hover {text-decoration: underline !important;}\r\n    </style>\r\n</head>\r\n\r\n<body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px; background-color: #f2f3f8;\" leftmargin=\"0\">\r\n    <!-- 100% body table -->\r\n    <table cellspacing=\"0\" border=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#f2f3f8\"\r\n        style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\">\r\n        <tr>\r\n            <td>\r\n                <table style=\"background-color: #f2f3f8; max-width:670px; margin:0 auto;\" width=\"100%\" border=\"0\"\r\n                    align=\"center\" cellpadding=\"0\" cellspacing=\"0\">\r\n                    <tr>\r\n                        <td style=\"height:80px;\">&nbsp;</td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"text-align:center;\">\r\n                            <a href=\"https://rakeshmandal.com\" title=\"logo\" target=\"_blank\">\r\n                            <img width=\"60\" src=\"https://i.ibb.co/hL4XZp2/android-chrome-192x192.png\" title=\"logo\" alt=\"logo\">\r\n                          </a>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"height:20px;\">&nbsp;</td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td>\r\n                            <table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                style=\"max-width:670px; background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);\">\r\n                                <tr>\r\n                                    <td style=\"height:40px;\">&nbsp;</td>\r\n                                </tr>\r\n                                <tr>\r\n                                    <td style=\"padding:0 35px;\">\r\n                                        <h1 style=\"color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;\">Hard Luck !!\r\n                                        </h1>\r\n                                        <p style=\"font-size:15px; color:#455056; margin:8px 0 0; line-height:24px;\">\r\n  You are accepted in our university , Enter to our website to continue Application Steps.  . Now go to the university website to register the courses.\r\n <br><strong>  We wish you success\r\n    .</p>\r\n                                        <span\r\n                                            style=\"display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;\"></span>\r\n                                        <p\r\n                                            style=\"color:#455056; font-size:18px;line-height:20px; margin:0; font-weight: 500;\">\r\n                                            <strong\r\n                                                                      <strong\r\n                                                style=\"display: block; font-size: 13px; margin: 24px 0 4px 0; f\r\n\r\n                                        <a href=\"https://localhost:44370/Home/Index\"\r  style=\"background:#20e277;text-decoration:none !important; display:inline-block; font-weight:500; margin-top:24px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\"> Go to the university website\r\n </a>\r\n                                    </td>\r\n                                </tr>\r\n                                <tr>\r\n                                    <td style=\"height:40px;\">&nbsp;</td>\r\n                                </tr>\r\n                            </table>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"height:20px;\">&nbsp;</td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"text-align:center;\">\r\n                            <p style=\"font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;\">&copy; <strong>www.Hamaideh_university.com</strong> </p>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"height:80px;\">&nbsp;</td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n    <!--/100% body table-->\r\n</body>\r\n\r\n</html>\r\n<!-- partial -->\r\n  \r\n</body>\r\n</html>\r\n";
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587; // 25 465
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential("iahmed.hamaideh@gmail.com", "yheigdkvtpoeqrhl");

                smtp.Send(mail);
            }
            else if(x==2)
            {

                var email = db.AspNetUsers.Find(id).Email;
                MailMessage mail = new MailMessage();
                mail.To.Add($"{email}".ToString().Trim());
                mail.From = new MailAddress("iahmed.hamaideh@gmail.com");
                mail.Subject = "Hello " + email;
                mail.Body = " <!DOCTYPE html>\r\n<html lang=\"en\" >\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  <title>CodePen - New Account Email Template</title>\r\n  \r\n\r\n</head>\r\n<body>\r\n<!-- partial:index.partial.html -->\r\n<!doctype html>\r\n<html lang=\"en-US\">\r\n\r\n<head>\r\n    <meta content=\"text/html; charset=utf-8\" http-equiv=\"Content-Type\" />\r\n    <title>New Account Email Template</title>\r\n    <meta name=\"description\" content=\"New Account Email Template.\">\r\n    <style type=\"text/css\">\r\n        a:hover {text-decoration: underline !important;}\r\n    </style>\r\n</head>\r\n\r\n<body marginheight=\"0\" topmargin=\"0\" marginwidth=\"0\" style=\"margin: 0px; background-color: #f2f3f8;\" leftmargin=\"0\">\r\n    <!-- 100% body table -->\r\n    <table cellspacing=\"0\" border=\"0\" cellpadding=\"0\" width=\"100%\" bgcolor=\"#f2f3f8\"\r\n        style=\"@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;\">\r\n        <tr>\r\n            <td>\r\n                <table style=\"background-color: #f2f3f8; max-width:670px; margin:0 auto;\" width=\"100%\" border=\"0\"\r\n                    align=\"center\" cellpadding=\"0\" cellspacing=\"0\">\r\n                    <tr>\r\n                        <td style=\"height:80px;\">&nbsp;</td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"text-align:center;\">\r\n                            <a href=\"https://rakeshmandal.com\" title=\"logo\" target=\"_blank\">\r\n                            <img width=\"60\" src=\"https://i.ibb.co/hL4XZp2/android-chrome-192x192.png\" title=\"logo\" alt=\"logo\">\r\n                          </a>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"height:20px;\">&nbsp;</td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td>\r\n                            <table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\"\r\n                                style=\"max-width:670px; background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);\">\r\n                                <tr>\r\n                                    <td style=\"height:40px;\">&nbsp;</td>\r\n                                </tr>\r\n                                <tr>\r\n                                    <td style=\"padding:0 35px;\">\r\n                                        <h1 style=\"color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;\">Congratulations !!\r\n                                        </h1>\r\n                                        <p style=\"font-size:15px; color:#455056; margin:8px 0 0; line-height:24px;\">\r\n  You are accepted in our university , Enter to our website to continue Application Steps.  . Now go to the university website to register the courses.\r\n <br><strong>  We wish you success\r\n    .</p>\r\n                                        <span\r\n                                            style=\"display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;\"></span>\r\n                                        <p\r\n                                            style=\"color:#455056; font-size:18px;line-height:20px; margin:0; font-weight: 500;\">\r\n                                            <strong\r\n                                                                      <strong\r\n                                                style=\"display: block; font-size: 13px; margin: 24px 0 4px 0; f\r\n\r\n                                        <a href=\"https://localhost:44370/Home/Index\"\r  style=\"background:#20e277;text-decoration:none !important; display:inline-block; font-weight:500; margin-top:24px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;\"> Go to the university website\r\n </a>\r\n                                    </td>\r\n                                </tr>\r\n                                <tr>\r\n                                    <td style=\"height:40px;\">&nbsp;</td>\r\n                                </tr>\r\n                            </table>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"height:20px;\">&nbsp;</td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"text-align:center;\">\r\n                            <p style=\"font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;\">&copy; <strong>www.Hamaideh_university.com</strong> </p>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"height:80px;\">&nbsp;</td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n    <!--/100% body table-->\r\n</body>\r\n\r\n</html>\r\n<!-- partial -->\r\n  \r\n</body>\r\n</html>\r\n";
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587; // 25 465
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential("iahmed.hamaideh@gmail.com", "yheigdkvtpoeqrhl");

                smtp.Send(mail);

            }
            ViewBag.Major_id = new SelectList(db.Majors, "MajorID", "MajorName", aspNetUser.Major_id);
            return View(aspNetUser);




        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,UserImage,Tawjihi_Image,SSN,Year_OfStudy,GPA,Is_Save,Is_Send,Major_id")] AspNetUser aspNetUser)
        {
            aspNetUser.Email = Session["email"].ToString();
            aspNetUser.PhoneNumber = Session["Phone"].ToString();
            aspNetUser.FullName = Session["Name"].ToString();
            aspNetUser.PasswordHash= Session["pass"].ToString();
            aspNetUser.UserImage = Session["UImage"].ToString();
         aspNetUser.Tawjihi_Image=   Session["TImage"].ToString() ;
            aspNetUser.SecurityStamp = Session["sec"].ToString();
            var x = Session["SSN"];
            int x1 = Convert.ToInt32(x);
            aspNetUser.SSN =x1 ;

            var y = Session["your_study"];
            int y1 = Convert.ToInt32(y);
          aspNetUser.Year_OfStudy = y1;
            Session["GPA"] = aspNetUser.GPA;
            //  Session["major"] = ;

            // var isSend =  db.AspNetUsers
            int accept = Convert.ToInt32(Request["Is_Send"]);
            string userId= Request["Id"].ToString();
            if(accept == 1)
            {

                db.AspNetUserRoles.Add(new AspNetUserRole() { UserId = userId , RoleId = "3" });

                

                
            }


            aspNetUser.UserName = Session["ECon"].ToString();
            if (ModelState.IsValid)
            {


                db.Entry(aspNetUser).State = EntityState.Modified;
                db.SaveChanges();

              

               
                return RedirectToAction("Index");



            }
            ViewBag.Major_id = new SelectList(db.Majors, "MajorID", "MajorName", aspNetUser.Major_id);
            return View(aspNetUser);
        }

        // GET: AspNetUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUser);
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
