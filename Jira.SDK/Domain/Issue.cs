using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using RestSharp;
using Jira.SDK.Domain;

namespace Jira.SDK
{
    public class Issue
    {
        public JiraEnvironment JiraEnvironment { get; set; }

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
            get
            {
                if (_subtasks == null)
                {
					_subtasks = JiraEnvironment.Client.GetSubtasksFromIssue(this.Key);
                    _subtasks.ForEach(subtask => subtask.JiraEnvironment = JiraEnvironment);
                }
                return _subtasks;
            }
        }

        public StatusEnum Status
        {
            get { return Fields.Status.ToEnum(); }
        }

        private TimeTracking _timeTracking;
        public TimeTracking TimeTracking
        {
            get { return (_timeTracking ?? (_timeTracking = Fields.TimeTracking ?? JiraEnvironment.Client.GetIssue(this.Key).TimeTracking)); }
        }

        private List<Worklog> _worklogs;
        public List<Worklog> GetWorklogs()
        {
            if (_worklogs == null)
            {
                _worklogs =
                   JiraEnvironment.Client.GetWorkLogs(this.Key).Worklogs;
                _worklogs.ForEach(wl => wl.Issue = this);
            }
            return _worklogs;
        }

        private Issue _parent;
        public Issue Parent
        {
            get
            {
                if (_parent == null && Fields.Parent != null)
                {
                    _parent = JiraEnvironment.Client.GetIssue(Fields.Parent.Key);
                    _parent.JiraEnvironment = JiraEnvironment;
                }
                return _parent;
            }
        }

        public User Assignee
        {
            get { return Fields.Assignee ?? User.UndefinedUser; }
            set { Fields.Assignee = value; }
        }
        public DateTime Created
        {
            get { return Fields.Created; }
            set { Fields.Created = value; }
        }
        public DateTime Updated
        {
            get { return Fields.Updated; }
            set { Fields.Updated = value; }
        }

		public DateTime Resolved
		{
			get
			{
				return Fields.ResolutionDate;
			}
		}

		private Project _project = null;
		public Project Project
		{
			get
			{
				if (_project == null)
				{
					_project = this.Fields.Project;
					_project.JiraEnvironment = JiraEnvironment;
				}
				return _project;
			}
		}

		private Issue _epic;
		public Issue Epic
		{
			get
			{
				if (_epic == null)
				{
					Field field = JiraEnvironment.Fields.Where(f => f.Name.Equals("Epic Link")).FirstOrDefault();
					if (field != null)
					{
						JiraEnvironment.Client.GetIssueCustomFieldsFromIssue(this.Key);
					}
				}
				return _epic;
			}
		}

		public Dictionary<String, String> CustomFields
		{
			get;
			set;
		}

        #region equality

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if(obj is Issue)
                return Key.Equals(((Issue)obj).Key);
            return false;
        }

        #endregion
    }

    public class IssueFields
    {
        public String Summary { get; set; }
        public String Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
		public DateTime ResolutionDate { get; set; }
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

		public Dictionary<String, String> Fields { get; set; }
    }
}
