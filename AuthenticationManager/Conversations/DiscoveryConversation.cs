using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Communication;
using System.Threading;
using Common.Messages;
using Common.Messages.Requests;
using Common.Messages.Replies;

namespace AuthenticationManager.Conversations
{
    class DiscoveryConversation : RequestReplyInitiator
    {
        public DiscoveryConversation() : base("ConvDiscovery")
        {
        }
        
        protected override void BeginConversation()
        {
            throw new NotImplementedException();
        }

        protected override void ProcessMessage(Envelope envelope)
        {
            throw new NotImplementedException();
        }
    }
}
