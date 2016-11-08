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

namespace Common.Communication
{
    public class ConversationManager : Threaded
    {
        public ConversationManager(Dictionary<Type, Type> msgConvRegistry, SharedProperties properties) : base("ConversationManager")
        {
            Logger.Info("Creating conversation manager");
            ConversationDictionary = new Dictionary<MessageNumber, Conversation>();
            if (properties.LocalEndpoint != null)
            {
                Communicator = CommunicationService.GetInstance(properties.LocalEndpoint.Port);
            }
            else
            {
                Communicator = CommunicationService.GetInstance();
            }
            
            foreach (Type messageType in msgConvRegistry.Keys)
            {
                ConversationFactory.RegisterNewConversationType(messageType, msgConvRegistry[messageType]);
            }
        }

        public ConversationManager(Dictionary<Type, Type> dictionary) : base ("ConversationManager")
        {
            this.dictionary = dictionary;
        }

        protected override void DerivedStop()
        {
            lock (ConvQueueLock)
            {
                foreach (Conversation conversation in ConversationDictionary.Values)
                {
                    conversation.Stop();
                }
            }

            Communicator.Stop();
        }

        public void Execute(Conversation conv)
        {
            lock (ConvQueueLock)
            {
                conv.Start();
                ConversationDictionary[conv.Id] = conv;
            }
        }

        protected override void Run()
        {
            Envelope envelope;

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
                    Thread.Sleep(1500);
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
        
        private Dictionary<MessageNumber, Conversation> ConversationDictionary;
        private CommunicationService Communicator;
        private object ConvQueueLock = new object();
        private Dictionary<Type, Type> dictionary;
    }
}
