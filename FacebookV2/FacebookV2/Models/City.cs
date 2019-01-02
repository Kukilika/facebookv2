using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FacebookV2.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }
        public int CountyId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }

        public virtual County County { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}