using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using RestSharp;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace Jira.SDK.Domain
{
    public class NotificationSchemeEventEvent
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public String Description { get; set; }
    }

    public class NotificationSchemeEventNotification
    {
        public Int32 Id { get; set; }
        public string NotificationType { get; set; }
    }

    public class NotificationSchemeEvent
    {
        public Jira Jira { get; set; }

        public NotificationSchemeEvent() { }

        public NotificationSchemeEventEvent Event { get; set; }

        public List<NotificationSchemeEventNotification> Notifications { get; set; }
    }
}
