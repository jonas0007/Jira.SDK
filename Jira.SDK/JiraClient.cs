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

        }

        public JiraClient(String url, String username, String password)
        {
            Client = new RestClient(url)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
            };
        }

        public List<Issue> SearchIssues(String jql)
        {
            return GetItem<IssueSearchResult>(JiraObjectEnum.Issues, new Dictionary<String, String>() { { "jql", jql } }).Issues;
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
            RestRequest request = new RestRequest(GetMethodForObject(objectType), Method.GET)
            {
                RequestFormat = DataFormat.Json
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

        private String GetMethodForObject(JiraObjectEnum objectType)
        {
            String method = "";
            switch (objectType)
            {
                case JiraObjectEnum.Projects:
                    method = "project";
                    break;
                case JiraObjectEnum.Project:
                    method = "project/{projectKey}";
                    break;
                case JiraObjectEnum.ProjectVersions:
                    method = "project/{projectKey}/versions";
                    break;
                case JiraObjectEnum.AssignableUser:
                    method = "user/assignable/search";
                    break;
                case JiraObjectEnum.Issue:
                    method = "issue/{issueKey}";
                    break;
                case JiraObjectEnum.Issues:
                    method = "search";
                    break;
                default:
                    throw new NotImplementedException();
            }

            return String.Format("/rest/api/latest/{0}/", method);
        }
    };
}
