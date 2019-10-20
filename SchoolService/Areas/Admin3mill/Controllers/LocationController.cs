using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchoolService.Models.DataModel;
using SchoolService.Areas.Admin3mill.Models;
using SchoolService.CustomFilters;

namespace SchoolService.Areas.Admin3mill.Controllers
{
    [AuthLog(Roles = "Admin")]
    public class LocationController : Controller
    {
        private SCEntities db = new SCEntities();

        // GET: /Admin3mill/Location///
        [PageTittleAttributeActionFilter(Function = "Location_ListState")]
        public ActionResult ListState()
        {
            ViewBag.jsNotifyMessage = TempData["Notification"];
            return View(db.AddressState.Where(u => u.isDelete == false).ToList());
        }

        // GET: /Admin3mill/Location/Details/5
        [PageTittleAttributeActionFilter(Function = "Location_ListCity")]
        public ActionResult ListCity(int StateId)
        {
     
            ViewBag.StateId = StateId;
            ViewBag.jsNotifyMessage = TempData["Notification"];
            return View(db.AddressCity.Where(u => u.isDelete == false && u.AddressState.isDelete == false && u.F_StateId == StateId));
        }

        // GET: /Admin3mill/Location/Create
        [PageTittleAttributeActionFilter(Function = "Location_AddState")]
        public ActionResult AddState()
        {
            ViewBag.jsNotifyMessage = TempData["Notification"];
            return View();
        }

        // POST: /Admin3mill/Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PageTittleAttributeActionFilter(Function = "Location_AddState")]
        public ActionResult AddState([Bind(Include = "Name")] AddressState addressstate)
        {
            if (string.IsNullOrEmpty(addressstate.Name))
            {
                ModelState.AddModelError("Name", Resource.Resource.View_ValidationError);
            }
            var found = db.AddressState.Where(u => u.isDelete == false && u.Name == addressstate.Name);
            if (found == null)
            {
                ModelState.AddModelError("Name", "نام مورد نظر تکراری است");
            }
            if (ModelState.IsValid)
            {
                addressstate.isDelete = false;
                db.AddressState.Add(addressstate);
                db.SaveChanges();
                TempData["Notification"] = "success";
                return RedirectToAction("ListState");
            }
            TempData["Notification"] = "error";
            return View(addressstate);
        }


        // GET: /Admin3mill/Location/Create
        [PageTittleAttributeActionFilter(Function = "Location_AddCity")]
        public ActionResult AddCity(int StateId)
        {  
            ViewBag.jsNotifyMessage = TempData["Notification"];
            return View();
        }

        // POST: /Admin3mill/Location/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PageTittleAttributeActionFilter(Function = "Location_AddCity")]
        public ActionResult AddCity([Bind(Include = "F_StateId,Name")] AddressCity addresscity, int StateId)
        {
            if (string.IsNullOrEmpty(addresscity.Name))
            {
                ModelState.AddModelError("Name", Resource.Resource.View_ValidationError);
            }
            var found = db.AddressCity.Where(u => u.isDelete == false && u.Name == addresscity.Name);
            if (found == null)
            {
                ModelState.AddModelError("Name", "نام مورد نظر تکراری است");
            }
            if (ModelState.IsValid)
            {
                addresscity.F_StateId = StateId;
                addresscity.isDelete = false;
                db.AddressCity.Add(addresscity);
                db.SaveChanges();
                TempData["Notification"] = "success";
                return RedirectToAction("ListCity", new { stateId = addresscity.F_StateId });
            }
            TempData["Notification"] = "error";
            return View(addresscity);
        }

        // GET: /Admin3mill/Location/Edit/5
        [PageTittleAttributeActionFilter(Function = "Location_EditState")]
        public ActionResult EditState(int? StateId)
        {
            if (StateId == null)
            {
                return View("NotFound");
            }
            AddressState addressstate = db.AddressState.Find(StateId);
            if (addressstate != null && addressstate.isDelete==false)
            {
                return View(addressstate);
            }
            return View("NotFound");

        }

