
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
            Timeout = 500;
            EventResponse = null;
            ReceivedMessage = null;
            SentMessage = null;
            WaitingForReply = true;
        }

        public SimpleConversation(string name, MessageNumber msgNum) : base(name, new UdpCommunicator(), msgNum)
        {
            Timeout = 500;
            EventResponse = null;
            ReceivedMessage = null;
            SentMessage = null;
            WaitingForReply = true;
        }

        public SimpleConversation(string name, MessageNumber msgNum, int port) : base(name, new UdpCommunicator(port), msgNum)
        {
            Timeout = 500;
            EventResponse = null;
            ReceivedMessage = null;
            SentMessage = null;
            WaitingForReply = true;
        }

        public void SendHeartbeatRequest(IPEndPoint remoteEndpoint)
        {
            AliveRequest request = new AliveRequest();
            request.ConvId = Id;
            request.ConvId.Pid = 1;
            SentMessage = new Envelope(remoteEndpoint, request);
            SendMessage(SentMessage);
            WaitingForReply = true;
        }

        public void SendHeartbeatReply(IPEndPoint remoteEndpoint)
        {
            AliveReply reply = new AliveReply {ConvId = Id};
            reply.ConvId.Pid = 0;
            SentMessage = new Envelope(remoteEndpoint, reply);
            SendMessage(SentMessage);
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

        protected override void ProcessResponse(Envelope envelope)
        {
            Logger.Info("Processing received message, boo yah!");
            EventResponse = Updated?.Invoke("SimpleConversation");
            ReceivedMessage = envelope;
            if (ReceivedMessage.Message.GetType() == typeof(LoginRequest))
            {
                LoginReply reply = new LoginReply {ConvId = Id};
                SentMessage = new Envelope(envelope.Address, reply);
                SendMessage(SentMessage);
                WaitingForReply = false;
            }
            else if (ReceivedMessage.Message.GetType() == typeof(AliveReply))
            {
                WaitingForReply = false;
            }
        }

        public delegate string SimpleMessageReceived(string something);
        public event SimpleMessageReceived Updated;

        public virtual void OnUpdate()
        {
            ProcessResponse(new Envelope(new LoginRequest()));
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
            }
        }
        public string EventResponse { get; set; }
        public Envelope ReceivedMessage { get; set; }
        public Envelope SentMessage { get; set; }
    }
}

/*
 * 
Objectives
Gain experience with public/private keys
Gain experience with security using public/private keys
Overview

Finish Implementing system
Security Protocol (1) Auth Proc / Auth Msg / Encrypt / Combo
Update Documentation (requirement definition, architectural design, and communication protocol)
Expand unit tests
Put in the cloud
 * 
 * */
