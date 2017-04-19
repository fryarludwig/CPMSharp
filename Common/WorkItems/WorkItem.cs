using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.WorkItems
{
    [DataContract]
    public abstract class WorkItem
    {
        protected WorkItem()
        {
            // Do nothing
        }

        public int BeginEffort(int userId)
        {
            Effort newEffort = Effort.GetNewEffort(userId, ContractId, PhaseId, TaskId);
            newEffort.StartEffort();
            EffortList[newEffort.EffortId] = newEffort;
            return newEffort.EffortId;
        }

        public TimeSpan EndEffort(int effortId)
        {
            if (EffortList.ContainsKey(effortId) && EffortList[effortId].StopEffort())
            {
                return EffortList[effortId].TimeElapsed;
            }
            else
            {
                return new TimeSpan(0);
            }
        }

        public TimeSpan TotalEffort
        {
            get
            {
                TimeSpan sum = new TimeSpan(0);
                foreach (Effort effort in EffortList.Values)
                {
                    sum += effort.TimeElapsed;
                }
                return sum;
            }
        }

        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public WorkItemType ItemType { get; set; }
        [DataMember]
        public ItemStatus Status { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
        [DataMember]
        public DateTime Deadline { get; set; }
        [DataMember]
        public string Priority { get; set; }
        [DataMember]
        public Dictionary<int, Effort> EffortList { get; set; }

        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Latitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public string Materials { get; set; }

        [DataMember]
        public List<Comment> Comments { get; set; }
        [DataMember]
        public int OwnerUserId { get; set; }
        [DataMember]
        public int ReviewerUserId { get; set; }

        [DataMember]
        public int PhaseId { get; set; }
        [DataMember]
        public int ContractId { get; set; }
        [DataMember]
        public int TaskId { get; set; }

        public enum WorkItemType
        {
            Unkown = 0,
            Contract = 1,
            Phase = 2,
            Task = 3
        }

        public enum ItemStatus
        {
            Draft = 0,
            Bid = 1,
            Preparation = 2,
            InProgress = 3,
            Reviewed = 4,
            Completed = 5,
            Billed = 6,
            Paid = 7,
            Closed = 8,
            OnHold = 9
        }
    }
}
