using FacebookV2.Enums;
using FacebookV2.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace FacebookV2.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        [HttpPost]
        public ActionResult AddComment(AddCommentViewModel model)
        {
            using (var db2 = ApplicationDbContext.Create())
            {
                try
                {
                    var photoExists = db2.Photos.Any(u => u.Id == model.PhotoId);
                    if (!photoExists)
                        return View("NotFound");

                    var currentUserId = User.Identity.GetUserId();
                    var userProfile = db2.Profiles.Where(p => p.Id == currentUserId).FirstOrDefault();
                    var comment = new Comment()
                    {
                        Body = model.Body,
                        PhotoId = model.PhotoId,
                        ProfileId = currentUserId,
                        Profile = userProfile,
                        CreatedOn = DateTime.Now,
                        Status = (byte)CommentStatusTypes.Pending
                    };
                    db2.Comments.Add(comment);
                    db2.SaveChanges();

                    var mappedComment = new CommentViewModel()
                    {
                        Id = comment.Id,
                        PhotoId = comment.PhotoId,
                        Body = comment.Body,
                        CreatedOn = comment.CreatedOn,
                        IsEditable = (comment.Status == (byte)CommentStatusTypes.Pending || comment.ProfileId == currentUserId),
                        FirstName = comment.Profile.FirstName,
                        LastName = comment.Profile.LastName,
                        ProfileId = comment.Profile.Id,
                        ProfilePhotoId = comment.Profile.ProfilePhotoId
                    };

                    return PartialView("_CommentView", mappedComment);
                }
                catch
                {
                    return View("InternalServerError");
                }
            }
        }

        [HttpGet]
        public ActionResult ShowMore(long photoId, int showedComments, int totalComments)
        {
            var photo = db.Photos.Where(p => p.Id == photoId)
                                 .FirstOrDefault();
            if (photo == null)
                return View("NotFound");
            var currentUserId = User.Identity.GetUserId();
            var commentsToShow = Math.Min((totalComments - showedComments), 3); //3 comentarii pe poza
            var comments = GetComments(photoId, showedComments, commentsToShow);

            var model = new List<CommentViewModel>();
            comments.ForEach(comment =>
            {
                var commentVm = new CommentViewModel()
                {
                    Id = comment.Id,
                    PhotoId = comment.PhotoId,
                    Body = comment.Body,
                    CreatedOn = comment.CreatedOn,
                    IsEditable = (comment.Status == (byte)CommentStatusTypes.Pending || comment.ProfileId == currentUserId),
                    FirstName = comment.Profile.FirstName,
                    LastName = comment.Profile.LastName,
                    ProfileId = comment.Profile.Id,
                    ProfilePhotoId = comment.Profile.ProfilePhotoId
                };

                model.Add(commentVm);
            });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public List<Comment> GetComments(long photoId, int showedComments, int commentsToShow)
        {
            using (var dbNew = ApplicationDbContext.Create())
            {
                return dbNew.Comments.Include(p => p.Profile)
                                    .Where(u => u.PhotoId == photoId)
                                    .OrderBy(u => u.CreatedOn)
                                    .Skip(showedComments)
                                    .Take(commentsToShow)
                                    .ToList();
            }
        }
    }
}