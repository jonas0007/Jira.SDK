using System;
using System.Collections.Generic;

namespace Jira.SDK.Domain
{
    public class Worklog
    {
        public User Author { get; set; }
        public String Comment { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Started { get; set; }
        public User UpdateAuthor { get; set; }
        public String Id { get; set; }

        public long TimeSpentSeconds { get; set; }

        public Issue Issue { get; set; }

        public TimeSpan TimeSpent
        {
            get { return TimeSpan.FromSeconds(TimeSpentSeconds); }
        }
    }

    public class WorklogSearchResult
    {
        public List<Worklog> Worklogs { get; set; }
    }
}
