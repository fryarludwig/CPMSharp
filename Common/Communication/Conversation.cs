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
            Timeout = 1000;
            MaxRetries = 5;
            Properties = SharedProperties.Instance;
            Communicator = ConversationManager.PrimaryCommunicator;
            NewMessages = new ConcurrentQueue<Envelope>();
            SentMessages = new ConcurrentQueue<Envelope>();
            ReceivedMessages = new ConcurrentDictionary<Message, IPEndPoint>();
            CallbacksRegistered = false;
            AllowRepeats = false;
            Register();
        }

        public Conversation(string name, BaseCommunicator communicator, MessageNumber convId = null) : base(name)
        {
            Id = convId ?? MessageNumber.Create();
            Timeout = 1000;
            MaxRetries = 5;
            Properties = SharedProperties.Instance;
            Communicator = communicator;
            NewMessages = new ConcurrentQueue<Envelope>();
            SentMessages = new ConcurrentQueue<Envelope>();
            ReceivedMessages = new ConcurrentDictionary<Message, IPEndPoint>();
            CallbacksRegistered = false;
            Register();
        }

        public virtual void RegisterConversationCallbacks(DistributedProcess process)
        {
            CallbacksRegistered = true;
        }

        public void Register()
        {
            ConversationManager.RegisterExistingConversation(this);
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

        protected bool AllowReceive(Envelope envelope)
        {
            if (envelope?.Address != null && envelope.Message != null)
            {
                if ((!ReceivedMessages.ContainsKey(envelope.Message))
                    || (ReceivedMessages.ContainsKey(envelope.Message) && AllowRepeats))
                {
                    return true;
                }
            }

            return false;
        }

        // Template method
        protected override void Run()
        {
            if (ValidateConversation())
            {
                uint availableRetries = MaxRetries;
                BeginConversation();

                while (ContinueThread && WaitingForReply)
                {
                    Envelope inbound;
                    if (!NewMessages.IsEmpty && NewMessages.TryDequeue(out inbound) && 
                        AllowReceive(inbound))
                    {
                            Logger.Info("Received some kind of response");
                            ReceivedMessages[inbound.Message] = inbound.Address;
                            ProcessResponse(inbound);
                    }
                    else if (availableRetries-- > 0)
                    {
                        RetryMessage();
                        Thread.Sleep(Timeout);
                    }
                    else if (WaitingForReply && availableRetries <= 0)
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

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public bool AllowRepeats { get; set; }
        public bool CallbacksRegistered { get; set; }
        protected bool WaitingForReply { get; set; }
        public MessageNumber Id { get; set; }
        public int Timeout { get; set; }
        public uint MaxRetries { get; set; }
        public SharedProperties Properties { get; set; }
        protected BaseCommunicator Communicator { get; set; }
        public Envelope InitialMessage { get; set; }
        public ConcurrentQueue<Envelope> NewMessages { get; }
        public ConcurrentQueue<Envelope> SentMessages { get; }
        public ConcurrentDictionary<Message, IPEndPoint> ReceivedMessages { get; set; }
    }
}
