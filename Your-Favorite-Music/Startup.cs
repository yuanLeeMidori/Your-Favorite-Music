using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(YFM.Startup))]
namespace YFM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
