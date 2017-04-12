using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.UI;
using System.Xml.Linq;
using Microsoft.AspNet.SignalR.Hubs;
using WebSitePerformanceTester.Hub;
using WebSitePerformanceTester.DataAccess;
using WebSitePerformanceTester.DataAccess.Repositories;

namespace WebSitePerformanceTester.Services
{
    public class UrlService : IUrlService
    {
        private IUoW _uow;
        //private IHubContext _hubContext;
        private IUrlhandler _handler;

        public UrlService()
        {
        }

        public UrlService( IUoW uow,  IUrlhandler handler){
            _uow = uow;
            //_hubContext = hubContext;
            _handler = handler;
        }

        public async Task addUrl(string urlToCheck, string connectionId)
        {
            var url = CheckUrl(urlToCheck);
            if (url == null) return;
            var siteMapurl = GetSiteMapUrl(url);

            var list = GetUriList(siteMapurl);
    
            try
            {
                Domain domain = _uow.Domains.Entities.SingleOrDefault(x => x.MainUrl.Contains(url));
                if (domain == null)
                {
                    domain = new Domain { MainUrl = url };
                    _uow.Domains.Add(domain);
                }

                var tTime = new TestTime
                {
                    Date = DateTime.Now,
                    Domain = domain
                };
                _uow.TestsTime.Add(tTime);
                await _uow.SaveChangesAsync();
                _handler.Domain = domain;
                _handler.tTime = tTime;
                _handler.ConnectionId = connectionId;
                //await _handler.MasureAndSendResponseTime(list, _uow);
                await MasureAndSendResponseTime(list);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public async Task MasureAndSendResponseTime(IEnumerable<Uri> list)
        {
            foreach (var page in list)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(page);
                Stopwatch timer = new Stopwatch();
                try
                {
                    timer.Start();
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        timer.Stop();
                        long timeTaken = timer.ElapsedMilliseconds;
                        var responseTime = new ResponseTime
                        {
                            Domain = _handler.Domain,
                            TestsTime = _handler.tTime,
                            Url = page.ToString(),
                            TimeMs = timeTaken
                        };
                        _uow.ResponsesTime.Add(responseTime);
                        await _uow.SaveChangesAsync();
                        await _handler.SendResponseTime(page, timeTaken);
                    }
                }
                catch
                {
                    // ignored
                }
            }
            _handler.Done();
        }


        public string GetSiteMapUrl(string url)
        {
            if (url.EndsWith(".xml")) return url;
            if (url.EndsWith("/")) return url + "sitemap.xml";
            url += "/sitemap.xml";
            return url;
        }

        public string CheckUrl(string urlToCheck)
        {
            try
            {
                var uri = new Uri(urlToCheck);
                var url = uri.AbsoluteUri;
                return url;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Uri> GetUriList(string url)
        {
            XDocument doc = XDocument.Load(url);
            List<Uri> d  = new List<Uri>();
            if (doc.Root.Name.LocalName == "sitemapindex")
            {
                var c = doc.Descendants().Where(z => z.Name.LocalName == "loc")
                    .Select(e => e.Value);
                foreach (var i in c)
                {
                    XDocument u;
                    try
                    {
                        u = XDocument.Load(i);
                        d.AddRange(u.Descendants()
                        .Where(dc => dc.Name.LocalName == "loc")
                        .Select(s => new Uri(s.Value)));
                    }
                    catch
                    {
                        // ignored
                    }
                }
                return d;
            }
            return  doc.Descendants()
                .Where(e => e.Name.LocalName == "loc")
                .Select(e => new Uri(e.Value));
        }

    }
}