using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Common.Communication;
using Common.Messages;

namespace TestCommon.TestObjects
{
    public class TestUdpSocket
    {
        public TestUdpSocket(int port)
        {
            LocalPort = port;
            InboundQueue = new ConcurrentQueue<Envelope>();
            OutboundQueue = new ConcurrentQueue<Envelope>();
            ActiveThread = new Thread(Run);
            ContinueThread = false;
        }

        protected void Run()
        {
            UdpClient socket = new UdpClient(new IPEndPoint(IPAddress.Any, LocalPort));
            IPEndPoint recvEndpoint = new IPEndPoint(IPAddress.Any, LocalPort);
            socket.Client.ReceiveTimeout = 2000;

            try
            {
                while (ContinueThread)
                {
                    if (socket.Available > 0)
                    {
                        byte[] bytesReceived = socket.Receive(ref recvEndpoint);
                        if (bytesReceived.Length > 0)
                        {
                            Envelope tempEnvelope = new Envelope(recvEndpoint, Message.Decode(bytesReceived));
                            if (tempEnvelope.Message != null)
                            {
                                InboundQueue.Enqueue(tempEnvelope);
                            }
                        }
                    }
                    else if (!OutboundQueue.IsEmpty)
                    {
                        if (OutboundQueue.TryDequeue(out Envelope outboundEnvelope))
                        {
                            byte[] bytesToSend = outboundEnvelope.Message.Encode();
                            if (bytesToSend.Length > 0)
                            {
                                socket.Send(bytesToSend, bytesToSend.Length, outboundEnvelope.Address);
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(25);
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Testing UDP socket exception : " + exc.Message);
            }

            socket.Close();
        }

        public void Send(Envelope envelope)
        {
            OutboundQueue.Enqueue(envelope);
        }

        public bool Receive(out Envelope envelope)
        {
            return InboundQueue.TryDequeue(out envelope);
        }

        public bool ReplyWaiting => !InboundQueue.IsEmpty;

        public void Start()
        {
            if (!ActiveThread.IsAlive)
            {
                ContinueThread = true;
                ActiveThread = new Thread(Run);
                ActiveThread.Start();
            }
        }

        public void Stop()
        {
            if (ActiveThread.IsAlive)
            {
                ContinueThread = false;
                ActiveThread.Join(2000);
            }
        }

        public bool IsActive()
        {
            return ContinueThread;
        }

        public int LocalPort { get; set; }
        protected Thread ActiveThread;
        protected volatile bool ContinueThread;
        protected ConcurrentQueue<Envelope> InboundQueue { get; }
        protected ConcurrentQueue<Envelope> OutboundQueue { get; }
    }
}

