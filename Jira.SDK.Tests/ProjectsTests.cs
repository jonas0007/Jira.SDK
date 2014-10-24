using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FakeItEasy;
using Jira.SDK.Domain;

namespace Jira.SDK.Tests
{
	
    public class ProjectTests
    {
		[Fact]
		public void GetProjectsTest()
		{
			Jira environment = new Jira();
			environment.Connect(new MockJiraClient());

			List<Project> projects = environment.GetProjects();

			Assert.NotNull(projects);
			Assert.Equal(2, projects.Count);
		}

		[Fact]
		public void GetExistingProjectTest()
		{
			Jira environment = new Jira();
			environment.Connect(new MockJiraClient());

			Project project = environment.GetProject("ITDEV");

			Assert.NotNull(project);
			Assert.Equal(project.Key, "ITDEV");
		}

		[Fact]
		public void GetUnknownProjectTest()
		{
			Jira environment = new Jira();
			environment.Connect(new MockJiraClient());

			Project project = environment.GetProject("NOTEXISTING");

			Assert.Null(project);
		}

		[Fact]
		public void ProjectLeadTest()
		{
			Jira environment = new Jira();
			environment.Connect(new MockJiraClient());

			Project project = environment.GetProject("ITDEV");

			Assert.NotNull(project);
			Assert.NotNull(project.ProjectLead);
			Assert.Equal("jverdick", project.ProjectLead.Username);
		}
    }
}
