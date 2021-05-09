using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace YFM.Models
{
    public class TrackWithAudioDetailsViewModel : TrackAddViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Sample Clip")]
        public string ClipUrl
        {
            get
            {
                return $"/clip/{Id}";
            }
        }
    }
}