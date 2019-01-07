using FacebookV2.Enums;
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
    public class FriendController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        [HttpGet]
        public ActionResult GetAll()
        {
            var friendRequestEntities = GetAllPending();

            var model = new FriendsDetailsViewModel()
            {
                FriendRequests = BuildFriendRequestModel(friendRequestEntities),
                Friends = GetAllFriends()
                                     .Select(friendResult => new FriendViewModel()
                                     {
                                         Id = friendResult.UserProfile.Id,
                                         FirstName = friendResult.UserProfile.FirstName,
                                         LastName = friendResult.UserProfile.LastName,
                                         CreatedDate = friendResult.CreatedDate,
                                         ProfilePhotoId = friendResult.UserProfile.ProfilePhotoId
                                     })
                                     .ToList()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult ApproveRequest(string requesterId)
        {
            var user = db.Profiles.Where(p => p.Id == requesterId)
                                  .FirstOrDefault();

            if (user == null)
                return View("NotFound");

            var currentUserId = User.Identity.GetUserId();
            var isFriendWith = db.Friends.Any(f => f.ReceiverId == currentUserId && f.SenderId == requesterId
                                               || f.ReceiverId == requesterId && f.SenderId == currentUserId);

            var friendRequest = db.FriendRequests.Where(fr => fr.RequesterProfileId == requesterId
                                                    && fr.RequestedProfileId == currentUserId
                                                    && fr.Status == (byte)FriendRequestStatusTypes.Pending)
                                                .FirstOrDefault();

            var canBeApproved = friendRequest != null
                        && currentUserId != user.Id
                        && !isFriendWith;
            if (!canBeApproved)
                return View("InvalidInputView");

            var friendEntity = new Friend
            {
                SenderId = requesterId,
                ReceiverId = currentUserId,
                CreatedDate = DateTime.Now
            };

            try
            {
                db.Friends.Add(friendEntity);
                db.SaveChanges();

                if (TryUpdateModel(friendRequest))
                {
                    friendRequest.Status = (int)FriendRequestStatusTypes.Approved;

                    db.SaveChanges();
                }
            }
            catch(Exception e)
            {
                return View("InternalServerError");
            }

            var model = new FriendViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreatedDate = DateTime.Now,
                ProfilePhotoId = user.ProfilePhotoId
            };

            return PartialView("_FriendView", model);
        }

        [HttpPost]
        public ActionResult RejectRequest(string requesterId)
        {
            var user = db.Profiles.Where(p => p.Id == requesterId)
                                  .FirstOrDefault();

            if (user == null)
                return View("NotFound");

            var currentUserId = User.Identity.GetUserId();
            var friendRequest = db.FriendRequests.Where(fr => fr.RequesterProfileId == requesterId
                                                    && fr.RequestedProfileId == currentUserId
                                                    && fr.Status == (byte)FriendRequestStatusTypes.Pending)
                                                .FirstOrDefault();

            var isFriendWith = db.Friends.Any(f => f.ReceiverId == currentUserId && f.SenderId == requesterId
                                               || f.ReceiverId == requesterId && f.SenderId == currentUserId);

            var canBeRejected =  friendRequest != null
                       && currentUserId != user.Id
                       && !isFriendWith;
            if (!canBeRejected)
                return View("InvalidInputView");

            try
            {
                if (TryUpdateModel(friendRequest))
                {
                    friendRequest.Status = (byte)FriendRequestStatusTypes.Rejected;

                    db.SaveChanges();
                }
            }
            catch(Exception e)
            {
                return View("InternalServerError");
            }

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SendRequest(string userId)
        {
            var user = db.Profiles.Where(p => p.Id == userId)
                                  .FirstOrDefault();
            if (user == null)
                return View("NotFound");

            var currentUserId = User.Identity.GetUserId();

            var canBeSent = CanSendFriendRequest(userId);
            if (!canBeSent)
                return View("InvalidInputView");

            var friendRequest = new FriendRequest
            {
                RequestedProfileId = user.Id,
                RequesterProfileId = currentUserId,
                CreatedOn = DateTime.Now,
                Status = (int)FriendRequestStatusTypes.Pending
            };

            try
            {
                db.FriendRequests.Add(friendRequest);
                db.SaveChanges();
            }
            catch(Exception e)
            {
                return View("InternalServerError");
            }

            return new EmptyResult();
        }

        [NonAction]
        private List<FriendRequestViewModel> BuildFriendRequestModel(List<FriendRequest> friendRequestEntities)
        {
            var model = new List<FriendRequestViewModel>();

            friendRequestEntities.ForEach(fr =>
            {
                var requester = db.Profiles.Find(fr.RequesterProfileId);

                var friendRequestModel = new FriendRequestViewModel()
                {
                    RequesterId = requester.Id,
                    RequesterFirstName = requester.FirstName,
                    RequesterLastName = requester.LastName,
                    CreatedOn = fr.CreatedOn,
                    RequesterProfilePictureId = requester.ProfilePhotoId
                };

                model.Add(friendRequestModel);
            });

            return model;
        }

        public List<FriendRequest> GetAllPending()
        {
            var currentUserId = User.Identity.GetUserId();

            return db.FriendRequests.Where(f => f.RequestedProfileId == currentUserId
                                            && f.Status == (byte)FriendRequestStatusTypes.Pending)
                                    .ToList();
        }

        public IQueryable<FriendResult> GetAllFriends()
        {
            var currentUserId = User.Identity.GetUserId();

            var friends1 = db.Friends
                    .Where(f => f.SenderId == currentUserId)
                    .Select(f => new FriendResult
                    {
                        UserProfile = f.Receiver,
                        CreatedDate = f.CreatedDate
                    });

            var friends2 = db.Friends
                    .Where(f => f.ReceiverId == currentUserId)
                     .Select(f => new FriendResult
                     {
                         UserProfile = f.Sender,
                         CreatedDate = f.CreatedDate
                     });

            return friends1.Union(friends2);
        }

        [NonAction]
        public bool CanSendFriendRequest(string userId)  //verificam daca currentUser ii poate trimite cerere unui alt user
        {
            var currentUserId = User.Identity.GetUserId();

            var isFriendWith = db.Friends.Any(f => f.ReceiverId == currentUserId && f.SenderId == userId
                                                || f.ReceiverId == userId && f.SenderId == currentUserId);

            var isPendingForUser = db.FriendRequests
                    .Any(u => u.RequestedProfileId == userId && u.RequesterProfileId == currentUserId && u.Status == (int)FriendRequestStatusTypes.Pending
                              || u.RequestedProfileId == currentUserId && u.RequesterProfileId == userId && u.Status == (int)FriendRequestStatusTypes.Pending);

            var isRejectedByUser = db.FriendRequests
                        .Any(u => u.RequestedProfileId == userId && u.RequesterProfileId == currentUserId && u.Status == (int)FriendRequestStatusTypes.Rejected);

            return currentUserId != userId
                    && !isFriendWith
                    && !isPendingForUser
                    && !isRejectedByUser;
        }
    }
}