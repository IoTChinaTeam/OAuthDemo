using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Owin;

[assembly: OwinStartup(typeof(OAuthDemo.Startup))]

namespace OAuthDemo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.SetLoggerFactory(new OwinTraceLoggerFactory());

            ConfigureAuth(app);
        }
    }
}
