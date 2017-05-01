namespace WebSitePerformanceTester.DataAccess
{
    public class ResponseTime
    {
     
        public int Id { get; set; }

        public int DomainId { get; set; }
        public Domain Domain { get; set; }
     
        public int TestsTimeId { get; set; }
        public TestTime TestsTime { get; set; }

        public string Url { get; set; }

        public long TimeMs { get; set; }
    }
}
