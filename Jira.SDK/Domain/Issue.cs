using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using RestSharp;
using System.Text.RegularExpressions;

namespace Jira.SDK.Domain
{
    public class Issue
    {
        private Jira _jira { get; set; }
        public Jira GetJira()
        {
            return _jira;
        }

        public void SetJira(Jira jira)
        {
            _jira = jira;
        }

        public String Key { get; set; }
        public IssueFields Fields { get; set; }

        public String Summary
        {
            get { return Fields.Summary; }
            set { Fields.Summary = value; }
        }

        public String Description
        {
            get { return Fields.Description; }
            set { Fields.Description = value; }
        }

        private List<Issue> _subtasks;
        public List<Issue> Subtasks
        {
            get
            {
                if (_subtasks == null)
                {
                    _subtasks = _jira.Client.GetSubtasksFromIssue(this.Key);
                    _subtasks.ForEach(subtask => subtask.SetJira(_jira));
                }
                return _subtasks;
            }
            set
            {
                _subtasks = value;
            }
        }

        public List<Comment> _comments;
        public List<Comment> Comments
        {
            get
            {
                if (_comments == null)
                {
                    _comments = Fields.Comment.Comments;
                }
                return _comments;
            }
        }

        public List<String> Labels
        {
            get
            {
                return Fields.Labels;
            }
        }

        public void AddComment(Comment comment)
        {
            Comments.Add(GetJira().Client.AddCommentToIssue(this, comment));
        }

        public Status Status
        {
            get { return Fields.Status; }
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
                        Issue issue = _jira.Client.GetIssue(this.Key);
                        _timeTracking = issue.Fields.TimeTracking;
                    }

