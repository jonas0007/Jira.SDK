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

        public TimeSpan OriginalEstimate
        {
            get { return TimeSpan.FromSeconds(this.OriginalEstimateSeconds); }
        }

        public TimeSpan RemainingEstimate
        {
            get { return TimeSpan.FromSeconds(this.RemainingEstimateSeconds); }
        }

        public TimeSpan TimeSpent
        {
            get { return TimeSpan.FromSeconds(this.TimeSpentSeconds); }
        }
    }
}
