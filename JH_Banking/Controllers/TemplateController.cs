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
    public class TemplateController : Controller
    {
        private JH_DBContext db = new JH_DBContext();

        // GET: Template
        public ActionResult Index()
        {
            int userId = Int32.Parse(User.Identity.Name);
            return View(db.Template.Where(t=> t.UserId == userId).ToList());
        }

        // GET: Template/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Template template = db.Template.Find(id);
            if (template == null)
            {
                return HttpNotFound();
            }
            return View(template);
        }

        // GET: Template/Create
        public ActionResult Create()
        {
            int userId = Int32.Parse(User.Identity.Name);
            ViewBag.BankAccounts = new SelectList(db.BankAccount.Where(ba => ba.UserId == userId).ToList(), "AccNum", "AccNum");

            return View();
        }
        [HttpPost]
        public ActionResult CreateAjax(Template template)
        {
            template.UserId = Int32.Parse(User.Identity.Name);
            if (ModelState.IsValid)
            {
                db.Template.Add(template);
                db.SaveChanges();
            }
            return Json(new { success=true});
        }
        // POST: Template/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TemplName,AccNum,DestAccNum,DestBankCode,Amount,UserId")] Template template)
        {
            template.UserId = Int32.Parse(User.Identity.Name);
            if (ModelState.IsValid)
            {
                db.Template.Add(template);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(template);
        }

        // GET: Template/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Template template = db.Template.Find(id);
            if (template == null)
            {
                return HttpNotFound();
            }
            int userId = Int32.Parse(User.Identity.Name);
            ViewBag.BankAccounts = new SelectList(db.BankAccount.Where(ba => ba.UserId == userId).ToList(), "AccNum", "AccNum");

            return View(template);
        }

        // POST: Template/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TemplName,AccNum,DestAccNum,DestBankCode,Amount,UserId")] Template template)
        {
            if (ModelState.IsValid)
            {
                template.UserId = Int32.Parse(User.Identity.Name);
                db.Entry(template).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(template);
        }

        public PartialViewResult TemplateData(int? selectedTemplate)
        {
            int userId = Int32.Parse(User.Identity.Name);

            Template template = db.Template.Where(t => t.TempId == selectedTemplate).SingleOrDefault();
            ViewBag.BankAccounts = db.BankAccount.Where(b => b.UserId == userId).ToList();
            return PartialView(template);
        }

        // GET: Template/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Template template = db.Template.Find(id);
            if (template == null)
            {
                return HttpNotFound();
            }
            return View(template);
        }

        // POST: Template/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Template template = db.Template.Find(id);
            db.Template.Remove(template);
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
