using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

using Common.Communication;
using Common.Messages.Replies;
using Common.Messages.Requests;
using Common.Utilities;

namespace Common.Messages
{
    [DataContract]
    public class Message
    {
        static Message()
        {
            Register(typeof(LoginReply));
            //Register(typeof(PublicKeyReply));
            Register(typeof(Reply));
            Register(typeof(AliveReply));

            Register(typeof(AliveRequest));
            //Register(typeof(DeadProcessNotification));
            Register(typeof(LoginRequest));
            //Register(typeof(LogoutRequest));
            //Register(typeof(ShutdownRequest));
            //Register(typeof(Routing));
        }

        private static readonly LogUtility Logger = new LogUtility("Message");

        private static List<Type> _serializableTypes;

        public static void Register(Type T)
        {
            if (_serializableTypes == null)
                _serializableTypes = new List<Type>();

            if (!_serializableTypes.Contains(T))
                _serializableTypes.Add(T);
        }

        [DataMember]
        public MessageNumber MsgId { get; set; }
        [DataMember]
        public MessageNumber ConvId { get; set; }

        /// <summary>
        /// This method sets up the message and conversation numbers for the first message of a conversation.
        /// Specifically, it creates a new message number, then that is copied for the conversation id.
        /// </summary>
        public void InitMessageAndConversationNumbers()
        {
            SetMessageAndConversationNumbers(MessageNumber.Create());
        }

        /// <summary>
        /// This method sets up the message and conversation id's for the first message of a conversation.
        /// Specifically, it sets the messsage id to the provided id, then that is copied for the conversation id.
        /// </summary>
        /// <param name="id">Message number that will become this message's message id and conversation id </param>
        public void SetMessageAndConversationNumbers(MessageNumber id)
        {
            SetMessageAndConversationNumbers(id, id.Clone());
        }

        /// <summary>
        /// This method set the message and conversations id's
        /// </summary>
        /// <param name="id"></param>
        /// <param name="convId"></param>
        public void SetMessageAndConversationNumbers(MessageNumber id, MessageNumber convId)
        {
            MsgId = id;
            ConvId = convId;
        }

        /// <summary>
        /// This method encodes a message into a byte array by first serializaing it into a JSON string and then
        /// converting that string to byte. 
        /// </summary>
        /// <returns>A byre array containing the JSON serializations of the message</returns>
        public byte[] Encode()
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Message), _serializableTypes);
            MemoryStream mstream = new MemoryStream();
            serializer.WriteObject(mstream, this);

            return mstream.ToArray();
        }

        /// <summary>
        /// This is a simpel factory methods that tries to create a message objects from a byte arrary contains
        /// a JSON serializations of the message.
        /// </summary>
        /// <param name="bytes">A byte array containing an ASCII encoding of a correct JSON serialization of a valid message.</param>
        /// <returns>If successful, a message object, instantied as an instance of the correct specialization of Message.
        /// If unsucessful because the byte array did not contain a correctly serialized message, then null.</returns>
        public static Message Decode(byte[] bytes)
        {
            Message result = null;
            if (bytes != null)
            {
                try
                {
                    MemoryStream mstream = new MemoryStream(bytes);
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Message), _serializableTypes);
                    result = (Message)serializer.ReadObject(mstream);
                }
                catch (Exception err)
                {
                    Logger.Warn($"Except warning in decoding a message: {err.Message}");
                }
            }
            return result;
        }

    }
}
