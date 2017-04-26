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
            _LocalEndpoint = new IPEndPoint(IPAddress.Any, 0);
            InboundQueue = new ConcurrentQueue<Envelope>();
            OutboundQueue = new ConcurrentQueue<Envelope>();
            State = STATE.STOPPED;
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
            State = STATE.READY;
        }

        protected override void Cleanup()
        {
            State = STATE.STOPPED;
        }

        public enum STATE
        {
            ERROR,
            STOPPED,
            READY,
            BUSY
        }
        
        protected ConcurrentQueue<Envelope> InboundQueue { get; }
        protected ConcurrentQueue<Envelope> OutboundQueue { get; }
        private STATE _State { get; set; }
        public STATE State
        {
            get
            {
                if (LOCK_STATE != null)
                    lock (LOCK_STATE) { return _State; }
                else
                    return _State;
            }
            set
            {
                if (LOCK_STATE != null)
                    lock (LOCK_STATE) { _State = value; }
                else
                    _State = value;
                State_OnChanged?.Invoke(State);
            }
        }

        private object LOCK_STATE = new object();

        public delegate void StateChanged(STATE state);
        public event StateChanged State_OnChanged;
        public delegate void MessageReceived(Envelope envelope);
        public event MessageReceived OnMessageReceived;
        public delegate void SendMessageCallback(Envelope envelope);
        public event SendMessageCallback OnSendMessage;
    }
}
