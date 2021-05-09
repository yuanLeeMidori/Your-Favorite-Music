using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YFM.Models
{
    public class ArtistAddFormViewModel : ArtistAddViewModel
    {
        public SelectList GenreList { get; set; }
        public MultiSelectList AlbumList { get; set; }
    }
}