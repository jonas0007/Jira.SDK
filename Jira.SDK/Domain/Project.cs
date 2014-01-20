using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK
{
    public class Project
    {
        public JiraEnvironment JiraEnvironment { get; set; }

        public String Key { get; set; }
        public String Name { get; set; }

        private List<User> _assignableUsers;
        public List<User> AssignableUsers
        {
            get
            {
                return _assignableUsers ??
                       (_assignableUsers =
                            JiraEnvironment.Client.GetList<User>(JiraClient.JiraObjectEnum.AssignableUser,
                               parameters: new Dictionary<string, string>() { { "project", this.Key } }));
            }
        }

        private List<ProjectVersion> _projectVersions;
        public List<ProjectVersion> ProjectVersions
        {
            get
            {
                if (_projectVersions == null)
                {
                    _projectVersions =
                           JiraEnvironment.Client.GetList<ProjectVersion>(JiraClient.JiraObjectEnum.ProjectVersions,
                               keys: new Dictionary<string, string>() { { "projectKey", this.Key } });

                    _projectVersions.ForEach(vers => vers.Project = this);
                }
                return _projectVersions;
            }
        }

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

        public ProjectVersion CurrentVersion
        {
            get
            {
                return ProjectVersions.FirstOrDefault(
                    vers => vers.StartDate.CompareTo(DateTime.Now) <= 0 && vers.ReleaseDate.CompareTo(DateTime.Now) > 0);
            }
        }

        public ProjectVersion NextVersion
        {
            get
            {
                return ProjectVersions.Where(vers => vers.StartDate.CompareTo(DateTime.Now) > 0 && vers.ReleaseDate.CompareTo(DateTime.Now) > 0).OrderBy(vers => vers.StartDate).FirstOrDefault();
            }
        }
    }
}
