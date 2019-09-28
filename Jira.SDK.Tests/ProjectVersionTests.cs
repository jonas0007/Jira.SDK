using Jira.SDK.Domain;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Jira.SDK.Tests
{
	public class ProjectVersionTests
	{
		[Fact]
		public void GetProjectVersions()
		{
			Jira environment = new Jira();
			environment.Connect(new MockJiraClient());

			Project project = environment.GetProject("ITDEV");

			Assert.NotNull(project);
			Assert.Equal("ITDEV", project.Key);

			List<ProjectVersion> projectVersions = project.ProjectVersions;

			Assert.NotNull(projectVersions);
			Assert.Equal(2, projectVersions.Count);
		}

		[Fact]
		public void GetProjectVersionIssuesTest()
		{
			Jira environment = new Jira();
			environment.Connect(new MockJiraClient());

			Project project = environment.GetProject("ITDEV");

			Assert.NotNull(project);
			Assert.Equal("ITDEV", project.Key);

			ProjectVersion firstProjectVersion = project.ProjectVersions.FirstOrDefault();

			Assert.NotNull(firstProjectVersion);
			Assert.NotNull(firstProjectVersion.Issues);
			Assert.Equal(2, firstProjectVersion.Issues.Count);

			ProjectVersion lastProjectVersion = project.ProjectVersions.LastOrDefault();

			Assert.NotNull(lastProjectVersion);
			Assert.NotNull(lastProjectVersion.Issues);
			Assert.Equal(5, lastProjectVersion.Issues.Count);
		}
	}
}
