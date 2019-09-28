using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jira.SDK.Domain
{
    public class Project
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


        public Int32 ID { get; set; }
        public String Key { get; set; }
        public String Name { get; set; }

        private User _lead;

        public User Lead { get; set; }

        [JsonIgnore]
        public User ProjectLead
        {
            get { return _lead ?? (_lead = _jira.Client.GetUser(Lead.Username)); }
        }

        private List<User> _assignableUsers;
        [JsonIgnore]
        public List<User> AssignableUsers
        {
            get
            {
                return _assignableUsers ??
                       (_assignableUsers =
                            _jira.Client.GetAssignableUsers(this.Key));
            }
        }

        private List<ProjectVersion> _projectVersions;
        [JsonIgnore]
        public List<ProjectVersion> ProjectVersions
        {
            get
            {
                if (_projectVersions == null)
                {
                    _projectVersions =
                           _jira.Client.GetProjectVersions(this.Key);

                    _projectVersions.ForEach(vers => vers.Project = this);
                }
                return _projectVersions;
            }
        }

        [JsonIgnore]
        public ProjectVersion PreviousVersion
        {
            get
            {
                return
                    ProjectVersions.Where(vers => vers.ReleaseDate.CompareTo(DateTime.Now) <= 0)
                        .OrderByDescending(vers => vers.ReleaseDate)
                        .FirstOrDefault();
            }
        }

        [JsonIgnore]
        public ProjectVersion CurrentVersion
        {
            get
            {
                return ProjectVersions.FirstOrDefault(
                    vers => vers.StartDate.CompareTo(DateTime.Now) <= 0 && vers.ReleaseDate.CompareTo(DateTime.Now) > 0 && !vers.Archived);
            }
        }

        [JsonIgnore]
        public ProjectVersion NextVersion
        {
            get
            {
                return ProjectVersions.Where(vers => vers.StartDate.CompareTo(DateTime.Now) > 0 && vers.ReleaseDate.CompareTo(DateTime.Now) > 0).OrderBy(vers => vers.StartDate).FirstOrDefault();
            }
        }

        private List<ProjectComponent> _components;
        [JsonIgnore]
        public List<ProjectComponent> Components
        {
            get
            {
                return _components ??
                   (_components =
                        _jira.Client.GetProjectComponents(this.Key));
            }
        }

        public List<Epic> GetEpics()
        {
            List<Issue> epicIssues = _jira.Client.GetEpicIssuesFromProject(this.Name);
            epicIssues.ForEach(epic => epic.SetJira(this.GetJira()));

            List<Epic> epics = epicIssues.Select(epic => Epic.FromIssue(epic)).ToList();

            return epics.OrderBy(epic => epic.Rank).ToList();
        }

        public Epic GetEpic(String epicName)
        {
            Issue epicIssue = _jira.Client.GetEpicIssueFromProject(this.Name, epicName);
            epicIssue.SetJira(this.GetJira());
            return Epic.FromIssue(epicIssue);
        }

        public Epic GetEpicByKey(String issueKey)
        {
            Issue epicIssue = _jira.Client.GetIssue(issueKey);
            epicIssue.SetJira(this.GetJira());
            return Epic.FromIssue(epicIssue);
        }


        public Issue CreateIssue(IssueFields fields)
        {
            fields.Project = new Project()
            {
                ID = this.ID
            };
            Issue issue = GetJira().Client.AddIssue(fields);
            issue.SetJira(this.GetJira());
            issue.Load();
            return issue;
        }

        public override int GetHashCode()
        {
            return this.Key.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (obj is Project) && this.Key.Equals(((Project)obj).Key);
        }
    }
}