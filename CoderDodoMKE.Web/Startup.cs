using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CoderDojoMKE.Web.Startup))]
namespace CoderDojoMKE.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
