using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK
{
    public class TimeTracking
    {
        public long OriginalEstimateSeconds { get; set; }
        public long RemainingEstimateSeconds { get; set; }
        public long TimeSpentSeconds { get; set; }
    }
}
