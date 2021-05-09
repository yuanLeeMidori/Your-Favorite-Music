using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace YFM.Models
{
    public class TrackEditClipFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        [Display(Name = "Sample clip")]
        public string ClipUpload { get; set; }
    }
}