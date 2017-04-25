using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Common.Messages;
using Common.Users;
using Common.Utilities;
using System.Net;

namespace Common.Communication
{
    public abstract class Conversation : Threaded
    {
        public Conversation(string name, MessageNumber convId = null) : base(name)
        {
            Id = convId ?? MessageNumber.Create();
            Timeout = 2000;
            MaxRetries = 5;
            Properties = SharedProperties.Instance;
            Communicator = ConversationManager.PrimaryCommunicator;
            MessageInbox = new ConcurrentQueue<Envelope>();
            SentMessages = new ConcurrentQueue<Envelope>();
            ReceivedMessages = new ConcurrentDictionary<Message, IPEndPoint>();
            CallbacksRegistered = false;
            AllowInboundMessages = true;
            WaitingForReply = true;

            RegisterWithConversationManager();
        }

        public Conversation(string name, BaseCommunicator communicator, MessageNumber convId = null) : base(name)
        {
            Id = convId ?? MessageNumber.Create();
            Timeout = 2000;
            MaxRetries = 5;
            Properties = SharedProperties.Instance;
            Communicator = communicator;
            Communicator.OnMessageReceived += new BaseCommunicator.MessageReceived(ReceiveDirectMessage);
            MessageInbox = new ConcurrentQueue<Envelope>();
            SentMessages = new ConcurrentQueue<Envelope>();
            ReceivedMessages = new ConcurrentDictionary<Message, IPEndPoint>();
            CallbacksRegistered = false;
            AllowInboundMessages = true;
            WaitingForReply = true;
        }

        public virtual void RegisterDistributedProcessCallbacks(DistributedProcess process)
        {
            CallbacksRegistered = true;
        }

        public void RegisterWithConversationManager()
        {
            ConversationManager.RegisterConversation(this);
            ConversationManager.RegisterCommunicatorCallback(Communicator);
        }

        protected virtual bool ValidateConversation()
        {
            if (Communicator?.LocalEndpoint != null)
            {
                if (!Communicator.IsActive()) { Communicator.Start(); }
                return true;
            }
            else
            {
                return false;
            }
        }

        protected bool VerifyReceivePermissions(Envelope envelope)
        {
            Logger.Trace("Verifying permissions to receive envelope");
            if (envelope?.Address != null && envelope.Message != null)
            {
                if (AllowInboundMessages &&(!ReceivedMessages.ContainsKey(envelope.Message))
                    || (ReceivedMessages.ContainsKey(envelope.Message)))
                {
                    Logger.Trace("Permission to receive is granted");
                    return true;
                }
            }

            Logger.Trace("We are not allow to receive this envelope");
            return false;
        }

        // Template method
        protected override void Run()
        {
            if (ValidateConversation())
            {
                BeginConversation();

                while (ContinueThread && WaitingForReply)
                {
                    Envelope inbound;
                    if (!MessageInbox.IsEmpty && MessageInbox.TryDequeue(out inbound) && 
                        VerifyReceivePermissions(inbound))
                    {
                            Logger.Info("Received some kind of response");
                            ReceivedMessages[inbound.Message] = inbound.Address;
                            ProcessResponse(inbound);
                    }
                    else if (WaitingForReply && AttemptedRetries < MaxRetries)
                    {
                        RetryMessage();
                        AttemptedRetries++;
                        Thread.Sleep(Timeout);
                    }
                    else if (WaitingForReply)
                    {
                        HandleNoResponse();
                    }
                    else
                    {
                        HandleConversationCompleted();
                        Stop();
                    }
                }
            }
            else
            {
                Stop();
                Logger.Error("Unable to start Coversation due to invalid settings");
                return;
            }

            Logger.Info("Ending conversation");
        }

        protected virtual void RetryMessage()
        {
            if (LastSentEnvelope != null)
            {
                Communicator.Send(LastSentEnvelope);
            }
        }

        protected virtual void BeginConversation()
        {
            // Do nothing
        }

        protected virtual void HandleConversationCompleted()
        {
            this.Stop();
        }

        protected virtual void HandleNoResponse()
        {
            this.Stop();
        }

        protected abstract void ProcessResponse(Envelope envelope);

        protected virtual void ProcessNullMessage(Envelope envelope)
        {
            RetryMessage();
        }

        protected Envelope LastSentEnvelope => (SentMessages.Count == 0) ? null : SentMessages.Last();

        protected void SendMessage(Envelope envelope)
        {
            if (envelope?.Address != null && envelope.Message != null)
            {
                Communicator.Send(envelope);
                SentMessages.Enqueue(envelope);
            }
            else if (envelope?.Message != null)
            {
                Logger.Error($"Null Address: Cannot send message {envelope.Message.ToString()}");
            }
            else
            {
                Logger.Error($"Not even going to try to send null message to null address");
            }
        }

        protected void ReceiveDirectMessage(Envelope envelope)
        {
            Logger.Info("Received message from my personal communicator");
            MessageInbox.Enqueue(envelope);

            if (envelope?.Message != null && envelope.Address != null)
            {
                Logger.Info($"Message from {envelope.Address.ToString()} to {envelope.ConvId}");
            }
            else
            {
                Logger.Warn("Invalid message!");
            }
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public bool AllowInboundMessages { get; set; }
        public bool CallbacksRegistered { get; set; }
        protected bool WaitingForReply { get; set; }
        public MessageNumber Id { get; set; }
        public int Timeout { get; set; }
        public uint MaxRetries { get; set; }
        public uint AttemptedRetries { get; set; }
        public SharedProperties Properties { get; set; }
        protected BaseCommunicator Communicator { get; set; }
        public Envelope InitialMessage { get; set; }
        public ConcurrentQueue<Envelope> MessageInbox { get; }
        public ConcurrentQueue<Envelope> SentMessages { get; }
        public ConcurrentDictionary<Message, IPEndPoint> ReceivedMessages { get; set; }
    }
}
