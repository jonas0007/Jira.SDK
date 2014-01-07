using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK
{
    public class Worklog
    {
        public User Author { get; set; }
        public String Comment { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Started { get; set; }
        public User UpdateAuthor { get; set; }

        public long TimeSpentSeconds { get; set; }

    }

    public class WorklogSearchResult
    {
        public List<Worklog> Worklogs { get; set; }
    }
}
