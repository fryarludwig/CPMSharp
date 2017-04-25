using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Runtime.Serialization;
using Common.Communication;
using Common.Users;

namespace Common.Messages.Replies
{
    [DataContract]
    public class LoginReply : Reply
    {
        public LoginReply(MessageNumber convId) : base(convId)
        {
            
        }

        [DataMember]
        public ProcessInfo ProcessInfo { get; set; }

        [DataMember]
        public IPEndPoint ProxyEndPoint { get; set; }

        //[DataMember]
        //public PublicEndPoint PennyBankEndPoint { get; set; }

        //[DataMember]
        //public PublicKey PennyBankPublicKey { get; set; }
    }
}
