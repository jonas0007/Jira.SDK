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
		public JiraEnvironment Environment { get; set; }

		private List<Sprint> _sprints;
		public List<Sprint> GetSprints()
		{
			if (_sprints == null)
			{
				_sprints = Environment.Client.GetSprintsFromAgileBoard(this.ID);
				_sprints.ForEach(sprint => sprint.Environment = this.Environment);
			}
			return _sprints;
		}

		private List<Sprint> _backlogsprints;
		public List<Sprint> GetBacklogSprints()
		{
			if (_backlogsprints == null)
			{
				_backlogsprints = Environment.Client.GetBacklogSprintsFromAgileBoard(this.ID);
				_backlogsprints.ForEach(sprint => sprint.Environment = this.Environment);
			}
			return _backlogsprints;
		}

		public Sprint GetSprint(Int32 sprintID)
		{
			Sprint sprint = Environment.Client.GetSprint(this.ID, sprintID);
			sprint.Environment = this.Environment;

			return sprint;
		}
	}
}
