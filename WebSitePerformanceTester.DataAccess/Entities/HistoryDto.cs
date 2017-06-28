using System;
using System.Collections.Generic;

namespace WebSitePerformanceTester.DataAccess
{
    public class HistoryDto
    {
        public string Url { get; set; }
        public List<DateTime> dates { get; set; }
    }
}