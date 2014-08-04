using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK.Domain
{
	public class AgileBoard
	{
		public Int32 ID { get; set; }
		public String Name { get; set; }
		public Boolean SprintSupport { get; set; }
		public Jira Jira { get; set; }

		private List<Sprint> _sprints;
		public List<Sprint> GetSprints()
		{
			if (_sprints == null)
			{
				_sprints = Jira.Client.GetSprintsFromAgileBoard(this.ID).OrderByDescending(sprint => sprint.Name).ToList();
				_sprints.ForEach(sprint => sprint.Jira = this.Jira);
			}
			return _sprints;
		}

		private List<Sprint> _backlogsprints;
		public List<Sprint> GetBacklogSprints()
		{
			if (_backlogsprints == null)
			{
				_backlogsprints = Jira.Client.GetBacklogSprintsFromAgileBoard(this.ID);
				_backlogsprints.ForEach(sprint => sprint.Jira = this.Jira);
			}
			return _backlogsprints;
		}

		public Sprint GetSprint(Int32 sprintID)
		{
			Sprint sprint = Jira.Client.GetSprint(this.ID, sprintID);
			sprint.Jira = this.Jira;

			return sprint;
		}
	}
}
