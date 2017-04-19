using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Common.WorkItems
{
    [DataContract]
    public class Phase : WorkItem
    {
        public Phase()
        {
            Tasks = new Dictionary<int, Task>();
        }

        public Dictionary<int, Task> Tasks { get; set; }
    }
}
