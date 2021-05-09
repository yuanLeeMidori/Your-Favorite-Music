using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YFM.EntityModels;

namespace YFM.Models
{
    public class MediaItemBaseViewModel
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public string ContentType { get; set; }
        public string StringId { get; set; }
        public DateTime Timestamp { get; set; }

    }
}