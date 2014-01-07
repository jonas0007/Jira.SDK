using System;
using System.Collections.Generic;
using System.Linq;

namespace Jira.SDK
{
	public class Jira
	{
	    private JiraClient _client;
	    private static Jira _instance;

	    public static Jira Instance
	    {
	        get
	        {
	            if (_instance == null)
	            {
	                _instance = new Jira();
	            }
	            return _instance;
	        }
	    }

	    internal JiraClient Client { get { return _client; }}

	    private Jira()
	    {
	    }

	    public void Connect(String url, String username, String password)
	    {
            _client = new JiraClient(url, username, password);
	    }

	    public List<Project> GetProjects()
	    {
	        return _client.GetList<Project>(JiraClient.JiraObjectEnum.Projects);
	    }

	    public Project GetProject(String key)
	    {
            return _client.GetList<Project>(JiraClient.JiraObjectEnum.Project, keys: new Dictionary<string, string>(){{ "projectKey", key}}).FirstOrDefault();
	    }
	}
}
