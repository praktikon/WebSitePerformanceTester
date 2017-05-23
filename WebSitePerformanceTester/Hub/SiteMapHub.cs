using System;
using System.Threading.Tasks;
using WebSitePerformanceTester.Services;

namespace WebSitePerformanceTester.Hub
{
    public class SiteMapHub : Microsoft.AspNet.SignalR.Hub
    {
        private readonly IUrlService _service;
        private readonly IUrlHIstoryService _historyService;
        public SiteMapHub(IUrlService service, IUrlHIstoryService hIstoryService)
        {
            _service = service;
            _historyService = hIstoryService;
        } 
        public async Task ProcessUrl(string strUrl)
        {
            string connectionId = Context.ConnectionId;
     
            await _service.AddUrl(strUrl, connectionId);

            //Clients.Caller.Item(dto.page, dto.timeTaken);
        }

        public async Task GetDatesByUrl(string url)
        {
         
            var list = await _historyService.GetDatesByUrl(url);
            Clients.Caller.dates(list);
        }
        public async Task GetResultsForUrlByDate(string url, string date)
        {
           
            var list = await _historyService.GetResultsForUrlByDateAsync(url, date); 
            Clients.Caller.results(list);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Console.WriteLine("disc");
            return base.OnDisconnected(stopCalled);
        }
    }
}