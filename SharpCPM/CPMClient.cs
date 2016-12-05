using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Utilities;
using Common.Messages;
using Common.Alerts;
using Common.Communication;
using Common.Users;
using Common.WorkItems;
using System.Net;
using System.Threading;

using SharpCPM.ClientConversation;
using Common.Messages.Replies;
using Common.Messages.Requests;

namespace SharpCPM
{
    public class CPMClient : DistributedProcess
    {
        public CPMClient() : base("CPMClient")
        {
            MyProcess.Type = ProcessInfo.ProcessType.Client;
            MyProcess.Status = ProcessInfo.StatusCode.NotInitialized;
            MyProcess.AliveRetries = 5;
            MyProcess.AliveTimestamp = DateTime.Now;
            //MyProcess.EndPoint = localEndpoint;
            MyProcess.Label = "CPM Client";

            
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

        protected void HandleLoginUpdated(ProcessInfo myProcess)
        {
            MyProcess = myProcess;
            Logger.Info("Login status updated!");
        }

        protected Envelope GetLoginMessage()
        {
            Envelope myEnvelope = new Envelope(AuthenticatorEndpoint, new LoginRequest());
            return myEnvelope;
        }

        public void Login()
        {

            if (MyProcess.Status != ProcessInfo.StatusCode.Initializing)
            {
                Logger.Info("Requesting a login");
                MyProcess.Status = ProcessInfo.StatusCode.Initializing;
                //ConversationHandler.Execute(ConversationFactory.CreateNewConversation<LoginConversation>());
            }
            else
            {
                Logger.Warn("Status is '" + MyProcess.StatusString + "', waiting for login to complete");
            }
        }
        
        protected void Logout(int Timeout)
        {
            Logger.Info("Requesting logout");
            MyProcess.Status = ProcessInfo.StatusCode.Terminating;
            //ConversationHandler.Execute(ConversationFactory.CreateType<LogoutConversation>());

            int timeSegement = Timeout / 5;
            while (Timeout > 0)
            {
                Thread.Sleep(timeSegement);
                Timeout -= timeSegement;
            }
        }
        
        
        protected int LoginRetries;
    }
}
