using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace YFM.Models
{
    public class TrackAddFormViewModel
    {
        [Required, StringLength(200)]
        [Display(Name = "Track name")]
        public string Name { get; set; }

        // Simple comma-separated string of all the track's composers
        [Required, StringLength(500)]
        [Display(Name = "Composer names (comma-seperated")]
        public string Composers { get; set; }

        public int GenreId { get; set; }

        public string Genre { get; set; }

        public int AlbumId { get; set; }
        public string AlbumName { get; set; }

        // User name who added/edited the track
        [StringLength(200)]
        public string Clerk { get; set; }

        public ICollection<AlbumAddViewModel> Albums { get; set; }
        [Display(Name = "Track Genre")]
        public SelectList GenreList { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Sample clip")]
        public string ClipUpload { get; set; }
    }
}