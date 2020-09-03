using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TaxManagementSystem.Startup))]
namespace TaxManagementSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
