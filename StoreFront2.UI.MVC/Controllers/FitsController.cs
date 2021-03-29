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
    public class FitsController : Controller
    {
        private StoreFrontEntities db = new StoreFrontEntities();

        // GET: Fits
        [AllowAnonymous]//ANYONE CAN SEE DETAILS
        public ActionResult Index()
        {
            return View(db.Fits.ToList());
        }

        // GET: Fits/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fit fit = db.Fits.Find(id);
            if (fit == null)
            {
                return HttpNotFound();
            }
            return View(fit);
        }

        // GET: Fits/Create
        [Authorize(Roles = "Admin")]//ADMIN ONLY
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]//ADMIN ONLY
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FitID,FitName")] Fit fit)
        {
            if (ModelState.IsValid)
            {
                db.Fits.Add(fit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fit);
        }

        // GET: Fits/Edit/5
        [Authorize(Roles = "Admin, Sales")]//ADMIN and SALES ONLY
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fit fit = db.Fits.Find(id);
            if (fit == null)
            {
                return HttpNotFound();
            }
            return View(fit);
        }

        // POST: Fits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Sales")]//ADMIN and SALES only
        public ActionResult Edit([Bind(Include = "FitID,FitName")] Fit fit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fit);
        }

        // GET: Fits/Delete/5
        [Authorize(Roles = "Admin")]//ADMIN ONLY
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Fit fit = db.Fits.Find(id);
            if (fit == null)
            {
                return HttpNotFound();
            }
            return View(fit);
        }

        // POST: Fits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]//ADMIN ONLY
        public ActionResult DeleteConfirmed(int id)
        {
            Fit fit = db.Fits.Find(id);
            db.Fits.Remove(fit);
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
