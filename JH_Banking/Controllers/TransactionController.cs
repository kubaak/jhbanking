using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JH_Banking.Models;
using System.Web.Security;
using JH_Banking.Models.BusynessLogic;
using System.Configuration;
using Newtonsoft.Json.Linq;

namespace JH_Banking.Controllers
{
    public class TransactionController : Controller
    {
        private JH_DBContext db = new JH_DBContext();

        // GET: Transaction
        public ActionResult Index(int? AccId)
        {
            int userId = Int32.Parse(User.Identity.Name);
            //checking if account's history that user is trying to view is his
            BankAccount bankAccount = db.BankAccount.Where(ba => ba.AccId == AccId && ba.UserId == userId).SingleOrDefault();
            if (bankAccount!=null)
            {
                string accNum = bankAccount.AccNum.ToString();
                return View(db.Transaction.Where(tr => tr.AccNum == accNum).ToList());
            }
            else
            {
                return View();
            }
        }

        [Authorize(Roles = "U")]
        public ActionResult Create(int? AccId)
        {

            //in case we are coming to this acction with chose bank account
            string accNum="";
            if (AccId!=null)
            {
                accNum = db.BankAccount.Where(ba => ba.AccId == AccId).SingleOrDefault().AccNum;
            }           
            int userId = Int32.Parse(User.Identity.Name);
            ViewBag.Templates = new SelectList(db.Template.Where(t => t.UserId == userId).ToList(), "TempId", "TemplName");
            ViewBag.BankAccounts = new SelectList(db.BankAccount.Where(ba => ba.UserId == userId).ToList(), "AccNum", "AccNum", accNum);
            return View();
        }

        // POST: Transaction/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AccNum,DestAccNum,DestBankCode,DueDate,Amount")] Transaction transaction)
        {
            var response = Request["g-recaptcha-response"];
            string secretKey = ConfigurationManager.AppSettings["captchaSecretKey"].ToString();
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            
            var obj = JObject.Parse(result);
            bool status = (bool)obj.SelectToken("success");

            int userId = Int32.Parse(User.Identity.Name);

            if (status)
            {
                
                BankAccount bankAccount = db.BankAccount.Where(ba => ba.AccNum == transaction.AccNum && ba.UserId == userId).SingleOrDefault();
                transaction.AccNum = bankAccount.AccNum;
                transaction.Date = DateTime.Now;
                Payment pay = new Payment();
                if (!pay.makePayment(transaction))
                {
                    ModelState.AddModelError("", "Unsuccessfull transaction. Please check your account's balance.");
                    ViewBag.BankAccounts = new SelectList(db.BankAccount.Where(ba => ba.UserId == userId).ToList(), "AccNum", "AccNum", transaction.AccNum);
                    ViewBag.Templates = new SelectList(db.Template.Where(t => t.UserId == userId).ToList(), "TempId", "TemplName");
                    return View();
                }

                return RedirectToAction("Index",new { AccId = db.BankAccount.Where(b=> b.AccNum ==transaction.AccNum).SingleOrDefault().AccId});
            }
            else
            {
                ModelState.AddModelError("", "Please confirm that you're not a robot.");
                ViewBag.BankAccounts = new SelectList(db.BankAccount.Where(ba => ba.UserId == userId).ToList(), "AccNum", "AccNum", transaction.AccNum);
                ViewBag.Templates = new SelectList(db.Template.Where(t => t.UserId == userId).ToList(), "TempId", "TemplName");

                return View();
            }
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
