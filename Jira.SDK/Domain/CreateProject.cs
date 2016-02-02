using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK.Domain
{
    public class CreateProject
    {
        public CreateProject()
        {
            AssigneeType = "PROJECT_LEAD";
        }

        [JsonIgnore()]
        public Jira Jira { get; set; }
        [JsonProperty(propertyName: "key")]
        public String Key { get; set; }
        [JsonProperty(propertyName: "name")]
        public String Name { get; set; }
        [JsonProperty(propertyName: "issueSecurityScheme")]
        public Int32 IssueSecurityScheme { get; set; }
        [JsonProperty(propertyName: "permissionScheme")]
        public Int32 PermissionScheme { get; set; }
        [JsonProperty(propertyName: "notificationScheme")]
        public Int32 NotificationScheme { get; set; }
        [JsonProperty(propertyName: "categoryId")]
        public Int32 CategoryId { get; set; }
        [JsonProperty(propertyName: "assigneeType")]
        public string AssigneeType { get; set; }
        [JsonProperty(propertyName: "lead")]
        public string Lead { get; set; }
        [JsonProperty(propertyName: "projectTypeKey")]
        public string ProjectTypeKey { get; set; }
        [JsonProperty(propertyName: "projectTemplateKey")]
        public string ProjectTemplateKey { get; set; }

        public override int GetHashCode()
        {
            return this.Key.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (obj is Project) && this.Key.Equals(((Project)obj).Key);
        }
    }
}