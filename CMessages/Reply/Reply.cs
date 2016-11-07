using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace CMessages.Reply
{
    [DataContract]
    public abstract class Reply : CMessage
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
