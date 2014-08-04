using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK.Domain
{
    public class IssueFilter
    {
        internal Jira Jira { get; set; }

        public String Name { get; set; }
        public String Description { get; set; }
        public String JQL { get; set; }

        private List<Issue> _issues;
        public List<Issue> GetIssues()
        {
                if (_issues == null)
                {
                    _issues = Jira.Client.SearchIssues(this.JQL);
                    _issues.ForEach(issue => issue.Jira = this.Jira);
                }
                return _issues;
        }
    }
}
