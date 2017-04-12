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
            var D = new Domain {MainUrl = "sdfsdfsdf"};
            context.Domains.Add(D);
            context.SaveChanges();
            try
            {
                base.Seed(context);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
