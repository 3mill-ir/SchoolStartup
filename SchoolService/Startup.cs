using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SchoolService.Startup))]
namespace SchoolService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
