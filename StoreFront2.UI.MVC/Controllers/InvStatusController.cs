using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreFront2.DATA.EF;

namespace StoreFront2.UI.MVC.Controllers
{
    [Authorize]
    public class InvStatusController : Controller
    {
        private StoreFrontEntities db = new StoreFrontEntities();

        // GET: InvStatus
        public ActionResult Index()
        {
            return View(db.InvStatus.ToList());
        }

        // GET: InvStatus/Details/5
        [Authorize(Roles ="Admin, Sales")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvStatu invStatu = db.InvStatus.Find(id);
            if (invStatu == null)
            {
                return HttpNotFound();
            }
            return View(invStatu);
        }

        // GET: InvStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InvStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvID,InvName")] InvStatu invStatu)
        {
            if (ModelState.IsValid)
            {
                db.InvStatus.Add(invStatu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invStatu);
        }

        // GET: InvStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvStatu invStatu = db.InvStatus.Find(id);
            if (invStatu == null)
            {
                return HttpNotFound();
            }
            return View(invStatu);
        }

        // POST: InvStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvID,InvName")] InvStatu invStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invStatu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invStatu);
        }

        // GET: InvStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvStatu invStatu = db.InvStatus.Find(id);
            if (invStatu == null)
            {
                return HttpNotFound();
            }
            return View(invStatu);
        }

        // POST: InvStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvStatu invStatu = db.InvStatus.Find(id);
            db.InvStatus.Remove(invStatu);
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
