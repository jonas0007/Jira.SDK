using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
