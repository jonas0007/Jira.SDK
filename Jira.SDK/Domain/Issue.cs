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
        public Jira Jira { get; set; }

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
                    _subtasks = Jira.Client.GetSubtasksFromIssue(this.Key);
                    _subtasks.ForEach(subtask => subtask.Jira = Jira);
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
            get
            {
                if (_timeTracking == null)
                {
                    if (Fields.TimeTracking != null)
                    {
                        _timeTracking = Fields.TimeTracking;
                    }
                    else
                    {
                        Issue issue = Jira.Client.GetIssue(this.Key);
                        _timeTracking = issue.Fields.TimeTracking;
                    }
                }
                return _timeTracking;
            }
        }

        private List<Worklog> _worklogs;
        public List<Worklog> GetWorklogs()
        {
            if (_worklogs == null)
            {
                _worklogs =
                   Jira.Client.GetWorkLogs(this.Key).Worklogs;
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
                    _parent = Jira.Client.GetIssue(Fields.Parent.Key);
                    _parent.Jira = Jira;
                }
                return _parent;
            }
        }

        public IssueType IssueType
        {
            get
            {
                return Fields.IssueType;
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
                    _project.Jira = Jira;
                }
                return _project;
            }
        }

        private Issue _epic;
        public Issue Epic
        {
            get
            {
                if (_epic == null && !String.IsNullOrEmpty(this.Fields.Customfield_10700))
                {
                    //Field field = JiraEnvironment.Fields.Where(f => f.Name.Equals("Epic Link")).FirstOrDefault();
                    //if (field != null)
                    //{
                    //	JiraEnvironment.Client.GetIssueCustomFieldsFromIssue(this.Key);
                    //}

                    _epic = Jira.Client.GetIssue(this.Fields.Customfield_10700);
                    _epic.Jira = this.Jira;
                }
                return _epic;
            }
            set
            {
                _epic = value;
                if (_epic.Jira == null)
                {
                    _epic.Jira = this.Jira;
                }
            }
        }

        public Int32 Rank
        {
            get
            {
                return Fields.Customfield_10004;
            }
            set
            {
                Fields.Customfield_10004 = value;
            }
        }

        public String ERPCode
        {
            get
            {
                return (Fields.Customfield_11000 != null ? Fields.Customfield_11000.Value : "");
            }
            set
            {
                Fields.Customfield_11000 = new CustomField() { Value = value };
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
            if (obj is Issue)
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
        public IssueType IssueType { get; set; }
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
        //Epic link
        public String Customfield_10700 { get; set; }
        //Rank
        public Int32 Customfield_10004 { get; set; }
        //ERP Code
        public CustomField Customfield_11000 { get; set; }
        public Dictionary<String, String> Fields { get; set; }
    }
}
