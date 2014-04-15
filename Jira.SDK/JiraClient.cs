using System.Dynamic;
using System.Net.Cache;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Extensions;
using Jira.SDK.Domain;

namespace Jira.SDK
{
	public class JiraClient : IJiraClient
	{
		public enum JiraObjectEnum
		{
			Projects,
			Project,
			AssignableUser,
			ProjectVersions,
			Issue,
			Issues,
			Worklog,
			User,
			AgileBoards,
			Sprints,
			SprintIssues
		}

		private RestClient Client { get; set; }

		private const String JiraAPIServiceURI = "/rest/api/latest";
		private const String JiraAgileServiceURI = "/rest/greenhopper/latest";

		private Dictionary<JiraObjectEnum, String> _methods = new Dictionary<JiraObjectEnum, String>()
        {
            {JiraObjectEnum.Projects, String.Format("{0}/project/", JiraAPIServiceURI)},
            {JiraObjectEnum.Project, String.Format("{0}/project/{{projectKey}}/", JiraAPIServiceURI)},
            {JiraObjectEnum.ProjectVersions, String.Format("{0}/project/{{projectKey}}/versions/", JiraAPIServiceURI)},
            {JiraObjectEnum.AssignableUser, String.Format("{0}/user/assignable/search/", JiraAPIServiceURI)},
            {JiraObjectEnum.Issue, String.Format("{0}/issue/{{issueKey}}/", JiraAPIServiceURI)},
            {JiraObjectEnum.Issues, String.Format("{0}/search/", JiraAPIServiceURI)},
            {JiraObjectEnum.Worklog, String.Format("{0}/issue/{{issueKey}}/worklog/", JiraAPIServiceURI)},
			{JiraObjectEnum.User, String.Format("{0}/user/", JiraAPIServiceURI)},
			{JiraObjectEnum.AgileBoards, String.Format("{0}/rapidviews/list/", JiraAgileServiceURI)},
			{JiraObjectEnum.Sprints, String.Format("{0}/sprintquery/{{boardID}}/", JiraAgileServiceURI)},
			{JiraObjectEnum.SprintIssues, String.Format("{0}/sprintquery/", JiraAgileServiceURI)}
        };

		public JiraClient(RestClient client)
		{
			Client = client;
		}

		public JiraClient(String url, String username, String password)
		{
			Client = new RestClient(url)
			{
				Authenticator = new HttpBasicAuthenticator(username, password)
			};
		}

		private List<Issue> SearchIssues(String jql)
		{
			return GetItem<IssueSearchResult>(JiraObjectEnum.Issues, new Dictionary<String, String>() { { "jql", jql }, { "maxResults", "200" } }).Issues;
		}

		#region Projects
		public List<Project> GetProjects()
		{
			return GetList<Project>(JiraObjectEnum.Projects);
		}

		public Project GetProject(String projectKey)
		{
			return GetList<Project>(JiraObjectEnum.Project, keys: new Dictionary<string, string>() { { "projectKey", projectKey } }).FirstOrDefault();
		}
		#endregion

		#region Project versions
		public List<ProjectVersion> GetProjectVersions(String projectKey)
		{
			return GetList<ProjectVersion>(JiraObjectEnum.ProjectVersions,
							   keys: new Dictionary<string, string>() { { "projectKey", projectKey } });
		}
		#endregion

		#region Users
		public User GetUser(String username)
		{
			return GetItem<User>(JiraObjectEnum.User, new Dictionary<string, string>() { { "username", username } });
		}

		public List<User> GetAssignableUsers(String projectKey)
		{
			return GetList<User>(JiraObjectEnum.AssignableUser,
							   parameters: new Dictionary<string, string>() { { "project", projectKey } });
		}
		#endregion

		#region Agile boards
		public List<AgileBoard> GetAgileBoards()
		{
			return GetList<AgileBoard>(JiraObjectEnum.AgileBoards);
		}

		public List<Sprint> GetSprintsFromAgileBoard(Int32 agileBoardID)
		{
			return GetList<Sprint>(JiraObjectEnum.Sprints, keys: new Dictionary<String, String>() { { "boardID", agileBoardID.ToString() } });
		}

		public List<Issue> GetIssuesFromSprint(int sprintID)
		{
			return SearchIssues(String.Format("Sprint = {0}", sprintID));
		}
		#endregion

		#region Issues
		public Issue GetIssue(String key)
		{
			return GetItem<Issue>(JiraObjectEnum.Issue, keys: new Dictionary<String, String>() { { "issueKey", key } });
		}

		public List<Issue> GetIssuesFromProjectVersion(String projectKey, String projectVersionName)
		{
			return SearchIssues(String.Format("project=\"{0}\"&fixversion=\"{1}\"",
							projectKey, projectVersionName));
		}
		#endregion

		#region Worklog
		public WorklogSearchResult GetWorkLogs(String issueKey)
		{
			return GetItem<WorklogSearchResult>(JiraObjectEnum.Worklog,
					   keys: new Dictionary<String, String>() { { "issueKey", issueKey } });
		}
		#endregion

		#region Jira communication
		private T GetItem<T>(JiraObjectEnum objectType, Dictionary<String, String> parameters = null,
			Dictionary<String, String> keys = null) where T : new()
		{
			return Execute<T>(objectType, parameters, keys);
		}

		private List<T> GetList<T>(JiraObjectEnum objectType, Dictionary<String, String> parameters = null, Dictionary<String, String> keys = null) where T : new()
		{
			return Execute<List<T>>(objectType, parameters, keys);
		}

		private T Execute<T>(JiraObjectEnum objectType, Dictionary<String, String> parameters = null, Dictionary<String, String> keys = null) where T : new()
		{
			IRestResponse<T> response = Client.Execute<T>(GetRequest(objectType, parameters ?? new Dictionary<String, String>(), keys ?? new Dictionary<String, String>()));

			if (response.ErrorException != null)
			{
				throw response.ErrorException;
			}
			if (response.ResponseStatus != ResponseStatus.Completed)
			{
				throw new Exception(response.ErrorMessage);
			}

			return response.Data;
		}

		public RestRequest GetRequest(JiraObjectEnum objectType, Dictionary<String, String> parameters,
			Dictionary<String, String> keys)
		{
			if (!_methods.ContainsKey(objectType))
				throw new NotImplementedException();

			RestRequest request = new RestRequest(_methods[objectType], Method.GET)
			{
				RequestFormat = DataFormat.Json,
				OnBeforeDeserialization = resp => resp.ContentType = "application/json"
			};

			foreach (KeyValuePair<String, String> key in keys)
			{
				request.AddParameter(key.Key, key.Value, ParameterType.UrlSegment);
			}

			foreach (KeyValuePair<String, String> parameter in parameters)
			{
				request.AddParameter(parameter.Key, parameter.Value);
			}

			return request;
		}
		#endregion
	};
}
