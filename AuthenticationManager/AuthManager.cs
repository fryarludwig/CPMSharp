using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Common.Utilities;
using Common.Communication;
using Common.Users;
using Common.Messages.Replies;
using Common.Messages.Requests;
using AuthenticationManager.Conversations;

namespace AuthenticationManager
{
    public class AuthManager : DistributedProcess
    {
        public AuthManager() : base("AuthManager")
        {
            Properties = SharedProperties.Instance;
            MyProcessInfo.ProcessId = 0;
            MyProcessInfo.Type = ProcessInfo.ProcessType.AuthenticationManager;
            MyProcessInfo.Status = ProcessInfo.StatusCode.NotInitialized;
            MyProcessInfo.AliveRetries = 5;
            MyProcessInfo.AliveTimestamp = DateTime.Now;
            MyProcessInfo.Label = "Authentication Manager";
            ConversationManager.RegisterNewConversationTypes(GetValidConversations());
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

        protected void ConversationEventCallbacks()
        {
            Dictionary<Type, Delegate> CallbackDictionary = new Dictionary<Type, Delegate>();
            CallbackDictionary[typeof(LoginConversation)] = null;
            CallbackDictionary[typeof(HeartbeatConversation)] = null;
        }

        public ProcessInfo LoginUpdated(object sender, EventArgs e)
        {
            return ValidateUser(new User());
        }

        // TODO - this, a lot
        public ProcessInfo ValidateUser(User user)
        {
            ProcessInfo processDetails = new ProcessInfo();
            int index = KnownProcesses.IndexOf(processDetails);

            if (index >= 0)
            {
                KnownProcesses[index] = processDetails;
            }

            processDetails.Label = user.Alias;

            Registration_OnChange?.Invoke(MyProcessInfo);
            return processDetails;
        }

        public delegate void RegistrationChanged(ProcessInfo processInfo);
        public event RegistrationChanged Registration_OnChange;
        private List<ProcessInfo> KnownProcesses { get; set; }
    }
}
