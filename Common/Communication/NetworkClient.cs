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
    public abstract class NetworkClient : Threaded
    {
        public NetworkClient(string name) : base(name)
        {
            State = STATE.Stopped;
            _LocalEndpoint = new IPEndPoint(IPAddress.Any, 0);
            InboundQueue = new ConcurrentQueue<Envelope>();
            OutboundQueue = new ConcurrentQueue<Envelope>();
        }

        protected void HandleReceivedMessage(Envelope envelope)
        {
            InboundQueue.Enqueue(envelope);
            OnMessageReceived?.Invoke(envelope);
        }

        public void Send(Envelope envelope)
        {
            OutboundQueue.Enqueue(envelope);
            OnSendMessage?.Invoke(envelope);
        }

        public bool Receive(out Envelope envelope)
        {
            return InboundQueue.TryDequeue(out envelope);
        }

        public bool ReplyWaiting => !InboundQueue.IsEmpty;
        protected IPEndPoint _LocalEndpoint { get; set; }
        public IPEndPoint LocalEndpoint
        {
            get => _LocalEndpoint;
            set
            {
                _LocalEndpoint = value ?? new IPEndPoint(IPAddress.Any, 0);
                if (IsActive)
                {
                    Stop();
                    Thread.Sleep(500);
                    Start();
                }
            }
        }

        protected override void Setup()
        {
            //State = STATE.Ready;
        }

        protected override void Cleanup()
        {
            State = STATE.Stopped;
        }

        public enum STATE
        {
            Error,
            Stopped,
            Ready,
            Busy,
            Listening
        }
        
        protected ConcurrentQueue<Envelope> InboundQueue { get; }
        protected ConcurrentQueue<Envelope> OutboundQueue { get; }
        private STATE _State { get; set; }
        public STATE State
        {
            get
            {
                if (_lockState != null)
                    lock (_lockState) { return _State; }
                else
                    return _State;
            }
            set
            {
                if (_lockState != null)
                    lock (_lockState) { _State = value; }
                else
                    _State = value;
                State_OnChanged?.Invoke(State);
            }
        }

        private object _lockState = new object();

        public delegate void StateChanged(STATE state);
        public event StateChanged State_OnChanged;
        public delegate void MessageReceived(Envelope envelope);
        public event MessageReceived OnMessageReceived;
        public delegate void SendMessageCallback(Envelope envelope);
        public event SendMessageCallback OnSendMessage;
    }
}
