using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;
using WebSitePerformanceTester.DataAccess;
using WebSitePerformanceTester.DataAccess.Repositories;

namespace WebSitePerformanceTester.Services
{
    public class UrlHIstoryService : IUrlHIstoryService
    {
        private UoW  _uow ;

        public UrlHIstoryService(UoW uow)
        {
            _uow = uow;
        }
        public List<HistoryDto> GetModel()
        {

            List<HistoryDto> hList  = new List<HistoryDto>();
           
            var list = _uow.Domains.Entities.Select(x => x.MainUrl).ToList();
            foreach (var u in list)
            {
                var urlHistory = new HistoryDto {Url = u};
                var dates = _uow.TestsTime.Entities
                    .Where(c=>c.Domain.MainUrl.Contains(u)).Select(x=>x.Date).ToList();
                urlHistory.dates = dates;
                hList.Add(urlHistory);
            }
            return hList;
        }

        public async Task<List<UrlDateResult>> GetResultsForUrlByDateAsync(string url, string date)
        {
            DateTime dt = Convert.ToDateTime(date);
            List<UrlDateResult> list = await _uow.ResponsesTime.Entities.Where(
                x => x.TestsTime.Domain.MainUrl.Contains(url)
                && SqlFunctions.DateDiff("second", x.TestsTime.Date, dt) == 0)
                .Select(d => new UrlDateResult() { Url = d.Url, Times = d.TimeMs })
                .OrderByDescending(o => o.Times)
                .ToListAsync();
            return list;
        }

        public async Task<List<string>> GetURls()
        {
            List<string> list = await _uow.Domains.Entities.Select(x => x.MainUrl).ToListAsync();
            return list;
        }

        public async Task<List<string>> GetDatesByUrl(string url)
        {
            List<string> list = await _uow.TestsTime.Entities
                            .Where(x => x.Domain.MainUrl.Contains(url))
                            .Select(s=>s.Date.ToString())
                            .ToListAsync();
            return list;
        }


    }

    public class UrlDateResult
    {
        public string Url { get; set; }
        public long Times { get; set; }
    }
}