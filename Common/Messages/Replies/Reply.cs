using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Messages.Replies
{
    [DataContract]
    public class Reply : Message
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Note { get; set; }
    }
}
