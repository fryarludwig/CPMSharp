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
        public Conversation(string name) : base(name)
        {
            Id = MessageNumber.Create();
            Timeout = 1000;
            MaxRetries = 5;
            Properties = SharedProperties.Instance;
            NewMessages = new ConcurrentQueue<Envelope>();
            SentMessages = new ConcurrentQueue<Envelope>();
        }

        // Template method
        protected override void Run()
        {
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

        protected bool WaitingForReply { get; set; }

        public MessageNumber Id { get; set; }
        public int Timeout { get; set; }
        public UInt32 MaxRetries { get; set; }

        public User IdentityInfo
        {
            get
            {
                return Properties.IdentityInfo;
            }
        }
        public ProcessInfo Process
        {
            get
            {
                return Properties.Process;
            }
        }
        public SharedProperties Properties { get; set; }
        private UdpCommunicator Communicator { get; }
        public ConcurrentQueue<Envelope> NewMessages { get; }
        public ConcurrentQueue<Envelope> SentMessages { get; }
    }
}
