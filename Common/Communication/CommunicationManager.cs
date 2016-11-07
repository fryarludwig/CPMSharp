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
    public class CommunicationManager : Threaded
    {
        private CommunicationManager(int localPort = 0) : base("Communicator")
        {
            LocalPort = localPort;
            CreateAsServer = false;
            InboundQueue = new ConcurrentQueue<Envelope>();
            OutboundQueue = new ConcurrentQueue<Envelope>();
        }

        public static CommunicationManager ServerInstance
        {
            get
            {
                if (instance == null)
                {
                    lock (InstanceLock)
                    {
                        if (instance == null)
                        {
                            instance = new CommunicationManager();
                            instance.CreateAsServer = true;
                            instance.Start();
                        }
                    }
                }

                return instance;
            }
        }

        public static CommunicationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (InstanceLock)
                    {
                        if (instance == null)
                        {
                            instance = new CommunicationManager();
                            instance.CreateAsServer = false;
                            instance.Start();
                        }
                    }
                }

                return instance;
            }
        }

        public static CommunicationManager GetInstanceAtLocalPort(int port = 0)
        {
            if (instance == null)
            {
                lock (InstanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CommunicationManager();
                        instance.CreateAsServer = false;
                        instance.Start();
                    }
                }
            }

            return instance;
        }


        protected override void Run()
        {
            UdpClient socket = new UdpClient(LocalPort);
            socket.Client.ReceiveTimeout = 2000;

            try
            {
                while (ContinueThread)
                {
                    if (socket.Available > 0)
                    {
                        Logger.Info("Information is available on the socket");
                        IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, LocalPort);
                        byte[] bytesReceived = socket.Receive(ref endpoint);
                        if (bytesReceived.Length > 0)
                        {
                            Envelope tempEnvelope = new Envelope(endpoint, Message.Decode(bytesReceived));
                            if (tempEnvelope.Message != null)
                            {
                                Logger.Info("Received message, enqueuing. Conversation ID " + tempEnvelope.Message.ConvId.Pid.ToString());
                                InboundQueue.Enqueue(tempEnvelope);
                            }
                        }
                    }
                    else if (!OutboundQueue.IsEmpty)
                    {

                        Envelope outboundEnvelope;
                        if (OutboundQueue.TryDequeue(out outboundEnvelope))
                        {
                            Logger.Info("Sending outbound message to " + outboundEnvelope.Address.ToString());
                            byte[] bytesToSend = outboundEnvelope.Message.Encode();
                            if (bytesToSend.Length > 0)
                            {
                                socket.Send(bytesToSend, bytesToSend.Length, outboundEnvelope.Address);
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }
            catch (Exception exc)
            {
                Logger.Error("UDP socket exception : " + exc.Message);
            }

            socket.Close();
            Logger.Info("Closing down Network Manager");
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

        public bool IsServer
        {
            get { return CreateAsServer && instance != null; }
        }
        public int LocalPort { get; set; }
        protected ConcurrentQueue<Envelope> InboundQueue { get; }
        protected ConcurrentQueue<Envelope> OutboundQueue { get; }
        protected bool CreateAsServer { get; set; }

        private static object InstanceLock = new object();
        private static volatile CommunicationManager instance;
    }
}
