using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Jared_Spring_2016_42.Startup))]
namespace Jared_Spring_2016_42
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
