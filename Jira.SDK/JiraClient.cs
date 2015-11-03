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
using System.Reflection;
using Jira.SDK.Tools;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp.Authenticators;

namespace Jira.SDK
{
    public class JiraClient : IJiraClient
    {
        public enum JiraObjectEnum
        {
            Fields,
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
            BacklogSprints,
            Sprint,
            SprintIssues,
            Filters,
            Transitions,
            ProjectComponents
        }

        private RestClient Client { get; set; }

        private const String JiraAPIServiceURI = "/rest/api/latest";
        private const String JiraAgileServiceURI = "/rest/greenhopper/latest";

        private Dictionary<JiraObjectEnum, String> _methods = new Dictionary<JiraObjectEnum, String>()
        {
            {JiraObjectEnum.Fields, String.Format("{0}/field/", JiraAPIServiceURI)},
            {JiraObjectEnum.Projects, String.Format("{0}/project/", JiraAPIServiceURI)},
            {JiraObjectEnum.Project, String.Format("{0}/project/{{projectKey}}/", JiraAPIServiceURI)},
            {JiraObjectEnum.ProjectVersions, String.Format("{0}/project/{{projectKey}}/versions/", JiraAPIServiceURI)},
            {JiraObjectEnum.AssignableUser, String.Format("{0}/user/assignable/search/", JiraAPIServiceURI)},
            {JiraObjectEnum.Issue, String.Format("{0}/issue/{{issueKey}}/", JiraAPIServiceURI)},
            {JiraObjectEnum.Issues, String.Format("{0}/search/", JiraAPIServiceURI)},
            {JiraObjectEnum.Worklog, String.Format("{0}/issue/{{issueKey}}/worklog/", JiraAPIServiceURI)},
            {JiraObjectEnum.Transitions, String.Format("{0}/issue/{{issueKey}}/transitions/", JiraAPIServiceURI)},
            {JiraObjectEnum.User, String.Format("{0}/user/", JiraAPIServiceURI)},
            {JiraObjectEnum.Filters, String.Format("{0}/filter/favourite", JiraAPIServiceURI)},
            {JiraObjectEnum.AgileBoards, String.Format("{0}/rapidviews/list/", JiraAgileServiceURI)},
            {JiraObjectEnum.Sprints, String.Format("{0}/sprintquery/{{boardID}}/", JiraAgileServiceURI)},
            {JiraObjectEnum.BacklogSprints, String.Format("{0}/xboard/plan/backlog/data.json", JiraAgileServiceURI)},
            {JiraObjectEnum.Sprint, String.Format("{0}/rapid/charts/sprintreport/", JiraAgileServiceURI)},
            {JiraObjectEnum.SprintIssues, String.Format("{0}/sprintquery/", JiraAgileServiceURI)},
            {JiraObjectEnum.ProjectComponents, String.Format("{0}/project/{{projectKey}}/components/", JiraAPIServiceURI)}
        };

        public JiraClient(RestClient client)
        {
            Client = client;
        }

        public JiraClient(String url)
        {
            Client = new RestClient(url);
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
            return GetIssues(_methods[JiraObjectEnum.Issues], new Dictionary<String, String>() { { "jql", jql }, { "maxResults", "700" }, { "fields", "*all" }, { "expand", "transitions" } });
        }

        #region Fields
        public List<Field> GetFields()
        {
            return GetList<Field>(JiraObjectEnum.Fields);
        }
        #endregion

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

