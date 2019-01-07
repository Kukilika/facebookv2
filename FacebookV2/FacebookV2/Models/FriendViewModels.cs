using System;
using System.Collections.Generic;

namespace FacebookV2.Models
{
    public class FriendsDetailsViewModel
    {
        public List<FriendRequestViewModel> FriendRequests { get; set; }

        public List<FriendViewModel> Friends { get; set; }
    }

    public class FriendRequestViewModel
    {
        public string RequesterId { get; set; }

        public string RequesterFirstName { get; set; }

        public string RequesterLastName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string DaysSinceRequested => (DateTime.Now.Subtract(CreatedOn).Days) > 0
                ? $"{ (DateTime.Now.Subtract(CreatedOn).Days)} days ago"
                : "Today";

        public long? RequesterProfilePictureId { get; set; }
    }

    public class FriendViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreatedDate { get; set; }

        public long? ProfilePhotoId { get; set; }
    }

    public class FriendResult
    {
        public Profile UserProfile { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}