using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace YFM.EntityModels
{
    public class MediaItem
    {
        public MediaItem()
        {
            Timestamp = DateTime.Now;
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            StringId = string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Caption { get; set; }
        public byte[] Content { get; set; }
        [StringLength(200)]
        public string ContentType { get; set; }
        [Required, StringLength(100)]
        public string StringId { get; set; }
        public DateTime Timestamp { get; set; }
        [Required]
        public Artist Artist { get; set; }

    }
}