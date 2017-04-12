using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
