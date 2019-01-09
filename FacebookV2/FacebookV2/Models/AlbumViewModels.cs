using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FacebookV2.Models
{
    public class ShowAlbumViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int NumberOfPhotos { get; set; }

        public long? FirstPhotoId { get; set; }

        public bool IsEditable { get; set; }
    }

    public class AddAlbumViewModel
    {
        public long Id { get; set; }

        [MaxLength(100, ErrorMessage = "Campul trebuie sa aiba maximum 100 de caracatere")]
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string Name { get; set; }
    }

    public class EditPhotosViewModel
    {
        public AddAlbumViewModel AlbumToAdd { get; set; }

        public List<ShowAlbumViewModel> Albums { get; set; }
    }

    public class PhotoViewModel
    {
        public long Id { get; set; }

        public string Caption { get; set; }

        public bool IsDeleted { get; set; }
    }

    public class PhotoWithDetailsViewModel
    {
        public PhotoWithDetailsViewModel()
        {
            Comments = new List<CommentViewModel>();
        }

        public long Id { get; set; }
        public long? AlbumId { get; set; }
        public string Caption { get; set; }
        public string ProfileId { get; set; }

        public int LikesNumber { get; set; }
        public int TotalCommentsNumber { get; set; }
        public int NumberOfShowedComments => Comments.Count;
        public bool IsLikedByCurrentUser { get; set; }

        public List<CommentViewModel> Comments { get; set; }
    }

    public class ShowDetailedAlbumViewModel
    {
        public List<PhotoWithDetailsViewModel> Photos { get; set; }
        public long Id { get; set; }
        public int NumberOfPages { get; set; }
        public int CurrentPage { get; set; }
        public int MaxButtons { get; set; }
    }

    public class EditAlbumViewModel : IValidatableObject
    {
        public EditAlbumViewModel()
        {
            Photos = new List<PhotoViewModel>();
        }

        public long Id { get; set; }

        [MaxLength(100, ErrorMessage = "Campul trebuie sa aiba maximum 100 de caracatere")]
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string Name { get; set; }

        public HttpPostedFileBase NewPhoto { get; set; }

        public List<PhotoViewModel> Photos { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (NewPhoto != null)
            {
                var mediaContentType = NewPhoto.ContentType.ToLower();

                var isPhoto = mediaContentType == "image/jpg" || mediaContentType == "image/png" || mediaContentType == "image/jpeg";

                if (!isPhoto)
                    result.Add(new ValidationResult("Puteti incarca doar poze in albume ! !", new List<string> { nameof(NewPhoto) }));
            }

            return result;
        }
    }
}