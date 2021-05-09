using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using YFM.EntityModels;

namespace YFM.Models
{
    public class ArtistAddViewModel
    {

        [Required, StringLength(100)]
        [Display(Name = "Artist name or stage name")]
        public string Name { get; set; }

        [StringLength(100)]
        [Display(Name = "If applicable, artist's birth name")]
        public string BirthName { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Birth date, or start date")]
        public DateTime BirthOrStartDate { get; set; }

        [Required, StringLength(200)]
        [Display(Name = "Artist photo")]
        public string UrlArtist { get; set; }


        [Display(Name = "Artists's primary genre")]
        public string Genre { get; set; }

        public int GenreId { get; set; }

        [StringLength(200)]
        public string Executive { get; set; }

        public ICollection<Album> Albums { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Artist portrayal")]
        public string Portrayal { get; set; }
    }
}