
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
        }

        public SimpleConversation(string name, MessageNumber msgNum = null) : base(name, new UdpCommunicator(), msgNum)
        {
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
            AliveReply reply = new AliveReply();
            reply.ConvId = Id;
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

        protected override void ProcessMessage(Envelope envelope)
        {
            Logger.Info("Processing received message, boo yah!");

            EventResponse = Updated?.Invoke("SimpleConversation");
            ReceivedMessage = envelope;
            if (ReceivedMessage.Message.GetType() == typeof(LoginRequest))
            {
                LoginReply reply = new LoginReply();
                reply.ConvId = Id;
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
            }
        }
        public string EventResponse { get; set; }
        public Envelope ReceivedMessage { get; set; }
        public Envelope SentMessage { get; set; }
    }
}

/*
 * Estimated time:   16-24 hours

Objectives
Gain experience with public/private keys
Gain experience with security using public/private keys
Overview
During this assignment, you finish implementing and testing your system, and implement some security for at least one communication protocol.  The security must use public/private keys to either a) authenticate a remote process, b) encrypt messages, c) authenticate message or shared resources, or d) some combination of these things.

You will need to ensure that your requirement definition, architectural design, and communication protocol specifications accurate describe your system.

You will also extend your unit test cases to verify the correctness of the new (and previous) functionality.

As with previous assignments, your execution environment will be a virtual machine in a cloud environment.
 * 
 * */
