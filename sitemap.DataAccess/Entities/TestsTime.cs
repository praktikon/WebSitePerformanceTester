using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebSitePerformanceTester.DataAccess
{
    public class TestTime
    {
        public TestTime()
        {
             ResponsesTime = new List<ResponseTime>();
        }
      
        public int Id { get; set; }

        public int DomainId { get; set; }
        public Domain Domain { get; set; }

        [Column(TypeName = "DateTime2")]
        public DateTime Date { get; set; }

        private ICollection<ResponseTime> ResponsesTime { get; set; }
    }
}
