using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FacebookV2.Models
{
    public class Photo
    {
        public long Id { get; set; }
        public long? AlbumId { get; set; }
        [StringLength(300)]
        public string Caption { get; set; }
        [Required]
        public byte[] Content { get; set; }
        [Required]
        public string ProfileId { get; set; }

        public virtual Profile Profile { get; set; }
        public virtual Album Album { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}