        // POST: /Admin3mill/Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PageTittleAttributeActionFilter(Function = "Location_EditState")]
        public ActionResult EditState([Bind(Include = "Id,Name")] AddressState addressstate)
        {
            if (string.IsNullOrEmpty(addressstate.Name))
            {
                ModelState.AddModelError("Name", Resource.Resource.View_ValidationError);
            }
            addressstate.Name = addressstate.Name.Trim();
            var found = db.AddressState.Where(u => u.isDelete == false && u.Name == addressstate.Name && u.Id!=addressstate.Id).FirstOrDefault();
            if (found != null)
            {
                ModelState.AddModelError("Name", "نام مورد نظر تکراری است");
            }
            if (ModelState.IsValid)
            {
                db.Entry(addressstate).State = EntityState.Modified;
                db.Entry(addressstate).Property(x => x.isDelete).IsModified = false;

                db.SaveChanges();
                TempData["Notification"] = "success";
                return RedirectToAction("ListState");
            }
            TempData["Notification"] = "error";
            return View(addressstate);
        }




        // GET: /Admin3mill/Location/Edit/5
        [PageTittleAttributeActionFilter(Function = "Location_EditCity")]
        public ActionResult EditCity(int CityId, int StateId)
        {
            AddressCity addresscity = db.AddressCity.Find(CityId);
            if (addresscity != null && addresscity.isDelete==false && addresscity.AddressState.isDelete==false)
            {
                return View(addresscity);
            }
            return View("NotFound");
        }

        // POST: /Admin3mill/Location/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [PageTittleAttributeActionFilter(Function = "Location_EditCity")]
        public ActionResult EditCity([Bind(Include = "Id,Name,F_StateId")] AddressCity addresscity, int CityId, int StateId)
        {
            if (string.IsNullOrEmpty(addresscity.Name))
            {
                ModelState.AddModelError("Name", Resource.Resource.View_ValidationError);
            }
            addresscity.Name = addresscity.Name.Trim();

            var found = db.AddressCity.Where(u => u.isDelete == false && u.Name == addresscity.Name && u.Id != addresscity.Id && u.F_StateId==addresscity.F_StateId).FirstOrDefault();
            if (found != null)
            {
                ModelState.AddModelError("Name", "نام مورد نظر تکراری است");
            }
            if (ModelState.IsValid)
            {
                db.Entry(addresscity).State = EntityState.Modified;
                db.Entry(addresscity).Property(x => x.isDelete).IsModified = false;
                db.Entry(addresscity).Property(x => x.F_StateId).IsModified = false;

                db.SaveChanges();
                TempData["Notification"] = "success";
                return RedirectToAction("ListCity", new { StateId = addresscity.F_StateId ?? default(int) });
            }
            TempData["Notification"] = "error";
            return View(addresscity);
        }

        // GET: /Admin3mill/Location/Delete/5
        [HttpPost]
        public ActionResult DeleteState(int? StateId)
        {
            if (StateId == null)
            {
                return View("NotFound");
            }
            AddressState addressstate = db.AddressState.Find(StateId);
            if (addressstate == null || addressstate.isDelete==true)
            {
                return View("NotFound");
            }
            if (addressstate.AddressCity.Count() > 0)
            {
                TempData["Notification"] = "error";
                return RedirectToAction("ListState", "location");
            }
            addressstate.isDelete = true;
            db.SaveChanges();
            TempData["Notification"] = "success";
            return RedirectToAction("ListState", "location");
        }


        [HttpPost]
        public ActionResult DeleteCity(int? CityId,int StateId)
        {
            if (CityId == null)
            {
                return View("NotFound");
            }
            AddressCity addresscity = db.AddressCity.Find(CityId);
            if (addresscity == null || addresscity.isDelete==true)
            {
                return View("NotFound");
            }
            if (addresscity.Madaares.Count() > 0 || addresscity.Nemayandegi.Count() > 0)
            {
                TempData["Notification"] = "error";
                return RedirectToAction("ListCity", "location", new { StateId = addresscity.F_StateId ?? default(int) });
            }
            addresscity.isDelete = true;
            db.SaveChanges();
            TempData["Notification"] = "success";
            return RedirectToAction("ListCity", "location", new { StateId = addresscity.F_StateId ?? default(int) });
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
