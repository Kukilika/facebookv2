using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FacebookV2.Models
{
    public class Message
    {
        [Key]
        [Column(Order = 1)]
        public string ProfileId { get; set; }
        [Key]
        [Column(Order = 2)]
        public long GroupId { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }

        public virtual Group Group { get; set; }
        public virtual Profile Profile { get; set; }
    }
}