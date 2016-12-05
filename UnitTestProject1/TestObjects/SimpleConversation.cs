
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
        public SimpleConversation() : base("Heartbeat", new UdpCommunicator())
        {
            EventResponse = null;
            ReceivedMessage = null;
            SentMessage = null;
            WaitingForReply = true;
            ConversationManager.RegisterCommunicatorCallback(base.Communicator);
        }

        public SimpleConversation(MessageNumber msgNum = null) : base("Heartbeat", new UdpCommunicator(), msgNum)
        {
            EventResponse = null;
            ReceivedMessage = null;
            SentMessage = null;
            WaitingForReply = true;
            ConversationManager.RegisterCommunicatorCallback(base.Communicator);
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
            Envelope tempEnv;
            if (Communicator.ReplyWaiting && Communicator.Receive(out tempEnv))
            {
                NewMessages.Enqueue(tempEnv);
            }
            else
            {
                AttemptedRetries++;
                base.RetryMessage();
            }
        }

        protected override void ProcessMessage(Envelope envelope)
        {
            EventResponse = Updated?.Invoke("SimpleConversation");
            ReceivedMessage = envelope;
            SentMessage = new Envelope(new LoginReply());
        }

        public delegate string SimpleMessageReceived(string something);
        public event SimpleMessageReceived Updated;

        public virtual void OnUpdate()
        {
            ProcessMessage(new Envelope(new LoginRequest()));
        }

        public int AttemptedRetries = 0;
        public bool HasCompleted { get; set; }

        public new BaseCommunicator Communicator
        {
            get
            {
                return base.Communicator;
            }
            set
            {
                base.Communicator = value;
                ConversationManager.RegisterCommunicatorCallback(value);
                //base.Communicator.NewMessage += new BaseCommunicator.MessageReceived(ProcessMessage);
            }
        }
        public string EventResponse { get; set; }
        public Envelope ReceivedMessage { get; set; }
        public Envelope SentMessage { get; set; }
    }
}