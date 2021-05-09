using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using YFM.EntityModels;

namespace YFM.Models
{
    public class AlbumAddViewModel
    {

        [Required, StringLength(100)]
        [Display(Name = "Album name")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Release date")]
        public DateTime ReleaseDate { get; set; }

        [Required, StringLength(200)]
        [Display(Name = "Album cover art")]
        public string UrlAlbum { get; set; }

        [Display(Name = "Album's primary genre")]
        public string Genre { get; set; }
        public int GenreId { get; set; }

        [StringLength(200)]
        public string Coordinator { get; set; }

        public int ArtistId { get; set; }
        public string ArtistName { get; set; }

        public ICollection<ArtistAddViewModel> Artists { get; set; }

        public ICollection<TrackAddViewModel> Tracks { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Album depiction")]
        public string Depiction { get; set; }
    }
}