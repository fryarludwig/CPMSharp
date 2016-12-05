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

namespace ContractManager
{
    public class ContractManager : DistributedProcess
    {
        public ContractManager() : base("Contract Manager")
        {


            ProcessInfo MyProcess = new ProcessInfo();
            MyProcess.ProcessId = 0;
            MyProcess.Type = ProcessInfo.ProcessType.ContractManager;
            MyProcess.Status = ProcessInfo.StatusCode.Unknown;
            MyProcess.AliveRetries = 5;
            MyProcess.AliveTimestamp = DateTime.Now;
            MyProcess.EndPoint = LocalEndpoint;
            MyProcess.Label = "Contract Manager";

            ConversationManager.RegisterNewConversationTypes(GetValidConversations());
        }


        protected override Dictionary<Type, Type> GetValidConversations()
        {
            Dictionary<Type, Type> typeMap = new Dictionary<Type, Type>();
            typeMap[typeof(LoginRequest)] = typeof(LoginConversation);
            typeMap[typeof(AliveRequest)] = typeof(HeartbeatConversation);
            return typeMap;
        }

        public override bool InitializeConnection()
        {
            Logger.Info("Starting Server");
            ConversationManager.PrimaryCommunicator.Start();
            LoginConversation loginConv = (LoginConversation)ConversationManager.CreateNewConversation(GetLoginMessage());
            loginConv.OnLoginUpdated += new LoginConversation.LoginStatusUpdated(HandleLoginUpdated);
            loginConv.Start();
            // Wait for 5 seconds, or for a value of true
            int checkCounter = 10;
            while (MyProcess.Status != ProcessInfo.StatusCode.Registered && checkCounter-- > 0)
            {
                Logger.Trace("Attempting to log in");
                Thread.Sleep(500);
            }
            return MyProcess.Status == ProcessInfo.StatusCode.Registered;
        }

        protected Envelope GetLoginMessage()
        {
            Envelope myEnvelope = new Envelope(AuthenticatorEndpoint, new LoginRequest());
            return myEnvelope;
        }

        protected void HandleLoginUpdated(ProcessInfo myProcess)
        {
            MyProcess = myProcess;
            Logger.Info("Login status updated!");
        }
                
        protected void Register(int attempt)
        {
            if (MyProcess.Status != ProcessInfo.StatusCode.Initializing)
            {
                Logger.Info("Requesting a login");
                MyProcess.Status = ProcessInfo.StatusCode.Initializing;
                Envelope envelope = new Envelope(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555), new LoginRequest());
                //InitiateConversation(envelope);
            }
            else if (attempt > LoginRetries)
            {
                Logger.Warn("Login attempt failed.");
                MyProcess.Status = ProcessInfo.StatusCode.Terminating;
            }
            else
            {
                Logger.Warn("Status is '" + MyProcess.StatusString + "', waiting for login to complete");
            }
        }

        public int LoginRetries { get; set; }
    }
}
