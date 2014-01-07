using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK
{
    public class Status
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }

        public StatusEnum ToEnum()
        {
            return (StatusEnum)Enum.Parse(typeof (StatusEnum), Name);
        }
    }

    public enum StatusEnum
    {
        Open,
        Reopened,
        Resolved,
        Closed
    }
}
