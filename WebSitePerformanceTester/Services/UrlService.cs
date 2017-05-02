﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using WebSitePerformanceTester.DataAccess;
using WebSitePerformanceTester.DataAccess.Repositories;
using HtmlAgilityPack;

namespace WebSitePerformanceTester.Services
{
    public class UrlService : IUrlService
    {
        private readonly IUoW _uow;
        private readonly IUrlhandler _handler;

        public UrlService()
        {
        }

        public UrlService( IUoW uow,  IUrlhandler handler){
            _uow = uow;
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

                if ((list is null) || list.Any())
                {
                    await MasureAndSendResponseTimeLinksFromScraping(url);
                   
                }
                else
                {
                    await MasureAndSendResponseTimeLinksFromSiteMap_Xml(list);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            _handler.Done();

        }


        public async Task MasureAndSendResponseTimeLinksFromScraping(string baseUrl)
        {
            HashSet<Uri> uries = new HashSet<Uri>();
            var uriHashSet = new HashSet<Uri>();
            HtmlWeb htmlDocument = new HtmlWeb();
            uriHashSet.Add(new Uri(baseUrl));
            while (uriHashSet.Any())
            {
                var tempUri = uriHashSet.LastOrDefault();
                await ProccessUrl(tempUri);
                uriHashSet.Remove(tempUri);
                HtmlDocument doc = htmlDocument.Load(tempUri.ToString());
                var nodes = doc.DocumentNode.SelectNodes("//a[@href]");
                if (nodes is null) continue;
                foreach (HtmlNode link in nodes)
                {
                    string hrefValue = link.GetAttributeValue("href", string.Empty);

                    if ((hrefValue.StartsWith("/") || hrefValue.StartsWith(baseUrl))
                        && !hrefValue.Contains("#")
                        && !hrefValue.Contains("?")
                        && hrefValue.Length > 1)
                    {
                        var baseUri = new Uri(baseUrl);
                        var uri = new Uri(baseUri, hrefValue);
                        if (!uries.Contains(uri))
                        {
                            uries.Add(uri);
                            if (!hrefValue.StartsWith(baseUrl))
                            {
                                uriHashSet.Add(uri);

                            }
                            else
                            {
                                var uri1 = new Uri(hrefValue);
                                uriHashSet.Add(uri1);
                            }

                        }

                    }
                }
            }
        }

        public async Task MasureAndSendResponseTimeLinksFromSiteMap_Xml(IEnumerable<Uri> list)
        {
            foreach (var page in list)
            {
                await ProccessUrl(page);
            }
        }


        public async Task ProccessUrl(Uri page)
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
                return url.TrimEnd('/');
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Uri> GetUriList(string url)
        {
            XDocument doc = null;
            try
            {
                doc = XDocument.Load(url);
            }
            catch (Exception e)
            {
                return null;
            }

            List<Uri> d  = new List<Uri>();
            if (doc.Root.Name.LocalName == "sitemapindex")
            {
                var c = doc.Descendants()
                            .Where(z => z.Name.LocalName == "loc")
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