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
    public abstract class DistributedProcess
    {
        public DistributedProcess(string name)
        {
            Logger = new LogUtility(name);
            Properties = SharedProperties.Instance;
            Properties.DistInstance = this;
            MyProcess = new ProcessInfo();
            ConversationHandler = new ConversationManager(GetValidConversations());
        }

        protected void DerivedShutdown()
        {
            Logger.Trace("Calling Derived Stop");
            ConversationHandler.Stop();
        }

        protected abstract Dictionary<Type, Type> GetValidConversations();

        public virtual bool InitializeConnection()
        {
            Logger.Info("Starting Server");
            ConversationHandler.Start();
            // Wait for 5 seconds, or for a value of true
            int checkCounter = 10;
            while (MyProcess.Status != ProcessInfo.StatusCode.Registered && checkCounter-- > 0)
            {
                Logger.Trace("Attempting to log in");
                Thread.Sleep(500);
            }
            return MyProcess.Status == ProcessInfo.StatusCode.Registered;
        }

        public virtual bool ShutdownProcess()
        {
            Logger.Info("Shutting down");
            DerivedShutdown();

            // Wait for 5 seconds, or for a value of true
            Logger.Trace("Waiting for shutdown messages to propogate");
            int checkCounter = 5;
            while (MyProcess != null && checkCounter-- > 0)
            {
                Thread.Sleep(1000);
            }

            return MyProcess.Status == ProcessInfo.StatusCode.Terminated || MyProcess.Status == ProcessInfo.StatusCode.Terminating;
        }



        //protected abstract void 

        // Auth
        // Secure port, set as registered, wait
        // For conversations, check credentials
        // If valid, populate user info and send back reply
        // If not valid, send back reply
        // Conversations be like: Brain, let's try this.
        // Brain be like, naw. Conversation be like, okay I'll tell them


        public IPEndPoint LocalEndpoint
        {
            get
            {
                if (ConversationFactory.PrimaryCommunicator != null)
                {
                    return ConversationFactory.PrimaryCommunicator.LocalEndpoint;
                }

                return null;
            }
            set
            {
                if (ConversationFactory.PrimaryCommunicator == null)
                {
                    ConversationFactory.PrimaryCommunicator = new UdpCommunicator();
                }

                ConversationFactory.PrimaryCommunicator.LocalEndpoint = value;
            }

        }

        public IPEndPoint ContractManagerEndpoint { get; set; }
        public IPEndPoint AuthenticatorEndpoint { get; set; }
        public SharedProperties Properties { get; set; }
        public ProcessInfo MyProcess { get; set; }
        protected ConversationManager ConversationHandler { get; set; }
        protected LogUtility Logger { get; set; }
    }
}
