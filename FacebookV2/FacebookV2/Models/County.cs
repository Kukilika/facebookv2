using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FacebookV2.Models
{
    public class County
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(2)]
        public string ShortName { get; set; }

        public virtual ICollection<Profile> Profiles { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}