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
            MyProcess.OnStatusChanged += new ProcessInfo.StatusUpdateEvent(HandleStatusChange);
            ConversationManager.RegisterNewConversationTypes(GetValidConversations());
        }

        public virtual void HandleStatusChange(ProcessInfo.StatusCode status)
        {
            OnStatusChanged?.Invoke(MyProcess);
        }
        
        protected abstract Dictionary<Type, Type> GetValidConversations();

        public virtual bool StartConnection()
        {
            Logger.Info("Starting network conncetion");
            ConversationManager.PrimaryCommunicator.Start();
            int checkCounter = 6;
            while (MyProcess.Status != ProcessInfo.StatusCode.Registered && checkCounter-- > 0)
            {
                Thread.Sleep(500);
            }

            return MyProcess.Status == ProcessInfo.StatusCode.Registered;
        }

        public virtual bool CloseConnection()
        {
            Logger.Warn("Shutting down network processes immediately");
            ConversationManager.ClearConversations();
            MyProcess.Status = ProcessInfo.StatusCode.Terminated;
            return MyProcess.Status == ProcessInfo.StatusCode.Terminated || MyProcess.Status == ProcessInfo.StatusCode.Terminating;
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
        public ProcessInfo MyProcess { get; set; }
        protected LogUtility Logger { get; set; }
    }
}
