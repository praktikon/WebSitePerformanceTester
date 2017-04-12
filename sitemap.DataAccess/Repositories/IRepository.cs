using System.Data.Entity;
using System.Linq;

namespace WebSitePerformanceTester.DataAccess.Repositories
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
  
        IDbSet<T> Entities { get; }
    }
}