using Jira.SDK.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Assert.Equal(1, secondIssue.GetWorklogs().Count);

            Assert.Equal("mwillem", secondIssue.GetWorklogs()[0].Author.Username);

            Assert.Equal("http://jira.example.com/browse/ITDEV-6", secondIssue.Url);

        }

        [Fact]
        public void UpdateIssueSummaryTest()
        {
            MockJiraClient mockClient = new MockJiraClient();
            Jira jira = new Jira();
            jira.Connect(mockClient);

            Issue issue = mockClient.GetIssue("ITDEV-7");
            issue.SetJira(jira);
            issue.UpdateSummary("New summary");

            issue = mockClient.GetIssue("ITDEV-7");

            Assert.Equal(issue.Summary, "New summary");
        }
    }
}
