using System;

namespace Jira.SDK.Domain
{
    public partial class PermissionScheme
    {
        public Jira Jira { get; set; }

        public PermissionScheme() { }

        public String Self { get; set; }
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
    }
}
