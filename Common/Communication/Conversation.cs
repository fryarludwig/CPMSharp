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
            Id = msgNum ?? MessageNumber.Create();
            Timeout = 250;
            MaxRetries = 5;
            Properties = SharedProperties.Instance;
            Communicator = ConversationManager.PrimaryCommunicator;
            NewMessages = new ConcurrentQueue<Envelope>();
            SentMessages = new ConcurrentQueue<Envelope>();
            Register();
        }

        public Conversation(string name, BaseCommunicator communicator, MessageNumber msgNum = null) : base(name)
        {
            Id = msgNum ?? MessageNumber.Create();
            Timeout = 250;
            MaxRetries = 5;
            Properties = SharedProperties.Instance;
            Communicator = communicator;
            NewMessages = new ConcurrentQueue<Envelope>();
            SentMessages = new ConcurrentQueue<Envelope>();
            Register();
        }

        public virtual void RegisterConversationCallbacks(DistributedProcess process)
        {
            // Do nothing
        }

        public void Register()
        {
            ConversationManager.RegisterExistingConversation(this);
            ConversationManager.RegisterCommunicatorCallback(Communicator);
        }

        protected virtual bool ValidateConversation()
        {
            if (Communicator != null && Communicator.LocalEndpoint != null)
            {
                if (!Communicator.IsActive()) { Communicator.Start(); }
                return true;
            }
            else
            {
                return false;
            }
        }

        // Template method
        protected override void Run()
        {
            if (!ValidateConversation())
            {
                UInt32 availableRetries = MaxRetries;
                Envelope tempEnvelope;
                BeginConversation();

                while (ContinueThread)
                {
                    if (!NewMessages.IsEmpty && NewMessages.TryDequeue(out tempEnvelope))
                    {
                        Logger.Info("Received some kind of response");
                        ProcessResponse(tempEnvelope);
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

        protected abstract void ProcessResponse(Envelope envelope);

        protected virtual void ProcessNullMessage(Envelope envelope)
        {
            RetryMessage();
        }

        protected Envelope LastMessage
        {
            get
            {
                return (SentMessages.Count == 0)? null : SentMessages.Last();
            }
        }

        protected void SendMessage(Envelope envelope)
        {
            if (envelope.Address != null && envelope.Message != null)
            {
                Communicator.Send(envelope);
                SentMessages.Enqueue(envelope);
            }
            else if (envelope.Message != null)
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

        protected bool WaitingForReply { get; set; }
        public MessageNumber Id { get; set; }
        public int Timeout { get; set; }
        public UInt32 MaxRetries { get; set; }
        public SharedProperties Properties { get; set; }
        protected BaseCommunicator Communicator { get; set; }
        public Envelope InitialMessage { get; set; }
        public ConcurrentQueue<Envelope> NewMessages { get; }
        public ConcurrentQueue<Envelope> SentMessages { get; }
    }
}
