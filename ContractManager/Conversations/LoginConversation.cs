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

namespace ContractManager.Conversations
{
    public class LoginConversation : RequestReplyInitiator
    {
        public LoginConversation() : base("Login Conv")
        {
        }

        public delegate void LoginStatusUpdated(ProcessInfo process);
        public event LoginStatusUpdated OnLoginUpdated;

        protected override void BeginConversation()
        {
            WaitingForReply = true;
            if (InitialMessage != null)
            {
                SendMessage(InitialMessage);
            }
            else
            {
                LoginRequest request = new LoginRequest();
                request.ConvId = Id;
                request.MsgId = Id;
                request.ProcessLabel = "CPM Client";
                request.ProcessType = ProcessInfo.ProcessType.Client;
                request.PublicKey = new PublicKey();
                Envelope toSend = new Envelope(Properties.DistInstance.AuthenticatorEndpoint, request);
                SendMessage(toSend);
            }
        }

        protected override void ProcessMessage(Envelope envelope)
        {
            if (envelope.Message != null && envelope.Message.GetType() == typeof(LoginReply))
            {
                LoginReply replyMessage = (LoginReply)envelope.Message;
                Logger.Info("Received Login response: " + replyMessage.Note);
                MyProcess = replyMessage.ProcessInfo;
                WaitingForReply = false;
            }
            else
            {
                Logger.Info("Received unexpected message: " + envelope.Message.ToString());
            }
        }

        protected override void HandleConversationCompleted()
        {
            if (Properties.DistInstance.MyProcessInfo.Status == ProcessInfo.StatusCode.Registered)
            {
                Logger.Info("Successfully connected");
            }
            else
            {
                Logger.Warn("Unable to log in");
            }

            OnLoginUpdated(MyProcess);
        }

        protected ProcessInfo MyProcess;
    }
}
