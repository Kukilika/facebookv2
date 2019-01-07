using FacebookV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace FacebookV2.App_Start
{
    public class ProfileRequiredActionFilter : IActionFilter
    {
        private ApplicationDbContext db = ApplicationDbContext.Create();
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentUserId = HttpContext.Current.User.Identity.GetUserId();

            if (filterContext.HttpContext.User.Identity.IsAuthenticated
                    && !filterContext.ActionDescriptor.GetCustomAttributes(typeof(SkipMyGlobalActionFilterAttribute), false).Any()
                    && !db.Profiles.Any(p => p.Id == currentUserId))
                filterContext.Result = new RedirectResult("/Profile/Create");

        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}