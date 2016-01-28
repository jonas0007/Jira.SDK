using Jira.SDK.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jira.SDK
{
    public class Jira
    {
        private IJiraClient _client;
		internal IJiraClient Client { get { return _client; } }

		public List<Field> Fields { get; private set; }

		public void Connect(IJiraClient client)
		{
			_client = client;
            Fields = _client.GetFields();
		}

        public void Connect(String url)
        {
            Connect(new JiraClient(url));
        }

        public void Connect(String url, String username, String password)
        {
            Connect(new JiraClient(url, username, password));
        }

        public User GetUser(string username)
        {
            return _client.GetUser(username);
        }

        public bool CreateProject(CreateProject newProject)
        {
            return _client.CreateProject(newProject);
        }

        public GroupResult GetGroup(string groupName)
        {
            var result = _client.GetGroup(groupName);
            result.Jira = this;
            return result;
        }

        public List<Project> GetProjects()
        {
			List<Project> projects = _client.GetProjects();
            projects.ForEach(project => project.SetJira(this));
            return projects;
        }

        public Project GetProject(String key)
        {
            Project project = _client.GetProject(key);
            if (project != null)
            {
                project.SetJira(this);
            }
            return project;
        }
        public List<ProjectCategory> GetProjectCategories()
        {
            var categories = _client.GetProjectCategories();
            categories.ForEach(cat => cat.Jira = this);
            return categories;
        }

        public List<ProjectRole> GetProjectRoles(String key)
        {
            var roles = _client.GetProjectRoles(key);
            roles.ForEach(role => role.Jira = this);
            return roles;
        }

        public ProjectRole AddGroupActor(String projectKey, Int32 id, String group)
        {
            var projectRole = _client.AddGroupActor(projectKey, id, group);
            projectRole.Jira = this;
            return projectRole;
        }

        public bool DeleteGroupActor(string projectKey, Int32 id, String group)
        {
            return _client.DeleteGroupActor(projectKey, id, group);
        }

        public List<ProjectType> GetProjectTypes()
        {
            var types = _client.GetProjectTypes();
            types.ForEach(cat => cat.Jira = this);
            return types;
        }

        public Issue GetIssue(String key)
		{
			Issue issue = _client.GetIssue(key);
            if(String.IsNullOrEmpty(issue.Key))
            {
                return null;
            }
			if (issue != null)
			{
				issue.SetJira(this);
			}

			return issue;
		}

        public List<Issue> SearchIssues(String jql)
        {
            List<Issue> issues = Client.SearchIssues(jql);
            issues.ForEach(issue => issue.SetJira(this));
            return issues;
        }

		public List<AgileBoard> GetAgileBoards()
		{
			List<AgileBoard> boards = _client.GetAgileBoards();
			boards.ForEach(board => board.SetJira(this));
			return boards;
		}

		public AgileBoard GetAgileBoard(Int32 agileBoardID)
		{
			AgileBoard board = GetAgileBoards().Where(b => b.ID == agileBoardID).FirstOrDefault();
			if (board == null)
			{
				throw new Exception(String.Format("The board with ID {0} does not exist", agileBoardID));
			}

			return board;
		}

        public List<IssueFilter> GetFilters()
        {
            List<IssueFilter> filters = _client.GetFavoriteFilters();
            filters.ForEach(filter => filter.SetJira(this));
            return filters;
        }

		public IssueFilter GetFilter(String filtername)
		{
			IssueFilter filter = GetFilters().Where(f => f.Name.Equals(filtername)).FirstOrDefault();
			if (filter == null)
			{
				throw new Exception(String.Format("The filter with name {0} does not exist", filtername));
			}

			return filter;
		}

        public List<IssueSecurityScheme> GetIssueSecuritySchemes()
        {
            var schemes = _client.GetIssueSecuritySchemes();
            schemes.ForEach(scheme => scheme.Jira = this);
            return schemes;
        }

        public List<PermissionScheme> GetPermissionSchemes()
        {
            var schemes = _client.GetPermissionSchemes();
            schemes.ForEach(scheme => scheme.Jira = this);
            return schemes;
        }

        public List<NotificationScheme> GetNotificationSchemes()
        {
            var schemes = _client.GetNotificationSchemes();
            schemes.ForEach(scheme => scheme.Jira = this);
            return schemes;
        }
    }
}
