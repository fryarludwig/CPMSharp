using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Common.WorkItems
{
    [DataContract]
    public class Effort
    {
        private Effort(int userId, int contractId, int phaseId, int taskId)
        {
            StartTime = DateTime.Now;
            EndTime = StartTime;
            TotalDuration = EndTime - StartTime;
            IsActive = false;

            UserId = userId;
            PhaseId = phaseId;
            ContractId = contractId;
            TaskId = taskId;
        }

        public static Effort GetNewEffort(int userId, int contractId, int phaseId, int taskId)
        {
            return new Effort(userId, contractId, phaseId, taskId);
        }

        public bool StartEffort()
        {
            if (IsActive)
            { 
                return false;
            }
            else
            {
                StartTime = DateTime.Now;
                IsActive = true;
                return true;
            }
        }

        public bool StopEffort()
        {
            if (!IsActive)
            {
                return false;
            }
            else
            {
                EndTime = DateTime.Now;
                IsActive = false;
                TotalDuration += EndTime - StartTime;
                return true;
            }
        }

        [DataMember]
        private bool IsActive { get; set; }

        [DataMember]
        private TimeSpan TotalDuration { get; set; }

        [DataMember]
        protected DateTime StartTime { get; set; }
        [DataMember]
        protected DateTime EndTime { get; set; }
        public TimeSpan TimeElapsed => TotalDuration;

        [DataMember]
        public int EffortId { get; }
        [DataMember]
        public int UserId { get; }
        [DataMember]
        public int TaskId { get; }
        [DataMember]
        public int PhaseId { get; }
        [DataMember]
        public int ContractId { get; }

        [DataMember]
        private static int EffortIdCounter { get; set; }
    }
}
