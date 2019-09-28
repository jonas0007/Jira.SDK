using System;

namespace Jira.SDK.Domain
{
    public class IssueType
    {
        public IssueType()
        {}

        public IssueType(Int32 id, String name)
        {
            this.ID = id;
            this.Name = name;
        }

        public IssueType(Int32 id, String name, String description, Boolean subtask)
            :this(id, name)
        {
            this.Description = description;
            this.Subtask = subtask;
        }

        public Int32 ID { get; set; }
        public String Name { get; set; }
        public String Description { get; set; }
        public Boolean Subtask { get; set; }
    }
}
