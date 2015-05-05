using Jira.SDK.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK.Domain
{
    public class TimeTracking
    {
        public long OriginalEstimateSeconds { get; set; }
        public long RemainingEstimateSeconds { get; set; }
        public long TimeSpentSeconds { get; set; }
		internal Issue Issue { get; set; }

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

		public double TimeOpenSeconds
		{
			get
			{
				return TimeOpen.TotalSeconds;
			}
		}

		public TimeSpan TimeOpen
		{
			get 
			{
				if (Issue.Resolved == DateTime.MaxValue)
				{
					return DateTime.Now.Subtract(Issue.Created);
				}
				else
				{
					return Issue.Resolved.Subtract(Issue.Created);
				}
			}
		}
    }
}
