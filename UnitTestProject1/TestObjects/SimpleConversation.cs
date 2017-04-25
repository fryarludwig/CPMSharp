
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
    public class SimpleRequestReplyInitiator : RequestReplyInitiator
    {

        public SimpleRequestReplyInitiator() : base("SimpleInitiator")
        {
            EventResponse = null;
        }

        public SimpleRequestReplyInitiator(IPEndPoint target) : base("SimpleInitiator")
        {
            Destination = target;
        }

        public SimpleRequestReplyInitiator(BaseCommunicator communicator, IPEndPoint target) : base("SimpleInitiator", communicator)
        {
            Destination = target;
        }

        protected override void BeginConversation()
        {
            AliveRequest request = new AliveRequest();
            request.InitMessageAndConversationNumbers();
            Envelope toSend = new Envelope(Destination, request);
            WaitingForReply = true;
            SendMessage(toSend);
        }

        protected override void ProcessResponse(Envelope envelope)
        {
            EventResponse = Updated?.Invoke("SimpleConversationInitiator");
            if (envelope?.Message?.GetType() == typeof(AliveReply))
            {
                WaitingForReply = false;
                ReceivedMessage = envelope;
            }
        }

        protected override void HandleConversationCompleted()
        {
            Logger.Info("Simple initiator completed conversation");
            //Heartbeat_OnUpdate?.Invoke(Id, ResponseReceived);
            base.HandleConversationCompleted();
        }

        protected override void RetryMessage()
        {
            AttemptedRetries++;
            base.RetryMessage();
        }

        public delegate void HeartbeatUpdated(MessageNumber ConvId, bool alive);
        public event HeartbeatUpdated Heartbeat_OnUpdate;
        public bool HasCompleted { get; set; }

        public string EventResponse { get; set; }
        public Envelope SentMessage
        {
            get { return LastSentEnvelope; }
        }
        public Envelope ReceivedMessage { get; set; }

        public delegate string SimpleMessageReceived(string something);
        public event SimpleMessageReceived Updated;
    }

    public class SimpleRequestReplyResponder : RequestReplyResponder
    {
        public SimpleRequestReplyResponder() : base("SimpleResponder")
        {
        }

        public SimpleRequestReplyResponder(BaseCommunicator communicator) : base("SimpleResponder", communicator)
        {
        }

        protected override void ProcessResponse(Envelope envelope)
        {
            EventResponse = Updated?.Invoke("SimpleConversationResponder");
            if (envelope?.Message?.GetType() == typeof(AliveRequest))
            {
                Id = envelope.ConvId;
                ReceivedMessage = envelope;
                AliveReply reply = new AliveReply { };
                SendMessage(new Envelope(envelope.Address, reply));
                WaitingForReply = false;
            }
        }

        protected override void HandleConversationCompleted()
        {
            Conversation_Responded?.Invoke(Id, ResponseReceived);
        }

        public delegate void OnSendConversationResponse(MessageNumber ConvId, bool alive);
        public event OnSendConversationResponse Conversation_Responded;
        private bool ResponseReceived { get; set; }
        
        public bool HasCompleted { get; set; }

        public string EventResponse { get; set; }
        public Envelope SentMessage
        {
            get { return LastSentEnvelope; }
        }
        public Envelope ReceivedMessage { get; set; }

        public delegate string SimpleMessageReceived(string something);
        public event SimpleMessageReceived Updated;
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
