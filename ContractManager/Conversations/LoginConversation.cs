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
            AllowRepeats = false;
            CallbacksRegistered = false;
        }

        public override void RegisterConversationCallbacks(DistributedProcess process)
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

        public LoginConversation(IPEndPoint target, ProcessInfo myProcess) : base("Login Conv")
        {
            WaitingForReply = true;
            Destination = target;
            Register();

            WaitingForReply = true;
            LoginRequest request = new LoginRequest();
            request.ConvId = MessageNumber.Create();
            request.ProcessLabel = myProcess.Label;
            request.ProcessType = myProcess.Type;
            request.PublicKey = new PublicKey();
            InitialMessage = new Envelope(target, request);
        }

        protected override void BeginConversation()
        {
            SendMessage(InitialMessage);
        }

        protected override void ProcessResponse(Envelope envelope)
        {
            if (envelope.Message != null && envelope.Message.GetType() == typeof(LoginReply))
            {
                LoginReply replyMessage = (LoginReply)envelope.Message;
                Logger.Info("Received Login response: " + replyMessage.Note);
                OnLoginUpdated?.Invoke(ProcessInfo.DeepCopy(replyMessage.ProcessInfo));
                WaitingForReply = false;
            }
            else
            {
                Logger.Info("Received unexpected message: " + envelope.Message.ToString());
            }
        }
        
        public delegate void LoginStatusUpdated(ProcessInfo process);
        public event LoginStatusUpdated OnLoginUpdated;
    }
}
