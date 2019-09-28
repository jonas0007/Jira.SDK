﻿using Jira.SDK.Domain;
using Xunit;

namespace Jira.SDK.Tests
{
    public class IssueTests
	{
		[Fact]
		public void GetWorklogTest()
		{
			MockJiraClient mockClient = new MockJiraClient();
			Jira jira = new Jira();
			jira.Connect(mockClient);

			//The first test issue contains 2 worklogs. One from Jonas, One from Marc
			Issue firstIssue = mockClient.GetIssue("ITDEV-7");

			firstIssue.SetJira(jira);

			Assert.NotNull(firstIssue.GetWorklogs());
			Assert.Equal(2, firstIssue.GetWorklogs().Count);

			Assert.Equal("jverdick", firstIssue.GetWorklogs()[0].Author.Username);
			Assert.Equal("mwillem", firstIssue.GetWorklogs()[1].Author.Username);

			//The second test issue contains 1 worklogs from Marc
			Issue secondIssue = mockClient.GetIssue("ITDEV-6");
			secondIssue.SetJira(jira);

			Assert.NotNull(secondIssue.GetWorklogs());
			Assert.Single(secondIssue.GetWorklogs());

			Assert.Equal("mwillem", secondIssue.GetWorklogs()[0].Author.Username);

			Assert.Equal("http://jira.example.com/browse/ITDEV-6", secondIssue.Url);

		}
	}
}
