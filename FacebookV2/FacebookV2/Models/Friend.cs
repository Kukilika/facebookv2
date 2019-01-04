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
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Profile Sender { get; set; }
        public virtual Profile Receiver { get; set; }
    }
}