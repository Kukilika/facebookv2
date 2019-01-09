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
    public class GroupController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        // GET: Group
        [HttpGet]
        public ActionResult Index()
        {
            var groups = from Group in db.Groups
                         orderby Group.Name
                         select Group;
            ViewBag.Groups = groups;
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(Group group)
        {
            var currentUserId = User.Identity.GetUserId();
            var currentDate = DateTime.Today;
            var newGroup = new Group();
            newGroup.Description = group.Description;
            newGroup.Name = group.Name;
            newGroup.Id = group.Id;
            newGroup.AdminId = currentUserId;
            newGroup.CreatedDate = currentDate;
            db.Groups.Add(newGroup);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}