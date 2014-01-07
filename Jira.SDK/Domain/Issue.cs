using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Jira.SDK
{
    public class Issue
    {
        public String Key { get; set; }
        public IssueFields Fields { get; set; }

        private List<Issue> _subtasks; 
        public List<Issue> Subtasks
        {
            get { return _subtasks ?? (_subtasks = Fields.Subtasks.Select(subtask => subtask.Issue).ToList()); }
        }

        public StatusEnum Status
        {
            get { return Fields.Status.ToEnum(); }
        }

        public TimeTracking TimeTracking
        {
            get { return Fields.TimeTracking; }
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
