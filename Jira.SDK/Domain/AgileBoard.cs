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
		private Jira _jira { get; set; }
		public Jira GetJira()
		{
			return _jira;
		}

		public void SetJira(Jira jira)
		{
			_jira = jira;
		}

		private List<Sprint> _sprints;
		public List<Sprint> GetSprints()
		{
			if (_sprints == null)
			{
				_sprints = GetDetailedSprints(_jira.Client.GetSprintsFromAgileBoard(this.ID).OrderByDescending(sprint => sprint.Name).ToList());
			}
			return _sprints;
		}

		private List<Sprint> _backlogsprints;
		public List<Sprint> GetBacklogSprints()
		{
			if (_backlogsprints == null)
			{
				_backlogsprints = GetDetailedSprints(_jira.Client.GetBacklogSprintsFromAgileBoard(this.ID));
			}
			return _backlogsprints;
		}

		private List<Sprint> GetDetailedSprints(List<Sprint> sprints)
		{
			List<Sprint> detailedSprints = new List<Sprint>();
			foreach (Sprint sprint in sprints)
			{
				detailedSprints.Add(GetSprint(sprint.ID));
			}

			return detailedSprints;
		}

		public Sprint GetSprint(Int32 sprintID)
		{
			Sprint sprint = _jira.Client.GetSprint(this.ID, sprintID);
			sprint.SetJira(_jira);

			return sprint;
		}
	}
}
