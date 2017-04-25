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
                if (!Communicator.IsActive) { Communicator.Start(); }
                return true;
            }

            return false;
        }

        protected bool VerifyReceivePermissions(Envelope envelope)
        {
            if (!AllowInboundMessages)
            {
                Logger.Warn("Inbound messages are not allowed for this conversation.");
            }
            else if (envelope?.Address == null || envelope.Message == null)
            {
                Logger.Warn("The envelope contains incomplete data - discarding.");
            }
            else if (ReceivedMessages.ContainsKey(envelope.Message))
            {
                Logger.Warn("Envelope is valid, however we have already received this exact message");
            }
            else
            {
                Logger.Trace("Message receipt is permitted");
                return true;
            }
            
            return false;
        }

        // Template method
        protected override void Run()
        {
            AttemptedRetries = 0;

            if (ValidateConversation())
            {
                BeginConversation();
                Thread.Sleep(150);

                while (ContinueThread && WaitingForReply)
                {
                    if (!MessageInbox.IsEmpty && MessageInbox.TryDequeue(out Envelope inbound) && 
                        VerifyReceivePermissions(inbound))
                    {
                        Logger.Info("Received some kind of response");
                        ReceivedMessages[inbound.Message] = inbound.Address;
                        ProcessResponse(inbound);
                    }
                    else if (WaitingForReply && AttemptedRetries > MaxRetries)
                    {
                        HandleNoResponse();
                    }
                    else if (WaitingForReply)
                    {
                        RetryMessage();
                        AttemptedRetries++;
                        Thread.Sleep(Timeout);
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
                Logger.Info("Resending last sent message");
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

        protected void SendMessage(Envelope envelope)
        {
            if (envelope == null)
            {
                Logger.Error($"Not even going to try to send null message to null address");
            }
            else if (envelope.Message == null || envelope.Address == null)
            {
                Logger.Error($"Null Address: Cannot send message {envelope.Message?.ToString() ?? "'null msg'"}");
            }
            else
            {
                SentMessages.Enqueue(envelope);
                Communicator.Send(envelope);
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

        public int Timeout = 2000;
        public uint MaxRetries = 5;
        public uint AttemptedRetries = 0;

        protected Envelope LastSentEnvelope => (SentMessages.Count == 0) ? null : SentMessages.Last();
        public bool AllowInboundMessages { get; set; }
        public bool CallbacksRegistered { get; set; }
        protected bool WaitingForReply { get; set; }
        public MessageNumber Id { get; set; }
        public SharedProperties Properties { get; set; }
        protected BaseCommunicator Communicator { get; set; }
        public ConcurrentQueue<Envelope> MessageInbox { get; }
        public ConcurrentQueue<Envelope> SentMessages { get; }
        public ConcurrentDictionary<Message, IPEndPoint> ReceivedMessages { get; set; }
    }
}
