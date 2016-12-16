using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Messages;
using System.Net;
using System.Runtime.Serialization;

namespace Common.Messages.Requests
{
    [DataContract]
    public class ShutdownRequest : Request
    {
        public ShutdownRequest() { }
        public ShutdownRequest(int id)
        {
            ProcessId = id;
        }

        [DataMember]
        public int ProcessId { get; set; }
    }
}
