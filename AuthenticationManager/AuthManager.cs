using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

using Common.Utilities;
using Common.Communication;
using Common.Users;
using Common.Messages.Replies;
using Common.Messages.Requests;
using AuthenticationManager.Conversations;
using System.Timers;
using Common.Messages;

namespace AuthenticationManager
{
    public class AuthManager : DistributedProcess
    {
        public AuthManager() : base("AuthManager")
        {
            MessageNumber.SetSeqNumber(84);
            HeartbeatIntervalMs = 30000;
            KnownClients = new Dictionary<int, ProcessInfo>();
            PendingHeartbeatReplies = new Dictionary<MessageNumber, int>();
            MyProcessInfo.ProcessId = 0;
            MyProcessInfo.Type = ProcessInfo.ProcessType.AuthenticationManager;
            MyProcessInfo.Status = ProcessInfo.StatusCode.NotInitialized;
            MyProcessInfo.AliveRetries = 5;
            MyProcessInfo.Label = "Authentication Manager";
            ContractManagerInfo = null;
            ConversationManager.RegisterNewConversationTypes(GetValidConversations());
        }

        private void OnHeartbeatExpired(object source, ElapsedEventArgs e, ProcessInfo process)
        {
            HeartbeatConversation conversation = new HeartbeatConversation(process.EndPoint);
            conversation.Heartbeat_OnUpdate += UpdateProcessStatus;
            conversation.Start();
        }

        protected void UpdateProcessStatus(MessageNumber convId, bool alive)
        {
            if (PendingHeartbeatReplies.ContainsKey(convId))
            {
                int processId = PendingHeartbeatReplies[convId];
                PendingHeartbeatReplies.Remove(convId);
                
                if (!alive && KnownClients.ContainsKey(processId))
                {
                    Logger.Info("Process " + processId + " is not responding, removing from current connections");
                    ProcessInfo process = KnownClients[processId];
                    process.Status = ProcessInfo.StatusCode.Terminated;
                    process.HeartbeatTimer.Stop();
                    KillProcessConversation conv = new KillProcessConversation(process.ProcessId, process.EndPoint);
                    conv.Start();
                }
                else
                {
                    Logger.Info("Process " + processId.ToString() + " is still responding");
                }
            }
            else
            {
                Logger.Error("Received unexpected response from heatbeat conversation");
            }

        }
        
        public override void StartConnection()
        {
            Logger.Info("Starting Authentication Server");
            ConversationManager.PrimaryCommunicator.State_OnChanged += CommunicatorState_OnChanged;
            ConversationManager.PrimaryCommunicator.Start();
            Registration_OnChange?.Invoke(MyProcessInfo);
        }

        private void CommunicatorState_OnChanged(NetworkClient.STATE state)
        {
            Logger.Trace("Communicator state has changed to " + state.ToString());

            switch (state)
            {
                case NetworkClient.STATE.Ready:
                    if (MyProcessInfo.Status != ProcessInfo.StatusCode.Registered && ConversationManager.PrimaryCommunicator.IsActive)
                    {
                        MyProcessInfo.Status = ProcessInfo.StatusCode.Registered;
                        MyProcessInfo.EndPoint = ConversationManager.PrimaryCommunicator.LocalEndpoint;
                    }
                    break;

                case NetworkClient.STATE.Error:
                    Logger.Error("Communicator encountered an error state, no can continue");
                    break;

                case NetworkClient.STATE.Busy:
                    Logger.Error("There's no use case for this log statement yet.");
                    break;

                case NetworkClient.STATE.Stopped:
                    if (MyProcessInfo.Status == ProcessInfo.StatusCode.Terminating)
                    {
                        MyProcessInfo.Status = ProcessInfo.StatusCode.Terminated;
                    }
                    break;
                default:
                    Logger.Error("Communicator has entered a state that should not have been possible");
                    break;
            }
        }

        protected override Dictionary<Type, Type> GetValidConversations()
        {
            Dictionary<Type, Type> typeMap = new Dictionary<Type, Type>();
            typeMap[typeof(LoginRequest)] = typeof(LoginConversation);
            typeMap[typeof(AliveRequest)] = typeof(HeartbeatConversation);
            return typeMap;
        }

        public ProcessInfo ValidateProcess(ProcessInfo newProcess)
        {
            if (newProcess == null) { return null; }

            Logger.Trace("Received login request from a process");
            bool success = true;
            switch (newProcess.Type)
            {
                case ProcessInfo.ProcessType.Client:
                    newProcess.ProcessId = NewProcessId;
                    KnownClients[newProcess.ProcessId] = newProcess;
                    break;
                case ProcessInfo.ProcessType.ContractManager:
                    newProcess.ProcessId = 1;
                    ContractManagerInfo = newProcess;
                    break;
                default:
                    success = false;
                    break;
            }

            if (success)
            {
                newProcess.Status = ProcessInfo.StatusCode.Registered;
                newProcess.HeartbeatTimer = Scheduler.GetIntervalTimerMillis(HeartbeatIntervalMs);
                newProcess.HeartbeatTimer.Elapsed += (sender, e) => OnHeartbeatExpired(sender, e, newProcess);
                newProcess.HeartbeatTimer.Enabled = true;
                Registration_OnChange?.Invoke(ProcessInfo.DeepCopy(newProcess));
            }
            else
            {
                newProcess.Status = ProcessInfo.StatusCode.Terminated;
                Logger.Info($"Cannot register {newProcess.LabelAndId} ");
            }

            return ProcessInfo.DeepCopy(newProcess);
        }

        public delegate void RegistrationChanged(ProcessInfo processInfo);
        public event RegistrationChanged Registration_OnChange;

        protected Timer HeartbeatTimer { get; set; }
        public int HeartbeatIntervalMs { get; set; }
        private Dictionary<MessageNumber, int> PendingHeartbeatReplies { get; set; }
        private Dictionary<int, ProcessInfo> KnownClients { get; set; }
        private int NewProcessId => KnownClients.Count + 2;
        private ProcessInfo ContractManagerInfo { get; set; }
    }
}
