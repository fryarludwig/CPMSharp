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
            //Properties.Process = MyProcess;

            ConversationHandler = new ConversationManager(GetValidConversations());
        }


        protected override Dictionary<Type, Type> GetValidConversations()
        {
            Dictionary<Type, Type> typeMap = new Dictionary<Type, Type>();
            typeMap[typeof(LoginRequest)] = typeof(LoginConversation);
            typeMap[typeof(AliveRequest)] = typeof(HeartbeatConversation);
            return typeMap;
        }

        
        public bool LoginHelper()
        {
            Logger.Info("Attempting to connect");

            // Wait for 5 seconds, or for a value of true
            int checkCounter = 10;
            while (MyProcess.Status != ProcessInfo.StatusCode.Registered && checkCounter-- > 0)
            {
                Thread.Sleep(500);
            }

            return MyProcess.Status == ProcessInfo.StatusCode.Registered;
        }

        public bool LogoutHelper()
        {
            Logger.Info("Attempting to log out");

            // Wait for 5 seconds, or for a value of true
            int checkCounter = 5;
            while (MyProcess != null && checkCounter-- > 0)
            {
                Logger.Trace("Attempting to log out");
                Thread.Sleep(1000);
            }

            return MyProcess == null || MyProcess.Status == ProcessInfo.StatusCode.Terminating;
        }

        public void SetLocalEndpoint(int port)
        {
            LocalEndpoint = new IPEndPoint(IPAddress.Any, port);
            MyProcess.EndPoint = LocalEndpoint;
        }
        
        public IPEndPoint AuthenticatorEndpoint { get; set; }
        
        protected void Register(int attempt)
        {
            if (MyProcess.Status != ProcessInfo.StatusCode.Initializing)
            {
                Logger.Info("Requesting a login");
                MyProcess.Status = ProcessInfo.StatusCode.Initializing;
                Envelope envelope = new Envelope(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555), new LoginRequest());
                ConversationHandler.InitiateConversation(envelope);
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