                    _timeTracking.Issue = this;
                }
                return _timeTracking;
            }
        }

        private List<Worklog> _worklogs;
        public List<Worklog> GetWorklogs()
        {
            if (_worklogs == null)
            {
                if (Fields.Worklog != null)
                {
                    _worklogs = Fields.Worklog.Worklogs;
                }
                else
                {
                    _worklogs =
                       _jira.Client.GetWorkLogs(this.Key).Worklogs;
                }
                _worklogs.ForEach(wl => wl.Issue = this);
            }
            return _worklogs;
        }

        public List<Transition> Transitions { get; set; }
        private List<Transition> _transitions;
        public List<Transition> GetTransitions()
        {
            if (_transitions == null)
            {
                if (Transitions != null)
                {
                    _transitions = Transitions;
                }
                else
                {
                    _transitions =
                       _jira.Client.GetTransitions(this.Key);
                }
            }
            return _transitions;
        }

        public void TransitionIssue(Transition transition, Comment comment)
        {
            _jira.Client.TransitionIssue(this, transition, comment);
        }

        private Issue _parent;
        public Issue Parent
        {
            get
            {
                if (_parent == null && Fields.Parent != null)
                {
                    _parent = _jira.Client.GetIssue(Fields.Parent.Key);
                    _parent.SetJira(_jira);
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

        public User Reporter
        {
            get { return Fields.Reporter ?? User.UndefinedUser; }
            set { Fields.Reporter = value; }
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
                if(Fields.ResolutionDate.CompareTo(DateTime.MinValue) == 0)
                {
                    Fields.ResolutionDate = DateTime.MaxValue;
                }
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
                    _project.SetJira(_jira);
                }
                return _project;
            }
        }

        private Epic _epic;
        public Epic Epic
        {
            get
            {
                if (_epic == null && !String.IsNullOrEmpty(this.Fields.Customfield_10700))
                {
                    Issue issue = _jira.Client.GetIssue(this.Fields.Customfield_10700);
                    issue.SetJira(_jira);

                    _epic = Epic.FromIssue(issue);
                }
                return _epic;
            }
            set
            {
                _epic = value;
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

        public Int32 SprintID
        {
            get
            {
                String sprintDescription = Fields.Customfield_10300;
                if (!String.IsNullOrEmpty(sprintDescription))
                {
                    MatchCollection matches = Regex.Matches(sprintDescription, ",id=(?<SprintID>\\d+)]");
                    Int32 id = -1;

                    foreach (Match match in matches)
                    {
                        if (match.Success)
                        {
                            id = Int32.Parse(match.Groups["SprintID"].Value);
                        }
                    }

                    return id;
                }
                return -1;
            }
        }

        public String Severity
        {
            get
            {
                return (Fields.Customfield_10103 != null ? Fields.Customfield_10103.Value : "");
            }
        }

        public Sprint Sprint { get; set; }

        public Dictionary<String, String> CustomFields
        {
            get;
            set;
        }

        public String CurrentSituation
        {
            get
            {
                return Fields.Customfield_10402;
            }
            set
            {
                Fields.Customfield_10402 = value;
            }
        }

        public String ToBeSituation
        {
            get
            {
                return Fields.Customfield_10401;
            }
            set
            {
                Fields.Customfield_10401 = value;
            }
        }

        public String Benefit
        {
            get
            {
                return Fields.Customfield_10400;
            }
            set
            {
                Fields.Customfield_10400 = value;
            }
        }

        private List<IssueLink> IssueLinks
        {
            get
            {
                return Fields.IssueLinks;
            }
            set
            {
                Fields.IssueLinks = value;
            }
        }

        /// <summary>
        /// This method returns all issues which where cloned from this one.
        /// </summary>
        /// <returns>The list of issues which where cloned from this one</returns>
        public List<Issue> GetClones()
        {
            List<Issue> clones = IssueLinks.Where(link => link.Type.ToEnum() == IssueLinkType.IssueLinkTypeEnum.Cloners && link.InwardIssue != null).Select(link => link.InwardIssue).ToList();
            loadIssues(clones);

            return clones;
        }

        /// <summary>
        /// This method returns all issues which are blocking this one.
        /// </summary>
        /// <returns>The list of issues which are blocking this one</returns>
        public List<Issue> GetBlockingIssues()
        {
            List<Issue> blockingIssues = IssueLinks.Where(link => link.Type.ToEnum() == IssueLinkType.IssueLinkTypeEnum.Blocks && link.InwardIssue != null).Select(link => link.InwardIssue).ToList();
            loadIssues(blockingIssues);

            return blockingIssues;
        }

        /// <summary>
        /// This method returns all of the issues which are blocked by this one.
        /// </summary>
        /// <returns>The list of issues which are blocked by this one</returns>
        public List<Issue> GetImpactedIssues()
        {
            List<Issue> impactedIssues = IssueLinks.Where(link => link.Type.ToEnum() == IssueLinkType.IssueLinkTypeEnum.Blocks && link.OutwardIssue != null).Select(link => link.OutwardIssue).ToList();
            loadIssues(impactedIssues);

            return impactedIssues;
        }

        /// <summary>
        /// This method returns all of the issues which are duplicates from this one.
        /// </summary>
        /// <returns>The list of issues which are duplicates from this one</returns>
        public List<Issue> GetDuplicateIssues()
        {
            List<Issue> duplicateIssues = IssueLinks.Where(link => link.Type.ToEnum() == IssueLinkType.IssueLinkTypeEnum.Duplicate).Select(link => (link.InwardIssue != null ? link.InwardIssue : link.OutwardIssue)).ToList();
            loadIssues(duplicateIssues);

            return duplicateIssues;
        }

        /// <summary>
        /// This method returns all of the issues which relate to this one.
        /// </summary>
        /// <returns>The list of issues which are relate to this one</returns>
        public List<Issue> GetRelatedIssues()
        {
            List<Issue> relatedIssues = IssueLinks.Where(link => link.Type.ToEnum() == IssueLinkType.IssueLinkTypeEnum.Relates).Select(link => (link.InwardIssue != null ? link.InwardIssue : link.OutwardIssue)).ToList();
            loadIssues(relatedIssues);

            return relatedIssues;
        }
        
        /// <summary>
        /// This method iterates every issue in the issue list and makes sure this issue is loaded and ready for querying.
        /// </summary>
        /// <param name="issues"></param>
        private void loadIssues(List<Issue> issues)
        {
            issues.Where(issue => issue != null).ToList().ForEach(issue =>
            {
                issue.SetJira(this._jira);
                issue.Load();
            });
        }

        public void Load()
        {
            Issue issue = _jira.Client.GetIssue(this.Key);
            this.Fields = issue.Fields;
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
        public CommentSearchResult Comment { get; set; }
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
        //Epic Status
        public CustomField Customfield_10702 { get; set; }
        //SprintID
        public String Customfield_10300 { get; set; }
        //Severity
        public CustomField Customfield_10103 { get; set; }
        //Current situation
        public String Customfield_10402 { get; set; }
        //To Be Situation
        public String Customfield_10401 { get; set; }
        //Benefit
        public String Customfield_10400 { get; set; }

        public WorklogSearchResult Worklog { get; set; }

        public List<IssueLink> IssueLinks { get; set; }

        public List<String> Labels { get; set; }
    }
}
