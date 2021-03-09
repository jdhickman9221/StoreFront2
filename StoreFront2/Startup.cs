using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StoreFront2.Startup))]
namespace StoreFront2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
