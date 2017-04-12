using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using WebSitePerformanceTester.DataAccess;

namespace WebSitePerformanceTester.Services
{
    public class Urlhandler : IUrlhandler
    {
        private readonly IHubContext _hubContext;
        public Domain Domain { get; set; }
        public TestTime tTime { get; set; }
        public string ConnectionId { get; set; }

        public Urlhandler(IHubContext hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendResponseTime(Uri page, long timeTaken)
        {
            await _hubContext.Clients.Client(ConnectionId).Item(page, timeTaken);
        }

        public void Done()
        {
            _hubContext.Clients.Client(ConnectionId).Done();
        }
    }
}