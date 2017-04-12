using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
