using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Messages;

namespace Common.Communication
{
    public abstract class RequestReplyInitiator : Conversation
    {
        public RequestReplyInitiator(string name) : base(name)
        {
            WaitingForReply = false;
        }

        protected abstract override void BeginConversation();

        protected abstract override void ProcessMessage(Envelope envelope);
    }
}
