using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using YFM.EntityModels;

namespace YFM.Models
{
    public class TrackAddViewModel
    {
        [Required, StringLength(200)]
        [Display(Name = "Track name")]
        public string Name { get; set; }

        [Display(Name = "Composer names (comma-seperated)")]
        [Required, StringLength(500)]
        public string Composers { get; set; }

        public int GenreId { get; set; }
        
        public string Genre { get; set; }

        public int AlbumId { get; set; }

        [Display(Name = "Album name")]
        public string AlbumName { get; set; }

        [StringLength(200)]
        public string Clerk { get; set; }

        public ICollection<AlbumAddViewModel> Albums { get; set; }

        public HttpPostedFileBase ClipUpload { get; set; }

    }
}