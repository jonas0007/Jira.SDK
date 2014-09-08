using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jira.SDK.Tools;

namespace Jira.SDK.Domain
{
	public class Sprint
	{
		public Int32 ID { get; set; }
		public String Name { get; set; }
		public String State { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime CompleteDate { get; set; }

		public List<DateTime> DaysWithinSprint
		{
			get
			{
				return StartDate.GetDaysUntil(EndDate, DateAndTimeExtentions.DaysBetweenOptions.IgnoreWeekends);
			}
		}

		private Jira _jira { get; set; }
		public Jira GetJira()
		{
			return _jira;
		}

		public void SetJira(Jira jira)
		{
			_jira = jira;
		}

		private List<Issue> _issues;
		public List<Issue> GetIssues()
		{
			if (_issues == null)
			{
				_issues = _jira.Client.GetIssuesFromSprint(this.ID);
				_issues.ForEach(issue => issue.SetJira(_jira));
			}
			return _issues;
		}

		private List<User> _users;
		public List<User> GetAssignableUsers(List<Issue> additionalIssues)
		{
			if (_users == null)
			{
				List<Project> projects = GetIssues().Select(issue => _jira.GetProject(issue.Project.Key)).Distinct().ToList();
				_users = projects.SelectMany(proj => proj.AssignableUsers).ToList();

				additionalIssues.AddRange(this.GetIssues());

				_users.ForEach(user =>
				{
					user.SetJira(this._jira);
					user.IsProjectLead = projects.Any(project => project.ProjectLead.Equals(user));
					user.SetWorkDays(additionalIssues, this.StartDate, this.EndDate);
				});
			}
			return _users;
		}
	}
}
