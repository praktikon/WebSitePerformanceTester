using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSitePerformanceTester.DataAccess.context
{
    public class SiteMapDbInitializer : DropCreateDatabaseIfModelChanges<SitemapDbContext>
    {

        protected override void Seed(SitemapDbContext context)
        {
            var dom = new Domain {MainUrl = "https://www.ukad-group.com" };
            context.Domains.Add(dom);
            context.SaveChanges();
            base.Seed(context);
        }
    }
}
