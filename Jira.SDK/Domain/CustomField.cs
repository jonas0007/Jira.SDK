using System;

namespace Jira.SDK.Domain
{
    public class CustomField
	{
		public Int32 ID { get; set; }
		public String Value { get; set; }
        

        public CustomField() { }

        public CustomField(String value)
        {
            this.ID = 0;
            this.Value = value;
        }

        public CustomField(Int32 id, String value)
        {
            this.ID = id;
            this.Value = value;
        }
    }
}
