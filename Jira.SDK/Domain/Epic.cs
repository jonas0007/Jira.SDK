using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK.Domain
{
	public class Epic : Issue
	{
		private List<Issue> _issues;
		public List<Issue> Issues
		{
			get
			{
				if (_issues == null)
				{
					throw new ArgumentException("The issues aren't loaded yet");
				}
				return _issues;
			}
			private set
			{
				_issues = value;
			}
		}

		public List<Sprint> Sprints { private get; set; }

		public String EpicStatus
		{
			get{
				return (Fields.Customfield_10702 != null ? Fields.Customfield_10702.Value : "");
			}
		}

		public double TimeSpentInSeconds { get; private set; }
		public double EstimateInSeconds { get; private set; }
		public double RemainingEstimateInSeconds { get; private set; }

		public double GetCost(Double costPerSecond)
		{
			return Math.Round(TimeSpentInSeconds * costPerSecond, 2);
		}

		public double GetEstimatedCost(Double costPerSecond)
		{
			return Math.Round(EstimateInSeconds * costPerSecond, 2);
		}

		public void LoadIssues(List<Sprint> sprints)
		{
			List<Issue> issues = GetJira().Client.GetIssuesWithEpicLink(this.Key);
			issues.ForEach(issue => issue.SetJira(GetJira()));

			foreach (Issue issue in issues)
			{
				issue.Sprint = sprints.Where(sprint => sprint.ID == issue.SprintID).FirstOrDefault();
			}

			LoadIssues(issues);
		}

		public void LoadIssues()
		{
			LoadIssues(GetJira().Client.GetIssuesWithEpicLink(this.Key));
		}

		public void LoadIssues(List<Issue> issues)
		{
			LoadIssues(issues, DateTime.MinValue, DateTime.MaxValue);
		}

		public void LoadIssues(List<Issue> issues, DateTime worklogStartdate, DateTime worklogEnddate)
		{
			Issues = issues;

			EstimateInSeconds = Issues.Sum(issue => (issue.TimeTracking != null ? issue.TimeTracking.OriginalEstimateSeconds : 0));
			TimeSpentInSeconds = Issues.Sum(issue => (issue.TimeTracking != null ? issue.TimeTracking.TimeSpentSeconds : 0));
			RemainingEstimateInSeconds = Issues.Sum(issue => (issue.TimeTracking != null ? issue.TimeTracking.RemainingEstimateSeconds : 0));

		}

		public static Epic UndefinedEpic
		{
			get
			{
				return new Epic("NONE", "Issues without feature");
			}
		}

		//The private constructor for undefined epics
		private Epic(String key, IssueFields fields, Jira jira)
		{
			base.Key = key;
			base.Fields = fields;
			base.SetJira(jira);
		}

		private Epic(String key, String summary)
		{
			Key = key;

			Fields = new IssueFields()
			{
				Assignee = User.UndefinedUser,
				Reporter = User.UndefinedUser,
				Summary = "",
				Created = DateTime.MinValue,
				Updated = DateTime.MinValue,
				Status = new Status() { ID = 0, Name = StatusEnum.Open.ToString() },
				TimeTracking = new TimeTracking()
				{
					Issue = this,
					OriginalEstimateSeconds = 0,
					RemainingEstimateSeconds = 0
				}
			};

			Summary = summary;
			ERPCode = "";
			Rank = 0;
			Reporter = User.UndefinedUser;
			Assignee = User.UndefinedUser;

			Issues = new List<Issue>();
			EstimateInSeconds = 0;
			TimeSpentInSeconds = 0;
		}

		public static Epic FromIssue(Issue issue)
		{
			return new Epic(issue.Key, issue.Fields, issue.GetJira());
		}
	}
}
