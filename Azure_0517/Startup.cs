using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


[assembly: OwinStartup(typeof(Azure_0517.Startup))]
namespace Azure_0517
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    
    }
}