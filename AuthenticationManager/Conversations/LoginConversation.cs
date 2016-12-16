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

namespace AuthenticationManager.Conversations
{
    public class LoginConversation : RequestReplyResponder
    {
        public LoginConversation() : base("Auth - Login Conv")
        {
            RemoteProcess = null;
            RemoteUser = null;
        }

        public override void RegisterConversationCallbacks(DistributedProcess process)
        {
            if (process.GetType() == typeof(AuthManager))
            {
                AuthManager manager = (AuthManager)process;
                
            }
        }

        protected override void ProcessResponse(Envelope envelope)
        {
            if (envelope.Message != null)
            {
                if (envelope.Message.GetType() == typeof(LoginRequest))
                {
                    LoginRequest received = (LoginRequest)envelope.Message;
                    ProcessInfo newInfo = new ProcessInfo();
                    newInfo.EndPoint = envelope.Address;
                    newInfo.Label = received.ProcessLabel;
                    newInfo.Type = received.ProcessType;
                    newInfo = OnLoginUpdated?.Invoke(newInfo);
                    
                    LoginReply reply = new LoginReply();
                    reply.ConvId = Id;
                    reply.MsgId = MessageNumber.Create();
                    reply.Note = (newInfo != null)? "Granted!" : "Bad request";
                    reply.Success = (newInfo != null);
                    reply.ProcessInfo = newInfo ?? new ProcessInfo();

                    Envelope env = new Envelope(newInfo.EndPoint, reply);
                    SendMessage(env);
                }
                else
                {
                    Logger.Info("Received unexpected message: " + envelope.Message);
                }
            }

            WaitingForReply = false;
        }

        public delegate ProcessInfo LoginResponseEvent(ProcessInfo newProcess);
        public event LoginResponseEvent OnLoginUpdated;

        protected ProcessInfo RemoteProcess { get; set; }
        protected User RemoteUser { get; set; }
    }
}
