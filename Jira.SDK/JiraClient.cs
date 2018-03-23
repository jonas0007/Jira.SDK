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
using System.Net;

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
            Group,
            AgileBoards,
            Sprints,
            BacklogSprints,
            Sprint,
            SprintIssues,
            Filters,
            Transitions,
            ProjectComponents,
            IssueSecuritySchemes,
            PermissionScheme,
            NotificationScheme,
            ProjectRoles,
            ProjectRole,
            ProjectCategories,
            ProjectTypes,
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
            {JiraObjectEnum.Group, String.Format("{0}/group", JiraAPIServiceURI) },
            {JiraObjectEnum.Filters, String.Format("{0}/filter/favourite", JiraAPIServiceURI)},
            {JiraObjectEnum.AgileBoards, String.Format("{0}/rapidviews/list/", JiraAgileServiceURI)},
            {JiraObjectEnum.Sprints, String.Format("{0}/sprintquery/{{boardID}}/", JiraAgileServiceURI)},
            {JiraObjectEnum.BacklogSprints, String.Format("{0}/xboard/plan/backlog/data.json", JiraAgileServiceURI)},
            {JiraObjectEnum.Sprint, String.Format("{0}/rapid/charts/sprintreport/", JiraAgileServiceURI)},
            {JiraObjectEnum.SprintIssues, String.Format("{0}/sprintquery/", JiraAgileServiceURI)},
            {JiraObjectEnum.ProjectComponents, String.Format("{0}/project/{{projectKey}}/components/", JiraAPIServiceURI)},
            {JiraObjectEnum.IssueSecuritySchemes, String.Format("{0}/issuesecurityschemes/", JiraAPIServiceURI)},
            {JiraObjectEnum.PermissionScheme, String.Format("{0}/permissionscheme/", JiraAPIServiceURI)},
            {JiraObjectEnum.NotificationScheme, String.Format("{0}/notificationscheme/", JiraAPIServiceURI)},
            {JiraObjectEnum.ProjectRoles, String.Format("{0}/project/{{projectKey}}/role/", JiraAPIServiceURI)},
            {JiraObjectEnum.ProjectRole, String.Format("{0}/project/{{projectKey}}/role/{{id}}", JiraAPIServiceURI)},
            {JiraObjectEnum.ProjectCategories, String.Format("{0}/projectCategory", JiraAPIServiceURI)},
            {JiraObjectEnum.ProjectTypes, String.Format("{0}/project/type", JiraAPIServiceURI)},
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

        public string GetBaseUrl()
        {
            return Client.BaseUrl.ToString();
        }

        public List<Issue> SearchIssues(String jql, Int32 maxResults = 700)
        {
            return GetIssues(_methods[JiraObjectEnum.Issues], new Dictionary<String, String>() { { "jql", jql }, { "maxResults", maxResults.ToString() }, { "fields", "*all" }, { "expand", "transitions" } });
        }

        #region Groups
        public GroupResult GetGroup(string groupName)
        {
            return Execute<GroupResult>(JiraObjectEnum.Group, parameters: new Dictionary<string, string> { { "groupname", groupName } });
        }
        #endregion

        #region Fields
        public List<Field> GetFields()
        {
            return GetList<Field>(JiraObjectEnum.Fields);
        }
        #endregion

        #region Projects
        public bool CreateProject(CreateProject newProject)
        {
            var request = GetRequest(JiraObjectEnum.Projects, new Dictionary<string, string>(), new Dictionary<string, string>());
            request.Method = Method.POST;
            request.AddJsonBody(newProject);
            var response = this.Client.Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.Created;
        }

        /// <summary></summary>
        /// <param name="existingProject">Any field left as NULL will not be updated. Key must be set.</param>
        /// <returns>True on success</returns>
        public bool UpdateProject(CreateProject existingProject)
        {
            if (string.IsNullOrWhiteSpace(existingProject.Key))
                throw new ArgumentOutOfRangeException("Project key not set");
            var request = GetRequest(JiraObjectEnum.Project, new Dictionary<string, string>(), new Dictionary<string, string>() { { "projectKey", existingProject.Key } });
            request.Method = Method.PUT;
            request.AddJsonBody(existingProject);
            var response = this.Client.Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public ProjectCategory CreateProjectCategory(string Name, string Description)
        {
            var request = GetRequest(JiraObjectEnum.ProjectCategories, new Dictionary<string, string>(), new Dictionary<string, string>());
            request.Method = Method.POST;
            request.AddJsonBody(new { name = Name, description = Description });
            var response = this.Client.Execute<ProjectCategory>(request);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                return response.Data;
            else
                return null;
        }

        public List<Project> GetProjects()
        {
            return GetList<Project>(JiraObjectEnum.Projects);
        }

        public Project GetProject(String projectKey)
        {
            return GetList<Project>(JiraObjectEnum.Project, keys: new Dictionary<string, string>() { { "projectKey", projectKey } }).FirstOrDefault();
        }

        public List<ProjectCategory> GetProjectCategories()
        {
            return GetList<ProjectCategory>(JiraObjectEnum.ProjectCategories);
        }

        public List<ProjectType> GetProjectTypes()
        {
            return GetList<ProjectType>(JiraObjectEnum.ProjectTypes);
        }
        #endregion

        #region Security
        private class hiddenIssueSecuritySchemes
        {
            public List<IssueSecurityScheme> IssueSecuritySchemes { get; set; }
        }
        public List<IssueSecurityScheme> GetIssueSecuritySchemes()
        {
            return GetItem<hiddenIssueSecuritySchemes>(JiraObjectEnum.IssueSecuritySchemes).IssueSecuritySchemes;
        }

        private class hiddenPermissionSchemes
        {
            public List<PermissionScheme> PermissionSchemes { get; set; }
        }
        public List<PermissionScheme> GetPermissionSchemes()
        {
            return GetItem<hiddenPermissionSchemes>(JiraObjectEnum.PermissionScheme).PermissionSchemes;
        }

        private class hiddenNotificationScheme
        {
            public Int32 MaxResults { get; set; }
            public Int32 StartAt { get; set; }
            public Int32 Total { get; set; }
            public bool IsLast { get; set; }
            public List<NotificationScheme> values { get; set; }
        }
        public List<NotificationScheme> GetNotificationSchemes()
        {
            var response = GetItem<hiddenNotificationScheme>(JiraObjectEnum.NotificationScheme);
            return response.values;
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

        #region Project roles
        public List<ProjectRole> GetProjectRoles(String projectKey)
        {
            var responses = this.Execute<Dictionary<string, string>>(JiraObjectEnum.ProjectRoles, keys: new Dictionary<string, string>() { { "projectKey", projectKey } });
            var roles = new List<ProjectRole>();
            foreach (var response in responses)
                roles.Add(new ProjectRole
                {
                    Name = response.Key,
                    Self = response.Value,
                    Id = Int32.Parse(response.Value.Split('/').Last())
                });
            return roles;
        }

        public ProjectRole AddGroupActor(String projectKey, Int32 id, String group)
        {
            var request = this.GetRequest(JiraObjectEnum.ProjectRole,
                new Dictionary<string, string>(),
                new Dictionary<string, string>() { { "projectKey", projectKey }, { "id", id.ToString() } });
            request.Method = Method.POST;
            request.AddJsonBody(new { group = new List<string> { group } });
            var response = Client.Execute<ProjectRole>(request);
            return response.Data;
        }

        public bool DeleteGroupActor(string projectKey, Int32 id, String group)
        {
            var request = this.GetRequest(JiraObjectEnum.ProjectRole,
                new Dictionary<string, string>() { { "group", group } },
                new Dictionary<string, string>() { { "projectKey", projectKey }, { "id", id.ToString() } });
            request.Method = Method.DELETE;
            var response = Client.Execute<ProjectRole>(request);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
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
                description = issueFields.Description,
                assignee = new
                {
                    name = issueFields.Assignee.Name
                },
                reporter = new
                {
                    name = issueFields.Reporter.Name
                }
            });

            List<Field> fields = GetFields();


            foreach (KeyValuePair<String, CustomField> customfield in issueFields.CustomFields)
            {
                Field field = fields.Where(f => f.ID.Equals(customfield.Key)).FirstOrDefault();

                switch (field.Schema.Custom)
                {
                    case "com.atlassian.jira.plugin.system.customfieldtypes:select":
                        tempjson.Add(customfield.Key.ToLower(), JToken.FromObject(new { value = customfield.Value.Value }));
                        break;
                    default:
                        tempjson.Add(customfield.Key.ToLower(), customfield.Value.Value);
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

        public void SetPriorityToIssue(Priority priority, Issue issue)
        {
            IRestRequest request = new RestRequest(String.Format("{0}/issue/{1}", JiraAPIServiceURI, issue.Key), Method.PUT);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { update = new { priority = new[] { new { set = new { name = priority.Name } } } } });

            IRestResponse<object> response = Client.Put<object>(request);

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new Exception(response.StatusCode.ToString());
            }
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

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException(response.Request.Resource);
            }
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
            if (jsonObject["fields"] == null)
            {
                return null;
            }
            return new Issue((String)jsonObject["key"], (JObject)jsonObject["fields"]);
        }
        #endregion
    }
}
