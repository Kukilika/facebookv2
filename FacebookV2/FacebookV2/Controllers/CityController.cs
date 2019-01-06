using FacebookV2.App_Start;
using FacebookV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacebookV2.Controllers
{
    [Authorize]
    public class CityController : Controller
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();

        [SkipMyGlobalActionFilter]
        [HttpGet]
        public ActionResult GetAllCitiesFromCounty(int? countyId)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var cities = db.Cities
                            .Where(c => c.CountyId == countyId)
                            .ToList();

            return Json(cities, JsonRequestBehavior.AllowGet);
        }
    }
}