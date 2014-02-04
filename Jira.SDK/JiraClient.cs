using System.Dynamic;
using System.Net.Cache;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Extensions;

namespace Jira.SDK
{
    internal class JiraClient
    {
        private RestClient Client { get; set; }

        public enum JiraObjectEnum
        {
            Projects,
            Project,
            AssignableUser,
            ProjectVersions,
            Issue,
            Issues,
            Worklog,
			User
        }

        private Dictionary<JiraObjectEnum, String> _methods = new Dictionary<JiraObjectEnum, String>()
        {
            {JiraObjectEnum.Projects,"project"},
            {JiraObjectEnum.Project,"project/{projectKey}"},
            {JiraObjectEnum.ProjectVersions,"project/{projectKey}/versions"},
            {JiraObjectEnum.AssignableUser,"user/assignable/search"},
            {JiraObjectEnum.Issue,"issue/{issueKey}"},
            {JiraObjectEnum.Issues,"search"},
            {JiraObjectEnum.Worklog,"issue/{issueKey}/worklog"},
			{JiraObjectEnum.User, "user"}
        };

        public JiraClient(String url, String username, String password)
        {
            Client = new RestClient(url)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
            };
        }

        public List<Issue> SearchIssues(String jql)
        {
            return GetItem<IssueSearchResult>(JiraObjectEnum.Issues, new Dictionary<String, String>() { { "jql", jql }, { "maxResults", "200"} }).Issues;
        }

        public T GetItem<T>(JiraObjectEnum objectType, Dictionary<String, String> parameters = null,
            Dictionary<String, String> keys = null) where T : new()
        {
            return Execute<T>(objectType, parameters, keys);
        }

        public List<T> GetList<T>(JiraObjectEnum objectType, Dictionary<String, String> parameters = null, Dictionary<String, String> keys = null) where T : new()
        {
            return Execute<List<T>>(objectType, parameters, keys);
        }

        private T Execute<T>(JiraObjectEnum objectType, Dictionary<String, String> parameters = null, Dictionary<String, String> keys = null) where T : new()
        {
            RestResponse<T> response = ((RestResponse<T>)Client.Execute<T>(GetRequest(objectType, parameters ?? new Dictionary<String, String>(), keys ?? new Dictionary<String, String>())));

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

        private RestRequest GetRequest(JiraObjectEnum objectType, Dictionary<String, String> parameters,
            Dictionary<String, String> keys)
        {
            if (!_methods.ContainsKey(objectType))
                throw new NotImplementedException();

            RestRequest request = new RestRequest(String.Format("/rest/api/latest/{0}/", _methods[objectType]), Method.GET)
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
    };
}
