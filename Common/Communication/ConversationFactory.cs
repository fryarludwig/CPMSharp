using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Messages;
using Common.Messages.Replies;
using Common.Messages.Requests;
using Common.Users;

namespace Common.Communication
{
    public static class ConversationFactory
    {
        static ConversationFactory()
        {
            Retries = 5;
            Timeout = 1000;
            Properties = SharedProperties.Instance;
            ConversationTypes = new Dictionary<Type, Type>();
        }

        public static void RegisterNewConversationType(Type messageType, Type conversationType)
        {
            ConversationTypes[messageType] = conversationType;
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
        
        #region Public member variables
        public static UInt32 Retries { get; set; }
        public static int Timeout { get; set; }
        public static SharedProperties Properties { get; set; }
        #endregion

        private static Dictionary<Type, Type> ConversationTypes { get; set; }
    }
}
