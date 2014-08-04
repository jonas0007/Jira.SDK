using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

		public JiraEnvironment Environment { get; set; }

		private List<Issue> _issues;
		public List<Issue> GetIssues()
		{
			if (_issues == null)
			{
				_issues = Environment.Client.GetIssuesFromSprint(this.ID);
				_issues.ForEach(issue => issue.JiraEnvironment = Environment);
			}
			return _issues;
		}

		private List<User> _users;
		public List<User> GetAssignableUsers()
		{
			List<Project> projects = GetIssues().Select(issue => Environment.GetProject(issue.Project.Key)).Distinct().ToList();
			return projects.SelectMany(proj => proj.AssignableUsers).ToList();
		}
	}
}
