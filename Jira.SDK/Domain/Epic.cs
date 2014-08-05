using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK.Domain
{
	public class Epic
	{
		public String Key { get; set; }
		public String Summary { get; set; }
		public String ERPCode { get; set; }
		public Int32 Rank { get; set; }

		public List<Issue> Issues { get; set; }
		public Sprint sprint { get; set; }
		public Epic(String key, String summary, String erpCode, Int32 rank,  List<Issue> issues, Sprint sprint)
		{
			Key = key;
			Summary = summary;
			ERPCode = erpCode;
			Rank = rank;

			Issues = issues;
			EstimateInSeconds = Issues.Sum(issue => (issue.TimeTracking != null ? issue.TimeTracking.OriginalEstimateSeconds : 0));
			TimeSpentInSeconds = Issues.Sum(issue => issue.GetWorklogs().Where(worklog => worklog.Started.CompareTo(sprint.StartDate) >= 0 && worklog.Started.CompareTo(sprint.EndDate) <= 0).Sum(worklog => worklog.TimeSpentSeconds));
		}
		public double TimeSpentInSeconds { get; private set; }
		public double EstimateInSeconds { get; private set; }
	}
}
