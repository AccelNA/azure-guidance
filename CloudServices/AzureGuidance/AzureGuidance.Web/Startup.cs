using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AzureGuidance.Web.Startup))]
namespace AzureGuidance.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
