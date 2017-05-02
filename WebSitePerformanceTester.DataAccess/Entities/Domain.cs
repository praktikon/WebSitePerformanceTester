using System.Collections.Generic;

namespace WebSitePerformanceTester.DataAccess
{
    public class Domain
    {
        public Domain()
        {
            TestsTime = new List<TestTime>();
        }

        public int Id { get; set; }

        public string MainUrl { get; set; }

        public ICollection<TestTime> TestsTime { get; set; }
    }
}
