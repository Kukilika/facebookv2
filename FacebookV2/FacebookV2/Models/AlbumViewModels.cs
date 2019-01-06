using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookV2.Models
{
    public class ShowAlbumViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int NumberOfPhotos { get; set; }

        public long? FirstPhotoId { get; set; }

        public bool IsEditable { get; set; }
    }
}