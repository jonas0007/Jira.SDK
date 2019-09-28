using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jira.SDK.Domain
{
    public class IssueSearchResult
    {
        public Int32 Total { get; set; }
        public List<Issue> Issues { get; set; }

        public IssueSearchResult(JObject searchResult)
        {
            Total = (Int32)searchResult["total"];

            JArray issues = (JArray)searchResult["issues"];
            Issues = issues.Select(issue => new Issue((String)issue["key"], (JObject)issue["fields"])).ToList();
        }
    }
}
