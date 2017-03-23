using System;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Timers;

namespace Common.Users
{
    [DataContract]
    public class ProcessInfo
    {
        public enum ProcessType { Unknown = 0, AuthenticationManager = 1, ContractManager = 2, Client = 3 };
        public enum StatusCode { Unknown = 0, NotInitialized = 1, Initializing = 2, Registered = 3, Terminating = 4, Terminated = 5 };

        private StatusCode status;
        private static readonly string[] typeNames = new string[] { "Unknown", "AuthenticationManager", "ContractManager", "Client" };
        private static readonly string[] statusNames = new string[] { "Unknown", "Not Initialized", "Initializing", "Registered", "Terminating", "Terminated" };
        private object myLock = new object();

        [DataMember]
        public int ProcessId { get; set; }
        [DataMember]
        public ProcessType Type { get; set; }
        [DataMember]
        public IPEndPoint EndPoint { get; set; }
        [DataMember]
        public string Label { get; set; }
        [DataMember]
        public StatusCode Status
        {
            get
            {
                StatusCode result;
                if (myLock != null)
                    lock (myLock) { result = status; }
                else
                    result = status;
                return result;
            }
            set
            {
                if (myLock != null)
                {
                    lock (myLock)
                    {
                        status = value;
                        OnStatusChanged?.Invoke(status);
                    }
                }
                else
                {
                    status = value;
                    OnStatusChanged?.Invoke(status);
                }
            }
        }

        public string StatusString { get { return statusNames[(int)Status]; } }
        public string TypeString { get { return typeNames[(int)Type]; } }
        
        public int AliveRetries { get; set; }
        public Timer HeartbeatTimer { get; set; }

        public ProcessInfo Clone()
        {
            return MemberwiseClone() as ProcessInfo;
        }

        public static ProcessInfo DeepCopy(ProcessInfo process)
        {
            ProcessInfo copy = new ProcessInfo();
            copy.ProcessId = process.ProcessId;
            copy.Type = process.Type;
            copy.EndPoint = process.EndPoint;
            copy.Label = process.Label;
            copy.Status = process.Status;
            copy.AliveRetries = process.AliveRetries;
            return copy;
        }

        public string LabelAndId
        {
            get
            {
                string result = (!string.IsNullOrWhiteSpace(Label)) ? Label : string.Empty;
                result = string.Format("{0}  ({1})", result, ProcessId);
                return result;
            }
        }

        public override string ToString()
        {
            return string.Format("Id={0}, Label={1}, Type={2}, EndPoint={3}, Status={4}",
                    ProcessId, Label, Type,
                    (EndPoint == null) ? string.Empty : EndPoint.ToString(),
                    StatusString);
        }

        public override int GetHashCode()
        {
            return (LabelAndId.GetHashCode() << 16) | ((int)Type & 0xFFFF);
        }
        
        public delegate void StatusUpdateEvent(StatusCode status);
        public event StatusUpdateEvent OnStatusChanged;
    }
}
