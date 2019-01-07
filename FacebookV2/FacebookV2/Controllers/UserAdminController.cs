using FacebookV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacebookV2.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserAdminController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        public ActionResult Show(int id)
        {
            try
            {
                Profile profile = db.Profiles.Find(id);
                ViewBag.Category = profile;
            }
            catch (Exception e)
            {
                Console.WriteLine("NO SE PUEDE");
                return View("ErrorView");
            }
            return View();
        }


        // GET: UserAdmin
        public ActionResult Index()
        {
            
            var profiles = from profile in db.Profiles
                                 orderby profile.FirstName
                                 select profile;
            ViewBag.Profiles = profiles;
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        //Post
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
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                Profile profile = db.Profiles.Find(id);
                profile.IsDeletedByAdmin = true;
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return View("ErrorView");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            try
            {
                Profile profile = db.Profiles.Find(id);
                ViewBag.Category = profile;
            }
            catch (Exception e)
            {
                return View("ErrorView");
            }
            return View();
        }

        [HttpPut]
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
        }
    }
}