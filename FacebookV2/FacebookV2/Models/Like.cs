using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FacebookV2.Models
{
    public class Like
    {
        public long Id { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        public long PhotoId { get; set; }
        [Required]
        public string ProfileId { get; set; }

        public virtual Photo Photo { get; set; }
        public virtual Profile Profile { get; set; }
    }
}