using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK.Domain
{
    public class GroupResult
    {
        public Jira Jira { get; set; }

        public string Self { get; set; }
        public Int32 MaxResults { get; set; }
        public Int32 StartAt { get; set; }
        public Int32 Total { get; set; }
        public bool IsLast { get; set; }
        [JsonProperty(propertyName: "values")]
        public List<User> Users { get; set; }
    }
}
