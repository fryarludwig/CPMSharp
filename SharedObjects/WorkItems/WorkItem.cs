using System;
using System.Collections.Generic;
using System.Text;

namespace SharedObjects.WorkItems
{
    public abstract class WorkItem
    {
        protected WorkItem(int workId)
        {
            WorkId = workId;
        }

        public int WorkId { get; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
