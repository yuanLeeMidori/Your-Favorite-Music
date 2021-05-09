using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YFM.Models;

namespace YFM.Controllers
{
    public class TracksController : Controller
    {
        Manager m = new Manager();
        // GET: Tracks
        public ActionResult Index()
        {
            return View(m.GetAllTracks());
        }

        // GET: Tracks/Details/5
        public ActionResult Details(int? id)
        {
            var t = m.GetTrackById(id.GetValueOrDefault());
            if (t == null)
            {
                return null;
            }
            else
            {
                return View(t);
            }
        }

        // GET: Tracks/Edit/5
        [Authorize(Roles = "Clerk")]
        public ActionResult Edit(int? id)
        {
            var o = m.GetTrackById(id.GetValueOrDefault());
            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                var form = m.mapper.Map<TrackEditClipFormViewModel>(o);
                return View(form);
            }
        }

        [Authorize(Roles = "Clerk")]
        // POST: Tracks/Edit/5
        [HttpPost]
        public ActionResult Edit(int? id, TrackEditClipViewModel newItem)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("edit", new { id = newItem.Id });
            }
            if (id.GetValueOrDefault() != newItem.Id)
            {
                return RedirectToAction("index");
            }
            var editedItem = m.EditTrack(newItem);
            if (editedItem == null)
            {
                return RedirectToAction("edit", new { id = newItem.Id });
            }
            else
            {
                return RedirectToAction("details", new { id = newItem.Id });
            }
        }

        // GET: Tracks/Delete/5
        public ActionResult Delete(int? id)
        {
            var itemToDelete = m.GetTrackById(id.GetValueOrDefault());
            if (itemToDelete == null)
            {
                return Redirect("~/track/index");
            }
            else
            {
                return View(itemToDelete);
            }
        }

        // POST: Tracks/Delete/5
        [HttpPost]
        public ActionResult Delete(int? id, FormCollection collection)
        {
            var wasDeleted = m.TrackDelete(id.GetValueOrDefault());
            return Redirect("~/tracks/index");
        }
    }
}
