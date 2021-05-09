using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using YFM.EntityModels;

namespace YFM.Models
{
    public class MediaItemAddViewModel
    {
        public int ArtistId { get; set; }
        public string Caption { get; set; }

        [Required]
        public HttpPostedFileBase MediaUpload { get; set; }
    }
}