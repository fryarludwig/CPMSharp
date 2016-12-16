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
            MyProcessInfo = new ProcessInfo();
            MyProcessInfo.OnStatusChanged += new ProcessInfo.StatusUpdateEvent(HandleStatusChange);
            ConversationManager.RegisterNewConversationTypes(GetValidConversations());
        }

        public virtual void HandleStatusChange(ProcessInfo.StatusCode status)
        {
            OnStatusChanged?.Invoke(MyProcessInfo);
        }
        
        protected abstract Dictionary<Type, Type> GetValidConversations();

        public virtual void StartConnection()
        {
            Logger.Info("Starting network conncetion");
            ConversationManager.PrimaryCommunicator.Start();
            MyProcessInfo.EndPoint = ConversationManager.PrimaryCommunicator.LocalEndpoint;
        }

        public virtual void CloseConnection()
        {
            Logger.Warn("Shutting down network processes immediately");
            ConversationManager.ClearConversations();
            MyProcessInfo.Status = ProcessInfo.StatusCode.Terminated;
        }
                
        public IPEndPoint LocalEndpoint
        {
            get
            {
                return ConversationManager.PrimaryCommunicator?.LocalEndpoint;
            }
            set
            {
                if (ConversationManager.PrimaryCommunicator == null)
                {
                    ConversationManager.PrimaryCommunicator = new UdpCommunicator();
                }

                ConversationManager.PrimaryCommunicator.LocalEndpoint = value;
            }
        }

        public delegate void ProcessStatusChanged(ProcessInfo processInfo);
        public event ProcessStatusChanged OnStatusChanged;

        public IPEndPoint ContractManagerEndpoint { get; set; }
        public IPEndPoint AuthenticatorEndpoint { get; set; }
        public SharedProperties Properties { get; set; }
        public ProcessInfo MyProcessInfo { get; set; }
        protected LogUtility Logger { get; set; }
    }
}
