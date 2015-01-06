using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK
{
    public class Status
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }

        public StatusEnum ToEnum()
        {
            return (StatusEnum)Enum.Parse(typeof (StatusEnum), Name.Replace(" ",""));
        }
    }

    public enum StatusEnum
    {
        Open = 0,
        InProgress = 20,
        Reopened = 1,
        Resolved = 30,
        Closed = 32,
		Waitingforinformation = 10,
		Readyfortesting = 31,
		InReview = 32,
		InDevelopment = 21,
		Analysing = 11,
		ReadyforRelease = 31,
		Waitingforapproval = 2,
		AnalysingbyQA = 12,
		Approved = 3,
    }
}
