using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FacebookV2.Models
{
    public class FriendRequest
    {
        public string RequesterProfileId { get; set; }
        public string RequestedProfileId { get; set; }
        public DateTime CreatedOn { get; set; }
        public byte Status { get; set; }

        public virtual Profile RequesterProfile { get; set; }
        public virtual Profile RequestedProfile { get; set; }
    }
}