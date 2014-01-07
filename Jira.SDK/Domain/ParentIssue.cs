using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK
{
    public class ParentIssue
    {
        public String Key { get; set; }

        private Issue _issue;
        public Issue Issue
        {
            get
            {
                return _issue ?? (_issue =
                    JiraEnvironment.Instance.Client.GetItem<Issue>(JiraClient.JiraObjectEnum.Issue,
                        keys: new Dictionary<string, string>() { { "issueKey", this.Key } }));
            }
        }
    }
}
