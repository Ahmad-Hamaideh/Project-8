using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Project_8_MVC_Batool.Startup))]
namespace Project_8_MVC_Batool
{


    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

        }


    }

}
