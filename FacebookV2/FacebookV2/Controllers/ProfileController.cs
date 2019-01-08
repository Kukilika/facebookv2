using FacebookV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using FacebookV2.Enums;
using Microsoft.AspNet.Identity;
using FacebookV2.App_Start;
using System.IO;

namespace FacebookV2.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        [HttpGet]
        public ActionResult ShowMyProfile()
        {
            var currentUserId = User.Identity.GetUserId();

            return RedirectToAction("Show", "Profile", new { userId = currentUserId });

        }

        [HttpGet]
        public ActionResult Show(string userId)
        {
            var user = db.Profiles
                    .Include(p => p.County)
                    .Include(p => p.City)
                    .Include(p => p.Albums)
                    .Where(p => p.Id == userId && !p.IsDeletedByAdmin)
                    .FirstOrDefault();

            if (user == null)
            {
                return View("NotFound");
            }

            var model = new ShowProfileViewModel();

            var currentUserId = User.Identity.GetUserId();
            //verificam daca user-ul curent(autentificat) este prieten cu cel al carui profil incercam sa-l accesam
            var isFriendWith = db.Friends.Any(f => f.ReceiverId == currentUserId && f.SenderId == userId
                                                || f.ReceiverId == userId && f.SenderId == currentUserId);

            if (!user.IsPublic   //daca user-ul al carui profil dorim sa-l accesam este privat
                    && !isFriendWith   //si nu suntem prieteni deja
                    && currentUserId != user.Id)  //si cazul in care nu cumva ne uitam chiar pe profilul nostru
            {                                   //atunci vom afisa doar un set limitate de date despre acel utilizator
                model.Id = user.Id;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.ProfilePhotoId = user.ProfilePhotoId;
            }
            else
            {
                model.Id = user.Id;
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.ProfilePhotoId = user.ProfilePhotoId;
                model.CountyName = user.County != null ? user.County.Name : "Not available";
                model.CityName = user.City != null ? user.City.Name : "Not available";
                model.GenderType = user.IsMale ? "Male" : "Female";
                model.Birthdate = user.Birthday;
                model.Albums = user.Albums.Select(album => new ShowAlbumViewModel
                                                        {
                                                            Id = album.Id,
                                                            Name = album.Name,
                                                            NumberOfPhotos = album.Photos.Count,
                                                            FirstPhotoId = album.Photos.FirstOrDefault()?.Id,
                                                            IsEditable = false
                                                        })
                                                        .ToList();

                model.IsAvailableToView = true;

                /*foreach (var album in model.Albums)
                {
                    album.FirstPhotoId = db.Photos.Where(u => u.AlbumId == album.Id)
                                                  .Select(u => u.Id)
                                                  .FirstOrDefault();
                    album.NumberOfPhotos = db.Photos.Where(u => u.AlbumId == album.Id)
                                                    .Count();
                }*/
            }

            var isPendingForUser = db.FriendRequests
                     .Any(u => u.RequestedProfileId == userId && u.RequesterProfileId == currentUserId && u.Status == (int)FriendRequestStatusTypes.Pending
                               || u.RequestedProfileId == currentUserId && u.RequesterProfileId == userId && u.Status == (int)FriendRequestStatusTypes.Pending);

            var isRejectedByUser = db.FriendRequests
                        .Any(u => u.RequestedProfileId == userId && u.RequesterProfileId == currentUserId && u.Status == (int)FriendRequestStatusTypes.Rejected);

            model.CanSendFriendRequest = currentUserId != userId
                                    && !isFriendWith
                                    && !isPendingForUser //exista o cerere de prietenie intre noi in asteptare (de a fi acceptata sau nu)
                                    && !isRejectedByUser; //daca persoana pe care vrem sa o vedem ne-a refuzat cererea de prietenie, nu vom mai putem trimite alta
            model.IsPendingForUser = isPendingForUser;

            return View(model);
        }

        [SkipMyGlobalActionFilter]
        [HttpGet]
        public ActionResult Create()
        {
            var currentUserId = User.Identity.GetUserId();

            var model = new CreateProfileViewModel
            {
                Id = currentUserId,
                Counties = GetAllCounties().ToList()
            };

            return View(model);
        }

        [SkipMyGlobalActionFilter]
        [HttpPost]
        public ActionResult Create(CreateProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Counties = GetAllCounties().ToList();

                if (model.CityId != null)
                {
                    model.CityId = null;
                }

                return View(model);
            }

            var newProfile = new Profile()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                IsMale = model.IsMale,
                IsPublic = model.IsPublic,
                Birthday = model.Birthday,
                IsDeletedByAdmin = false,
                CountyId = model.CountyId,
                CityId = model.CityId
            };

            db.Profiles.Add(newProfile);
            db.SaveChanges();

            if (model.ProfilePhoto != null)
            {
                MemoryStream target = new MemoryStream();
                model.ProfilePhoto.InputStream.CopyTo(target);
                byte[] data = target.ToArray();

                var newPhotoToAdd = new Photo()
                {
                    Content = data,
                    ProfileId = model.Id
                };

                db.Photos.Add(newPhotoToAdd);
                db.SaveChanges();

                if (TryUpdateModel(newProfile))
                {
                    newProfile.ProfilePhotoId = newPhotoToAdd.Id;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Show", "Profile", new { userId = newProfile.Id });
        }


        [HttpGet]
        public ActionResult Edit()
        {
            var currentUserId = User.Identity.GetUserId();

            var user = db.Profiles
                    .Include(p => p.County)
                    .Include(p => p.City)
                    .Include(p => p.Albums)
                    .Where(p => p.Id == currentUserId && !p.IsDeletedByAdmin)
                    .FirstOrDefault();

            if (user == null)
                return View("NotFound");

            var model = new EditProfileViewModel()
            {
                Id = currentUserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Birthday = user.Birthday,
                IsMale = user.IsMale,
                IsPublic = user.IsPublic,
                CountyId = user.CountyId,
                CityId = user.CityId
            };

            model.Counties = GetAllCounties().ToList();
            if (user.CountyId != null)
                model.Cities = GetAllCitiesFromCounty(user.CountyId).ToList();

            return View(model);
        }

        [HttpPut]
        public ActionResult Edit(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Counties = GetAllCounties().ToList();

                if (model.CityId != null)
                {
                    model.CityId = null;
                }

                return View(model);
            }

            var profileToEdit = db.Profiles.Find(model.Id);
            if (model.Birthday == null)
                model.Birthday = profileToEdit.Birthday;

            if (model.ProfilePhoto != null)
            {
                MemoryStream target = new MemoryStream();
                model.ProfilePhoto.InputStream.CopyTo(target);
                byte[] data = target.ToArray();

                var newPhotoToAdd = new Photo()
                {
                    Content = data,
                    ProfileId = model.Id
                };

                db.Photos.Add(newPhotoToAdd);
                db.SaveChanges();

                if(TryUpdateModel(profileToEdit))
                {
                    profileToEdit.ProfilePhotoId = newPhotoToAdd.Id;
                    db.SaveChanges();
                }
            }

            if (TryUpdateModel(profileToEdit))
            {
                profileToEdit.FirstName = model.FirstName;
                profileToEdit.LastName = model.LastName;
                profileToEdit.IsMale = model.IsMale;
                profileToEdit.IsPublic = model.IsPublic;
                profileToEdit.Birthday = model.Birthday;
                profileToEdit.CountyId = model.CountyId;
                profileToEdit.CityId = model.CityId;
                
                db.SaveChanges();
            }

            return RedirectToAction("Show", "Profile", new { userId = profileToEdit.Id });
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCounties()
        {
            var counties = db.Counties.ToList();

            return counties.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCitiesFromCounty(int? countyId)
        {
            var cities = db.Cities
                            .Where(c => c.CountyId == countyId)
                            .ToList();

            return cities.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });
        }
    }
}