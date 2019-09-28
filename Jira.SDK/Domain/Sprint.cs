using System;
using System.Collections.Generic;
using System.Linq;
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
			return GetAssignableUsers(additionalIssues, this.StartDate, this.EndDate).Where(user => !user.Username.StartsWith("svc_")).ToList();
		}

		public List<User> GetAssignableUsers(List<Issue> additionalIssues, DateTime from, DateTime until)
		{
			if (_users == null)
			{
				List<Project> projects = GetIssues().Select(issue => _jira.GetProject(issue.Project.Key)).Distinct().ToList();
				_users = projects.SelectMany(proj => proj.AssignableUsers).Where(user => !user.Username.Contains("svc")).ToList();

				additionalIssues.AddRange(this.GetIssues());

				//Remove the double issues.
				additionalIssues.RemoveAll(issue => additionalIssues.Where(doubleissue => doubleissue.Equals(issue)).Count() > 1);

				_users.ForEach(user =>
				{
					user.SetJira(this._jira);
					user.IsProjectLead = projects.Any(project => project.ProjectLead.Equals(user));
					user.SetWorkDays(additionalIssues, from, until);
				});
			}
			return _users;
		}

		public List<Epic> GetEpics()
		{
			Epic undefinedEpic = Epic.UndefinedEpic;
			undefinedEpic.SetJira(this.GetJira());

			Dictionary<Epic, List<Issue>> issuesByEpic = GetIssues().GroupBy(issue => issue.Epic != null ? issue.Epic : undefinedEpic).ToDictionary(group => group.Key, group => group.ToList());
			List<Epic> epics = new List<Epic>();
			foreach (Epic epic in issuesByEpic.Keys)
			{
				epic.LoadIssues(issuesByEpic[epic], this.StartDate, this.EndDate);
				epics.Add(epic);
			}

			return epics;
		}

		#region Equality
		public override int GetHashCode()
		{
			return this.ID.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			Boolean equals = false;
			if (obj is Sprint && (obj as Sprint).ID == ID)
			{
				equals = true;
			}

			return equals;
		}
		#endregion
	}
}
