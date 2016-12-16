using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Common.Messages.Requests
{
    [DataContract]
    public class Request : Message
    {
        public Request()
        {
            MsgId = MessageNumber.Create();
            ConvId = MessageNumber.Create();
        }
    }
}
