using System;
using System.Threading.Tasks;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Ninject;
using Ninject.Web.Mvc;
using Owin;
using WebSitePerformanceTester.Hub;
using WebSitePerformanceTester.Infrastructure;
using WebSitePerformanceTester.Services;

[assembly: OwinStartup(typeof(WebSitePerformanceTester.Startup))]

namespace WebSitePerformanceTester
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
