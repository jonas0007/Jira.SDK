using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.SDK
{
    public class User
    {
        public String Key { get; set; }
        public String Name { get; set; }
        public String EmailAddress { get; set; }

        public String Username { get { return Key; }}
        public String Fullname { get { return Name;  }}
    }
}
