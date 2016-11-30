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
            Properties.Process.ProcessId = 0;
            Properties.Process.Type = ProcessInfo.ProcessType.AuthenticationManager;
            Properties.Process.Status = ProcessInfo.StatusCode.Initializing;
            Properties.Process.AliveRetries = 5;
            Properties.Process.AliveTimestamp = DateTime.Now;
            Properties.Process.EndPoint = Properties.LocalEndpoint;
            Properties.Process.Label = "Authentication Manager";

            Logger.Trace("Initialized Client");
        }

        protected override Dictionary<Type, Type> GetValidConversations()
        {
            Dictionary<Type, Type> typeMap = new Dictionary<Type, Type>();
            typeMap[typeof(LoginRequest)] = typeof(LoginConversation);
            typeMap[typeof(AliveRequest)] = typeof(HeartbeatConversation);
            return typeMap;
        }
        

        public void Login()
        {

            if (Process.Status != ProcessInfo.StatusCode.Initializing)
            {
                Logger.Info("Requesting a login");
                Process.Status = ProcessInfo.StatusCode.Initializing;
                //ConversationHandler.Execute(ConversationFactory.CreateType<LoginConversation>());
            }
            else
            {
                Logger.Warn("Status is '" + Process.StatusString + "', waiting for login to complete");
            }
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


        protected void Logout(int Timeout)
        {
            Logger.Info("Requesting logout");
            Process.Status = ProcessInfo.StatusCode.Terminating;
            //ConversationHandler.Execute(ConversationFactory.CreateType<LogoutConversation>());

            int timeSegement = Timeout / 5;
            while (Timeout > 0)
            {
                Thread.Sleep(timeSegement);
                Timeout -= timeSegement;
            }
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

        protected override void Run()
        {

            while (ContinueThread)
            {
                if (Process.Status == ProcessInfo.StatusCode.Unknown || Process.Status == ProcessInfo.StatusCode.NotInitialized)
                {
                    Login();
                }
                else if (Process.Status == ProcessInfo.StatusCode.Registered)
                {
                }
                else if (Process.Status == ProcessInfo.StatusCode.Terminating)
                {
                    break;
                }

                Thread.Sleep(250);
            }

            Logout(2000);
        }

        public void SetRegistryEndpoint(string ip, int port)
        {
            RegistryEndpoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        #region Public Member Variables

        public IPEndPoint RegistryEndpoint
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
        public SharedProperties Properties { get; }
        #endregion

        #region Protected and Private Member Variables
        protected int LoginRetries;
        protected ConversationManager ConversationHandler;
        #endregion
    }
}
