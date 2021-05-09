using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YFM.Models
{
    public class AlbumAddFormViewModel : AlbumAddViewModel
    {
        public SelectList GenreList { get; set; }
        public MultiSelectList TrackList { get; set; }
        public MultiSelectList ArtistList { get; set; }
    }
}