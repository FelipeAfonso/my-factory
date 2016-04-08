using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(myFactorySite.Startup))]
namespace myFactorySite
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
