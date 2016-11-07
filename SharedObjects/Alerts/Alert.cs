using System;
using System.Collections.Generic;
using System.Text;

using SharedObjects.Users;

namespace SharedObjects.Alerts
{
    public abstract class Alert
    {
        public Alert()
        {

        }

        public enum Priority
        {
            CRITICAL,
            IMPORTANT,
            NOTIFY,
            ADVISE,
            NOT_PRESSING
        }

        public enum Status
        {
            WAITING,
            DELIVERED,
            COMPLETED,
            SNOOZED
        }

        public List<Identity> ApplicableUsers { get; set; }
        public Priority AlertPriority { get; set; }
        public Status AlertStatus { get; set; }
        // Triggers
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
