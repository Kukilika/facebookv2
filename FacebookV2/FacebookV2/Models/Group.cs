using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FacebookV2.Models
{
    public class Group
    {
        public long Id { get; set; }
        [Required]
        public string AdminId { get; set; }
        [StringLength(150)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }

        public virtual Profile Admin { get; set; }
        public virtual ICollection<Profile> Profiles { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
}