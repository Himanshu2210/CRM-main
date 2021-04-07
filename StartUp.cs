using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CRM.App_Start.StartUp))]
namespace CRM.App_Start
{
    public partial class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}