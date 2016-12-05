using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Common.Messages;
using Common.Utilities;

namespace Common.Communication
{
    public abstract class BaseCommunicator : Threaded
    {
        public BaseCommunicator(string name) : base(name)
        {
            _LocalEndpoint = new IPEndPoint(IPAddress.Any, 0);
            InboundQueue = new ConcurrentQueue<Envelope>();
            OutboundQueue = new ConcurrentQueue<Envelope>();
        }
        
        protected void HandleReceivedMessage(Envelope envelope)
        {
            InboundQueue.Enqueue(envelope);
            NewMessageToSend?.Invoke(envelope);
        }

        public void Send(Envelope envelope)
        {
            OutboundQueue.Enqueue(envelope);
        }

        public bool Receive(out Envelope envelope)
        {
            return InboundQueue.TryDequeue(out envelope);
        }

        public bool ReplyWaiting
        {
            get
            {
                return !InboundQueue.IsEmpty;
            }
        }

        protected IPEndPoint _LocalEndpoint { get; set; }
        public IPEndPoint LocalEndpoint
        {
            get { return _LocalEndpoint; }
            set
            {
                _LocalEndpoint = value ?? new IPEndPoint(IPAddress.Any, 0);

                if (IsActive())
                {
                    Stop();
                    Start();
                }
            }
        }
        protected ConcurrentQueue<Envelope> InboundQueue { get; }
        protected ConcurrentQueue<Envelope> OutboundQueue { get; }


        public delegate void MessageReceived(Envelope envelope);
        public event MessageReceived NewMessageReceived;
        public delegate void SendMessageCallback(Envelope envelope);
        public event SendMessageCallback NewMessageToSend;
    }
}
