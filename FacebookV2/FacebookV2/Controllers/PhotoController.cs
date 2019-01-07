using FacebookV2.Models;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace FacebookV2.Controllers
{
    public class PhotoController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        [NonAction]
        public bool CanDownloadPicture(long? pictureId)
        {
            var picture = db.Photos.Include(u => u.Profile)
                                   .Where(p => p.Id == pictureId)
                                   .FirstOrDefault();

            if (picture == null)
                return false;

            var currentUserId = User.Identity.GetUserId();
            var userId = picture.Profile.Id;

            if (currentUserId == userId)
                return true;

            var isFriendWith = db.Friends.Any(f => f.ReceiverId == currentUserId && f.SenderId == userId
                                                || f.ReceiverId == userId && f.SenderId == currentUserId);

            if (!picture.Profile.IsPublic
                    && isFriendWith
                    && picture.Id != picture.Profile.ProfilePhotoId
                    && currentUserId != userId)
                return false;

            return true;
        }

        public byte[] GetProfilePictureContent(long? profilePictureId)
        {
            return db.Photos
                        .Where(p => p.Id == profilePictureId)
                        .Select(p => p.Content)
                        .FirstOrDefault();
        }

        public ActionResult GetProfilePicture(long? id)
        {
            if (!CanDownloadPicture(id))
                return View("NotFound");

            var profilePictureContent = GetProfilePictureContent(id);

            if (profilePictureContent == null)
                return View("NotFound");

            return File(profilePictureContent, "application/octet-stream");
        }
    }
}