﻿using System;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.Generic;

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
        public Int32 ProcessId { get; set; }
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

        public DateTime? AliveTimestamp { get; set; }
        public Int32 AliveRetries { get; set; }

        public ProcessInfo Clone()
        {
            return MemberwiseClone() as ProcessInfo;
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
