using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YFM.Models;

namespace YFM.Controllers
{
    public class ArtistsController : Controller
    {
        Manager m = new Manager();
        // GET: Artists
        public ActionResult Index()
        {
            return View(m.GetAllArtists());
        }

        // GET: Artists/Details/5
        public ActionResult Details(int? id)
        {
            var o = m.GetArtistWithMediaItemById(id.GetValueOrDefault());
            if (o == null)
            {
                return HttpNotFound();
            } else
            {
                return View(o);
            }
        }

        // GET: Artists/Create
        public ActionResult Create()
        {
            var form = new ArtistAddFormViewModel();
            form.GenreList = new SelectList
                (items: m.GetAllGenres(),
                 dataValueField: "Id",
                 dataTextField: "Name");

            return View(form);
        }

        // POST: Artists/Create
        [Authorize(Roles = "Executive")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(ArtistAddViewModel newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }
            var addedItem = m.AddNewArtist(newItem);
            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("details", new { id = addedItem.Id });
            }
        }

        [Authorize(Roles = "Coordinator")]
        [Route("artists/{id}/addalbum")]
        public ActionResult AddAlbum(int? id)
        {
            var artist = m.GetArtistById(id.GetValueOrDefault());
            if (artist == null)
            {
                return HttpNotFound();
            }
            else
            {
                var form = new AlbumAddFormViewModel();
                form.ArtistId = id.GetValueOrDefault();
                form.ArtistName = artist.Name;
                form.GenreList = new SelectList(
                    items: m.GetAllGenres(),
                    dataValueField: "Id",
                    dataTextField: "Name");
                return View(form);
            }
        }

        [Authorize(Roles = "Coordinator")]
        [Route("artists/{id}/addalbum")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddAlbum(AlbumAddViewModel newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }
            var addedItem = m.AddNewAlbum(newItem);
            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("details", "albums", new { id = addedItem.Id });
            }

        }

        [Authorize(Roles = "Executive")]
        [Route("Artists/{id}/addmedia")]
        public ActionResult AddMedia(int? id)
        {
            var o = m.GetArtistById(id.GetValueOrDefault());
            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                var form = new MediaItemAddFormViewModel();
                form.ArtistId = o.Id;
                form.ArtistName = o.Name;
                return View(form);
            }
        }

        [Authorize(Roles = "Executive")]
        [Route("Artists/{id}/addmedia")]
        [HttpPost]
        public ActionResult AddMedia(int? id, MediaItemAddViewModel newItem)
        {
            if (!ModelState.IsValid)
            {
                return View(newItem);
            }
            var addedItem = m.AddMediaItemToArtist(newItem);
            if (addedItem == null)
            {
                return View(newItem);
            }
            else
            {
                return RedirectToAction("details", new { id = addedItem.Id });
            }
        }
    }
}
