using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Communication;
using Common.Messages;
using Common.Messages.Replies;
using Common.Messages.Requests;
using Common.Utilities;
using System.Threading;

namespace AuthenticationManager.Conversations
{
    public class HeartbeatConversation : RequestReplyInitiator
    {
        public HeartbeatConversation() : base("Heartbeat")
        {
            ResponseReceived = false;
        }
        
        protected override void BeginConversation()
        {
            AliveRequest request = new AliveRequest();
            request.InitMessageAndConversationNumbers();
            // TODO Add list of subscribers
            Envelope toSend = new Envelope(request);
            SendMessage(toSend);
        }
        
        protected override void ProcessMessage(Envelope envelope)
        {
            if (envelope.Message.GetType() == typeof(AliveReply))
            {
                ResponseReceived = true;
            }

            HandleConversationCompleted();
        }

        protected override void HandleConversationCompleted()
        {
            if (ResponseReceived)
            {
                Logger.Info("This process is still alive");
            }
            else
            {
                Logger.Warn("Beginning deregistration process");
            }
        }

        private bool ResponseReceived { get; set; }
    }
}
