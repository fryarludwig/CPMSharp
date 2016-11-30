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
    public class ContractManager : Threaded
    {
        public ContractManager() : base("Contract Manager")
        {
            Properties = SharedProperties.Instance;
            Properties.AuthenticatorEndpoint = AuthenticatorEndpoint;

            ProcessInfo MyProcess = new ProcessInfo();
            MyProcess.ProcessId = 0;
            MyProcess.Type = ProcessInfo.ProcessType.ContractManager;
            MyProcess.Status = ProcessInfo.StatusCode.Unknown;
            MyProcess.AliveRetries = 5;
            MyProcess.AliveTimestamp = DateTime.Now;
            MyProcess.EndPoint = LocalEndpoint;
            MyProcess.Label = "Contract Manager";
            Properties.Process = MyProcess;

            ConversationHandler = new ConversationManager(PopulateConversationTypes(), Properties);

            Logger.Trace("Initialized Authentication Manager");
        }


        private Dictionary<Type, Type> PopulateConversationTypes()
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
            Logger.Info("Attempting to connect");

            base.Start();

            // Wait for 5 seconds, or for a value of true
            int checkCounter = 10;
            while (Process.Status != ProcessInfo.StatusCode.Registered && checkCounter-- > 0)
            {
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
                Logger.Trace("Attempting to log out");
                Thread.Sleep(1000);
            }

            return Process == null || Process.Status == ProcessInfo.StatusCode.Terminating;
        }

        public void SetLocalEndpoint(int port)
        {
            LocalEndpoint = new IPEndPoint(IPAddress.Any, port);
            Properties.LocalEndpoint = LocalEndpoint;
        }

        public void SetAuthenticatorEndpoint(string ip, int port)
        {
            AuthenticatorEndpoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }


        #region Public Member Variables

        public IPEndPoint AuthenticatorEndpoint
        {
            get
            {
                return Properties.AuthenticatorEndpoint;
            }
            set
            {
                Properties.AuthenticatorEndpoint = value;
            }
        }

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

        public User IdentityInfo
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

        protected void Register(int attempt)
        {
            if (Process.Status != ProcessInfo.StatusCode.Initializing)
            {
                Logger.Info("Requesting a login");
                Process.Status = ProcessInfo.StatusCode.Initializing;
                Envelope envelope = new Envelope(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5555), new LoginRequest());
                ConversationHandler.InitiateConversation(envelope);
            }
            else if (attempt > LoginRetries)
            {
                Logger.Warn("Login attempt failed.");
                Stop();
                Process.Status = ProcessInfo.StatusCode.Terminating;
            }
            else
            {
                Logger.Warn("Status is '" + Process.StatusString + "', waiting for login to complete");
            }
        }

        protected override void Run()
        {
            int loginAttempt = 0;

            while (ContinueThread)
            {
                Logger.Info($"The deets: {Properties.Process.ToString()}");

                if (Process.Status == ProcessInfo.StatusCode.Unknown || Process.Status == ProcessInfo.StatusCode.NotInitialized)
                {
                    Register(loginAttempt++);
                }
                else if (Process.Status == ProcessInfo.StatusCode.Registered)
                {
                    Logger.Info("Successfully registered!");
                }
                else if (Process.Status == ProcessInfo.StatusCode.Terminating)
                {
                    break;
                }

                Thread.Sleep(500);
            }

            Logger.Warn("Closing Connection");

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
