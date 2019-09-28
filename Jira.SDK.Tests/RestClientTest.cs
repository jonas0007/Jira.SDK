using FakeItEasy;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Jira.SDK.Tests
{
    public class RestClientTest
	{
		private JiraClient FakeJiraClient
		{
			get
			{
				return new JiraClient(A.Fake<RestClient>());
			}
		}

		[Fact]
		///Check if the technical loadout from a restrequest send using RestSharp is correct.
		public void RequestTechnicalLoadoutTest()
		{
			RestRequest request = FakeJiraClient.GetRequest(JiraClient.JiraObjectEnum.Projects, new Dictionary<String, String>(), new Dictionary<String, String>());

			//Check if the rest method is GET
			Assert.Equal(Method.GET, request.Method);

			//Check if the requested format is JSON
			Assert.Equal(DataFormat.Json, request.RequestFormat);

			//Check if the content type of the response is JSON
			RestResponse response = new RestResponse();
			request.OnBeforeDeserialization(response);

			Assert.Equal("application/json", response.ContentType);
		}

		[Fact]
		public void GetProjectsRestTest()
		{
			RestRequest request = FakeJiraClient.GetRequest(JiraClient.JiraObjectEnum.Projects, new Dictionary<String, String>(), new Dictionary<String, String>());

			Assert.Equal("/rest/api/latest/project/", request.Resource);
		}

		[Fact]
		public void GetProjectRestTest()
		{
			RestRequest request = FakeJiraClient.GetRequest(JiraClient.JiraObjectEnum.Projects, new Dictionary<String, String>(), new Dictionary<String, String>() { { "projectKey", "TESTKEY" } });

			Assert.Equal("/rest/api/latest/project/", request.Resource);

			Assert.NotNull(request.Parameters);
			Assert.Single(request.Parameters);
			Assert.Equal("projectKey", request.Parameters.First().Name);
			Assert.Equal("TESTKEY", request.Parameters.First().Value);
			Assert.Equal(ParameterType.UrlSegment, request.Parameters.First().Type);
		}

		[Fact]
		public void GetProjectVersionsRestTest()
		{
			RestRequest request = FakeJiraClient.GetRequest(JiraClient.JiraObjectEnum.ProjectVersions, new Dictionary<String, String>(), new Dictionary<String, String>() { { "projectKey", "TESTKEY" } });

			Assert.Equal("/rest/api/latest/project/{projectKey}/versions/", request.Resource);

			Assert.NotNull(request.Parameters);
			Assert.Single(request.Parameters);
			Assert.Equal("projectKey", request.Parameters.First().Name);
			Assert.Equal("TESTKEY", request.Parameters.First().Value);
			Assert.Equal(ParameterType.UrlSegment, request.Parameters.First().Type);
		}

		[Fact]
		public void GetUserRestTest()
		{
			RestRequest request = FakeJiraClient.GetRequest(JiraClient.JiraObjectEnum.User, new Dictionary<String, String>() { { "username", "testuser" } }, new Dictionary<String, String>());

			Assert.Equal("/rest/api/latest/user/", request.Resource);

			Assert.NotNull(request.Parameters);
			Assert.Single(request.Parameters);
			Assert.Equal("username", request.Parameters.First().Name);
			Assert.Equal("testuser", request.Parameters.First().Value);
			Assert.Equal(ParameterType.GetOrPost, request.Parameters.First().Type);
		}

		[Fact]
		public void GetAssignableUsersRestTest()
		{
			RestRequest request = FakeJiraClient.GetRequest(JiraClient.JiraObjectEnum.AssignableUser, new Dictionary<String, String>() { { "project", "TESTKEY" } }, new Dictionary<String, String>());

			Assert.Equal("/rest/api/latest/user/assignable/search/", request.Resource);

			Assert.NotNull(request.Parameters);
			Assert.Single(request.Parameters);
			Assert.Equal("project", request.Parameters.First().Name);
			Assert.Equal("TESTKEY", request.Parameters.First().Value);
			Assert.Equal(ParameterType.GetOrPost, request.Parameters.First().Type);
		}

		[Fact]
		public void GetAgileBoardsRestTest()
		{
			RestRequest request = FakeJiraClient.GetRequest(JiraClient.JiraObjectEnum.AgileBoards, new Dictionary<String, String>(), new Dictionary<String, String>() { { "boardID", "1" } });

			Assert.Equal("/rest/greenhopper/latest/rapidviews/list/", request.Resource);
		}

		[Fact]
		public void GetSprintsFromAgileBoardRestTest()
		{
			RestRequest request = FakeJiraClient.GetRequest(JiraClient.JiraObjectEnum.Sprints, new Dictionary<String, String>(), new Dictionary<String, String>() { { "boardID", "1" } });

			Assert.Equal("/rest/greenhopper/latest/sprintquery/{boardID}/", request.Resource);

			Assert.NotNull(request.Parameters);
			Assert.Single(request.Parameters);
			Assert.Equal("boardID", request.Parameters.First().Name);
			Assert.Equal("1", request.Parameters.First().Value);
			Assert.Equal(ParameterType.UrlSegment, request.Parameters.First().Type);
		}

		[Fact]
		public void GetIssueRestTest()
		{
			RestRequest request = FakeJiraClient.GetRequest(JiraClient.JiraObjectEnum.Issue, new Dictionary<String, String>(), new Dictionary<String, String>() { { "issueKey", "1" } });

			Assert.Equal("/rest/api/latest/issue/{issueKey}/", request.Resource);

			Assert.NotNull(request.Parameters);
			Assert.Single(request.Parameters);
			Assert.Equal("issueKey", request.Parameters.First().Name);
			Assert.Equal("1", request.Parameters.First().Value);
			Assert.Equal(ParameterType.UrlSegment, request.Parameters.First().Type);
		}

		[Fact]
		public void GetIssuesFromProjectVersionRestTest()
		{
			RestRequest request = FakeJiraClient.GetRequest(JiraClient.JiraObjectEnum.Issues, new Dictionary<String, String>() { { "jql", "project=\"PROJECTKEY\"&fixversion=\"fixversion\"" }, { "maxResults", "200" } }, new Dictionary<String, String>());

			Assert.Equal("/rest/api/latest/search/", request.Resource);

			Assert.NotNull(request.Parameters);
			Assert.Equal(2, request.Parameters.Count);
			Assert.Equal("jql", request.Parameters.First().Name);
			Assert.Equal("project=\"PROJECTKEY\"&fixversion=\"fixversion\"", request.Parameters.First().Value);
			Assert.Equal(ParameterType.GetOrPost, request.Parameters.First().Type);

			Assert.Equal("maxResults", request.Parameters.Last().Name);
			Assert.Equal("200", request.Parameters.Last().Value);
			Assert.Equal(ParameterType.GetOrPost, request.Parameters.Last().Type);
		}

		[Fact]
		public void GetWorkLogsRestTest()
		{
			RestRequest request = FakeJiraClient.GetRequest(JiraClient.JiraObjectEnum.Worklog, new Dictionary<String, String>(), new Dictionary<String, String>() { { "issueKey", "1" } });

			Assert.Equal("/rest/api/latest/issue/{issueKey}/worklog/", request.Resource);

			Assert.NotNull(request.Parameters);
			Assert.Single(request.Parameters);
			Assert.Equal("issueKey", request.Parameters.First().Name);
			Assert.Equal("1", request.Parameters.First().Value);
			Assert.Equal(ParameterType.UrlSegment, request.Parameters.First().Type);
		}
	}
}
