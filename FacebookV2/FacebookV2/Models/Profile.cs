using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FacebookV2.Models
{
    public class Profile
    {
        [Key]
        public long Id { get; set; }
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [Required]
        public bool IsMale { get; set; }
        [Required]
        public bool IsPublic { get; set; }
        public DateTime? Birthday { get; set; }
        public int? CityId { get; set; }
        public int? CountyId { get; set; }
        public long? ProfilePhotoId { get; set; }
        public bool IsDeletedByAdmin { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual City City { get; set; }
        public virtual County County { get; set; }
    }
}