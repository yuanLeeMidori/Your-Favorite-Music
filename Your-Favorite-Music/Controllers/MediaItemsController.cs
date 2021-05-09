using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YFM.Controllers
{
    public class MediaItemsController : Controller
    {
        Manager m = new Manager();
        // GET: MediaItems
        public ActionResult Index()
        {
            return View();
        }

        [Route("mediaItem/{stringId}")]
        public ActionResult Details(string stringId = "")
        {
            var o = m.GetArtistMediaItemById(stringId);
            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                return File(o.Content, o.ContentType);
            }
        }

        [Route("mediaItem/{stringId}/download")]
        public ActionResult DetailsDownload(string stringId = "")
        {
            var o = m.GetArtistMediaItemById(stringId);
            if (o == null)
            {
                return HttpNotFound();
            }
            else
            {
                string extension;
                RegistryKey key;
                object value;

                key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + o.ContentType, false);
                value = (key == null) ? null : key.GetValue("Extension", null);
                extension = (value == null) ? string.Empty : value.ToString();
                var cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = $"{stringId}{extension}",
                    Inline = false
                };
                Response.AppendHeader("Content-Disposition", cd.ToString());
                return File(o.Content, o.ContentType);
            }
        }

        // GET: MediaItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MediaItems/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MediaItems/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MediaItems/Edit/5
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

        // GET: MediaItems/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MediaItems/Delete/5
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
