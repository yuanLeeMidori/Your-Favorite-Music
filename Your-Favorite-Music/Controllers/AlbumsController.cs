using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YFM.Models;

namespace YFM.Controllers
{
    public class AlbumsController : Controller
    {
        Manager m = new Manager();
        // GET: Albums
        public ActionResult Index()
        {
            return View(m.GetAllAlbums());
        }

        // GET: Albums/Details/5
        public ActionResult Details(int id)
        {
            var a = m.GetAlbumById(id);
            if (a == null)
            {
                return HttpNotFound();
            } else
            {
                return View(a);
            }
        }

        [Authorize(Roles = "Clerk")]
        [Route("albums/{id}/addtrack")]
        public ActionResult AddTrack(int? id)
        {
            var album = m.GetAlbumById(id.GetValueOrDefault());
            if (album == null)
            {
                return HttpNotFound();
            }
            else
            {
                var form = new TrackAddFormViewModel();
                form.AlbumId = id.GetValueOrDefault();
                form.AlbumName = album.Name;
                form.GenreList = new SelectList(
                    items: m.GetAllGenres(),
                    dataValueField: "Id",
                    dataTextField: "Name");
                return View(form);
            }

        }

        [Authorize(Roles = "Clerk")]
        [Route("albums/{id}/addtrack")]
        [HttpPost]
        public ActionResult AddTrack(TrackAddViewModel newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }
            var addedItem = m.AddNewTrack(newItem);
            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("details", "tracks", new { id = addedItem.Id });
            }
        }

        // GET: Albums/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Albums/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Albums/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Albums/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
