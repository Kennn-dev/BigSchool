using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BigSchool.Startup))]
namespace BigSchool
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
