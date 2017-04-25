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
using Common.Messages;

namespace ContractManager
{
    public class ContractManager : DistributedProcess
    {
        public ContractManager() : base("Contract Manager")
        {
            MessageNumber.SetSeqNumber(4923);
            MyProcessInfo.Type = ProcessInfo.ProcessType.ContractManager;
            MyProcessInfo.Status = ProcessInfo.StatusCode.NotInitialized;
            MyProcessInfo.AliveRetries = 5;
            MyProcessInfo.EndPoint = LocalEndpoint;
            MyProcessInfo.Label = "Contract Manager";
            ConversationManager.RegisterNewConversationTypes(GetValidConversations());
        }

        protected override Dictionary<Type, Type> GetValidConversations()
        {
            Dictionary<Type, Type> typeMap = new Dictionary<Type, Type>();
            typeMap[typeof(LoginReply)] = typeof(LoginConversation);
            typeMap[typeof(AliveRequest)] = typeof(HeartbeatConversation);
            return typeMap;
        }

        public override void StartConnection()
        {
            Logger.Info("Attempting to authenticate application");
            MyProcessInfo.Status = ProcessInfo.StatusCode.Initializing;
            ConversationManager.PrimaryCommunicator.Start();
            LoginConversation loginConv = new LoginConversation(AuthenticatorEndpoint, MyProcessInfo);
            loginConv.RegisterDistributedProcessCallbacks(this);
            loginConv.Start();
        }

        public override void CloseConnection()
        {
            Logger.Info("Attempting to authenticate application");
            MyProcessInfo.Status = ProcessInfo.StatusCode.Terminating;
            
            ConversationManager.PrimaryCommunicator.Stop();
        }

        public void HandleLoginUpdated(ProcessInfo myProcess)
        {
            Logger.Trace("Received login response");
            MyProcessInfo = myProcess;
            Registration_OnChange?.Invoke(myProcess);
        }

        public delegate void RegistrationChanged(ProcessInfo processInfo);
        public event RegistrationChanged Registration_OnChange;
    }
}
