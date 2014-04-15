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
	}
}
