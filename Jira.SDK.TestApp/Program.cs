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
            List<Task> tasks = new List<Task>();
            for(int i = 1; i <= 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() => {
                    Console.WriteLine("Started " + i);
                    Jira jira = new Jira();
                    jira.Connect("http://jira.prod.hostengine.be", "svc_jira_datawriter", "Zoew6679");
                    DateTime start = DateTime.Now;
                    jira.SearchIssues("((created <= \"2015-04-06\" AND resolved >= \"2015-04-06\") OR (created >= \"2015-04-06\")) AND created <= \"2015-05-06\" AND project = \"IT-DEV Scrum\" AND Type = Ticket");
                    Console.WriteLine(DateTime.Now.Subtract(start));
                }));
            }

            Task.WaitAll(tasks.ToArray());
            Console.Read();
        }
    }
}
