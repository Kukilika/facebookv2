using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FacebookV2.Models
{
    public class County
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(2)]
        public string ShortName { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<City> Cities { get; set; }
    }
}