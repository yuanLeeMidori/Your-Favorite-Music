using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YFM.Models
{
    public class ArtistWithMediaItemViewModel : ArtistBaseViewModel
    {
        public ArtistWithMediaItemViewModel()
        {
            MediaItems = new List<MediaItemBaseViewModel>();
        }
        public IEnumerable<MediaItemBaseViewModel> MediaItems { get; set; }
    }
}