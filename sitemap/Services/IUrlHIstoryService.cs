using System.Collections.Generic;
using System.Threading.Tasks;
using WebSitePerformanceTester.DataAccess;

namespace WebSitePerformanceTester.Services
{
    public interface IUrlHIstoryService
    {
        List<HistoryDto> GetModel();
        Task<List<UrlDateResult>> GetResultsForUrlByDateAsync(string url, string date);
        Task<List<string>> GetURls();
        Task<List<string>> GetDatesByUrl(string url);
    }
}