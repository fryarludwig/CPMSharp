using System;
using System.Collections.Generic;
using System.Text;

namespace Common.WorkItems
{
    public abstract class WorkItem
    {
        protected WorkItem(int workId = 0)
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
