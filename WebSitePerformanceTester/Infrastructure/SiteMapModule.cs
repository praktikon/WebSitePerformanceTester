using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Ninject.Modules;
using WebSitePerformanceTester.DataAccess.context;
using WebSitePerformanceTester.DataAccess.Repositories;
using WebSitePerformanceTester.Hub;
using WebSitePerformanceTester.Services;

namespace WebSitePerformanceTester.Infrastructure
{
    public class SiteMapModule :NinjectModule
    {
        public override void Load()
        {

            //Bind<SiteMapHub>().ToSelf();

        }
    }
}