using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSitePerformanceTester.DataAccess.context;

namespace WebSitePerformanceTester.DataAccess.Repositories
{
    public class UoW : IUoW
    {
        private IRepository<ResponseTime> _responseTimeRepository;
        private IRepository<TestTime> _testTimeRepository ;
        private IRepository<Domain> _domainssRepository;
        private DbContext _context;
        public UoW(SitemapDbContext context)
        {
            _context = context;
        }

        public IRepository<ResponseTime> ResponsesTime => _responseTimeRepository ??
                                          (_responseTimeRepository = new Repository<ResponseTime>(_context));

        public IRepository<TestTime> TestsTime => _testTimeRepository ??
                                          (_testTimeRepository = new Repository<TestTime>(_context));
        public IRepository<Domain> Domains => _domainssRepository ??
                                          (_domainssRepository = new Repository<Domain>(_context));


        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}
