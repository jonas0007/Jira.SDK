using Jira.SDK.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Jira.SDK.Tests
{
	public class AgileBoardTests
	{
		[Fact]
		public void GetAgileBoardTest()
		{
			Jira environment = new Jira();
			environment.Connect(new MockJiraClient());

			List<AgileBoard> agileboards = environment.GetAgileBoards();

			Assert.NotNull(agileboards);
			Assert.Equal(3, agileboards.Count);
		}

		[Fact]
		public void GetSprintsFromAgileBoardTest()
		{
			Jira environment = new Jira();
			environment.Connect(new MockJiraClient());

			AgileBoard agileboard = environment.GetAgileBoards().First();
			Assert.Equal(1, agileboard.ID);

			List<Sprint> sprints = agileboard.GetSprints();

			Assert.NotNull(sprints);
			Assert.Equal(3, sprints.Count);
		}

		[Fact]
		public void GetIssuesFromSprintTest()
		{
			Jira environment = new Jira();
			environment.Connect(new MockJiraClient());

			//Get the first agile board
			AgileBoard agileboard = environment.GetAgileBoards().First();
			Assert.Equal(1, agileboard.ID);

			//Get the first sprint from the first agile board
			List<Sprint> sprints = agileboard.GetSprints();
			Assert.Equal(3, sprints.Count);

            //Get a sprint and try to select an issue.
            Sprint sprint = sprints.Where(s => s.ID == 1).First();

			List<Issue> issues = sprint.GetIssues();
			Assert.NotNull(issues);
			Assert.Equal(4, issues.Count);
		}
	}
}
