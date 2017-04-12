using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using RestSharp;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Jira.SDK.Domain
{
    public partial class Issue
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

        public Issue() { }

        public Issue(String key, JObject fields)
        {
            this.Key = key;
            this.Fields = new IssueFields(fields);
        }

        public String GetCustomFieldValue(String customFieldName)
        {
            Field field = GetJira().Fields.FirstOrDefault(f => f.Name.Equals(customFieldName));
            if (field == null)
            {
                throw new ArgumentException(String.Format("The field with name {0} does not exist.", customFieldName), customFieldName);
            }
            String fieldId = field.ID;
            return (Fields.CustomFields[fieldId] != null ? Fields.CustomFields[fieldId].Value : "");
        }

        public void SetCustomFieldValue(String customFieldName, String value)
        {
            Field field = GetJira().Fields.FirstOrDefault(f => f.Name.Equals(customFieldName));
            if (field == null)
            {
                throw new ArgumentException(String.Format("The field with name {0} does not exist.", customFieldName), customFieldName);
            }

            if (Fields.CustomFields[field.ID] == null)
            {
                Fields.CustomFields[field.ID] = new CustomField(value);
            }
            Fields.CustomFields[field.ID].Value = value;
        }

        public String Key { get; set; }
        public IssueFields Fields { get; set; }
        public String Url
        {
            get
            {
                return string.Format("{0}browse/{1}", _jira.Client.GetBaseUrl(), Key);
            }
        }

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

        public void SetPriority(Priority priority)
        {
            GetJira().Client.SetPriorityToIssue(priority, this);
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

        public void RefreshWorklogs()
        {
            // this will force the re-loading of any work logs
            _worklogs = _jira.Client.GetWorkLogs(this.Key).Worklogs;
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
                if (Fields.ResolutionDate.CompareTo(DateTime.MinValue) == 0)
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

        public Sprint Sprint { get; set; }

        public Dictionary<String, String> CustomFields
        {
            get;
            set;
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
        /// This method returns all issues which where cloned from this one.
        /// </summary>
        /// <returns>The list of issues which where cloned from this one</returns>
        public Issue GetClonedIssue()
        {
            Issue cloned = IssueLinks.Where(link => link.Type.ToEnum() == IssueLinkType.IssueLinkTypeEnum.Cloners && link.OutwardIssue != null).Select(link => link.OutwardIssue).FirstOrDefault();
            if (cloned != null)
            {
                loadIssues(new List<Issue>() { cloned });
            }

            return cloned;
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

        #region Custom fields for Jira Agile
        public Int32 SprintID
        {
            get
            {
                String sprintDescription = GetCustomFieldValue("Sprint");
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

        public String SprintNames
        {
            get
            {
                String sprintDescription = GetCustomFieldValue("Sprint");
                if (!String.IsNullOrEmpty(sprintDescription))
                {
                    MatchCollection matches = Regex.Matches(sprintDescription, ",name=(?<SprintName>.*?),");
                    String names = "";

                    foreach (Match match in matches)
                    {
                        if (match.Success)
                        {
                            names += match.Groups["SprintName"].Value;
                            if (match.NextMatch().Success)
                            {
                                names += ", ";
                            }
                        }
                    }

                    return names;
                }
                return "";
            }
        }

        private Epic _epic;
        public Epic Epic
        {
            get
            {
                if (_epic == null && !String.IsNullOrEmpty(GetCustomFieldValue("Epic Link")))
                {
                    Issue issue = GetJira().Client.GetIssue(GetCustomFieldValue("Epic Link"));
                    if (issue != null)
                    {
                        issue.SetJira(GetJira());
                        _epic = Epic.FromIssue(issue);
                    }

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
                MatchCollection matches = Regex.Matches(GetCustomFieldValue("Rank"), ",^(?<Rank>\\d+)]");
                Int32 rank = -1;

                foreach (Match match in matches)
                {
                    if (match.Success)
                    {
                        rank = Int32.Parse(match.Groups["Rank"].Value);
                    }
                }

                return rank;
            }
            set
            {
                SetCustomFieldValue("Rank", value.ToString());
            }
        }

        public String Severity
        {
            get
            {
                return (GetCustomFieldValue("Severity") != null ? GetCustomFieldValue("Severity") : "");
            }
        }
        #endregion

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
        public List<ProjectVersion> AffectsVersions { get; set; }
        public List<Component> Components { get; set; }
        public Project Project { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public Resolution Resolution { get; set; }
        public ParentIssue Parent { get; set; }
        public List<Subtask> Subtasks { get; set; }
        public TimeTracking TimeTracking { get; set; }
        public WorklogSearchResult Worklog { get; set; }
        public List<IssueLink> IssueLinks { get; set; }
        public List<String> Labels { get; set; }
        public Dictionary<String, CustomField> CustomFields { get; set; }

        public IssueFields()
        {
            Comment = new CommentSearchResult();
            FixVersions = new List<ProjectVersion>();
            AffectsVersions = new List<ProjectVersion>();
            Components = new List<Component>();
            Resolution = new Resolution();
            Parent = new ParentIssue();
            Subtasks = new List<Subtask>();
            TimeTracking = new TimeTracking();
            Worklog = new WorklogSearchResult();
            IssueLinks = new List<IssueLink>();
            Labels = new List<String>();
            CustomFields = new Dictionary<String, CustomField>();
        }

        public IssueFields(JObject fieldsObj) : this()
        {
            Dictionary<String, Object> fields = fieldsObj.ToObject<Dictionary<String, Object>>();

            Created = fields["created"] != null ? (DateTime)fields["created"] : DateTime.MinValue;
            Updated = fields["updated"] != null ? (DateTime)fields["updated"] : DateTime.MinValue;
            ResolutionDate = fields["resolutiondate"] != null ? (DateTime)fields["resolutiondate"] : DateTime.MinValue;

            IssueType = null;
            if (fields.ContainsKey("issuetype") && fields["issuetype"] != null)
            {
                IssueType = ((JObject)fields["issuetype"]).ToObject<IssueType>();
            }

            Reporter = null;
            if (fields.ContainsKey("reporter") && fields["reporter"] != null)
            {
                Reporter = ((JObject)fields["reporter"]).ToObject<User>();
            }

            Assignee = null;
            if (fields.ContainsKey("assignee") && fields["assignee"] != null)
            {
                Assignee = ((JObject)fields["assignee"]).ToObject<User>();
            }

            Summary = "";
            if (fields.ContainsKey("summary") && fields["summary"] != null)
            {
                Summary = (String)fields["summary"];
            }

            Comment = new CommentSearchResult();
            if (fields.ContainsKey("comment") && fields["comment"] != null)
            {
                Comment = ((JObject)fields["comment"]).ToObject<CommentSearchResult>();
            }

            Description = "";
            if (fields.ContainsKey("description") && fields["description"] != null)
            {
                Description = (String)fields["description"];
            }

            FixVersions = new List<ProjectVersion>();
            if (fields.ContainsKey("fixVersions") && fields["fixVersions"] != null)
            {
                JArray versionArray = (JArray)fields["fixVersions"];
                if (versionArray.Count > 0)
                {
                    FixVersions = ((JArray)fields["fixVersions"]).ToObject<List<ProjectVersion>>();
                }
            }

            AffectsVersions = new List<ProjectVersion>();
            if (fields.ContainsKey("versions") && fields["versions"] != null)
            {
                JArray versionArray = (JArray)fields["versions"];
                if (versionArray.Count > 0)
                {
                    AffectsVersions = ((JArray)fields["versions"]).ToObject<List<ProjectVersion>>();
                }
            }

            Components = new List<Component>();
            if (fields.ContainsKey("components") && fields["components"] != null)
            {
                JArray versionArray = (JArray)fields["components"];
                if (versionArray.Count > 0)
                {
                    Components = ((JArray)fields["components"]).ToObject<List<Component>>();
                }
            }

            Project = null;
            if (fields.ContainsKey("project") && fields["project"] != null)
            {
                Project = ((JObject)fields["project"]).ToObject<Project>();
            }

            Status = null;
            if (fields.ContainsKey("status") && fields["status"] != null)
            {
                Status = ((JObject)fields["status"]).ToObject<Status>();
            }

            Priority = null;
            if (fields.ContainsKey("priority") && fields["priority"] != null)
            {
                Priority = ((JObject)fields["priority"]).ToObject<Priority>();
            }

            Resolution = null;
            if (fields.ContainsKey("resolution") && fields["resolution"] != null)
            {
                Resolution = ((JObject)fields["resolution"]).ToObject<Resolution>();
            }

            Parent = null;
            if (fields.ContainsKey("parent") && fields["parent"] != null)
            {
                //Parent = new Issue();
            }

            Subtasks = null;
            if (fields.ContainsKey("subtasks"))
            {
                JArray subtasks = (JArray)fields["subtasks"];

                //Subtasks = null;
            }

            Worklog = new WorklogSearchResult();
            if (fields.ContainsKey("worklog"))
            {
                Worklog = ((JObject)fields["worklog"]).ToObject<WorklogSearchResult>();
            }

            IssueLinks = new List<IssueLink>();
            if (fields.ContainsKey("issuelinks"))
            {
                JArray linkArray = (JArray)fields["issuelinks"];
                if (linkArray.Count > 0)
                {
                    IssueLinks = linkArray.Select(link => ((JObject)link).ToObject<IssueLink>()).ToList();
                }
            }

            Labels = new List<String>();
            if (fields.ContainsKey("labels"))
            {
                JArray labelArray = (JArray)fields["labels"];
                if (labelArray.Count > 0)
                {
                    Labels = labelArray.Select(label => (String)label).ToList();
                }
            }

            TimeTracking = null;
            if (fields.ContainsKey("timetracking") && fields["timetracking"] != null)
            {
                TimeTracking = ((JObject)fields["timetracking"]).ToObject<TimeTracking>();
            }

            CustomFields = new Dictionary<String, CustomField>();
            foreach (String customFieldName in fields.Keys.Where(key => key.StartsWith("customfield_")))
            {
                switch (fieldsObj[customFieldName].Type)
                {
                    case JTokenType.Object:
                        CustomFields.Add(customFieldName, ((JObject)fieldsObj[customFieldName]).ToObject<CustomField>());
                        break;
                    case JTokenType.Null:
                        CustomFields.Add(customFieldName, null);
                        break;
                    case JTokenType.Array:
                        // TODO Handle Array Type
                        CustomFields.Add(customFieldName, new CustomField(((JArray)fieldsObj[customFieldName]).ToString(Newtonsoft.Json.Formatting.None)));
                        break;
                    case JTokenType.Float:
                        CustomFields.Add(customFieldName, new CustomField(((float)fieldsObj[customFieldName]).ToString()));
                        break;
                    default:
                        CustomFields.Add(customFieldName, new CustomField((String)fieldsObj[customFieldName]));
                        break;
                }

            }
        }
    }
}
