using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YFM.Models
{
    public class TrackEditClipViewModel
    {
        public int Id { get; set; }
        public HttpPostedFileBase ClipUpload { get; set; }
    }
}