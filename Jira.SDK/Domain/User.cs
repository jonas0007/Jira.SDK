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
        public String DisplayName { get; set; }
        public String EmailAddress { get; set; }

        public String Username { get { return Key ?? Name; } }
        public String Fullname { get { return DisplayName ?? Name; } }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is User)
            {
                return this.Username.Equals((obj as User).Username);    
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.Username.GetHashCode();
        }
    }
}
