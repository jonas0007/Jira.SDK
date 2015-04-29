using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jira.SDK;
using Jira.SDK.Domain;

namespace Jira.SDK.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Jira jira = new Jira();
            jira.Connect("http://jira.prod.hostengine.be", "svc_jira_datawriter", "Zoew6679");
            Issue issue = jira.GetIssue("CR-256");
            List<Issue> blocker = issue.GetBlockingIssues();

            //blocker.GetImpactedIssues();

        }
    }
}
