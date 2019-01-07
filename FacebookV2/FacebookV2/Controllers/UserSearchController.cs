using FacebookV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacebookV2.Controllers
{
    [Authorize]
    public class UserSearchController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        [HttpGet]
        public ActionResult FindUsers(string q)
        {
            var users = Search(q, 5).Select(i => new
                                            {
                                                 id = i.Id,
                                                 text = $"{i.FirstName} {i.LastName}"
                                            });

            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public IEnumerable<Profile> Search(string name, int resultsPerPage)
        {
            return db.Profiles.Where(p => p.FirstName.Contains(name) || p.LastName.Contains(name))
                            .ToList()
                            .Take(resultsPerPage);
        }
    }
}