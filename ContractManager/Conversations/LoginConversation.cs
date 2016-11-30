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
        protected override void BeginConversation()
        {
            WaitingForReply = true;

            LoginRequest request = new LoginRequest();
            request.ConvId = Id;
            request.MsgId = Id;
            request.IdentityInfo = IdentityInfo;
            request.ProcessLabel = Process.Label;
            request.ProcessType = Process.Type;
            request.PublicKey = new PublicKey();
            Envelope toSend = new Envelope(Properties.AuthenticatorEndpoint, request);
            SendMessage(toSend);
        }

        protected override void ProcessMessage(Envelope envelope)
        {
            if (envelope.Message != null && envelope.Message.GetType() == typeof(LoginReply))
            {
                LoginReply replyMessage = (LoginReply)envelope.Message;
                Logger.Info("Received Login response: " + replyMessage.Note);
                Properties.Process = replyMessage.ProcessInfo;
                HandleConversationCompleted();
            }
            else
            {
                Logger.Info("Received unexpected message: " + envelope.Message.ToString());
                RetryMessage();
            }
        }

        protected override void HandleConversationCompleted()
        {
            if (Properties.Process.Status == ProcessInfo.StatusCode.Registered)
            {
                Logger.Info("Successfully connected");
            }
            else
            {
                Logger.Warn("Unable to log in");
            }
        }
    }
}
