using System;
using System.Collections.Generic;
using System.Linq;

namespace Jira.SDK
{
    public class JiraEnvironment
    {
        private JiraClient _client;
        internal JiraClient Client { get { return _client; } }

        public void Connect(String url, String username, String password)
        {
            _client = new JiraClient(url, username, password);
        }

        public List<Project> GetProjects()
        {
            List<Project> projects = _client.GetList<Project>(JiraClient.JiraObjectEnum.Projects);
            projects.ForEach(p => p.JiraEnvironment = this);
            return projects;
        }

        public Project GetProject(String key)
        {
            Project project = _client.GetList<Project>(JiraClient.JiraObjectEnum.Project, keys: new Dictionary<string, string>() { { "projectKey", key } }).FirstOrDefault();
            if (project != null)
            {
                project.JiraEnvironment = this;
            }
            return project;
        }
    }
}
