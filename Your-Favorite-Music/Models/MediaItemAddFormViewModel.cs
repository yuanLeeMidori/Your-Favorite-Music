using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YFM.Models
{
    public class MediaItemAddFormViewModel
    {
        public int ArtistId { get; set; }
        public string ArtistName { get; set; }
        public string Caption { get; set; }

        [Required]
        [Display(Name = "Media item")]
        [DataType(DataType.Upload)]
        public string MediaUpload { get; set; }
    }
}