using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Messages.Replies
{
    [DataContract]
    public class Reply : Message
    {
        public Reply(MessageNumber conversationId = null)
        {
            ConvId = conversationId ?? MessageNumber.Create();
            MsgId = MessageNumber.Create();
        }

        //public Reply(MessageNumber convId, MessageNumber msgId)
        //{
        //    ConvId = convId;
        //}

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Note { get; set; }
    }
}
