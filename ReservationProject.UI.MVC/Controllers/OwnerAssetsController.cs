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
    public class OwnerAssetsController : Controller
    {
        private SkatecampReservationsEntities db = new SkatecampReservationsEntities();

        // GET: OwnerAssets
        public ActionResult Index()
        {
            if (User.IsInRole("User"))
            {
                string currentUser = User.Identity.GetUserId();
                var ownerAssets = db.OwnerAssets.Where(w => w.OwnerId == currentUser).Include(o => o.UserDetail);
                return View(ownerAssets.ToList());
            }
            else
            {
                var ownerAssets = db.OwnerAssets.Include(o => o.UserDetail);
                return View(ownerAssets.ToList());
            }
            
        }

        // GET: OwnerAssets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerAsset ownerAsset = db.OwnerAssets.Find(id);
            if (ownerAsset == null)
            {
                return HttpNotFound();
            }
            return View(ownerAsset);
        }

        // GET: OwnerAssets/Create
        [Authorize(Roles = "Admin, User")]
        public ActionResult Create()
        {
            ViewBag.OwnerId = new SelectList(db.UserDetails, "UserId", "Parent");
            return View();
        }

        // POST: OwnerAssets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OwnerAssetId,ChildName,OwnerId,ChildPhoto,SpecialNotes,IsActive,DateAdded,ChildDOB,SkillLevel")] OwnerAsset ownerAsset, HttpPostedFileBase childImage)
        {
            if (ModelState.IsValid)
            {
                #region FileUpload
                string imageName = "noImage.png";
                if (childImage != null)
                {
                    imageName = childImage.FileName;
                    string ext = imageName.Substring(imageName.LastIndexOf('.'));
                    string[] goodExts = new string[] { ".jpeg", ".jpg", ".png", ".gif" };
                    if (goodExts.Contains(ext.ToLower())) 
                    {
                        imageName = Guid.NewGuid() + ext;
                        childImage.SaveAs(Server.MapPath("~/Content/images/" + imageName));
                    }
                    else
                    {
                        imageName = "noImage.png";
                    }
                }
                
                ownerAsset.ChildPhoto = imageName;
                if (User.IsInRole("User"))
                {
                    ownerAsset.OwnerId = User.Identity.GetUserId();
                }
                

                #endregion
                ownerAsset.IsActive = true;
                ownerAsset.DateAdded = DateTime.Now;
                db.OwnerAssets.Add(ownerAsset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerId = new SelectList(db.UserDetails, "UserId", "Parent", ownerAsset.OwnerId);
            return View(ownerAsset);
        }

        // GET: OwnerAssets/Edit/5
        [Authorize(Roles = "Admin, User, Staff")]       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerAsset ownerAsset = db.OwnerAssets.Find(id);
            if (ownerAsset == null)
            {
                return HttpNotFound();
            }
            string currentUserID = User.Identity.GetUserId();
            ViewBag.OwnerId = new SelectList(db.UserDetails, "UserId", "FirstName", ownerAsset.OwnerId);
            return View(ownerAsset);
        }

        // POST: OwnerAssets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OwnerAssetId,ChildName,OwnerId,ChildPhoto,SpecialNotes,IsActive,DateAdded,ChildDOB,SkillLevel")] OwnerAsset ownerAsset, HttpPostedFileBase childImage)
        {
            if (ModelState.IsValid)
            {
                #region FileUpload
                if (childImage != null)
                {
                    string imageName = childImage.FileName;
                    string ext = imageName.Substring(imageName.LastIndexOf('.'));
                    string[] goodExts = new string[] { ".jpeg", ".jpg", ".png", ".gif" };

                    if (goodExts.Contains(ext.ToLower()))
                    {
                        imageName = Guid.NewGuid() + ext;
                        childImage.SaveAs(Server.MapPath("~/Content/images/" + imageName));
                        string currentFile = Request.Params["ChildPhoto"];
                        if (currentFile != "noImage.png" && currentFile != null)
                        {
                            System.IO.File.Delete(Server.MapPath("~/Content/images/" + currentFile));
                        }
                    }

                    ownerAsset.ChildPhoto = imageName;
                }
                #endregion

                db.Entry(ownerAsset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerId = new SelectList(db.UserDetails, "UserId", "FirstName", ownerAsset.OwnerId);
            return View(ownerAsset);
        }

        // GET: OwnerAssets/Delete/5
        [Authorize(Roles = "Admin, User")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OwnerAsset ownerAsset = db.OwnerAssets.Find(id);
            if (ownerAsset == null)
            {
                return HttpNotFound();
            }
            return View(ownerAsset);
        }

        // POST: OwnerAssets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OwnerAsset ownerAsset = db.OwnerAssets.Find(id);
            db.OwnerAssets.Remove(ownerAsset);
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
