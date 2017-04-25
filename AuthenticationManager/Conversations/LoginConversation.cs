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
        }

        public override void RegisterDistributedProcessCallbacks(DistributedProcess process)
        {
            if (!CallbacksRegistered && process.GetType() == typeof(AuthManager))
            {
                AuthManager manager = (AuthManager)process;
                OnLoginUpdated += manager.ValidateProcess;
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

        protected override void ProcessResponse(Envelope envelope)
        {
            if (envelope.Message.GetType() == typeof(LoginRequest))
            {
                LoginRequest received = (LoginRequest)envelope.Message;
                ProcessInfo newInfo = new ProcessInfo
                {
                    EndPoint = envelope.Address,
                    Label = received.ProcessLabel,
                    Type = received.ProcessType
                };
                ProcessInfo procResponse = OnLoginUpdated?.Invoke(newInfo);

                LoginReply reply = new LoginReply(envelope.ConvId)
                {
                    Success = (procResponse != null && procResponse?.Status == ProcessInfo.StatusCode.Registered)
                };
                reply.Note = (reply.Success) ? "Granted!" : "Bad request";
                reply.ProcessInfo = procResponse;

                Envelope env = new Envelope(envelope.Address, reply);
                SendMessage(env);
            }
            else
            {
                Logger.Info("Received unexpected message: " + envelope.Message);
            }

            WaitingForReply = false;
            AllowInboundMessages = false;
        }
        
        public delegate ProcessInfo LoginResponseEvent(ProcessInfo newProcess);
        public event LoginResponseEvent OnLoginUpdated;
    }
}
