using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
/// <summary>
/// Summary description for Startup
/// </summary>
public class Startup
{
	
		public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
	
}