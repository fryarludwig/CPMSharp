using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Common.Utilities;
using Common.Users;
using Common.Messages;
using Common.Messages.Replies;
using Common.Messages.Requests;
using System.Reflection;
using System.Net;

namespace Common.Communication
{
    public class ConversationManager : Threaded
    {
        public ConversationManager(Dictionary<Type, Type> msgConvRegistry) : base("ConversationManager")
        {
            Logger.Info("Creating conversation manager");
            ConversationDictionary = new ConcurrentDictionary<MessageNumber, Conversation>();
            Communicator = ConversationFactory.PrimaryCommunicator;

            foreach (Type messageType in msgConvRegistry.Keys)
            {
                ConversationFactory.RegisterNewConversationType(messageType, msgConvRegistry[messageType]);
            }
        }

        protected override void DerivedStop()
        {
            foreach (Conversation conversation in ConversationDictionary.Values)
            {
                if (conversation.IsActive())
                {
                    conversation.Stop();
                }
            }

            Communicator.Stop();
            ConversationDictionary.Clear();
        }

        public void Execute(Conversation conv)
        {
            conv.Start();
            ConversationDictionary[conv.Id] = conv;
        }

        // TODO - Testing
        // AWS as well
        // Test derived classes independently
        protected override void Run()
        {
            Envelope envelope = null;

            while (ContinueThread)
            {
                if (Communicator.ReplyWaiting)
                {
                    Logger.Info("Message waiting - attempting to retrieve");
                    if (Communicator.Receive(out envelope) && envelope != null)
                    {
                        if (ConversationDictionary.ContainsKey(envelope.ConvId))
                        {
                            ConversationDictionary[envelope.ConvId].NewMessages.Enqueue(envelope);
                        }
                        else if (InitiateConversation(envelope))
                        {
                            ConversationDictionary[envelope.ConvId].NewMessages.Enqueue(envelope);
                        }
                        else
                        {
                            Logger.Error("Received a message of unknown type. Cannot act on conversation " + envelope.ConvId.ToString());
                        }
                    }
                }
                else
                {
                    Thread.Sleep(250);
                }
            }

            Logger.Info("Conversation manager has closed");
        }

        public bool InitiateConversation(Envelope envelope)
        {
            bool success = false;

            Conversation newConversation = ConversationFactory.CreateNewConversation(envelope);

            if (newConversation != null)
            {
                Execute(newConversation);
                success = true;
            }

            return success;
        }

        private ConcurrentDictionary<MessageNumber, Conversation> ConversationDictionary;
        private BaseCommunicator Communicator { get; set; }
        public event EventHandler ConversationCompleted = delegate { };
    }
}
