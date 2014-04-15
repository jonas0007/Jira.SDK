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
		public JiraEnvironment Environment {get;set;}

		public List<Sprint> GetSprints()
		{
			List<Sprint> sprints = Environment.Client.GetSprintsFromAgileBoard(this.ID);
			sprints.ForEach(sprint => sprint.Environment = this.Environment);
			return sprints;
		}
	}
}
