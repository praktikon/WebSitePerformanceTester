using WebSitePerformanceTester.DataAccess.Infrastructure;
using WebSitePerformanceTester.Hub;
using WebSitePerformanceTester.Infrastructure;
using WebSitePerformanceTester.Services;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebSitePerformanceTester.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WebSitePerformanceTester.App_Start.NinjectWebCommon), "Stop")]

namespace WebSitePerformanceTester.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Web.WebApi;
    using WebSitePerformanceTester.DataAccess.Infrastructure;
    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using WebSitePerformanceTester.Hub;
    using Microsoft.AspNet.SignalR.Infrastructure;
    using System.Web.Http;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }
        private static void RegisterWithSignalr(IKernel kernel)
        {
            GlobalHost.DependencyResolver = new NinjectSignalRDependencyResolver(kernel);
        }
        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {

            GlobalHost.DependencyResolver.Register(typeof(IHubActivator),
                () => new HubActivator(kernel));
            
            kernel.Bind<IHubContext>().ToMethod( _ => GlobalHost.ConnectionManager.GetHubContext<SiteMapHub>());
            kernel.Bind<IUrlHIstoryService>().To<UrlHIstoryService>();
            kernel.Bind<IUrlService>().To<UrlService>();
            kernel.Bind<IUrlhandler>().To<Urlhandler>();
        

            kernel.Load(new DataAccessModule(), new SiteMapModule());
        }        
    }
}
