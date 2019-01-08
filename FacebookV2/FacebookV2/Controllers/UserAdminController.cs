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
    [Authorize(Roles = "Administrator")]
    public class UserAdminController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        public ActionResult Show(string userId)
        {
            var user = db.Profiles
                    .Include(p => p.County)
                    .Include(p => p.City)
                    .Include(p => p.Albums)
                    .Where(p => p.Id == userId)
                    .FirstOrDefault();
            if (user == null)
            {
                return View("NotFound");
            }

            var model = new ShowProfileViewModel();

            model.Id = user.Id;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            model.ProfilePhotoId = user.ProfilePhotoId;
            model.CountyName = user.County != null ? user.County.Name : "Not available";
            model.CityName = user.City != null ? user.City.Name : "Not available";
            model.GenderType = user.IsMale ? "Male" : "Female";
            model.Birthdate = user.Birthday;
            model.IsAvailableToView = true;

            foreach (var album in model.Albums)
            {
                album.FirstPhotoId = db.Photos.Where(u => u.AlbumId == album.Id)
                                              .Select(u => u.Id)
                                              .FirstOrDefault();
                album.NumberOfPhotos = db.Photos.Where(u => u.AlbumId == album.Id)
                                                .Count();
            }

            return View("~/Views/Profile/Show.cshtml", model);

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

        // GET: UserAdmin
        [HttpGet]
        public ActionResult Index()
        {
            
            var profiles = from profile in db.Profiles
                                 orderby profile.FirstName
                                 select profile;
            ViewBag.Profiles = profiles;
            return View();
        }

        /*public ActionResult New()
        {
            return View("~/Views/Profile/Create.cshtml");
        }*/

        /*//Post
        [HttpPost]
        public ActionResult New(Profile profile)
        {
            try
            {
                db.Profiles.Add(profile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View("ErrorView");
            }
        }*/

        /*Dintrun anumit motiv Delete merge doar cu get*/
        [HttpGet]
        public ActionResult Undelete(String id)
        {
            try
            {
                Profile profile = db.Profiles.Find(id);
                if(profile.IsDeletedByAdmin == false)
                {
                    return View("AlreadyStatus");
                }
                profile.IsDeletedByAdmin = false;
                //db.Profiles.Remove(profile);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return View("ErrorView");
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Delete(String id)
        {
            try
            {
                Profile profile = db.Profiles.Find(id);
                if (profile.IsDeletedByAdmin == true)
                {
                    return View("AlreadyStatus");
                }
                profile.IsDeletedByAdmin = true;
                //db.Profiles.Remove(profile);
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return View("ErrorView");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(String id)
        {
            var user = db.Profiles
                    .Include(p => p.County)
                    .Include(p => p.City)
                    .Include(p => p.Albums)
                    .Where(p => p.Id == id)
                    .FirstOrDefault();

            if (user == null)
                return View("NotFound");

            var model = new EditProfileViewModel()
            {
                Id = id,
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

            return View("~/Views/Profile/Edit.cshtml", model);
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

                if (TryUpdateModel(profileToEdit))
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

        /*[HttpPut]
        public ActionResult Edit(int id, Profile requestProfile)
        {
            try
            {
                Profile profile = db.Profiles.Find(id);
                if (TryUpdateModel(profile))
                {
                    profile.FirstName = requestProfile.FirstName;
                    profile.LastName = requestProfile.LastName;
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View("ErrorView");
            }
        }*/
    }
}