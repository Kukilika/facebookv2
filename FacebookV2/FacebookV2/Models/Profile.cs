using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FacebookV2.Models
{
    public class Profile
    {
        [Key,ForeignKey("User")]
        public string Id { get; set; } //incercam sa fie atat PK, cat si FK in tabela aceasta
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(100)]
        public string LastName { get; set; }
        [Required]
        public bool IsMale { get; set; }
        [Required]
        public bool IsPublic { get; set; }
        [Required]
        public bool IsDeletedByAdmin { get; set; }
        public long? ProfilePhotoId { get; set; }
        public DateTime? Birthday { get; set; }
        public int? CityId { get; set; }
        public int? CountyId { get; set; }
        

        public virtual ApplicationUser User { get; set; }
        public virtual City City { get; set; }
        public virtual County County { get; set; }
        public virtual ICollection<Friend> FriendsSenderProfile { get; set; }
        public virtual ICollection<Friend> FriendsReceiverProfile { get; set; }
        public virtual ICollection<FriendRequest> FriendRequestsRequestedProfile { get; set; }
        public virtual ICollection<FriendRequest> FriendRequestsRequesterProfile { get; set; }
        public virtual ICollection<Album> Albums { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
    }
    /*
    public class ProfileDBContext : DbContext
    {
        public ProfileDBContext() : base("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=aspnet-FacebookV2-20181226054138;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False") { }
        public DbSet<Profile> Profiles { get; set; }
    }*/

}