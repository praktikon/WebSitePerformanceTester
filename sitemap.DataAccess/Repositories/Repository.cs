using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using WebSitePerformanceTester.DataAccess.context;

namespace WebSitePerformanceTester.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T: class
    {
        private readonly DbContext _context;
        private IDbSet<T> _entities;
        public Repository(DbContext context)
        {
                _context = context;
        }

        public void Add(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }
                Entities.Add(entity);
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

     
        public IDbSet<T> Entities => _entities ?? (_entities = _context.Set<T>());
    }
}
