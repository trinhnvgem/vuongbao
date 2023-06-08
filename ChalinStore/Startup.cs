using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChalinStore.Startup))]
namespace ChalinStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
