using System;
using System.Threading.Tasks;
using System.Web.Http;
using WebSitePerformanceTester.Services;

namespace WebSitePerformanceTester
{
    public class UrlController : ApiController
    {
        private IUrlHIstoryService _service;

        public UrlController(IUrlHIstoryService service)
        {
            _service = service;
        }
        // GET api/<controller>
        public async Task<IHttpActionResult> GetDates(string url)
        {
            var dates = await _service.GetDatesByUrl(url);
            return Ok(dates);
        }

        public async Task<IHttpActionResult> GetResults(string url, string date)
        {
            var results = await _service.GetResultsForUrlByDateAsync(url, date);
            return Ok(results);
        }
    }
}