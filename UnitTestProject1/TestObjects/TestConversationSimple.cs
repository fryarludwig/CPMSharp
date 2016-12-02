
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

namespace TestCommon.TestObjects
{

    public class SimpleConversation : RequestReplyResponder
    {
        public SimpleConversation() : base("Heartbeat")
        {
            EventResponse = null;
            ReceivedMessage = null;
            SentMessage = null;
            WaitingForReply = true;
        }

        protected override void ProcessMessage(Envelope envelope)
        {
            EventResponse = Updated?.Invoke("SimpleConversation");
            ReceivedMessage = envelope;
            SentMessage = new Envelope(new LoginReply());
        }

        public delegate string MessageReceived(string something);
        public event MessageReceived Updated;
        
        public virtual void OnUpdate()
        {
            ProcessMessage(new Envelope(new LoginRequest()));
        }

        public string EventResponse { get; set; }
        public Envelope ReceivedMessage { get; set; }
        public Envelope SentMessage { get; set; }
    }
}