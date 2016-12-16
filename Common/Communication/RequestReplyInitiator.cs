using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Messages;
using System.Net;
using Common.Utilities;

namespace Common.Communication
{
    public abstract class RequestReplyInitiator : Conversation
    {
        public RequestReplyInitiator(string name) : base(name)
        {
            WaitingForReply = false;
        }

        public RequestReplyInitiator(string name, IPEndPoint target) : base(name)
        {
            WaitingForReply = false;
            Destination = target;
        }

        public new virtual void RegisterConversationCallbacks(DistributedProcess process)
        {
            // Do nothing
        }

        protected abstract override void BeginConversation();

        protected abstract override void ProcessResponse(Envelope envelope);
        protected IPEndPoint Destination { get; set; }
    }
}
