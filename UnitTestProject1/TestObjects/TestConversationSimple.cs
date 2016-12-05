
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

namespace TestCommon.TestObjects
{

    public class SimpleConversation : Conversation
    {
        public SimpleConversation() : base("Heartbeat")
        {
            EventResponse = null;
            ReceivedMessage = null;
            SentMessage = null;
            WaitingForReply = true;
            Communicator = new UdpCommunicator();
        }

        public void SendHeartbeatRequest(IPEndPoint remoteEndpoint)
        {
            SendMessage(new Envelope(remoteEndpoint, new AliveRequest()));
            WaitingForReply = true;
        }

        public void SendHeartbeatReply(IPEndPoint remoteEndpoint)
        {
            SendMessage(new Envelope(remoteEndpoint, new AliveReply()));
            WaitingForReply = true;
        }

        protected override void HandleConversationCompleted()
        {
            HasCompleted = true;
            Stop();
       }

        protected override void RetryMessage()
        {
            AttemptedRetries++;
            base.RetryMessage();
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

        public int AttemptedRetries = 0;
        public bool HasCompleted { get; set; }

        public new BaseCommunicator Communicator { get; set; }
        public string EventResponse { get; set; }
        public Envelope ReceivedMessage { get; set; }
        public Envelope SentMessage { get; set; }
    }
}