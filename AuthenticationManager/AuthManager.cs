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

        public bool StartServerHelper()
        {
            Logger.Info("Starting Server");
            ConversationHandler = new ConversationManager(GetValidConversations(), Properties);
            ConversationHandler.Start();
            base.Start();
            Thread.Sleep(1000);
            return Process.Status == ProcessInfo.StatusCode.Idle;
        }

        public bool ShutdownServerHelper()
        {
            Logger.Info("Shuttind down server");
            if (base.ContinueThread)
            {
                base.Stop();
            }

            // Wait for 5 seconds, or for a value of true
            Logger.Trace("Waiting for shutdown messages to propogate");
            int checkCounter = 5;
            while (Process != null && checkCounter-- > 0)
            {
                Thread.Sleep(1000);
            }

            return Process == null || Process.Status == ProcessInfo.StatusCode.Terminating;
        }


        public void SetLocalEndpoint(int port)
        {
            LocalEndpoint = new IPEndPoint(IPAddress.Any, port);
            Properties.AuthenticatorEndpoint = LocalEndpoint;
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
            Process.Status = ProcessInfo.StatusCode.Idle;

            while (ContinueThread)
            {
                Logger.Info($"The deets: {Properties.Process.ToString()}");
                if (Process.Status == ProcessInfo.StatusCode.Unknown || Process.Status == ProcessInfo.StatusCode.NotInitialized)
                {
                    //Login();
                }
                else if (Process.Status == ProcessInfo.StatusCode.Idle)
                {
                }
                else if (Process.Status == ProcessInfo.StatusCode.Terminating)
                {
                    break;
                }

                Thread.Sleep(1000);
            }

            Logger.Trace("Closing Connection");
            Process.Status = ProcessInfo.StatusCode.Terminated;
        }

        public SharedProperties Properties { get; }
        #endregion

        #region Protected and Private Member Variables
        protected ConversationManager ConversationHandler;
        #endregion
    }
}
