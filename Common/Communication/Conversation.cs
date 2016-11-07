using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Common.Messages;
using Common.Messages.Replies;
using Common.Messages.Requests;
using Common.Users;
using Common.Utilities;

namespace Common.Communication
{
    abstract public class Conversation : Threaded
    {
        public Conversation(string name) : base(name)
        {
            Id = MessageNumber.Create();
            Timeout = 1000;
            MaxRetries = 0;
            Properties = SharedProperties.Instance;
            NewMessages = new ConcurrentQueue<Envelope>();
            Communicator = CommunicationManager.Instance;
        }
        
        protected Envelope PopulateEnvelope()
        {
            return new Envelope(Properties.RegistryEndpoint, CreateMessage());
        }

        protected abstract Message CreateMessage();

        public MessageNumber Id { get; set; }
        public int Timeout { get; set; }
        public UInt32 MaxRetries { get; set; }

        public Identity IdentityInfo
        {
            get
            {
                return Properties.IdentityInfo;
            }
        }
        public ProcessInfo Process
        {
            get
            {
                return Properties.Process;
            }
        }
        public SharedProperties Properties { get; set; }
        public CommunicationManager Communicator { get; }
        public ConcurrentQueue<Envelope> NewMessages { get; }
    }
}
