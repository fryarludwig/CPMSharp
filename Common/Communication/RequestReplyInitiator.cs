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
        }

        public RequestReplyInitiator(string name, NetworkClient communicator) : base(name, communicator)
        {
        }

        public RequestReplyInitiator(string name, IPEndPoint target) : base(name)
        {
            Destination = target;
        }

        public RequestReplyInitiator(string name, NetworkClient communicator, IPEndPoint target) : base(name, communicator)
        {
            Destination = target;
        }

        protected abstract override void BeginConversation();

        protected abstract override void ProcessResponse(Envelope envelope);
        protected IPEndPoint Destination { get; set; }
        public Envelope InitialMessage { get; set; }
    }
}
