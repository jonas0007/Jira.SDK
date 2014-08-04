using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK
{
    public class ProjectVersion
    {
        public Int32 ID { get; set; }
        public String Description { get; set; }
        public String Name { get; set; }

        public Project Project { get; set; }

        public Boolean Archived { get; set; }
        public Boolean Released { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime ReleaseDate { get; set; }

        private List<Issue> _issues;

        public List<Issue> Issues
        {
            get
            {
                if (_issues == null)
                {
                    _issues =
                        Project.Jira.Client.GetIssuesFromProjectVersion(this.Project.Key, this.Name);
                    _issues.ForEach(issue => issue.Jira = Project.Jira);
                }
                return _issues;
            }
        }
    }
}
