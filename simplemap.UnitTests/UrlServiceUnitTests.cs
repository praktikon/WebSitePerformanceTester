using System.Linq;
using NUnit.Framework;
using WebSitePerformanceTester.Services;

namespace simplemap.UnitTests
{
    [TestFixture]
    public class UrlServiceUnitTests
    {
        private UrlService _service;

        public UrlServiceUnitTests()
        {
                _service = new UrlService();
        }

        #region CheckUrl Method
        [Test]
        public void CheckUrl_ShouldReturn_Url()
        {
            
            var url = _service.CheckUrl("https://www.asp.net/");
            Assert.AreEqual("https://www.asp.net/", url);
        }
        [Test]
        public void CheckUrl_ShouldReturn_null()
        {
         
            var url = _service.CheckUrl("hasp.net/");
            Assert.AreEqual(null, url);
        }
        #endregion

        #region GetSiteMap Method
        [Test]
        public void GetSiteMapRe_ShouldReturn_SitemapUrl()
        {
            var siteMapUrl = _service.GetSiteMapUrl("https://www.asp.net");
            Assert.AreEqual("https://www.asp.net/sitemap.xml", siteMapUrl);

        }

        #endregion

        #region GetUrlList
        [Test]
        public void GetSiteMapList_ShouldReturn_UrlList()
        {
            var list = _service.GetUriList("https://www.asp.net/sitemap.xml");
            Assert.AreEqual(1842, list.Count());
        }
        #endregion
        //public void AddUrl_SholdReturn_
    }
}
