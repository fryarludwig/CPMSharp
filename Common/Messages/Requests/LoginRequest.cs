using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using Common.Communication;
using Common.Messages;
using Common.Users;
using Common.Utilities;

namespace Common.Messages.Requests
{
    [DataContract]
    public class LoginRequest : Request
    {
        [DataMember]
        public ProcessInfo.ProcessType ProcessType { get; set; }

        [DataMember]
        public string ProcessLabel { get; set; }

        [DataMember]
        public User IdentityInfo { get; set; }

        [DataMember]
        public PublicKey PublicKey { get; set; }
    }
}
