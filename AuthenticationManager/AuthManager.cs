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
    public class AuthManager : Threaded
    {
        public AuthManager() : base("AuthManager")
        {
            Properties = SharedProperties.Instance;
            Properties.AuthenticatorEndpoint = LocalEndpoint;

            ProcessInfo MyProcess = new ProcessInfo();
            MyProcess.ProcessId = 0;
            MyProcess.Type = ProcessInfo.ProcessType.AuthenticationManager;
            MyProcess.Status = ProcessInfo.StatusCode.Initializing;
            MyProcess.AliveRetries = 5;
            MyProcess.AliveTimestamp = DateTime.Now;
            MyProcess.EndPoint = LocalEndpoint;
            MyProcess.Label = "Authentication Manager";

            Properties.Process = MyProcess;

            CommunicationManager tempInstance = CommunicationManager.ServerInstance;
            ConversationHandler = new ConversationManager(GetValidConversations(), Properties);
            ConversationHandler.Start();

            if (tempInstance.IsServer)
            {
                Logger.Info("Boom, we got a server");
            }
            else
            {
                Logger.Info("Failed to start a server");
            }

            Logger.Trace("Initialized Authentication Manager");
        }


        private Dictionary<Type, Type> GetValidConversations()
        {
            Dictionary<Type, Type> typeMap = new Dictionary<Type, Type>();
            typeMap[typeof(LoginRequest)] = typeof(LoginConversation);
            typeMap[typeof(AliveRequest)] = typeof(HeartbeatConversation);
            return typeMap;
        }


        protected override void DerivedStop()
        {
            Logger.Trace("Calling Derived Stop");
            ConversationHandler.Stop();
        }

        public bool LoginHelper()
        {
            Logger.Info("Attempting to log in");

            base.Start();

            // Wait for 5 seconds, or for a value of true
            int checkCounter = 10;
            while (Process.Status != ProcessInfo.StatusCode.Registered && checkCounter-- > 0)
            {
                Logger.Trace("Attempting to log in");
                Thread.Sleep(500);
            }

            return Process.Status == ProcessInfo.StatusCode.Registered;
        }

        public bool LogoutHelper()
        {
            Logger.Info("Attempting to log out");
            if (base.ContinueThread)
            {
                base.Stop();
            }

            // Wait for 5 seconds, or for a value of true
            int checkCounter = 5;
            while (Process != null && checkCounter-- > 0)
            {
                Logger.Trace("Attempting to log player out");
                Thread.Sleep(1000);
            }

            return Process == null || Process.Status == ProcessInfo.StatusCode.Terminating;
        }


        public void SetLocalEndpoint(int port)
        {
            LocalEndpoint = new IPEndPoint(IPAddress.Any, port);
        }


        #region Public Member Variables

        public IPEndPoint LocalEndpoint
        {
            get
            {
                return Properties.LocalEndpoint;
            }
            set
            {
                Properties.LocalEndpoint = value;
            }
        }

        public Identity IdentityInfo
        {
            get
            {
                return Properties.IdentityInfo;
            }
        }

        public ProcessInfo Process
        {
            get
            {
                return Properties.Process;
            }
        }

        protected override void Run()
        {

            while (ContinueThread)
            {
                Logger.Info($"The deets: {Properties.Process.ToString()}");
                if (Process.Status == ProcessInfo.StatusCode.Unknown || Process.Status == ProcessInfo.StatusCode.NotInitialized)
                {
                    //Login();
                }
                else if (Process.Status == ProcessInfo.StatusCode.Registered)
                {
                }
                else if (Process.Status == ProcessInfo.StatusCode.Terminating)
                {
                    break;
                }


                Thread.Sleep(1000);
            }

            Logger.Trace("Closing Connection");

            //Logout(2000);
        }

        public SharedProperties Properties { get; }
        #endregion

        #region Protected and Private Member Variables
        protected int LoginRetries;
        protected ConversationManager ConversationHandler;
        #endregion
    }
}
