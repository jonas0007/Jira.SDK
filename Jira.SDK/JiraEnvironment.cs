using Jira.SDK.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jira.SDK
{
    public class JiraEnvironment
    {
        private IJiraClient _client;
		internal IJiraClient Client { get { return _client; } }

		public List<Field> Fields { get; private set; }

		public void Connect(IJiraClient client)
		{
			_client = client;
			Fields = _client.GetFields();
		}

        public void Connect(String url, String username, String password)
        {
            Connect(new JiraClient(url, username, password));
        }

        public List<Project> GetProjects()
        {
			List<Project> projects = _client.GetProjects();
            projects.ForEach(p => p.JiraEnvironment = this);
            return projects;
        }

        public Project GetProject(String key)
        {
            Project project = _client.GetProject(key);
            if (project != null)
            {
                project.JiraEnvironment = this;
            }
            return project;
        }

		public List<AgileBoard> GetAgileBoards()
		{
			List<AgileBoard> boards = _client.GetAgileBoards();
			boards.ForEach(board => board.Environment = this);
			return boards;
		}
    }
}
