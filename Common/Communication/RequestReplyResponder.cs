﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Messages;

namespace Common.Communication
{
    public abstract class RequestReplyResponder : Conversation
    {
        public RequestReplyResponder(string name) : base(name)
        {
            WaitingForReply = false;
        }
        
        protected override void BeginConversation()
        {
            // Do nothing
        }

        protected abstract override void ProcessMessage(Envelope envelope);
    }
}
