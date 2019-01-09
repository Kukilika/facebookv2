using FacebookV2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacebookV2.Controllers
{
    [Authorize]
    public class LikeController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        [HttpPost]
        public ActionResult RatePhoto(long photoId)
        {
            using (var db2 = ApplicationDbContext.Create())
            {
                var photoExists = db2.Photos.Any(u => u.Id == photoId);
                if (!photoExists)
                    return View("NotFound");

                var currentUserId = User.Identity.GetUserId();
                var isAlreadyLiked = db2.Likes.Any(u => u.PhotoId == photoId && u.ProfileId == currentUserId);

                if (isAlreadyLiked)
                {
                    var likeToRemove = db2.Likes.Where(u => u.PhotoId == photoId && u.ProfileId == currentUserId)
                                        .FirstOrDefault();

                    db2.Likes.Remove(likeToRemove);
                    db2.SaveChanges();
                }
                else
                {
                    var like = new Like()
                    {
                        ProfileId = currentUserId,
                        PhotoId = photoId,
                        CreatedOn = DateTime.Now
                    };

                    db2.Likes.Add(like);
                    db2.SaveChanges();
                }

                return Json(isAlreadyLiked);
            }
        }
    }
}