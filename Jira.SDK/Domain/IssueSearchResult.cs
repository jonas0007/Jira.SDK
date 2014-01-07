using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK
{
    public class IssueSearchResult
    {
        public Int32 Total { get; set; }
        public List<Issue> Issues { get; set; }
    }
}
