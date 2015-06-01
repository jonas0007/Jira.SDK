using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
