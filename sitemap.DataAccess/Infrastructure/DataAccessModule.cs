using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using WebSitePerformanceTester.DataAccess.context;
using WebSitePerformanceTester.DataAccess.Repositories;

namespace WebSitePerformanceTester.DataAccess.Infrastructure
{
    public class DataAccessModule : NinjectModule
    {
        public override void Load()
        {
            Bind<SitemapDbContext>().ToSelf();
            Bind(typeof(IRepository<>)).To(typeof(Repository<>));
            Bind<IUoW>().To<UoW>();
        }
    }
}
