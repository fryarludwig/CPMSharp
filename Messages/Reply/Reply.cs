using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Messages.Reply
{
    [DataContract]
    public abstract class Reply : Message
    {
        public Reply()
        {

        }

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Note { get; set; }
    }
}
