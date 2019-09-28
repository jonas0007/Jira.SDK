using System;

namespace Jira.SDK.Domain
{
    public partial class IssueSecurityScheme
    {
        public Jira Jira { get; set; }

        public IssueSecurityScheme() { }

        public String Self { get; set; }
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public String Description { get; set; }

        public Int32 DefaultSecurityLevelId { get; set; }
    }
}
