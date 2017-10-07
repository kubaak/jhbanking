using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JH_Banking.Models;

namespace JH_Banking.Controllers
{
    public class BankAccountController : Controller
    {
        private JH_DBContext db = new JH_DBContext();

        // GET: BankAccount
        //public ActionResult Index()
        //{
        //    return View(db.BankAccount.ToList());
        //}
        [Authorize(Roles ="A")]
        public ActionResult Index(int userId)
        {
            ViewBag.UserId = userId;
            return View(db.BankAccount.Where(ba => ba.UserId == userId).ToList());
        }
        [Authorize(Roles ="U")]
        public ActionResult userBAList()
        {
            int userId = Int32.Parse(User.Identity.Name);
            return View(db.BankAccount.Where(ba => ba.UserId == userId).ToList());
        }

        // GET: BankAccount/Create
        [Authorize(Roles = "A")]
        public ActionResult Create(int userId)
        {
            ViewBag.UserId = userId;
            return View();
        }

        // POST: BankAccount/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "A")]
        public ActionResult Create([Bind(Include = "AccNum,UserId,Balance")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {             
                db.spCreateBankAccount(bankAccount.Balance, bankAccount.UserId);
                //db.BankAccount.Add(bankAccount);
                db.SaveChanges();
                return RedirectToAction("Index",new { userId = bankAccount.UserId});
            }

            return View(bankAccount);
        }

        // GET: BankAccount/Edit/5
        [Authorize(Roles = "A")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccount.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "A")]
        public ActionResult Edit([Bind(Include = "AccId,Balance,CardNum")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                BankAccount dbBankAccount = db.BankAccount.Where(ba => ba.AccId == bankAccount.AccId).SingleOrDefault();
                bankAccount.UserId = dbBankAccount.UserId;
                db.spUpdateBankAccount(bankAccount);
                db.SaveChanges();
                return RedirectToAction("Index",new { userId = bankAccount.UserId});
            }
            return View(bankAccount);
        }

        // GET: BankAccount/Delete/5
        [Authorize(Roles = "A")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = db.BankAccount.Find(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccount/Delete/5
        [Authorize(Roles = "A")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankAccount bankAccount = db.BankAccount.Find(id);
            db.BankAccount.Remove(bankAccount);
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
