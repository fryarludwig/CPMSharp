using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Utilities;
using Common.Communication;
using Common.Users;
using Common.Messages.Replies;
using Common.Messages.Requests;
using ContractManager.Conversations;
using System.Threading;
using System.Net;
using Common.Messages;

namespace ContractManager
{
    public class ContractManager : DistributedProcess
    {
        public ContractManager() : base("Contract Manager")
        {
            MyProcessInfo.Type = ProcessInfo.ProcessType.ContractManager;
            MyProcessInfo.Status = ProcessInfo.StatusCode.NotInitialized;
            MyProcessInfo.AliveRetries = 5;
            MyProcessInfo.AliveTimestamp = DateTime.Now;
            MyProcessInfo.EndPoint = LocalEndpoint;
            MyProcessInfo.Label = "Contract Manager";
            ConversationManager.RegisterNewConversationTypes(GetValidConversations());
        }

        protected override Dictionary<Type, Type> GetValidConversations()
        {
            Dictionary<Type, Type> typeMap = new Dictionary<Type, Type>();
            typeMap[typeof(LoginRequest)] = typeof(LoginConversation);
            typeMap[typeof(AliveRequest)] = typeof(HeartbeatConversation);
            return typeMap;
        }

        public override void StartConnection()
        {
            Logger.Info("Attempting to authenticate application");
            ConversationManager.PrimaryCommunicator.Start();
            LoginConversation loginConv = (LoginConversation)ConversationManager.CreateNewConversation(GetLoginMessage());
            loginConv.OnLoginUpdated += new LoginConversation.LoginStatusUpdated(HandleLoginUpdated);
            loginConv.Start();
            // Wait for 5 seconds, or for a value of true
            //int checkCounter = 10;
            //while (MyProcessInfo.Status != ProcessInfo.StatusCode.Registered && checkCounter-- > 0) { Thread.Sleep(500); }
            //return MyProcessInfo.Status == ProcessInfo.StatusCode.Registered;
        }

        protected Envelope GetLoginMessage()
        {
            Envelope myEnvelope = new Envelope(AuthenticatorEndpoint, new LoginRequest());
            return myEnvelope;
        }

        protected void HandleLoginUpdated(ProcessInfo myProcess)
        {
            MyProcessInfo = myProcess;
            Logger.Info("Login status updated!");
        }
                
        protected void Register(int attempt)
        {
            if (MyProcessInfo.Status != ProcessInfo.StatusCode.Initializing)
            {
                Logger.Info("Requesting a login");
                MyProcessInfo.Status = ProcessInfo.StatusCode.Initializing;
                Envelope envelope = new Envelope(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555), new LoginRequest());
                //InitiateConversation(envelope);
            }
            else if (attempt > LoginRetries)
            {
                Logger.Warn("Login attempt failed.");
                MyProcessInfo.Status = ProcessInfo.StatusCode.Terminating;
            }
            else
            {
                Logger.Warn("Status is '" + MyProcessInfo.StatusString + "', waiting for login to complete");
            }
        }

        public int LoginRetries { get; set; }
    }
}
