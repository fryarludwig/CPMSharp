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
using System.Net;

namespace AuthenticationManager.Conversations
{
    public class HeartbeatConversation : RequestReplyInitiator
    {
        public HeartbeatConversation() : base("Heartbeat")
        {
            ResponseReceived = false;
        }

        public HeartbeatConversation(IPEndPoint target) : base("Heartbeat")
        {
            ResponseReceived = false;
            Destination = target;
        }

        protected override void BeginConversation()
        {
            AliveRequest request = new AliveRequest();
            request.InitMessageAndConversationNumbers();
            Envelope toSend = new Envelope(Destination, request);
            SendMessage(toSend);
        }
        
        protected override void ProcessResponse(Envelope envelope)
        {
            ResponseReceived = (envelope.Message.GetType() == typeof(AliveReply));
            WaitingForReply = false;
        }

        protected override void HandleConversationCompleted()
        {
            Heartbeat_OnUpdate?.Invoke(Id, ResponseReceived);
        }

        public delegate void HeartbeatUpdated(MessageNumber ConvId, bool alive);
        public event HeartbeatUpdated Heartbeat_OnUpdate;
        private bool ResponseReceived { get; set; }
    }
}
