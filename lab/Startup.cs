using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(lab.Startup))]
namespace lab
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
