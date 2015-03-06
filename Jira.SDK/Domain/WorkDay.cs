using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK.Domain
{
	public class WorkDay
	{
		public DateTime Day { get; set; }
		public TimeSpan TimeSpent { get; set; }
		public Boolean IsMissingWork { get; set; }
		public List<Worklog> Worklogs { get; set; }

		public WorkDay(DateTime day, List<Worklog> worklogs)
		{
			this.Day = day;
			this.Worklogs = worklogs;

			this.TimeSpent = TimeSpan.FromSeconds(worklogs.Sum(worklog => worklog.TimeSpentSeconds));
			this.IsMissingWork = (day.CompareTo(DateTime.Now.Date) < 0 && TimeSpent.TotalSeconds < new TimeSpan(7, 0, 0).TotalSeconds);
		}
	}
}
