using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Common.Messages;
using Common.Messages.Replies;
using Common.Messages.Requests;
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
        }

        protected Envelope PopulateEnvelope()
        {
            //return new Envelope(Properties.AuthenticatorEndpoint, CreateMessage());
        }

        // Template method
        // Potentially request/reply initiator
        // Potenntially request./reply responder
        protected override void Run()
        {
            UInt32 availableRetries = MaxRetries;
            Envelope tempEnvelope;

            while (ContinueThread)
            {
                if (!NewMessages.IsEmpty)
                {
                    Logger.Info("Received some kind of response");
                    if (NewMessages.TryDequeue(out tempEnvelope))
                    {
                        if (tempEnvelope.Message != null)
                        {
                            ProcessMessage(tempEnvelope.Message.GetType(), tempEnvelope);
                        }
                        else
                        {
                            ProcessNullMessage(tempEnvelope);
                        }
                    }
                }
                else if (WaitingForReply && availableRetries-- > 0)
                {
                    RetryMessage();
                    Envelope toSend = new Envelope(Properties.AuthenticatorEndpoint, CreateMessage());
                    Communicator.Send(PopulateEnvelope());
                    Thread.Sleep(Timeout);
                }
                else if (WaitingForReply && availableRetries <= 0)
                {
                    HandleNoResponse();
                }
                else
                {
                    HandleConversationCompleted();
                }
            }

            Logger.Info("Ending conversation");
        }

        protected abstract void RetryMessage();
        protected abstract void HandleConversationCompleted();
        protected abstract void HandleNoResponse();
        protected abstract void ProcessMessage(Type messageType, Envelope envelope);
        protected abstract void ProcessNullMessage(Envelope envelope);
        protected abstract Message CreateMessage();
        protected bool WaitingForReply { get; set; }

        public MessageNumber Id { get; set; }
        public int Timeout { get; set; }
        public UInt32 MaxRetries { get; set; }

        public Identity IdentityInfo
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
        public UdpCommunicator Communicator { get; }
        public ConcurrentQueue<Envelope> NewMessages { get; }
    }
}
