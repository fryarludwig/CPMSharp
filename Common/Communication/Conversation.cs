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

namespace Common.Communication
{
    abstract public class Conversation : Threaded
    {
        public Conversation(string name, MessageNumber msgNum = null) : base(name)
        {
            Id = msgNum ?? new MessageNumber();
            Timeout = 1000;
            MaxRetries = 5;
            Properties = SharedProperties.Instance;
            Communicator = ConversationManager.PrimaryCommunicator;
            NewMessages = new ConcurrentQueue<Envelope>();
            SentMessages = new ConcurrentQueue<Envelope>();
        }

        public Conversation(string name, BaseCommunicator communicator, MessageNumber msgNum = null) : base(name)
        {
            Id = msgNum ?? new MessageNumber();
            Timeout = 1000;
            MaxRetries = 5;
            Properties = SharedProperties.Instance;
            Communicator = communicator;
            NewMessages = new ConcurrentQueue<Envelope>();
            SentMessages = new ConcurrentQueue<Envelope>();
        }

        public void Register()
        {
            ConversationManager.RegisterExistingConversation(this);
        }

        protected virtual bool ValidateConversation()
        {
            return Communicator != null && Communicator.LocalEndpoint != null;
        }

        // Template method
        protected override void Run()
        {
            if (!ValidateConversation())
            {
                Stop();
                Logger.Error("Unable to start Coversation due to invalid settings");
                return;
            }
            else if (!Communicator.IsActive())
            {
                Communicator.Start();
            }

            UInt32 availableRetries = MaxRetries;
            Envelope tempEnvelope;

            BeginConversation();

            while (ContinueThread)
            {
                if (!NewMessages.IsEmpty && NewMessages.TryDequeue(out tempEnvelope))
                {
                    Logger.Info("Received some kind of response");
                    ProcessMessage(tempEnvelope);
                }
                else if (WaitingForReply && availableRetries-- > 0)
                {
                    RetryMessage();
                    Thread.Sleep(Timeout);
                }
                else if (WaitingForReply && availableRetries <= 0)
                {
                    HandleNoResponse();
                }
                else if (!WaitingForReply)
                {
                    HandleConversationCompleted();
                }
            }

            Logger.Info("Ending conversation");
        }

        protected virtual void RetryMessage()
        {
            if (LastMessage != null)
            {
                Communicator.Send(LastMessage);
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

        protected abstract void ProcessMessage(Envelope envelope);

        protected virtual void ProcessNullMessage(Envelope envelope)
        {
            RetryMessage();
        }

        protected Envelope LastMessage
        {
            get
            {
                if (SentMessages.Count == 0)
                {
                    return null;
                }
                else
                {
                    return SentMessages.Last();
                }
            }
        }

        protected void SendMessage(Envelope envelope)
        {
            Communicator.Send(envelope);
            SentMessages.Enqueue(envelope);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        protected bool WaitingForReply { get; set; }
        public MessageNumber Id { get; set; }
        public int Timeout { get; set; }
        public UInt32 MaxRetries { get; set; }
        public SharedProperties Properties { get; set; }
        protected BaseCommunicator Communicator { get; set; }
        public ConcurrentQueue<Envelope> NewMessages { get; }
        public ConcurrentQueue<Envelope> SentMessages { get; }
    }
}
