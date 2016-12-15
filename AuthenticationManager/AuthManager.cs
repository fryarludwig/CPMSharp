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
            ProcessInfo MyProcess = new ProcessInfo();
            MyProcess.ProcessId = 0;
            MyProcess.Type = ProcessInfo.ProcessType.AuthenticationManager;
            MyProcess.Status = ProcessInfo.StatusCode.NotInitialized;
            MyProcess.AliveRetries = 5;
            MyProcess.AliveTimestamp = DateTime.Now;
            MyProcess.Label = "Authentication Manager";

            Logger.Trace("Initialized Authentication Manager");

            ConversationManager.RegisterNewConversationTypes(GetValidConversations());
        }

        protected override void DerivedShutdown()
        {
            Logger.Trace("Calling Derived Stop");
            ConversationManager.ClearConversations();
            MyProcess.Status = ProcessInfo.StatusCode.Terminated;
        }

        public override bool InitializeConnection()
        {
            Logger.Info("Starting Server");
            ConversationManager.PrimaryCommunicator.Start();
            // Wait for 5 seconds, or for a value of true
            int checkCounter = 10;
            while (!ConversationManager.PrimaryCommunicator.IsActive() && checkCounter-- > 0)
            {
                Logger.Trace("Attempting to log in");
                Thread.Sleep(500);
            }

            if (ConversationManager.PrimaryCommunicator.IsActive())
            {
                MyProcess.Status = ProcessInfo.StatusCode.Registered;
            }

            return MyProcess.Status == ProcessInfo.StatusCode.Registered;
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

        public ProcessInfo ValidateUser(User user)
        {
            ProcessInfo processDetails = new ProcessInfo();

            processDetails.Label = user.Alias;

            return processDetails;
        }
    }
}
