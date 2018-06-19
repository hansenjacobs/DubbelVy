using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Dubbelvy.Startup))]
namespace Dubbelvy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