        #region Project components
        public List<ProjectComponent> GetProjectComponents(String projectKey)
        {
            return GetList<ProjectComponent>(JiraObjectEnum.ProjectComponents,
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
            return GetItem<AgileBoardView>(JiraObjectEnum.AgileBoards).Views;
        }

        public List<Sprint> GetSprintsFromAgileBoard(Int32 agileBoardID)
        {
            return GetItem<SprintResult>(JiraObjectEnum.Sprints, keys: new Dictionary<String, String>() { { "boardID", agileBoardID.ToString() } }).Sprints;
        }

        public List<Sprint> GetBacklogSprintsFromAgileBoard(Int32 agileBoardID)
        {
            return GetItem<SprintResult>(JiraObjectEnum.BacklogSprints, parameters: new Dictionary<String, String>() { { "rapidViewId", agileBoardID.ToString() } }).Sprints;
        }

        public Sprint GetSprint(Int32 agileBoardID, Int32 sprintID)
        {
            return GetItem<SprintResult>(JiraObjectEnum.Sprint, parameters: new Dictionary<String, String>() { { "rapidViewId", agileBoardID.ToString() }, { "sprintId", sprintID.ToString() } }).Sprint;
        }

        public List<Issue> GetIssuesFromSprint(int sprintID)
        {
            return SearchIssues(String.Format("Sprint = {0}", sprintID));
        }
        #endregion

        #region Issues
        public Issue GetIssue(String key)
        {
            return GetIssue(_methods[JiraObjectEnum.Issue], keys: new Dictionary<String, String>() { { "issueKey", key } });
        }

        public List<Issue> GetSubtasksFromIssue(String issueKey)
        {
            return SearchIssues(String.Format("parent=\"{0}\"", issueKey));
        }

        public List<Issue> GetIssuesFromProjectVersion(String projectKey, String projectVersionName)
        {
            return SearchIssues(String.Format("project=\"{0}\"&fixversion=\"{1}\"",
                            projectKey, projectVersionName));
        }

        public List<IssueFilter> GetFavoriteFilters()
        {
            return GetList<IssueFilter>(JiraObjectEnum.Filters);
        }

        public List<Issue> GetIssuesWithEpicLink(String epicLink)
        {
            return SearchIssues(String.Format("'Epic Link' = {0}", epicLink));
        }

        public List<Issue> GetEpicIssuesFromProject(String projectName)
        {
            return SearchIssues(String.Format("project = '{0}' AND Type = Epic", projectName));
        }

        public Issue GetEpicIssueFromProject(String projectName, String epicName)
        {
            return SearchIssues(String.Format("project = '{0}' AND Type = Epic and 'Epic Name' = '{1}'", projectName, epicName)).FirstOrDefault();
        }

        public Issue AddIssue(IssueFields issueFields)
        {
            IRestRequest request = new RestRequest(String.Format("{0}/issue", JiraAPIServiceURI), Method.POST);
            request.RequestFormat = DataFormat.Json;

            JObject tempjson = JObject.FromObject(new
            {
                project = new
                {
                    id = issueFields.Project.ID
                },
                issuetype = new
                {
                    id = issueFields.IssueType.ID
                },
                summary = issueFields.Summary,
                //description = issueFields.Description,

                //User Reporter { get; set; }
                //User Assignee { get; set; }
            });

            List<Field> fields = GetFields();
            

            foreach (KeyValuePair<String, CustomField> customfield in issueFields.CustomFields)
            {
                Field field = fields.Where(f => f.ID.Equals(customfield.Key)).FirstOrDefault();
                
                switch(field.Schema.Custom)
                {
                    case "com.atlassian.jira.plugin.system.customfieldtypes:select":
                        tempjson.Add(customfield.Key.ToLower(), JToken.FromObject(new { value = customfield.Value.Value }));
                        break;
                    default:
                        tempjson.Add(customfield.Key.ToLower(), customfield.Value.Value );
                        break;
                }
            }

            JObject json = new JObject();
            json.Add("fields", tempjson.Root);

            request.AddParameter("Application/Json", json.ToString(), ParameterType.RequestBody);

            IRestResponse<Issue> response = Client.Post<Issue>(request);

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

        public Comment AddCommentToIssue(Issue issue, Comment comment)
        {
            IRestRequest request = new RestRequest(String.Format("{0}/issue/{1}/comment", JiraAPIServiceURI, issue.Key), Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { body = comment.Body });

            IRestResponse<Comment> response = Client.Post<Comment>(request);

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
        #endregion

        #region Worklog
        public WorklogSearchResult GetWorkLogs(String issueKey)
        {
            return GetItem<WorklogSearchResult>(JiraObjectEnum.Worklog,
                       keys: new Dictionary<String, String>() { { "issueKey", issueKey } });
        }
        #endregion

        #region Transition
        public List<Transition> GetTransitions(string issueKey)
        {
            return GetItem<TransitionSearchResult>(JiraObjectEnum.Transitions,
                       keys: new Dictionary<String, String>() { { "issueKey", issueKey } }).Transitions;
        }

        public void TransitionIssue(Issue issue, Transition transition, Comment comment)
        {
            IRestRequest request = new RestRequest(String.Format("{0}/issue/{1}/transitions", JiraAPIServiceURI, issue.Key), Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(
                new
                {
                    transition = new
                    {
                        id = transition.ID.ToString()
                    },
                    update = new
                    {
                        comment = new[]{
                            new {
                                add = new {
                                    body = comment.Body
                                }
                            }
                        }
                    }
                });

            IRestResponse response = Client.Post(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception(response.ErrorMessage);
            }
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

            return GetRequest(_methods[objectType], parameters, keys);
        }

        public RestRequest GetRequest(String url, Dictionary<String, String> parameters, Dictionary<String, String> keys)
        {
            RestRequest request = new RestRequest(url, Method.GET)
            {
                RequestFormat = DataFormat.Json,
                OnBeforeDeserialization = resp => resp.ContentType = "application/json",
                JsonSerializer = new RestSharpJsonNetSerializer()
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

        private List<Issue> GetIssues(String url, Dictionary<String, String> parameters = null, Dictionary<String, String> keys = null)
        {
            IRestResponse response = Client.Execute(GetRequest(url, parameters ?? new Dictionary<String, String>(), keys ?? new Dictionary<String, String>()));

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception(response.ErrorMessage);
            }

            return DeserializeIssues(response.Content);
        }

        private Issue GetIssue(String url, Dictionary<String, String> parameters = null, Dictionary<String, String> keys = null)
        {
            IRestResponse response = Client.Execute(GetRequest(url, parameters ?? new Dictionary<String, String>(), keys ?? new Dictionary<String, String>()));

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception(response.ErrorMessage);
            }

            return DeserializeIssue(response.Content);
        }

        private List<Issue> DeserializeIssues(String json)
        {
            JObject jsonObject = JObject.Parse(json);

            return new IssueSearchResult(jsonObject).Issues;
        }

        private Issue DeserializeIssue(String json)
        {
            JObject jsonObject = JObject.Parse(json);

            return new Issue((String)jsonObject["key"], (JObject)jsonObject["fields"]);
        }
        #endregion
    }
}
