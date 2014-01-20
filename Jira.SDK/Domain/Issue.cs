using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using RestSharp;

namespace Jira.SDK
{
    public class Issue
    {
        public String Key { get; set; }
        public IssueFields Fields { get; set; }

        public String Summary
        {
            get { return Fields.Summary; }
            set { Fields.Summary = value; }
        }

        private List<Issue> _subtasks;
        public List<Issue> Subtasks
        {
            get { return _subtasks ?? (_subtasks = Fields.Subtasks.Select(subtask => subtask.Issue).ToList()); }
        }

        public StatusEnum Status
        {
            get { return Fields.Status.ToEnum(); }
        }

        private TimeTracking _timeTracking;
        public TimeTracking TimeTracking
        {
            get { return (_timeTracking ?? (_timeTracking = Fields.TimeTracking ?? JiraEnvironment.Instance.Client.GetItem<Issue>(JiraClient.JiraObjectEnum.Issue, keys: new Dictionary<String, String>() { { "issueKey", this.Key }}).TimeTracking)); }
        }

        private List<Worklog> _worklogs;
        public List<Worklog> GetWorklogs()
        {
            if (_worklogs == null)
            {
                _worklogs =
                   JiraEnvironment.Instance.Client.GetItem<WorklogSearchResult>(JiraClient.JiraObjectEnum.Worklog,
                       keys: new Dictionary<String, String>() { { "issueKey", this.Key } }).Worklogs;
                _worklogs.ForEach(wl => wl.Issue = this);
            }
            return _worklogs;
        }

        public ParentIssue Parent
        {
            get { return Fields.Parent; }
            set { Fields.Parent = value; }
        }

        public User Assignee
        {
            get { return Fields.Assignee; }
            set { Fields.Assignee = value; }
        }
    }

    public class IssueFields
    {
        public String Summary { get; set; }
        public String Description { get; set; }
        public User Reporter { get; set; }
        public User Assignee { get; set; }
        public List<ProjectVersion> FixVersions { get; set; }
        public Project Project { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public Resolution Resolution { get; set; }
        public ParentIssue Parent { get; set; }
        public List<Subtask> Subtasks { get; set; }
        public TimeTracking TimeTracking { get; set; }
    }
}
