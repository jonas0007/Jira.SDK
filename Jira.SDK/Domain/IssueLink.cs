using System;

namespace Jira.SDK.Domain
{
    public class IssueLink
	{
		public Int32 ID { get; set; }
		public IssueLinkType Type { get; set; }
		public Issue InwardIssue { get; set; }
        public Issue OutwardIssue { get; set; }
	}
}
