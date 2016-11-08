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
    public class CommunicationService : Threaded
    {
        private CommunicationService(int localPort) : base("Communicator")
        {
            LocalPort = localPort;
            InboundQueue = new ConcurrentQueue<Envelope>();
            OutboundQueue = new ConcurrentQueue<Envelope>();
        }

        public static CommunicationService GetInstance(int port = 0)
        {
            if (instance == null)
            {
                lock (InstanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CommunicationService(port);
                        instance.Start();
                    }
                }
            }
            else if (!instance.IsActive())
            {
                instance.Start();
            }

            return instance;
        }

        public static CommunicationService GetUniqueUdpInstance(int port = 0)
        {
            CommunicationService tempInstance = new CommunicationService(port);
            tempInstance.Start();

            return tempInstance;
        }

        protected override void Run()
        {
            UdpClient socket = new UdpClient(new IPEndPoint(IPAddress.Any, LocalPort));
            IPEndPoint recvEndpoint = new IPEndPoint(IPAddress.Any, LocalPort);
            socket.Client.ReceiveTimeout = 2000;

            while (ContinueThread)
            {
                try
                {
                    if (socket.Available > 0)
                    {
                        //Logger.Info("Information is available on the socket");
                        byte[] bytesReceived = socket.Receive(ref recvEndpoint);
                        if (bytesReceived.Length > 0)
                        {
                            IPEndPoint endpoint = new IPEndPoint(recvEndpoint.Address, recvEndpoint.Port);
                            Envelope tempEnvelope = new Envelope(endpoint, Message.Decode(bytesReceived));
                            if (tempEnvelope.Message != null)
                            {
                                Logger.Info("Received message, enqueuing.");
                                InboundQueue.Enqueue(tempEnvelope);
                            }
                        }
                    }
                    else if (!OutboundQueue.IsEmpty)
                    {
                        Envelope outboundEnvelope;
                        if (OutboundQueue.TryDequeue(out outboundEnvelope))
                        {
                            byte[] bytesToSend = outboundEnvelope.Message.Encode();
                            Logger.Info($"Sending outbound message of length {bytesToSend.Length} to {outboundEnvelope.Address.ToString()}");
                            if (bytesToSend.Length > 0)
                            {
                                socket.Send(bytesToSend, bytesToSend.Length, outboundEnvelope.Address);
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(50);
                    }
                }
                catch (Exception exc)
                {
                    Logger.Error("UDP socket exception : " + exc.Message);
                }
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

        public int LocalPort { get; set; }
        public IPEndPoint AllowedIP { get; set; }
        protected ConcurrentQueue<Envelope> InboundQueue { get; }
        protected ConcurrentQueue<Envelope> OutboundQueue { get; }


        private static object InstanceLock = new object();
        private static volatile CommunicationService instance;
    }
}
