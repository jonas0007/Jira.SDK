using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK.Domain
{
    public class Transition
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }
        public Status To { get; set; }
    }
}
