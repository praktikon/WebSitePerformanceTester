using System.Threading.Tasks;

namespace WebSitePerformanceTester.DataAccess.Repositories
{
    public interface IUoW
    {
        IRepository<ResponseTime> ResponsesTime { get; }
        IRepository<TestTime> TestsTime { get; }
        IRepository<Domain> Domains { get; }
        Task SaveChangesAsync();
    }
}