using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacebookV2.App_Code
{
    public class BaseController : Controller
    {
        public ActionResult InternalServerError()
        {
            return new HttpStatusCodeResult(500);
        }

        public ActionResult Unprocessable()
        {
            return new HttpStatusCodeResult(422);
        }

        public ActionResult InternalServerErrorView()
        {
            return View("InternalServerError");
        }

        public ActionResult NotFoundView()
        {
            return View("NotFound");
        }

        public ActionResult ForbidView()
        {
            return View("Forbid");
        }

        public ActionResult InvalidInputView()
        {
            return View("InvalidInputView");
        }
    }
}