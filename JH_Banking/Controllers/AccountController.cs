using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JH_Banking.Models;
using System.Data.Entity;
using System.Web.Security;
using JH_Banking.Models.Helpers;
using System.Data.SqlClient;
using System.Net;

namespace JH_Banking.Controllers
{
    
    public class AccountController : Controller
    {
        // GET: Account
        [Authorize(Roles = "A")]
        public ActionResult Index()
        {
            using (JH_DBContext db = new JH_DBContext())
            {
                return View(db.UserAccount.ToList());
            }
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
            if (user.UserName !=null || user.Password != null)
            {
                using (JH_DBContext db = new JH_DBContext())
                {
                    string hashedPass = PasswordHelper.HashPassword(user.Password);

                    UserAccount usr = db.UserAccount.Where(u => u.UserName == user.UserName).SingleOrDefault();
                    if (usr != null)// && )
                    {
                        if (PasswordHelper.VerifyHashedPassword(usr.Password, user.Password))
                        {
                            Session["UserId"] = usr.UserId.ToString();
                            Session["UserName"] = usr.UserName.ToString();
                            FormsAuthentication.SetAuthCookie(usr.UserId.ToString(), false);
                            if (usr.Admin)
                            {
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                return RedirectToAction("LoggedIn");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Username or password wrong.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Username or password wrong.");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Username or password wrong.");
            }
            return View();
        }
        [Authorize(Roles = "U")]
        public ActionResult LoggedIn()
        {
            int userId = Int32.Parse(User.Identity.Name);
            using (JH_DBContext db = new JH_DBContext())
            {
                ViewBag.Name = db.UserAccount.Where(acc => acc.UserId == userId).SingleOrDefault().FirstName;
            }
            return View();         
        }
        [Authorize(Roles = "A,U")]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        [Authorize(Roles = "A")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (JH_DBContext db = new JH_DBContext())
            {
                UserAccount userAccount = db.UserAccount.Single(u => u.UserId == id);
                return View(userAccount);
            }               
        }
        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult Edit([Bind(Exclude = "UserName,Password")]UserAccount user)
        {
            if (ModelState.IsValid)
            {
                using (JH_DBContext db = new JH_DBContext())
                {
                    UserAccount dbuser = db.UserAccount.Where(u => u.UserId == user.UserId).SingleOrDefault();
                    user.UserName = dbuser.UserName;
                    user.Password = dbuser.Password;
                    db.spUpdateUser(user);
                    db.SaveChanges();
                }

                ViewBag.Message = "Acount for user " + user.FirstName + " " + user.LastName + " was updated.";
            }
            return View();
        }
        [Authorize(Roles = "A")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult Create(UserAccount u)
        {
            if (ModelState.IsValid)
            {
                string pass = PasswordHelper.GeneratePassword();
                string hashedPass = PasswordHelper.HashPassword(pass);


                using (JH_DBContext db = new JH_DBContext())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        //db.UserAccount.Add(u);
                        string generatedUsername;
                        generatedUsername = db.spCreateUser(u.FirstName, u.LastName, u.PidNum, u.PlaceOfBirth, u.Citizenship, u.Street, u.NumberOfDescriptive, u.Town, u.Zip, u.Email, u.Phone, u.MailStreet, u.MailNumberOfDescriptive, u.MailTown, u.MailZip, hashedPass, u.Admin);
                        //var entry = db.Entry(user);

                        if (EmailHelper.SendCredentials(generatedUsername, pass,u.Email))
                        {
                            ModelState.AddModelError("", "Email was sent.");
                            db.SaveChanges();
                            transaction.Commit();
                            ViewBag.Message = "Acount for user " + u.FirstName + " " + u.LastName + " was created.";
                        }
                        else
                        {
                            ModelState.AddModelError("", "Email wasn't sent. Please contact helpdesk.");
                            transaction.Rollback();
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Some fields have invalid values.");
            }
            return View();
        }
        [Authorize(Roles = "A")]
        public ActionResult Details(int id)
        {
            using (JH_DBContext db = new JH_DBContext())
            {
                UserAccount userAccount = db.UserAccount.Single(u => u.UserId == id);
                return View(userAccount);
            }
        }
        [Authorize(Roles = "U")]
        public ActionResult UsrDetails()
        {
            using (JH_DBContext db = new JH_DBContext())
            {
                int userId = Int32.Parse(User.Identity.Name);
                UserAccount userAccount = db.UserAccount.Single(u => u.UserId == userId);
                return View(userAccount);
            }
        }
        [Authorize(Roles = "U")]
        [HttpGet]
        public ActionResult UsrEdit()
        {
            using (JH_DBContext db = new JH_DBContext())
            {
                int userId = Int32.Parse(User.Identity.Name);
                UserAccount userAccount = db.UserAccount.Single(u => u.UserId == userId);
                return View(userAccount);
            }
        }
        [Authorize(Roles = "U")]
        [HttpPost]
        public ActionResult UsrEdit([Bind(Include = "Email,Phone,MailStreet,MailNumberOfDescriptive,MailTown,MailZip")] UserAccount user)
        {
            //validating model only for provided values
            List<string> PropertyNames = new List<string>()
                {
                    "Email",
                    "Phone",
                    "MailStreet",
                    "MailNumberOfDescriptive",
                    "MailTown",
                    "MailZip"
                };
            bool test1 = ModelState.IsValidField("Email");
            bool test2 = PropertyNames.Any(p => ModelState.IsValidField(p));
            if (PropertyNames.All(p => ModelState.IsValidField(p)))
            {
                using (JH_DBContext db = new JH_DBContext())
                {
                    int userId = Int32.Parse(User.Identity.Name);
                    //getting user  from db to prevent unintended  updates
                    UserAccount dbUser = db.UserAccount.Where(u => u.UserId == userId).SingleOrDefault(); ;

                    dbUser.Email = user.Email;
                    dbUser.Phone = user.Phone;
                    dbUser.MailStreet = user.MailStreet;
                    dbUser.MailNumberOfDescriptive = user.MailNumberOfDescriptive;
                    dbUser.MailTown = user.MailTown;
                    dbUser.MailZip = user.MailZip;
                    db.Entry(dbUser).State = EntityState.Modified;

                    db.SaveChanges();
                }

                ViewBag.Message = "Acount for user " + user.FirstName + " " + user.LastName + " was updated.";
            }
            else
            {
                ModelState.AddModelError("", "Some fields have invalid values.");
            }
            return View();
        }
        // GET: UserAccount/Delete/5
        [Authorize(Roles = "A")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            using (JH_DBContext db = new JH_DBContext())
            {
                UserAccount userAccount = db.UserAccount.Find(id);
                if (userAccount == null)
                {
                    return HttpNotFound();
                }
                return View(userAccount);
            }
        }

        // POST: UserAccount/Delete/5
        [Authorize(Roles = "A")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (JH_DBContext db = new JH_DBContext())
            {
                UserAccount userAccount = db.UserAccount.Find(id);
                db.UserAccount.Remove(userAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }

}