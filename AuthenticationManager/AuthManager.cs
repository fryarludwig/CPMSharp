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
            KnownClients = new Dictionary<ProcessInfo, ProcessInfo>();
            Properties = SharedProperties.Instance;
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
            PendingHeartbeatReplies[conversation.Id] = process;
            Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);
        }

        protected void UpdateProcessStatus(MessageNumber convId, bool alive)
        {
            if (PendingHeartbeatReplies.ContainsKey(convId))
            {
                ProcessInfo process = PendingHeartbeatReplies[convId];
                PendingHeartbeatReplies.Remove(convId);

                if (!alive)
                {
                    process.Status = ProcessInfo.StatusCode.Terminated;
                    process.HeartbeatTimer.Stop();
                    KillProcessConversation conv = new KillProcessConversation(process.ProcessId, process.EndPoint);
                    conv.Register();
                    conv.Start();
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

        private void CommunicatorState_OnChanged(BaseCommunicator.STATE state)
        {
            Logger.Trace("Communicator state has changed to " + state.ToString());

            switch (state)
            {
                case BaseCommunicator.STATE.READY:
                    if (MyProcessInfo.Status != ProcessInfo.StatusCode.Registered && ConversationManager.PrimaryCommunicator.IsActive())
                    {
                        MyProcessInfo.Status = ProcessInfo.StatusCode.Registered;
                        MyProcessInfo.EndPoint = ConversationManager.PrimaryCommunicator.LocalEndpoint;
                    }
                    break;

                case BaseCommunicator.STATE.ERROR:
                    Logger.Error("Communicator encountered an error state, no can continue");
                    break;

                case BaseCommunicator.STATE.BUSY:
                    Logger.Error("There's no use case for this log statement yet.");
                    break;

                case BaseCommunicator.STATE.STOPPED:
                    if (MyProcessInfo.Status == ProcessInfo.StatusCode.Terminating)
                    {
                        MyProcessInfo.Status = ProcessInfo.StatusCode.Terminated;
                    }
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

        public ProcessInfo ValidateProcess(ProcessInfo processDetails)
        {
            bool success = true;

            switch (processDetails.Type)
            {
                case ProcessInfo.ProcessType.Client:
                    processDetails.ProcessId = NewProcessId;
                    KnownClients[processDetails] = processDetails;
                    break;
                case ProcessInfo.ProcessType.ContractManager:
                    processDetails.ProcessId = 1;
                    ContractManagerInfo = processDetails;
                    break;
                default:
                    success = false;
                    break;
            }

            if (success)
            {
                processDetails.Status = ProcessInfo.StatusCode.Registered;
                processDetails.HeartbeatTimer = Scheduler.GetIntervalTimerMinutes(HB_INTERVAL);
                processDetails.HeartbeatTimer.Elapsed += (sender, e) => OnHeartbeatExpired(sender, e, processDetails);
                processDetails.HeartbeatTimer.Enabled = true;
            }
            else
            {
                processDetails = null;
                Logger.Info($"Cannot register {processDetails.LabelAndId} registered");
            }

            return processDetails;
        }

        public delegate void RegistrationChanged(ProcessInfo processInfo);
        public event RegistrationChanged Registration_OnChange;

        protected Timer HeartbeatTimer { get; set; }
        private const int HB_INTERVAL = 5;
        private Dictionary<MessageNumber, ProcessInfo> PendingHeartbeatReplies { get; set; }
        private Dictionary<ProcessInfo, ProcessInfo> KnownClients { get; set; }
        private int NewProcessId { get { return KnownClients.Count + 2; } }
        private ProcessInfo ContractManagerInfo { get; set; }
    }
}
