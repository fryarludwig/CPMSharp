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
            Logger = new LogUtility("ConversationManager");
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
            communicator.OnMessageReceived += new BaseCommunicator.MessageReceived(ReceiveMessage);
        }

        public static void RemoveCommunicatorCallback(BaseCommunicator communicator)
        {
            communicator.OnMessageReceived -= new BaseCommunicator.MessageReceived(ReceiveMessage);
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

            ConversationDictionary.Clear();
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

        public static void RegisterConversation(Conversation conversation)
        {
            conversation?.RegisterDistributedProcessCallbacks(Properties?.DistInstance);

            if (!ConversationDictionary.ContainsKey(conversation.Id))
            {
                ConversationDictionary[conversation.Id] = conversation;
            }
            else
            {
                Logger.Warn("Conversation already exists, cannot register it");
            }
        }

        public static Conversation CreateNewConversation(Envelope envelope, bool registerConversation = true)
        {
            Conversation newConversation = null;

            if (envelope?.Message != null && ConversationTypes.ContainsKey(envelope.Message.GetType()))
            {
                newConversation = Activator.CreateInstance(ConversationTypes[envelope.Message.GetType()]) as Conversation;
                if (newConversation != null)
                {
                    newConversation.Id = envelope.ConvId;
                    newConversation.MessageInbox.Enqueue(envelope);
                }
            }

            return newConversation;
        }

        public static void ReceiveMessage(Envelope envelope)
        {
            Logger.Info("Received message from communicator");

            if (envelope?.Message != null && envelope.Address != null)
            {
                Logger.Info($"Message from {envelope.Address.ToString()} to {envelope.ConvId}");

                if (ConversationDictionary.ContainsKey(envelope.ConvId))
                {
                    ConversationDictionary[envelope.ConvId].MessageInbox.Enqueue(envelope);
                }
                else
                {
                    Conversation newConversation = CreateNewConversation(envelope);
                    RegisterConversation(newConversation);

                    if (newConversation != null)
                    {
                        ConversationDictionary[envelope.ConvId].MessageInbox.Enqueue(envelope);
                        Logger.Info($"Started a new conversation as {envelope.ConvId}");
                        newConversation.Start();
                    }
                    else
                    {
                        Logger.Warn($"Unable to process message from {envelope.Address.ToString()}");
                    }
                }
            }
            else
            {
                Logger.Warn("Invalid message!");
            }
        }

        private static BaseCommunicator _PrimaryCommunicator { get; set; }
        public static BaseCommunicator PrimaryCommunicator
        {
            get { return _PrimaryCommunicator ?? (_PrimaryCommunicator = new UdpCommunicator()); }
            set
            {
                _PrimaryCommunicator = value ?? new UdpCommunicator();
                RegisterCommunicatorCallback(_PrimaryCommunicator);
            }
        }

        public static uint Retries { get; set; }
        public static int Timeout { get; set; }
        public static int CommunicatorPort { get { return PrimaryCommunicator.LocalEndpoint.Port; } }
        public static SharedProperties Properties { get; set; }

        private static Dictionary<Type, Type> ConversationTypes { get; set; }
        public static ConcurrentDictionary<MessageNumber, Conversation> ConversationDictionary;

        private static LogUtility Logger { get; set; }
    }
}
