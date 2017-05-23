using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using WebSitePerformanceTester.DataAccess;

namespace WebSitePerformanceTester.Services
{
    public interface IUrlService
    {
        Task AddUrl(string urlToCheck, string connectionId);

        //Task MasureAndSendResponseTimeLinksFromSiteMap_Xml(IEnumerable<Uri> list,  
        //    Domain domain , TestTime tTime, IHubContext hubContext, string connectionId);

        string GetSiteMapUrl(string url);
        string CheckUrl(string urlToCheck);
        IEnumerable<Uri> GetUriList(string url);
    }
}