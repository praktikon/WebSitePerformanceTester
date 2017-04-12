using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebSitePerformanceTester.DataAccess.Repositories;
using WebSitePerformanceTester.DataAccess;

namespace WebSitePerformanceTester.Services
{
    public interface IUrlhandler
    {
        Domain Domain { get; set; }
        TestTime tTime { get; set; }
        string ConnectionId { get; set; }
        Task SendResponseTime(Uri page, long timeTaken);
        void Done();
    }
}