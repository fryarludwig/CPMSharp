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
    public static class ConversationManager
    {
        static ConversationManager()
        {
            Retries = 5;
            Timeout = 1000;
            Properties = SharedProperties.Instance;
            ConversationTypes = new Dictionary<Type, Type>();
            ConversationDictionary = new ConcurrentDictionary<MessageNumber, Conversation>();
            PrimaryCommunicator = new UdpCommunicator();
            RegisterCommunicatorCallback(PrimaryCommunicator);
        }

        public static void RegisterCommunicatorCallback(BaseCommunicator communicator)
        {
            communicator.NewMessageReceived += new BaseCommunicator.MessageReceived(ReceiveMessage);
        }

        public static void RemoveCommunicatorCallback(BaseCommunicator communicator)
        {
            communicator.NewMessageReceived -= new BaseCommunicator.MessageReceived(ReceiveMessage);
        }

        public static void ClearConversations()
        {
            foreach (Conversation conversation in ConversationDictionary.Values)
            {
                if (conversation.IsActive())
                {
                    conversation.Stop();
                }
            }

            PrimaryCommunicator.Stop();
            ConversationDictionary.Clear();
        }

        public static bool InitiateConversation(Envelope envelope)
        {
            bool success = false;

            Conversation newConversation = ConversationManager.CreateNewConversation(envelope);

            if (newConversation != null)
            {
                ConversationDictionary[newConversation.Id] = newConversation;
                newConversation.Start();
                success = true;
            }

            return success;
        }

        public static void RegisterNewConversationTypes(Dictionary<Type, Type> msgConvRegistry)
        {
            foreach (Type messageType in msgConvRegistry.Keys)
            {
                ConversationTypes[messageType] = msgConvRegistry[messageType];
            }
        }

        public static void RegisterNewConversationType(Type messageType, Type conversationType)
        {
            ConversationTypes[messageType] = conversationType;
        }

        public static void RegisterExistingConversation(Conversation conversation)
        {
            if (!ConversationDictionary.ContainsKey(conversation.Id))
            {
                ConversationDictionary[conversation.Id] = conversation;
            }
        }

        public static Conversation CreateNewConversation(Envelope envelope)
        {
            Conversation newConversation = null;

            if (envelope != null && envelope.Message != null)
            {
                // May be true if we originate the message
                if (envelope.Message.ConvId == null && envelope.Message.MsgId == null)
                {
                    envelope.Message.InitMessageAndConversationNumbers();
                }

                if (ConversationTypes.ContainsKey(envelope.Message.GetType()))
                {
                    newConversation = Activator.CreateInstance(ConversationTypes[envelope.Message.GetType()]) as Conversation;
                    newConversation.Id = envelope.Message.ConvId;
                }
            }

            return newConversation;
        }

        public static void ReceiveMessage(Envelope envelope)
        {
            if (PrimaryCommunicator.Receive(out envelope) && envelope != null)
            {
                if (ConversationDictionary.ContainsKey(envelope.ConvId))
                {
                    ConversationDictionary[envelope.ConvId].NewMessages.Enqueue(envelope);
                }
                else if (InitiateConversation(envelope))
                {
                    ConversationDictionary[envelope.ConvId].NewMessages.Enqueue(envelope);
                }
            }
        }

        private static BaseCommunicator _PrimaryCommunicator { get; set; }
        public static BaseCommunicator PrimaryCommunicator
        {
            get
            {
                if (_PrimaryCommunicator == null)
                {
                    _PrimaryCommunicator = new UdpCommunicator();
                }

                return _PrimaryCommunicator;
            }
            set
            {
                _PrimaryCommunicator = value ?? new UdpCommunicator();
                RegisterCommunicatorCallback(_PrimaryCommunicator);
            }
        }

        public static UInt32 Retries { get; set; }
        public static int Timeout { get; set; }
        public static SharedProperties Properties { get; set; }

        private static Dictionary<Type, Type> ConversationTypes { get; set; }
        public static ConcurrentDictionary<MessageNumber, Conversation> ConversationDictionary;
    }
}
