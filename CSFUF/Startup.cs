using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(CSFUF.Startup))]
namespace CSFUF
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

    }
}
