using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FacebookV2.Startup))]
namespace FacebookV2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
