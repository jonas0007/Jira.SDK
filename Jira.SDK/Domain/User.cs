using Jira.SDK.Domain;
using Jira.SDK.Tools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jira.SDK
{
    public class User
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
        public String Name { get; set; }
        public String DisplayName { get; set; }
        public String EmailAddress { get; set; }

        public String Username { get { return Key ?? Name; } }
        public String Fullname { get { return DisplayName ?? Name; } }

		public Boolean IsProjectLead { get; set; }

		private List<WorkDay> _workdays;
		public void SetWorkDays(List<Issue> issues, DateTime from, DateTime until)
		{
			List<Worklog> worklogs = issues.SelectMany(issue => issue.GetWorklogs()).Where(worklog => worklog.Author.Equals(this)).ToList();
			List<DateTime> days = from.GetDaysUntil(until, DateAndTimeExtentions.DaysBetweenOptions.IgnoreWeekends);
			_workdays = days.Select(day => new WorkDay(day, worklogs.Where(worklog => worklog.Started.Date.Equals(day)).ToList())).ToList();
		}

		public List<WorkDay> GetWorkDays()
		{
			return _workdays;
		}

        public override bool Equals(object obj)
        {
            if (obj != null && obj is User)
            {
                return this.Username.Equals((obj as User).Username);    
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.Username.GetHashCode();
        }

        public static User UndefinedUser
        {
            get
            {
                return new User()
                {
                    DisplayName = "Unassigned",
                    EmailAddress = "",
                    Name = "Unassigned",
                    Key = "Unassigned"
                };
            }
        }
    }
}
