using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK.Domain
{
    public class IssueFilter
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

        public String Name { get; set; }
        public String Description { get; set; }
        public String JQL { get; set; }

        private List<Issue> _issues;
        public List<Issue> GetIssues(Int32 maxResults = 700)
        {
            if (_issues == null)
            {
                _issues = _jira.Client.SearchIssues(this.JQL, maxResults);
                _issues.ForEach(issue => issue.SetJira(this._jira));
            }
            return _issues;
        }
    }
}
