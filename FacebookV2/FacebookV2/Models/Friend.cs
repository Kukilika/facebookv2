using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FacebookV2.Models
{
    public class Friend
    {
        [Key]
        [Column(Order = 1)]
        public string SenderId { get; set; }
        [Key]
        [Column(Order = 2)]
        public string ReceiverId { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }

        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
    }
}