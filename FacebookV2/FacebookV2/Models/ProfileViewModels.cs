using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacebookV2.Models
{
    public class ShowProfileViewModel
    {
        public ShowProfileViewModel()
        {
            Albums = new List<ShowAlbumViewModel>();
        }

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CountyName { get; set; }

        public string CityName { get; set; }

        public string GenderType { get; set; }

        public DateTime? Birthdate { get; set; }

        public long? ProfilePhotoId { get; set; }

        public bool CanSendFriendRequest { get; set; }

        public bool IsPendingForUser { get; set; }

        public bool IsAvailableToView { get; set; }

        public List<ShowAlbumViewModel> Albums { get; set; }
    }

    public class CreateProfileViewModel : IValidatableObject
    {
        public string Id { get; set; }

        [MaxLength(100, ErrorMessage = "Campul trebuie sa aiba maximum 100 de caracatere")]
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string FirstName { get; set; }

        [MaxLength(100, ErrorMessage = "Campul trebuie sa aiba maximum 100 de caracatere")]
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }

        public bool IsMale { get; set; }

        public bool IsPublic { get; set; }

        public int? CountyId { get; set; }

        public int? CityId { get; set; }

        public List<SelectListItem> Counties { get; set; }

        public List<SelectListItem> Cities { get; set; }

        public HttpPostedFileBase ProfilePhoto { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (ProfilePhoto != null)
            {
                var mediaContentType = ProfilePhoto.ContentType.ToLower();

                var isPhoto = mediaContentType == "image/jpg" || mediaContentType == "image/png" || mediaContentType == "image/jpeg";

                if (!isPhoto)
                    result.Add(new ValidationResult("Puteti incarca doar o imagine pentru poza de profil !", new List<string> { nameof(ProfilePhoto) }));
            }

            if (Birthday < DateTime.Parse("1/1/1900") || Birthday > DateTime.Now)
                result.Add(new ValidationResult("Data nasterii este invalida !", new List<string> { nameof(Birthday) }));

            return result;
        }
    }

    public class EditProfileViewModel : IValidatableObject
    {
        public string Id { get; set; }

        [MaxLength(100, ErrorMessage = "Campul trebuie sa aiba maximum 100 de caracatere")]
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string FirstName { get; set; }

        [MaxLength(100, ErrorMessage = "Campul trebuie sa aiba maximum 100 de caracatere")]
        [Required(ErrorMessage = "Campul este obligatoriu!")]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Birthday { get; set; }

        public bool IsMale { get; set; }

        public bool IsPublic { get; set; }

        public int? CountyId { get; set; }

        public int? CityId { get; set; }

        public List<SelectListItem> Counties { get; set; }

        public List<SelectListItem> Cities { get; set; }

        public HttpPostedFileBase ProfilePhoto { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (ProfilePhoto != null)
            {
                var mediaContentType = ProfilePhoto.ContentType.ToLower();

                var isPhoto = mediaContentType == "image/jpg" || mediaContentType == "image/png" || mediaContentType == "image/jpeg";

                if (!isPhoto)
                    result.Add(new ValidationResult("Puteti incarca doar o imagine pentru poza de profil !", new List<string> { nameof(ProfilePhoto) }));
            }

            if (Birthday < DateTime.Parse("1/1/1900") || Birthday > DateTime.Now)
                result.Add(new ValidationResult("Data nasterii este invalida ! Introduceti o data de dupa 01.01.1900!", new List<string> { nameof(Birthday) }));

            return result;
        }
    }
}

