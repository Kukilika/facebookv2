using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FacebookV2.Models
{
    public class CommentViewModel
    {
        public long Id { get; set; }
        public long PhotoId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        //public byte Status { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileId { get; set; }
        public long? ProfilePhotoId { get; set; }
        public string ProfilePhotoUrl => ProfilePhotoId.HasValue ? $"/Photo/GetProfilePicture/{ProfilePhotoId}" : "~/images/default_profilePicture.jpg";
        public bool IsEditable { get; set; }
    }

    public class AddCommentViewModel
    {
        public long PhotoId { get; set; }

        public string Body { get; set; }
    }
}