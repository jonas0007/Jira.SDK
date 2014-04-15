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

		public JiraEnvironment Environment { get; set; }

		private List<Issue> _issues;
		public List<Issue> GetIssues()
		{
			return _issues ?? (_issues = Environment.Client.GetIssuesFromSprint(this.ID));
		}
	}
}
