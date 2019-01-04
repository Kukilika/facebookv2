using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FacebookV2.Models
{
    public class Album
    {
        public long Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string ProfileId { get; set; }

        public virtual Profile Profile { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}