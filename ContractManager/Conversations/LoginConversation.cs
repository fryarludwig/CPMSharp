using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Communication;
using Common.Messages.Replies;
using Common.Messages.Requests;
using Common.Messages;
using Common.Users;
using Common.Utilities;
using System.Threading;
using System.Net;

namespace ContractManager.Conversations
{
    public class LoginConversation : RequestReplyInitiator
    {
        public LoginConversation() : base("Login Conv")
        {
            WaitingForReply = true;
        }

        public LoginConversation(IPEndPoint target, ProcessInfo myProcess) : base("Login Conv", target)
        {
            WaitingForReply = true;
            LoginRequest request = new LoginRequest
            {
                ProcessLabel = myProcess.Label,
                ProcessType = myProcess.Type,
                PublicKey = new PublicKey()
            };
            InitialMessage = new Envelope(target, request);
        }

        public override void RegisterDistributedProcessCallbacks(DistributedProcess process)
        {
            if (!CallbacksRegistered && process.GetType() == typeof(ContractManager))
            {
                ContractManager manager = (ContractManager)process;
                OnLoginUpdated += manager.HandleLoginUpdated;
                CallbacksRegistered = true;
                Logger.Info("Callbacks are now registered");
            }
            else if (CallbacksRegistered)
            {
                Logger.Info("Callbacks have already been registered");
            }
            else
            {
                Logger.Warn("Unable to assign unknown dist process to event");
            }
        }

        protected override void BeginConversation()
        {
            if (InitialMessage != null)
            {
                SendMessage(InitialMessage);
            }
        }

        protected override void ProcessResponse(Envelope envelope)
        {
            if (envelope?.Message != null && envelope.Message?.GetType() == typeof(LoginReply))
            {
                LoginReply replyMessage = (LoginReply)envelope.Message;
                Logger.Info("Received Login response: " + replyMessage.Note);
                OnLoginUpdated?.Invoke(ProcessInfo.DeepCopy(replyMessage.ProcessInfo));
                WaitingForReply = false;
            }
            else if (envelope != null)
            {
                Logger.Info("Received unexpected message: " + envelope.Message?.ToString());
            }
            else
            {
                Logger.Info("Received completely null envelope");
            }
        }
        
        public delegate void LoginStatusUpdated(ProcessInfo process);
        public event LoginStatusUpdated OnLoginUpdated;
    }
}
