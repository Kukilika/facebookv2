using FacebookV2.App_Start;
using System.Web;
using System.Web.Mvc;

namespace FacebookV2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ProfileRequiredActionFilter());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
