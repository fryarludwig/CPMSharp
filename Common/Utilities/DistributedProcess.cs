using Common.Communication;
using Common.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public abstract class DistributedProcess : Threaded
    {
        public DistributedProcess(string name) : base(name)
        {
            Properties = SharedProperties.Instance;
            Properties.AuthenticatorEndpoint = Properties.LocalEndpoint;

            ConversationHandler = new ConversationManager(GetValidConversations());


            Logger.Trace("Initialized Authentication Manager");
        }

        protected override void DerivedStop()
        {
            Logger.Trace("Calling Derived Stop");
            ConversationHandler.Stop();
        }

        protected abstract Dictionary<Type, Type> GetValidConversations();



        public bool InitializeConnection(IPEndPoint localEndpoint, IPEndPoint remoteEndpoint)
        {
            Logger.Info("Starting Server");
            ConversationHandler = new ConversationManager(GetValidConversations(), Properties);
            ConversationHandler.Start();
            base.Start();
            Thread.Sleep(1000);
            return Properties.Process.Status == ProcessInfo.StatusCode.Idle;
        }
        
        public bool ShutdownProcess()
        {
            Logger.Info("Shuttind down server");
            if (base.ContinueThread)
            {
                base.Stop();
            }

            // Wait for 5 seconds, or for a value of true
            Logger.Trace("Waiting for shutdown messages to propogate");
            int checkCounter = 5;
            while (Properties.Process != null && checkCounter-- > 0)
            {
                Thread.Sleep(1000);
            }

            return Properties.Process == null || Properties.Process.Status == ProcessInfo.StatusCode.Terminating;
        }

        public void SetLocalEndpoint(int port)
        {
            Properties.LocalEndpoint = new IPEndPoint(IPAddress.Any, port);
            Properties.AuthenticatorEndpoint = Properties.LocalEndpoint;
        }


        
        protected override void Run()
        {
            Properties.Process.Status = ProcessInfo.StatusCode.Idle;

            while (ContinueThread)
            {
                // We have a status code. 
                // We have an identity
                // We want to generalize this

                Logger.Info($"The deets: {Properties.Process.ToString()}");
                if (Properties.Process.Status == ProcessInfo.StatusCode.Unknown || Properties.Process.Status == ProcessInfo.StatusCode.NotInitialized)
                {
                    //Login();
                }
                else if (Properties.Process.Status == ProcessInfo.StatusCode.Idle)
                {
                }
                else if (Properties.Process.Status == ProcessInfo.StatusCode.Terminating)
                {
                    break;
                }

                Thread.Sleep(500);
            }

            Logger.Trace("Closing Connection");
            Properties.Process.Status = ProcessInfo.StatusCode.Terminated;
        }

        

        public SharedProperties Properties { get; set; }


        protected ConversationManager ConversationHandler { get; set; }
    }
}
