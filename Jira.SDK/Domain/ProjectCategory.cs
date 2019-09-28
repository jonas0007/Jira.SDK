using Newtonsoft.Json;
using System;

namespace Jira.SDK.Domain
{
    public class ProjectCategory
    {
        [JsonIgnore()]
        public Jira Jira { get; set; }
        public string Self { get; set; }
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
