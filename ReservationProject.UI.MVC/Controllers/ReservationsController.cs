using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReservationProject.DATA.EF;
using Microsoft.AspNet.Identity;

namespace ReservationProject.UI.MVC.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private SkatecampReservationsEntities db = new SkatecampReservationsEntities();

        // GET: Reservations
        public ActionResult Index()
        {
            if (User.IsInRole("User"))
            {
                string currentUser = User.Identity.GetUserId();
                var reservations = db.Reservations.Where(w => w.OwnerAsset.OwnerId == currentUser).Include(r => r.Location).Include(r => r.OwnerAsset);
                return View(reservations.ToList());
            }
            else
            {
                var reservations = db.Reservations.Include(r => r.Location).Include(r => r.OwnerAsset);
                return View(reservations.ToList());
            }



        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        [Authorize(Roles = "Admin, User")]
        public ActionResult Create()
        {
            if (User.IsInRole("User"))
            {
                string currentUserID = User.Identity.GetUserId();
                ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName");
                ViewBag.OwnerAssetId = new SelectList(db.OwnerAssets.Where(oa => oa.UserDetail.UserId == currentUserID), "OwnerAssetId", "ChildName");
            }
            else
            {
                ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName");
                ViewBag.OwnerAssetId = new SelectList(db.OwnerAssets, "OwnerAssetId", "ChildName");
            }
            
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReservationId,OwnerAssetId,LocationId,ReservationDate")] Reservation reservation)
        {
            if (User.IsInRole("User"))
            {
                string currentUserID = User.Identity.GetUserId();
                ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", reservation.LocationId);
                ViewBag.OwnerAssetId = new SelectList(db.OwnerAssets.Where(oa => oa.UserDetail.UserId == currentUserID), "OwnerAssetId", "ChildName", reservation.OwnerAssetId);
            }
            else
            {
                ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", reservation.LocationId);
                ViewBag.OwnerAssetId = new SelectList(db.OwnerAssets, "OwnerAssetId", "ChildName", reservation.OwnerAssetId);
            }

            if (ModelState.IsValid)
            {
                int reservNbr = db.Reservations.Where(x => x.ReservationDate == reservation.ReservationDate &&
                    x.LocationId == reservation.LocationId).Count();
                Location locationReserv = db.Locations.Find(reservation.LocationId);
                if (Convert.ToInt32(locationReserv.ReservationLimit) > reservNbr)
                {
                    db.Reservations.Add(reservation);
                    db.SaveChanges();
                    
                }
                else
                {
                    if (Convert.ToInt32(locationReserv.ReservationLimit) <= reservNbr && User.IsInRole("Admin"))
                    {
                        db.Reservations.Add(reservation);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.Message = "There are too many participants on that date. Try a different date.";
                    return View(reservation);
                }
                return RedirectToAction("Index");

                //db.Reservations.Add(reservation);
                //db.SaveChanges();
                //return RedirectToAction("Index");

            }

            
            
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        [Authorize(Roles = "Admin, User")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", reservation.LocationId);
            ViewBag.OwnerAssetId = new SelectList(db.OwnerAssets, "OwnerAssetId", "ChildName", reservation.OwnerAssetId);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ReservationId,OwnerAssetId,LocationId,ReservationDate")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "LocationName", reservation.LocationId);
            ViewBag.OwnerAssetId = new SelectList(db.OwnerAssets, "OwnerAssetId", "ChildName", reservation.OwnerAssetId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "Admin, User")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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
