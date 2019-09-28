using Newtonsoft.Json;

namespace Jira.SDK.Domain
{
    public class ProjectType
    {
        [JsonIgnore()]
        public Jira Jira { get; set; }
        
        [JsonProperty(propertyName: "key")]
        public string Key { get; set; }
        [JsonProperty(propertyName: "formattedkey")]
        public string FormattedKey { get; set; }
        [JsonProperty(propertyName: "color")]
        public string Color { get; set; }
        [JsonProperty(propertyName: "description|i18nKey")]
        public string Description { get; set; }
        [JsonProperty(propertyName: "icon")]
        public string icon { get; set; }
    }
}
